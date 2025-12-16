using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;
using Election.UI;

namespace Election.UI.Forms
{
    public partial class frmLogin : Form
    {
        private readonly HttpClient _client = new HttpClient();

        // LoginResponse class for API response
        public class LoginResponse
        {
            public string fullName { get; set; } = "";
            public string role { get; set; } = "";
            public bool isApproved { get; set; }
            public string email { get; set; } = "";
            public string message { get; set; } = "";
            public bool success { get; set; }
        }

        public frmLogin()
        {
            InitializeComponent();
            _client.BaseAddress = new Uri("https://localhost:7208");
            _client.Timeout = TimeSpan.FromSeconds(30);

            // Center the panel when form loads
            this.Load += frmLogin_Load;
            this.Resize += (s, e) => CenterLoginPanel();

            // Handle Enter key press for login
            this.KeyPreview = true;
            this.KeyDown += FrmLogin_KeyDown;

            // Connect button and link events
            button1.Click += button1_Click;
            linkRegister.LinkClicked += linkRegister_LinkClicked;

            // NO placeholder event handlers needed
        }

        /// <summary>
        /// Centers the login panel both horizontally and vertically
        /// </summary>
        private void CenterLoginPanel()
        {
            if (pnllogin != null && this.ClientSize.Width > 0 && this.ClientSize.Height > 0)
            {
                pnllogin.Left = (this.ClientSize.Width - pnllogin.Width) / 2;
                pnllogin.Top = (this.ClientSize.Height - pnllogin.Height) / 2;
            }
        }

        /// <summary>
        /// Handle Enter key press for login
        /// </summary>
        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Form Load: Set empty fields and focus
        /// </summary>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Start with empty fields (NO PLACEHOLDERS)
            txtUsername.Text = "";
            txtPassword.Text = "";

            // Set focus to username field
            txtUsername.Focus();

            // Center panel
            CenterLoginPanel();
        }

        /// <summary>
        /// Login button click - authenticate user
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Get username and password directly
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Basic validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Please enter your username", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter your password", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                Cursor = Cursors.WaitCursor;
                button1.Enabled = false;
                button1.Text = "Logging in...";

                var loginData = new { Username = username, Password = password };
                var response = await _client.PostAsJsonAsync("api/auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>(options);

                    if (result != null)
                    {
                        // Store user session data (only using existing properties)
                        UserSession.Username = username;
                        UserSession.Email = result.email ?? "";
                        UserSession.Role = result.role ?? "";

                        // IsLoggedIn will automatically be true because Username is set

                        string role = result.role ?? "Unknown";
                        string fullName = result.fullName ?? username;

                        if (result.success || !string.IsNullOrEmpty(result.role))
                        {
                            MessageBox.Show($"Welcome {fullName}!\nRole: {role}", "Login Successful",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();

                            // Navigate based on role
                            if (role.Equals("Candidate", StringComparison.OrdinalIgnoreCase))
                            {
                                new frmCandidateApplication().Show();
                            }
                            else if (role.Equals("Voter", StringComparison.OrdinalIgnoreCase))
                            {
                                new frmVoterDashboard(1, username).Show();
                            }
                            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                new frmAdminDashboard().Show();
                            }
                            else
                            {
                                MessageBox.Show($"Unknown role: {role}\nPlease contact administrator.",
                                    "Role Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show(result.message ?? "Login failed. Please check your credentials.",
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid response from server", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    MessageBox.Show($"{errorMessage}\n\nStatus: {response.StatusCode}", "Login Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Cannot connect to server!\n\nError: {ex.Message}\n\nPlease check:\n1. Server is running\n2. Internet connection\n3. API endpoint is correct",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
            finally
            {
                Cursor = Cursors.Default;
                button1.Enabled = true;
                button1.Text = "Login";
            }
        }

        /// <summary>
        /// Register link click - open registration form
        /// </summary>
        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new frmRegister().Show();
        }

        // Empty event handlers (required by Designer)
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            // No implementation needed
        }

        private void lblPassword_Click(object sender, EventArgs e)
        {
            // No implementation needed
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // No implementation needed
        }
    }
}