// Location: Election.UI/Forms/frmVoterDashboard.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace Election.UI.Forms
{
    public partial class frmVoterDashboard : Form
    {
        // ===== CONTROL DECLARATIONS =====
        private Panel panel1;
        private PictureBox picLogo;
        private Label lblSystemTitle;
        private Button btnLogout;
        private Panel panel2;
        private Label label1;
        private Button btnSubmit;
        private FlowLayoutPanel flowCandidates;
        private Label lblSelectedCandidate;
        private Label lblVoterId;
        private Button btnCancel;

        private readonly HttpClient _httpClient;
        private int _selectedCandidateId = 0;
        private string _selectedCandidateName = "";
        private string _selectedCandidateParty = "";
        private readonly int _voterId;
        private readonly string _voterEmail;
        private readonly Dictionary<int, Panel> _candidatePanels = new Dictionary<int, Panel>();

        private readonly Color primaryColor = Color.FromArgb(44, 62, 80);
        private readonly Color accentColor = Color.FromArgb(52, 152, 219);
        private readonly Color cardColor = Color.FromArgb(236, 240, 241);
        private readonly Color selectedColor = Color.FromArgb(46, 204, 113);

        // ===== AUTO-REFRESH VARIABLES =====
        private System.Windows.Forms.Timer _autoRefreshTimer;
        private DateTime _lastRefreshTime;
        private Label lblLastRefresh;
        private Button btnManualRefresh;

        // ===== DTO CLASSES =====
        public class HasVotedResponse
        {
            public bool Success { get; set; }
            public bool HasVoted { get; set; }
        }

        public class VoteSubmitResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
            public int VoteId { get; set; }
            public string CandidateName { get; set; } = "";
            public string Party { get; set; } = "";
            public string VoteDate { get; set; } = "";
            public string ErrorCode { get; set; } = "";
        }

        public class ErrorResponseDto
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
            public string ErrorCode { get; set; } = "";
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

        // ===== CONSTRUCTOR =====
        public frmVoterDashboard(int voterId, string voterEmail)
        {
            InitializeComponent();

            // Form properties
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.WindowState = FormWindowState.Maximized;

            _voterId = voterId;
            _voterEmail = voterEmail;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7208") };

            // Initialize controls
            InitializeControls();

            // Initialize auto-refresh timer
            InitializeAutoRefresh();

            // Handle form events
            this.Resize += FrmVoterDashboard_Resize;
            this.FormClosing += FrmVoterDashboard_FormClosing;
        }

        // ===== FORM RESIZE HANDLER =====
        private void FrmVoterDashboard_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;
            AdjustLayout();
        }

        // ===== FORM CLOSING HANDLER =====
        private void FrmVoterDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup auto-refresh timer
            if (_autoRefreshTimer != null)
            {
                _autoRefreshTimer.Stop();
                _autoRefreshTimer.Dispose();
                _autoRefreshTimer = null;
            }
        }

        private void AdjustLayout()
        {
            if (panel2 != null)
            {
                panel2.Size = new Size(this.ClientSize.Width - 198, this.ClientSize.Height - 155);
                panel2.Location = new Point(99, 119);

                if (flowCandidates != null)
                {
                    flowCandidates.Size = new Size(panel2.Width - 108, 400);

                    foreach (Control control in flowCandidates.Controls)
                    {
                        if (control is Panel card)
                        {
                            card.Width = Math.Min(760, panel2.Width - 120);
                        }
                    }
                }

                if (lblSelectedCandidate != null)
                {
                    lblSelectedCandidate.Location = new Point(50, panel2.Height - 106);
                }

                if (lblVoterId != null)
                {
                    lblVoterId.Location = new Point(panel2.Width - 350, panel2.Height - 106);
                }

                if (btnSubmit != null)
                {
                    btnSubmit.Location = new Point(
                        (panel2.Width - btnSubmit.Width) / 2,
                        panel2.Height - 63
                    );
                }

                // Adjust refresh label
                if (lblLastRefresh != null)
                {
                    lblLastRefresh.Location = new Point(panel2.Width - 350, panel2.Height - 80);
                }

                // Adjust refresh button
                if (btnManualRefresh != null)
                {
                    btnManualRefresh.Location = new Point(panel2.Width - 150, 20);
                }
            }
        }

        private void InitializeControls()
        {
            // Find controls from Designer
            panel1 = Controls["panel1"] as Panel;
            picLogo = panel1?.Controls["picLogo"] as PictureBox;
            lblSystemTitle = panel1?.Controls["lblSystemTitle"] as Label;
            btnLogout = panel1?.Controls["btnLogout"] as Button;

            panel2 = Controls["panel2"] as Panel;
            label1 = panel2?.Controls["label1"] as Label;
            btnSubmit = panel2?.Controls["btnSubmit"] as Button;

            // Create dynamic controls
            if (flowCandidates == null)
            {
                flowCandidates = new FlowLayoutPanel
                {
                    Name = "flowCandidates",
                    Location = new Point(50, 60),
                    Size = new Size(1042, 400),
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                panel2.Controls.Add(flowCandidates);
            }

            if (lblSelectedCandidate == null)
            {
                lblSelectedCandidate = new Label
                {
                    Name = "lblSelectedCandidate",
                    Location = new Point(50, 470),
                    Size = new Size(400, 25),
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = accentColor,
                    Text = "Your selection: None",
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                };
                panel2.Controls.Add(lblSelectedCandidate);
            }

            if (lblVoterId == null)
            {
                lblVoterId = new Label
                {
                    Name = "lblVoterId",
                    Location = new Point(800, 470),
                    Size = new Size(300, 25),
                    Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                    ForeColor = Color.White,
                    Text = $"Your Voter ID: {_voterId:D3}-XXX-{(_voterId % 1000):D3}",
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right
                };
                panel2.Controls.Add(lblVoterId);
            }

            // Create last refresh time label
            lblLastRefresh = new Label
            {
                Name = "lblLastRefresh",
                Location = new Point(800, 500),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.LightGray,
                Text = "Last refresh: --:--:--",
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            panel2.Controls.Add(lblLastRefresh);

            // Create manual refresh button
            btnManualRefresh = new Button
            {
                Name = "btnManualRefresh",
                Text = "🔄 Refresh",
                Location = new Point(panel2.Width - 150, 20),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnManualRefresh.FlatAppearance.BorderSize = 0;
            btnManualRefresh.Click += async (s, e) => await ManualRefreshCandidates();
            panel2.Controls.Add(btnManualRefresh);

            // Wire up events
            if (btnSubmit != null) btnSubmit.Click += BtnSubmit_Click;
            if (btnLogout != null) btnLogout.Click += BtnLogout_Click;
        }

        // ===== AUTO-REFRESH METHODS =====
        private void InitializeAutoRefresh()
        {
            _autoRefreshTimer = new System.Windows.Forms.Timer();
            _autoRefreshTimer.Interval = 10000; // 10 seconds
            _autoRefreshTimer.Tick += async (sender, e) =>
            {
                if (this.Visible && !this.IsDisposed)
                {
                    try
                    {
                        await LoadApprovedCandidates();
                        _lastRefreshTime = DateTime.Now;
                        UpdateLastRefreshLabel();
                    }
                    catch { /* Silent fail */ }
                }
            };

            _autoRefreshTimer.Start();
            _lastRefreshTime = DateTime.Now;
            UpdateLastRefreshLabel();
        }

        private async Task ManualRefreshCandidates()
        {
            try
            {
                btnManualRefresh.Enabled = false;
                btnManualRefresh.Text = "Refreshing...";
                await LoadApprovedCandidates();
                _lastRefreshTime = DateTime.Now;
                UpdateLastRefreshLabel();
                btnManualRefresh.Enabled = true;
                btnManualRefresh.Text = "🔄 Refresh";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnManualRefresh.Enabled = true;
                btnManualRefresh.Text = "🔄 Refresh";
            }
        }

        private void UpdateLastRefreshLabel()
        {
            if (lblLastRefresh != null && !lblLastRefresh.IsDisposed)
            {
                lblLastRefresh.Text = $"Last refresh: {_lastRefreshTime:HH:mm:ss}";
            }
        }

        // ===== FORM LOAD =====
        private async void FrmVoterDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadApprovedCandidates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}", "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== LOAD APPROVED CANDIDATES =====
        private async Task LoadApprovedCandidates()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/candidate/approved");

                if (response.IsSuccessStatusCode)
                {
                    // Read the response
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the JSON
                    using var jsonDoc = JsonDocument.Parse(responseContent);
                    var json = jsonDoc.RootElement;

                    List<CandidateDto> candidates = new List<CandidateDto>();

                    // Extract candidates from "candidates" property
                    if (json.TryGetProperty("candidates", out var candidatesArray))
                    {
                        foreach (var candidate in candidatesArray.EnumerateArray())
                        {
                            var candidateDto = new CandidateDto
                            {
                                Id = candidate.GetProperty("id").GetInt32(),
                                Name = candidate.GetProperty("name").GetString(),
                                Party = candidate.GetProperty("party").GetString(),
                                Region = candidate.GetProperty("region").GetString(),
                                Age = candidate.GetProperty("age").GetInt32(),
                                PhotoPath = candidate.GetProperty("photoPath").GetString(),
                                IsApproved = candidate.GetProperty("isApproved").GetBoolean()
                            };
                            candidates.Add(candidateDto);
                        }
                    }

                    // Display candidates
                    if (candidates.Count == 0)
                    {
                        // Show no candidates message
                        DisplayCandidates(new List<CandidateDto>());
                        return;
                    }

                    DisplayCandidates(candidates);

                    // Update last refresh time
                    _lastRefreshTime = DateTime.Now;
                    UpdateLastRefreshLabel();

                    // Update selection label
                    if (lblSelectedCandidate != null && _selectedCandidateId == 0)
                    {
                        lblSelectedCandidate.Text = $"Select from {candidates.Count} approved candidate(s)";
                        lblSelectedCandidate.ForeColor = accentColor;
                    }
                }
                else
                {
                    MessageBox.Show("Failed to load candidates from server.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading candidates: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== DISPLAY CANDIDATES WITH RADIO BUTTONS =====
        private void DisplayCandidates(List<CandidateDto> candidates)
        {
            if (flowCandidates == null) return;

            flowCandidates.SuspendLayout();
            flowCandidates.Controls.Clear();
            _candidatePanels.Clear();

            if (candidates.Count == 0)
            {
                Label lblNoCandidates = new Label
                {
                    Text = "📭 No approved candidates available for voting.\n\nCandidates are being reviewed by administrators.\nPlease check back later.",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(20)
                };
                flowCandidates.Controls.Add(lblNoCandidates);
                flowCandidates.ResumeLayout();
                return;
            }

            foreach (var candidate in candidates)
            {
                var candidateCard = CreateCandidateCard(candidate);
                flowCandidates.Controls.Add(candidateCard);
                _candidatePanels[candidate.Id] = candidateCard;
            }

            // If there's a previously selected candidate, highlight it
            if (_selectedCandidateId > 0 && _candidatePanels.ContainsKey(_selectedCandidateId))
            {
                SelectCandidate(_selectedCandidateId, _selectedCandidateName, _selectedCandidateParty);
            }

            flowCandidates.ResumeLayout();
        }

        // ===== CREATE CANDIDATE CARD WITH RADIO BUTTON =====
        private Panel CreateCandidateCard(CandidateDto candidate)
        {
            var card = new Panel
            {
                Width = Math.Min(760, flowCandidates.Width - 40),
                Height = 100,
                BackColor = cardColor,
                Margin = new Padding(10),
                Padding = new Padding(15),
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand,
                Tag = candidate.Id
            };

            // ===== 1. RADIO BUTTON (LEFT SIDE) =====
            RadioButton radioCandidate = new RadioButton
            {
                Name = "radioCandidateGroup", // Same name groups them together
                Text = "", // No text
                Location = new Point(15, 35),
                Size = new Size(20, 20),
                Tag = candidate.Id,
                Checked = (_selectedCandidateId == candidate.Id),
                Appearance = Appearance.Normal
            };

            // ===== 2. PHOTO =====
            var photoContainer = new Panel
            {
                Width = 70,
                Height = 70,
                Location = new Point(45, 15), // Moved right to make space for radio
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var photoBox = new PictureBox
            {
                Width = 66,
                Height = 66,
                Location = new Point(2, 2),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            LoadCandidatePhoto(photoBox, candidate.PhotoPath, candidate.Name);
            photoContainer.Controls.Add(photoBox);

            // ===== 3. CANDIDATE INFO =====
            var lblName = new Label
            {
                Text = candidate.Name.ToUpper(),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = primaryColor,
                Location = new Point(130, 20), // Adjusted position
                AutoSize = true
            };

            var lblParty = new Label
            {
                Text = candidate.Party,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(130, 50), // Adjusted position
                AutoSize = true
            };

            var lblApproved = new Label
            {
                Text = "✅ APPROVED",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.Green,
                Location = new Point(card.Width - 120, 20),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            // ===== 4. RADIO BUTTON EVENT =====
            radioCandidate.CheckedChanged += (s, e) =>
            {
                if (radioCandidate.Checked)
                {
                    SelectCandidate(candidate.Id, candidate.Name, candidate.Party);
                }
            };

            // ===== 5. CARD CLICK ALSO SELECTS RADIO =====
            card.Click += (s, e) =>
            {
                radioCandidate.Checked = true;
            };

            // Click events for other controls
            photoContainer.Click += (s, e) => radioCandidate.Checked = true;
            photoBox.Click += (s, e) => radioCandidate.Checked = true;
            lblName.Click += (s, e) => radioCandidate.Checked = true;
            lblParty.Click += (s, e) => radioCandidate.Checked = true;
            lblApproved.Click += (s, e) => radioCandidate.Checked = true;

            // ===== 6. ADD ALL CONTROLS TO CARD =====
            card.Controls.Add(radioCandidate);
            card.Controls.Add(photoContainer);
            card.Controls.Add(lblName);
            card.Controls.Add(lblParty);
            card.Controls.Add(lblApproved);

            return card;
        }

        // ===== SELECT CANDIDATE METHOD =====
        private void SelectCandidate(int candidateId, string name, string party)
        {
            // Reset all cards to default appearance
            foreach (var panel in _candidatePanels.Values)
            {
                panel.BackColor = cardColor;
                panel.BorderStyle = BorderStyle.None;
            }

            // Highlight selected card
            if (_candidatePanels.TryGetValue(candidateId, out var selectedPanel))
            {
                selectedPanel.BackColor = selectedColor;
                selectedPanel.BorderStyle = BorderStyle.FixedSingle;
            }

            _selectedCandidateId = candidateId;
            _selectedCandidateName = name;
            _selectedCandidateParty = party;

            if (lblSelectedCandidate != null)
            {
                lblSelectedCandidate.Text = $"Your selection: {name} ({party})";
                lblSelectedCandidate.ForeColor = accentColor;
            }
        }

        private async void LoadCandidatePhoto(PictureBox pictureBox, string photoPath, string candidateName)
        {
            try
            {
                if (!string.IsNullOrEmpty(photoPath))
                {
                    var photoUrl = $"https://localhost:7208{photoPath}";
                    using var imageStream = await _httpClient.GetStreamAsync(photoUrl);
                    pictureBox.Image = Image.FromStream(imageStream);
                }
                else
                {
                    pictureBox.Image = CreateDefaultPhoto(candidateName);
                }
            }
            catch
            {
                pictureBox.Image = CreateDefaultPhoto(candidateName);
            }
        }

        private Bitmap CreateDefaultPhoto(string name)
        {
            var bmp = new Bitmap(66, 66);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(189, 195, 199));
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Color.FromArgb(127, 140, 141)))
                {
                    g.FillEllipse(brush, 0, 0, 65, 65);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    var initials = GetInitials(name);
                    var font = new Font("Segoe UI", 16, FontStyle.Bold);
                    var textSize = g.MeasureString(initials, font);

                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(initials, font, brush,
                            (66 - textSize.Width) / 2,
                            (66 - textSize.Height) / 2);
                    }
                }
            }
            return bmp;
        }

        private static string GetInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return $"{parts[0][0]}{parts[1][0]}".ToUpper();
            else if (parts.Length == 1 && parts[0].Length >= 2)
                return $"{parts[0][0]}{parts[0][1]}".ToUpper();
            else
                return "CD";
        }

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (_selectedCandidateId == 0)
            {
                MessageBox.Show("Please select a candidate before submitting your vote.",
                    "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"CONFIRM YOUR VOTE:\n\nCandidate: {_selectedCandidateName}\nParty: {_selectedCandidateParty}\n\nThis action cannot be undone!",
                "Final Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (confirm != DialogResult.OK) return;

            try
            {
                btnSubmit.Enabled = false;
                btnSubmit.Text = "Submitting...";

                var voteRequest = new { UserId = _voterId, CandidateId = _selectedCandidateId };
                var response = await _httpClient.PostAsJsonAsync("api/vote/submit", voteRequest);
                string responseContent = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(responseContent);
                var json = jsonDoc.RootElement;

                bool success = json.GetProperty("success").GetBoolean();
                string message = json.GetProperty("message").GetString();

                if (success)
                {
                    string candidateName = json.TryGetProperty("candidateName", out var cn) ? cn.GetString() : _selectedCandidateName;

                    MessageBox.Show($"✅ VOTE SUCCESSFULLY SUBMITTED!\n\nCandidate: {candidateName}\nYour vote has been recorded.\n\nThank you for participating in the election!",
                        "Vote Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Stop auto-refresh after voting
                    if (_autoRefreshTimer != null)
                    {
                        _autoRefreshTimer.Stop();
                    }

                    RedirectToLogin();
                }
                else
                {
                    string errorCode = json.TryGetProperty("errorCode", out var ec) ? ec.GetString() : "UNKNOWN_ERROR";

                    switch (errorCode)
                    {
                        case "ALREADY_VOTED":
                        case "VOTE_ALREADY_RECORDED":
                        case "DB_DUPLICATE_VOTE":
                            string voteDate = json.TryGetProperty("voteDate", out var vd) ? vd.GetString() : "previously";

                            var result = MessageBox.Show($"⛔ YOU HAVE ALREADY VOTED!\n\nYou voted {voteDate}.\n\nWould you like to reset your voting status? (Admin only)",
                                "Already Voted", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if (result == DialogResult.Yes)
                            {
                                await ResetUserVotingStatus(_voterId);
                            }
                            else
                            {
                                RedirectToLogin();
                            }
                            break;

                        case "USER_NOT_FOUND":
                            MessageBox.Show("Your user account was not found. Please log in again.",
                                "Account Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            RedirectToLogin();
                            break;

                        case "CANDIDATE_NOT_FOUND":
                            MessageBox.Show("The selected candidate is no longer available. Please select another candidate.",
                                "Candidate Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;

                        default:
                            MessageBox.Show($"Failed to submit vote:\n{message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Submission Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Submit My Vote";
            }
        }

        private async Task ResetUserVotingStatus(int userId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/vote/reset-user/{userId}", null);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using var jsonDoc = JsonDocument.Parse(content);
                    var json = jsonDoc.RootElement;

                    if (json.GetProperty("success").GetBoolean())
                    {
                        MessageBox.Show("✅ Your voting status has been reset!\n\nYou can now vote again.",
                            "Reset Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        await LoadApprovedCandidates();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to reset voting: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RedirectToLogin()
        {
            if (_autoRefreshTimer != null)
            {
                _autoRefreshTimer.Stop();
                _autoRefreshTimer.Dispose();
                _autoRefreshTimer = null;
            }

            Hide();
            new frmLogin().Show();
            Close();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes) RedirectToLogin();
        }

        private void FlowCandidates_Paint(object sender, PaintEventArgs e) { }
        private void Panel2_Paint(object sender, PaintEventArgs e) { }
    }
}