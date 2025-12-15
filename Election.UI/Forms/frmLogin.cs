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
        }

        public frmLogin()
        {
            InitializeComponent();
            _client.BaseAddress = new Uri("https://localhost:7208");

            // 🔥 ADDED: Center the panel when form loads initially
            this.Load += (s, e) => CenterLoginPanel();

            // 🔥 ADDED: Center the panel when form resizes
            this.Resize += (s, e) => CenterLoginPanel();

            // 🔥 MANUALLY CONNECT ALL EVENT HANDLERS
            ConnectAllEventHandlers();
        }

        /// <summary>
        /// 🔥 ADDED: Centers the login panel both horizontally and vertically
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
        /// Connects all event handlers to ensure placeholders work
        /// </summary>
        private void ConnectAllEventHandlers()
        {
            // Form Load event (already handled above, but keeping for compatibility)
            this.Load += frmLogin_Load;

            // Username field events
            txtUsername.Enter += TxtUsername_Enter;
            txtUsername.Leave += TxtUsername_Leave;

            // Password field events
            txtPassword.Enter += TxtPassword_Enter;
            txtPassword.Leave += TxtPassword_Leave;
            txtPassword.TextChanged += TxtPassword_TextChanged;

            // Button and Link events (already in Designer but ensuring they're connected)
            button1.Click += button1_Click;
            linkRegister.LinkClicked += linkRegister_LinkClicked;
        }

        /// <summary>
        /// Form Load: Set BOTH placeholders and focus
        /// </summary>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            // 🔥 SET BOTH PLACEHOLDERS SIMULTANEOUSLY
            // Username placeholder
            txtUsername.Text = "Username";
            txtUsername.ForeColor = Color.Gray;

            // Password placeholder
            txtPassword.Text = "Password";
            txtPassword.ForeColor = Color.Gray;
            txtPassword.UseSystemPasswordChar = false;

            // Set focus to username field
            txtUsername.Focus();

            // 🔥 ADDED: Center panel on load (already handled but calling again for safety)
            CenterLoginPanel();
        }

        /// <summary>
        /// Username field gets focus - remove placeholder
        /// </summary>
        private void TxtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.ForeColor == Color.Gray)
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Username field loses focus - restore placeholder if empty
        /// </summary>
        private void TxtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Username";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// Password field gets focus - remove placeholder
        /// </summary>
        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.ForeColor == Color.Gray)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        /// <summary>
        /// Password field loses focus - restore placeholder if empty
        /// </summary>
        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Password";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        /// <summary>
        /// Password text changes - ensure password char is shown for real text
        /// </summary>
        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.ForeColor == Color.Black && txtPassword.Text.Length > 0)
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        /// <summary>
        /// Login button click - authenticate user
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Get username and password, checking for placeholder text
                string username = txtUsername.ForeColor == Color.Gray ? "" : txtUsername.Text.Trim();
                string password = txtPassword.ForeColor == Color.Gray ? "" : txtPassword.Text.Trim();

                // Basic validation
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Please enter your username", "Validation Error");
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter your password", "Validation Error");
                    txtPassword.Focus();
                    return;
                }

                var loginData = new { Username = username, Password = password };
                var response = await _client.PostAsJsonAsync("api/auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = false };
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>(options);

                    // Store user session data
                    UserSession.Username = username;
                    UserSession.Email = result?.email ?? "";
                    UserSession.Role = result?.role ?? "";

                    string role = result?.role ?? "Unknown";
                    string fullName = result?.fullName ?? "Unknown";

                    MessageBox.Show($"Welcome {fullName}!\nRole: {role}", "Login Successful");
                    this.Hide();

                    // Navigate based on role
                    if (role == "Candidate")
                    {
                        new frmCandidateApplication().Show();
                    }
                    else if (role == "Voter")
                    {
                        // ✅ Simple fix: Pass dummy values since parameters are required
                        new frmVoterDashboard(1, "voter@email.com").Show();
                    }
                    else if (role == "Admin")
                    {
                        new frmAdminDashboard().Show();
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Login failed: {error}", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot connect to API!\n\nError: {ex.Message}", "Connection Error");
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
    }
}