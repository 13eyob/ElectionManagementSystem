#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace Election.UI.Forms
{
    public partial class FrmVoterDashboard : Form
    {
        // Dynamic controls (not in Designer)
        private Label? lblSelectedCandidate;
        private Label? lblVoterId;
        private Label? lblLastRefresh;
        private Button? btnManualRefresh;

        private readonly HttpClient _httpClient;
        private int _selectedCandidateId = 0;
        private string _selectedCandidateName = "";
        private string _selectedCandidateParty = "";
        private readonly int _voterId;
        private readonly string _voterEmail;
        private readonly Dictionary<int, Panel> _candidatePanels = [];

        // Premium Color Palette
        private static readonly Color ColorPrimary = Color.FromArgb(15, 22, 40);
        private static readonly Color ColorAccent = Color.FromArgb(52, 152, 219);
        private static readonly Color ColorCard = Color.FromArgb(240, 244, 248);
        private static readonly Color ColorSelected = Color.FromArgb(46, 204, 113);
        private static readonly Color ColorSelectionBg = Color.FromArgb(232, 245, 233);

        private System.Windows.Forms.Timer? _autoRefreshTimer;
        private DateTime _lastRefreshTime;

        public class HasVotedResponse
        {
            public bool Success { get; set; }
            public bool HasVoted { get; set; }
        }

        public class CandidateDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Party { get; set; } = "";
            public string Region { get; set; } = "";
            public int Age { get; set; }
            public string PhotoPath { get; set; } = "";
            public bool IsApproved { get; set; }
        }

        public FrmVoterDashboard(int voterId, string voterEmail)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;

            _voterId = voterId;
            _voterEmail = voterEmail;
            _httpClient = new() { BaseAddress = new("https://localhost:7208") };

            InitializeControls();
            InitializeAutoRefresh();

            this.Load += FrmVoterDashboard_Load;
            this.Resize += FrmVoterDashboard_Resize;
            this.FormClosing += FrmVoterDashboard_FormClosing;
        }

        private void FrmVoterDashboard_Resize(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;
            AdjustLayout();
        }

        private void FrmVoterDashboard_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _autoRefreshTimer?.Stop();
            _autoRefreshTimer?.Dispose();
            _httpClient.Dispose();
        }

        private void AdjustLayout()
        {
            if (panel2 != null)
            {
                panel2.Size = new(this.ClientSize.Width - 198, this.ClientSize.Height - 200);
                panel2.Location = new(99, 119);

                // No flowCandidates adjustments needed

                if (lblSelectedCandidate != null) lblSelectedCandidate.Location = new(50, panel2.Height - 90);
                if (lblVoterId != null) lblVoterId.Location = new(panel2.Width - 350, panel2.Height - 90);
                if (btnSubmit != null) btnSubmit.Location = new((panel2.Width - btnSubmit.Width) / 2, panel2.Height - 60);
                if (lblLastRefresh != null) lblLastRefresh.Location = new(panel2.Width - 350, panel2.Height - 65);
                if (btnManualRefresh != null) btnManualRefresh.Location = new(panel2.Width - 150, 20);
            }
        }

        private void InitializeControls()
        {
            lblSelectedCandidate = new()
            {
                Text = "Selected: None",
                Font = new("Segoe UI Semibold", 13),
                AutoSize = true,
                ForeColor = Color.White,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            lblVoterId = new()
            {
                Text = $"Voter ID: {_voterId}",
                Font = new("Segoe UI", 10),
                AutoSize = true,
                ForeColor = Color.LightGray,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            lblLastRefresh = new()
            {
                Text = "Last Refresh: Never",
                Font = new("Segoe UI", 9),
                AutoSize = true,
                ForeColor = Color.LightGray,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            btnManualRefresh = new()
            {
                Text = "🔄 Refresh",
                Size = new(100, 32),
                BackColor = ColorAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new("Segoe UI Semibold", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnManualRefresh.FlatAppearance.BorderSize = 0;
            btnManualRefresh.Click += async (s, e) => await ManualRefreshCandidates();

            panel2.Controls.Add(lblSelectedCandidate);
            panel2.Controls.Add(lblVoterId);
            panel2.Controls.Add(lblLastRefresh);
            panel2.Controls.Add(btnManualRefresh);

            if (btnSubmit != null)
            {
                btnSubmit.FlatStyle = FlatStyle.Flat;
                btnSubmit.FlatAppearance.BorderSize = 0;
                btnSubmit.BackColor = ColorAccent;
                btnSubmit.Font = new("Segoe UI Bold", 12);
                btnSubmit.Click += BtnSubmit_Click;
            }
            if (btnLogout != null) btnLogout.Click += BtnLogout_Click;
        }

        private void InitializeAutoRefresh()
        {
            _autoRefreshTimer = new() { Interval = 15000 };
            _autoRefreshTimer.Tick += async (s, e) =>
            {
                if (this.Visible && !this.IsDisposed)
                {
                    try { await LoadApprovedCandidates(); } catch { }
                }
            };
            _autoRefreshTimer.Start();
            _lastRefreshTime = DateTime.Now;
            UpdateLastRefreshLabel();
        }

        private async Task ManualRefreshCandidates()
        {
            if (btnManualRefresh == null) return;
            try
            {
                btnManualRefresh.Enabled = false;
                btnManualRefresh.Text = "Updating...";
                await LoadApprovedCandidates();
                btnManualRefresh.Enabled = true;
                btnManualRefresh.Text = "🔄 Refresh";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                btnManualRefresh.Enabled = true;
                btnManualRefresh.Text = "🔄 Refresh";
            }
        }

        private void UpdateLastRefreshLabel()
        {
            if (lblLastRefresh != null && !lblLastRefresh.IsDisposed)
                lblLastRefresh.Text = $"Last sync: {_lastRefreshTime:HH:mm:ss}";
        }

        private async void FrmVoterDashboard_Load(object? sender, EventArgs e) => await LoadApprovedCandidates();

        private async Task LoadApprovedCandidates()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/candidate/approved");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    List<CandidateDto> candidates = [];
                    if (result.TryGetProperty("candidates", out var array))
                    {
                        foreach (var cand in array.EnumerateArray())
                        {
                            candidates.Add(new()
                            {
                                Id = cand.TryGetProperty("id", out var id) ? id.GetInt32() : 0,
                                Name = cand.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "",
                                Party = cand.TryGetProperty("party", out var p) ? p.GetString() ?? "" : "",
                                Region = cand.TryGetProperty("region", out var r) ? r.GetString() ?? "" : "",
                                PhotoPath = cand.TryGetProperty("photoPath", out var pp) ? pp.GetString() ?? "" : ""
                            });
                        }
                    }

                    DisplayCandidates(candidates);
                    _lastRefreshTime = DateTime.Now;
                    UpdateLastRefreshLabel();

                    if (lblSelectedCandidate != null && _selectedCandidateId == 0)
                        lblSelectedCandidate.Text = "Select an official candidate";
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void DisplayCandidates(List<CandidateDto> candidates)
        {
            // Remove existing candidate panels from panel2
            var controlsToRemove = new List<Control>();
            foreach (Control control in panel2.Controls)
            {
                if (control.Tag is int || control.Name?.StartsWith("CandidateCard_") == true)
                {
                    controlsToRemove.Add(control);
                }
            }

            foreach (var control in controlsToRemove)
            {
                panel2.Controls.Remove(control);
                control.Dispose();
            }

            _candidatePanels.Clear();

            if (candidates.Count == 0)
            {
                Label lbl = new()
                {
                    Text = "No approved candidates found.\nCheck back later.",
                    Font = new("Segoe UI", 12),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new(panel2.Width / 2 - 150, panel2.Height / 2 - 50),
                    Size = new(300, 100)
                };
                panel2.Controls.Add(lbl);
                lbl.BringToFront();
            }
            else
            {
                int startY = 60;
                int cardSpacing = 20;

                for (int i = 0; i < candidates.Count; i++)
                {
                    var c = candidates[i];
                    var card = CreateCandidateCard(c);
                    card.Location = new Point(20, startY + (i * (card.Height + cardSpacing)));
                    card.Name = $"CandidateCard_{c.Id}";
                    panel2.Controls.Add(card);
                    _candidatePanels[c.Id] = card;
                    card.BringToFront();
                }
            }

            if (_selectedCandidateId > 0 && _candidatePanels.TryGetValue(_selectedCandidateId, out var p))
                HighlightCard(p, true);
        }

        private Panel CreateCandidateCard(CandidateDto c)
        {
            var card = new Panel
            {
                Width = Math.Min(800, panel2.Width - 80),
                Height = 120,
                BackColor = ColorCard,
                Margin = new(10, 5, 10, 15),
                Padding = new(15),
                Cursor = Cursors.Hand,
                Tag = c.Id
            };

            // Custom Modern Radio Button (Indicator)
            var indicator = new Panel
            {
                Size = new(24, 24),
                Location = new(20, 48),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Tag = "Indicator"
            };
            indicator.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                bool isSelected = _selectedCandidateId == c.Id;
                using var p = new Pen(isSelected ? ColorSelected : Color.LightGray, 2);
                e.Graphics.DrawEllipse(p, 2, 2, 19, 19);
                if (isSelected)
                {
                    using var b = new SolidBrush(ColorSelected);
                    e.Graphics.FillEllipse(b, 6, 6, 11, 11);
                }
            };

            var photoBox = new PictureBox
            {
                Size = new(80, 80),
                Location = new(65, 20),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            LoadCandidatePhoto(photoBox, c.PhotoPath, c.Name);

            var lblName = new Label
            {
                Text = c.Name,
                Font = new("Segoe UI Semibold", 15),
                ForeColor = ColorPrimary,
                Location = new(160, 30),
                AutoSize = true
            };

            var lblParty = new Label
            {
                Text = c.Party,
                Font = new("Segoe UI", 11),
                ForeColor = Color.DimGray,
                Location = new(160, 60),
                AutoSize = true
            };

            card.Click += (s, e) => SelectCandidate(c.Id, c.Name, c.Party);
            indicator.Click += (s, e) => SelectCandidate(c.Id, c.Name, c.Party);
            lblName.Click += (s, e) => SelectCandidate(c.Id, c.Name, c.Party);
            lblParty.Click += (s, e) => SelectCandidate(c.Id, c.Name, c.Party);
            photoBox.Click += (s, e) => SelectCandidate(c.Id, c.Name, c.Party);

            // Hover effect
            card.MouseEnter += (s, e) => { if (_selectedCandidateId != c.Id) card.BackColor = Color.FromArgb(232, 241, 250); };
            card.MouseLeave += (s, e) => { if (_selectedCandidateId != c.Id) card.BackColor = ColorCard; };

            card.Controls.Add(indicator);
            card.Controls.Add(photoBox);
            card.Controls.Add(lblName);
            card.Controls.Add(lblParty);

            return card;
        }

        private void SelectCandidate(int id, string name, string party)
        {
            foreach (var p in _candidatePanels.Values) HighlightCard(p, false);
            if (_candidatePanels.TryGetValue(id, out var panel)) HighlightCard(panel, true);

            _selectedCandidateId = id;
            _selectedCandidateName = name;
            _selectedCandidateParty = party;

            if (lblSelectedCandidate != null)
                lblSelectedCandidate.Text = "Selected: " + name;
        }

        private static void HighlightCard(Panel card, bool highlight)
        {
            card.BackColor = highlight ? ColorSelectionBg : ColorCard;
            card.BorderStyle = highlight ? BorderStyle.FixedSingle : BorderStyle.None;
            var ind = card.Controls.Find("Indicator", false).FirstOrDefault();
            ind?.Invalidate();
        }

        private async void LoadCandidatePhoto(PictureBox pb, string path, string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    using var stream = await _httpClient.GetStreamAsync("https://localhost:7208" + path);
                    pb.Image = Image.FromStream(stream);
                }
                else pb.Image = CreateDefaultPhoto(name);
            }
            catch { pb.Image = CreateDefaultPhoto(name); }
        }

        private static Bitmap CreateDefaultPhoto(string name)
        {
            var bmp = new Bitmap(80, 80);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(230, 234, 238));
            using var b = new SolidBrush(Color.FromArgb(170, 180, 190));
            g.FillEllipse(b, 5, 5, 70, 70);

            var initials = string.Concat(name.Split(' ').Where(x => x.Length > 0).Select(x => x[0])).ToUpper();
            if (initials.Length > 2) initials = initials[..2];

            using var f = new Font("Segoe UI", 20, FontStyle.Bold);
            var sz = g.MeasureString(initials, f);
            using var tb = new SolidBrush(Color.White);
            g.DrawString(initials, f, tb, (80 - sz.Width) / 2, (80 - sz.Height) / 2);
            return bmp;
        }

        private async void BtnSubmit_Click(object? sender, EventArgs e)
        {
            if (_selectedCandidateId == 0)
            {
                MessageBox.Show("Please select a candidate first.", "No Candidate Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Cast your vote for {_selectedCandidateName}?", "Confirm Vote",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (btnSubmit != null) { btnSubmit.Enabled = false; btnSubmit.Text = "Submitting..."; }

                    var resp = await _httpClient.PostAsJsonAsync("api/vote/submit",
                        new { UserId = _voterId, CandidateId = _selectedCandidateId });

                    if (resp.IsSuccessStatusCode)
                    {
                        _autoRefreshTimer?.Stop();

                        // ✅ REQUIREMENT #1: Show success message after vote is saved
                        MessageBox.Show(
                            "✅ Vote submitted successfully. Thank you for participating in the election.\n\n" +
                            $"Candidate: {_selectedCandidateName}\n" +
                            $"Party: {_selectedCandidateParty}\n\n" +
                            "Your vote has been recorded securely.",
                            "Vote Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // ✅ REQUIREMENT #1 & #4: Disable voting button permanently
                        if (btnSubmit != null)
                        {
                            btnSubmit.Enabled = false;
                            btnSubmit.Text = "✓ Already Voted";
                            btnSubmit.BackColor = Color.Gray;
                        }

                        // Disable candidate selection
                        foreach (var panel in _candidatePanels.Values)
                        {
                            panel.Enabled = false;
                        }

                        // Update selection label
                        if (lblSelectedCandidate != null)
                        {
                            lblSelectedCandidate.Text = $"✓ Voted for: {_selectedCandidateName}";
                            lblSelectedCandidate.ForeColor = Color.LightGreen;
                        }

                        // ✅ REQUIREMENT #4: Logout and prevent re-login
                        await Task.Delay(3000); // Give user time to read the message
                        RedirectToLogin();
                    }
                    else
                    {
                        var m = await resp.Content.ReadFromJsonAsync<JsonElement>();
                        string errorMessage = m.TryGetProperty("message", out var msg) ? msg.GetString() ?? "Vote failed." : "Vote failed.";

                        // ✅ REQUIREMENT #2: Show clear message if already voted
                        if (errorMessage.Contains("already voted", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show(
                                "⚠️ You have already voted. Multiple votes are not allowed.\n\n" +
                                "Each voter can only cast one vote per election.",
                                "Already Voted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            // Disable voting
                            if (btnSubmit != null)
                            {
                                btnSubmit.Enabled = false;
                                btnSubmit.Text = "✓ Already Voted";
                                btnSubmit.BackColor = Color.Gray;
                            }
                        }
                        else
                        {
                            MessageBox.Show(errorMessage, "Vote Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (btnSubmit != null && btnSubmit.Enabled)
                    {
                        btnSubmit.Enabled = true;
                        btnSubmit.Text = "Cast My Vote";
                    }
                }
            }
        }

        private void BtnLogout_Click(object? sender, EventArgs e) { _autoRefreshTimer?.Stop(); RedirectToLogin(); }

        private void RedirectToLogin()
        {
            this.Hide();
            new FrmLogin().Show();
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}