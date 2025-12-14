// Location: Election.UI/Forms/frmVoterDashboard.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace Election.UI.Forms
{
    public partial class frmVoterDashboard : Form
    {
        private HttpClient _httpClient;
        private int _selectedCandidateId = 0;
        private string _selectedCandidateName = "";
        private string _selectedCandidateParty = "";
        private int _voterId;
        private string _voterEmail;
        private Dictionary<int, Panel> _candidatePanels = new Dictionary<int, Panel>();

        private Color primaryColor = Color.FromArgb(44, 62, 80);
        private Color accentColor = Color.FromArgb(52, 152, 219);
        private Color cardColor = Color.FromArgb(236, 240, 241);
        private Color selectedColor = Color.FromArgb(46, 204, 113);

        // DTO classes that MATCH your VoteController responses
        public class HasVotedResponse
        {
            public bool success { get; set; }
            public bool hasVoted { get; set; }
        }

        public class VoteSubmitResponse
        {
            public bool success { get; set; }
            public string message { get; set; } = "";
            public int voteId { get; set; }
            public string candidateName { get; set; } = "";
            public string party { get; set; } = "";
            public string voteDate { get; set; } = "";
            public string errorCode { get; set; } = "";
        }

        public class ErrorResponseDto
        {
            public bool success { get; set; }
            public string message { get; set; } = "";
            public string errorCode { get; set; } = "";
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

        public frmVoterDashboard(int voterId, string voterEmail)
        {
            InitializeComponent();
            _voterId = voterId;
            _voterEmail = voterEmail;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7208") };
            SetupForm();
        }

        private void SetupForm()
        {
            lblVoterId.Text = $"Your Voter ID: {_voterId:D3}-XXX-{(_voterId % 1000):D3}";
        }

        private async void frmVoterDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                // Check if already voted - USING CORRECT DTO
                var hasVotedResponse = await _httpClient.GetAsync($"api/vote/hasvoted/{_voterId}");
                if (hasVotedResponse.IsSuccessStatusCode)
                {
                    // ✅ CORRECT: Use HasVotedResponse DTO that matches API
                    var result = await hasVotedResponse.Content.ReadFromJsonAsync<HasVotedResponse>();
                    if (result != null && result.success && result.hasVoted)
                    {
                        MessageBox.Show("You have already voted!", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Redirect to login page if already voted
                        RedirectToLogin();
                        return;
                    }
                }

                // Load approved candidates
                await LoadApprovedCandidates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}", "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadApprovedCandidates()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/candidate/approved");
                if (response.IsSuccessStatusCode)
                {
                    var candidates = await response.Content.ReadFromJsonAsync<List<CandidateDto>>();

                    if (candidates == null || candidates.Count == 0)
                    {
                        MessageBox.Show("No approved candidates available for voting.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    DisplayCandidates(candidates);
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

        private void DisplayCandidates(List<CandidateDto> candidates)
        {
            flowCandidates.Controls.Clear();
            _candidatePanels.Clear();

            foreach (var candidate in candidates)
            {
                var candidateCard = CreateCandidateCard(candidate);
                flowCandidates.Controls.Add(candidateCard);
                _candidatePanels[candidate.Id] = candidateCard;
            }
        }

        private Panel CreateCandidateCard(CandidateDto candidate)
        {
            var card = new Panel
            {
                Width = 760,
                Height = 100,
                BackColor = cardColor,
                Margin = new Padding(10),
                Padding = new Padding(15),
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand,
                Tag = candidate.Id
            };

            // Photo container
            var photoContainer = new Panel
            {
                Width = 70,
                Height = 70,
                Location = new Point(15, 15),
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

            // Load photo
            LoadCandidatePhoto(photoBox, candidate.PhotoPath, candidate.Name);
            photoContainer.Controls.Add(photoBox);

            // Candidate name
            var lblName = new Label
            {
                Text = candidate.Name.ToUpper(),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = primaryColor,
                Location = new Point(100, 20),
                AutoSize = true
            };

            // Party name
            var lblParty = new Label
            {
                Text = candidate.Party,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(100, 50),
                AutoSize = true
            };

            // Add click events
            card.Click += (s, e) => SelectCandidate(candidate.Id, candidate.Name, candidate.Party);
            photoContainer.Click += (s, e) => SelectCandidate(candidate.Id, candidate.Name, candidate.Party);
            photoBox.Click += (s, e) => SelectCandidate(candidate.Id, candidate.Name, candidate.Party);
            lblName.Click += (s, e) => SelectCandidate(candidate.Id, candidate.Name, candidate.Party);
            lblParty.Click += (s, e) => SelectCandidate(candidate.Id, candidate.Name, candidate.Party);

            // Add controls to card
            card.Controls.Add(photoContainer);
            card.Controls.Add(lblName);
            card.Controls.Add(lblParty);

            return card;
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

        private Image CreateDefaultPhoto(string name)
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

        private string GetInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return $"{parts[0][0]}{parts[1][0]}".ToUpper();
            else if (parts.Length == 1 && parts[0].Length >= 2)
                return $"{parts[0][0]}{parts[0][1]}".ToUpper();
            else
                return "CD";
        }

        private void SelectCandidate(int candidateId, string name, string party)
        {
            // Reset all cards
            foreach (var panel in _candidatePanels.Values)
            {
                panel.BackColor = cardColor;
                panel.BorderStyle = BorderStyle.None;
            }

            // Highlight selected
            if (_candidatePanels.ContainsKey(candidateId))
            {
                _candidatePanels[candidateId].BackColor = selectedColor;
                _candidatePanels[candidateId].BorderStyle = BorderStyle.FixedSingle;
            }

            // Update selection
            _selectedCandidateId = candidateId;
            _selectedCandidateName = name;
            _selectedCandidateParty = party;

            // Update label
            lblSelectedCandidate.Text = $"Your selection: {name}";
            lblSelectedCandidate.ForeColor = accentColor;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_selectedCandidateId == 0)
            {
                MessageBox.Show("Please select a candidate before submitting your vote.",
                    "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"CONFIRM YOUR VOTE:\n\n" +
                $"Candidate: {_selectedCandidateName}\n" +
                $"Party: {_selectedCandidateParty}\n\n" +
                $"This action cannot be undone!",
                "Final Confirmation",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Exclamation);

            if (confirm != DialogResult.OK)
                return;

            try
            {
                // Disable button during submission
                btnSubmit.Enabled = false;
                btnSubmit.Text = "Submitting...";

                var voteRequest = new { UserId = _voterId, CandidateId = _selectedCandidateId };
                var response = await _httpClient.PostAsJsonAsync("api/vote/submit", voteRequest);

                // Read response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Try to parse as JSON
                try
                {
                    using var jsonDoc = JsonDocument.Parse(responseContent);
                    var json = jsonDoc.RootElement;

                    bool success = json.GetProperty("success").GetBoolean();
                    string message = json.GetProperty("message").GetString();

                    if (success)
                    {
                        // SUCCESS
                        string candidateName = json.TryGetProperty("candidateName", out var cn) ? cn.GetString() : _selectedCandidateName;

                        MessageBox.Show(
                            $"✅ VOTE SUCCESSFULLY SUBMITTED!\n\n" +
                            $"Candidate: {candidateName}\n" +
                            $"Your vote has been recorded.\n\n" +
                            $"Thank you for participating in the election!",
                            "Vote Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Redirect to login
                        RedirectToLogin();
                    }
                    else
                    {
                        // ERROR - Check error type
                        string errorCode = json.TryGetProperty("errorCode", out var ec) ? ec.GetString() : "UNKNOWN_ERROR";

                        switch (errorCode)
                        {
                            case "ALREADY_VOTED":
                            case "VOTE_ALREADY_RECORDED":
                            case "DB_DUPLICATE_VOTE":
                                // User already voted
                                string voteDate = json.TryGetProperty("voteDate", out var vd) ? vd.GetString() : "previously";

                                var result = MessageBox.Show(
                                    $"⛔ YOU HAVE ALREADY VOTED!\n\n" +
                                    $"You voted {voteDate}.\n\n" +
                                    $"Would you like to reset your voting status? (Admin only)",
                                    "Already Voted",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning);

                                if (result == DialogResult.Yes)
                                {
                                    // Try to reset voting status
                                    await ResetUserVotingStatus(_voterId);
                                }
                                else
                                {
                                    // Redirect to login
                                    RedirectToLogin();
                                }
                                break;

                            case "USER_NOT_FOUND":
                                MessageBox.Show(
                                    "Your user account was not found. Please log in again.",
                                    "Account Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                RedirectToLogin();
                                break;

                            case "CANDIDATE_NOT_FOUND":
                                MessageBox.Show(
                                    "The selected candidate is no longer available. Please select another candidate.",
                                    "Candidate Not Available",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                break;

                            default:
                                MessageBox.Show(
                                    $"Failed to submit vote:\n{message}",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                break;
                        }
                    }
                }
                catch (JsonException)
                {
                    // Not JSON, show raw response
                    MessageBox.Show(
                        $"Server response:\n{responseContent}",
                        "Submission Result",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show(
                    $"Cannot connect to server:\n{httpEx.Message}\n\n" +
                    $"Please check your internet connection and try again.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error: {ex.Message}",
                    "Submission Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Submit My Vote";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_selectedCandidateId > 0)
            {
                var confirm = MessageBox.Show("Are you sure you want to cancel voting?\nYour selection will be lost.",
                    "Cancel Voting", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.No)
                    return;
            }

            // ✅ REDIRECT TO LOGIN PAGE WHEN CANCELLING
            RedirectToLogin();
        }

        // Helper method to reset user's voting status
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
                        MessageBox.Show(
                            "✅ Your voting status has been reset!\n\n" +
                            "You can now vote again.",
                            "Reset Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Refresh candidate list
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

        /// <summary>
        /// Helper method to redirect to login page
        /// </summary>
        private void RedirectToLogin()
        {
            this.Hide(); // Hide current voter dashboard

            // Create and show login form
            frmLogin loginForm = new frmLogin();
            loginForm.Show();

            this.Close(); // Close voter dashboard completely
        }
    }
}