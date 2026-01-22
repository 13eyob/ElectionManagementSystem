using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;  // ✅ Added
using System.Windows.Forms;
using Election.UI;

namespace Election.UI.Forms
{
    public partial class FrmLogin : Form
    {
        private readonly HttpClient _client = new HttpClient();

        // ✅ FIXED: Proper JSON property mapping
        public record LoginResponse(
            [property: JsonPropertyName("success")] bool Success = false,
            [property: JsonPropertyName("userId")] int UserId = 0,
            [property: JsonPropertyName("fullName")] string FullName = "",
            [property: JsonPropertyName("role")] string Role = "",
            [property: JsonPropertyName("isApproved")] bool IsApproved = false,
            [property: JsonPropertyName("email")] string Email = "",      // ✅ Maps "email" from API
            [property: JsonPropertyName("region")] string Region = "",    // ✅ Maps "region" from API
            [property: JsonPropertyName("hasVoted")] bool HasVoted = false,
            [property: JsonPropertyName("message")] string Message = "",
            [property: JsonPropertyName("errorCode")] string ErrorCode = ""
        );

        public FrmLogin()
        {
            InitializeComponent();
            _client.BaseAddress = new Uri("https://localhost:7208");
            _client.Timeout = TimeSpan.FromSeconds(30);

            // Center panel events
            this.Load += FrmLogin_Load;
            this.Resize += (s, e) => CenterLoginPanel();

            // Keyboard support
            this.KeyPreview = true;
            this.KeyDown += FrmLogin_KeyDown;

            // Button and link events
            button1.Click += Button1_Click;
            linkRegister.LinkClicked += LinkRegister_LinkClicked;
        }

        private void CenterLoginPanel()
        {
            if (pnllogin != null && this.ClientSize.Width > 0 && this.ClientSize.Height > 0)
            {
                pnllogin.Left = (this.ClientSize.Width - pnllogin.Width) / 2;
                pnllogin.Top = (this.ClientSize.Height - pnllogin.Height) / 2;
            }
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
            CenterLoginPanel();
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Please enter your username",
                                   "Validation Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter your password",
                                   "Validation Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // UI state update
                Cursor = Cursors.WaitCursor;
                button1.Enabled = false;
                button1.Text = "Logging in...";

                // Prepare login data
                var loginData = new { Username = username, Password = password };
                var response = await _client.PostAsJsonAsync("api/auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true  // ✅ This helps too
                    };
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>(options);

                    if (result != null)
                    {
                        // ✅ DEBUG: Show what we received
                        Console.WriteLine($"DEBUG Login Response:");
                        Console.WriteLine($"- Email: {result.Email}");
                        Console.WriteLine($"- Region: {result.Region}");
                        Console.WriteLine($"- FullName: {result.FullName}");
                        Console.WriteLine($"- Role: {result.Role}");

                        // Store user session with all new fields
                        UserSession.UserId = result.UserId;
                        UserSession.Username = username;

                        // ✅ CRITICAL FIX: These will now get the correct values
                        UserSession.Email = result.Email ?? "";
                        UserSession.Region = result.Region ?? "";
                        UserSession.Role = result.Role ?? "";
                        UserSession.Token = "dummy_token";

                        string role = !string.IsNullOrEmpty(result.Role) ? result.Role : "Unknown";
                        string fullName = !string.IsNullOrEmpty(result.FullName) ? result.FullName : username;

                        if (result.Success || !string.IsNullOrEmpty(result.Role))
                        {
                            MessageBox.Show($"Welcome {fullName}!\nRole: {role}\nEmail: {UserSession.Email}\nRegion: {UserSession.Region}",
                                           "Login Successful",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Information);

                            this.Hide();

                            // Navigate based on role - ACTUALLY OPEN THE FORMS
                            if (role.Equals("Candidate", StringComparison.OrdinalIgnoreCase))
                            {
                                // ✅ Now email and region are properly stored in UserSession
                                var candidateForm = new FrmCandidateApplication(fullName);
                                candidateForm.Show();
                            }
                            else if (role.Equals("Voter", StringComparison.OrdinalIgnoreCase))
                            {
                                // ✅ Open Voter Dashboard with actual userId from login response
                                var voterForm = new FrmVoterDashboard(result.UserId, UserSession.Email ?? username);
                                voterForm.Show();
                            }
                            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                // Open Admin Dashboard Form
                                var adminForm = new FrmAdminDashboard();
                                adminForm.Show();
                            }
                            else
                            {
                                MessageBox.Show($"Unknown role: {role}\nPlease contact administrator.",
                                               "Role Error",
                                               MessageBoxButtons.OK,
                                               MessageBoxIcon.Error);
                                this.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show(!string.IsNullOrEmpty(result.Message) ? result.Message : "Login failed. Please check your credentials.",
                                           "Login Failed",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Error);
                            this.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid response from server",
                                       "Error",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                        this.Show();
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    string errorMessage = "Login failed";

                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<JsonElement>(error);
                        if (errorObj.TryGetProperty("message", out var messageProp))
                        {
                            errorMessage = messageProp.GetString() ?? errorMessage;
                        }
                    }
                    catch { }

                    MessageBox.Show($"{errorMessage}\n\nStatus: {response.StatusCode}",
                                   "Login Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                    this.Show();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Cannot connect to server!\n\nError: {ex.Message}",
                               "Connection Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                this.Show();
            }
            finally
            {
                // Reset UI state
                Cursor = Cursors.Default;
                button1.Enabled = true;
                button1.Text = "Login";
            }
        }

        private void LinkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var registerForm = new frmRegister();
            registerForm.Show();
        }

        // Event handlers for designer-generated events
        private void TxtUsername_TextChanged(object sender, EventArgs e) { }
        private void LblPassword_Click(object sender, EventArgs e) { }
        private void Label1_Click(object sender, EventArgs e) { }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            _client?.Dispose();
        }
    }
}