using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;
using Election.UI;  // ADD THIS

namespace Election.UI.Forms
{
    public partial class frmLogin : Form
    {
        private readonly HttpClient _client = new HttpClient();

        // Update LoginResponse class
        public class LoginResponse
        {
            public string fullName { get; set; } = "";
            public string role { get; set; } = "";
            public bool isApproved { get; set; }
            public string email { get; set; } = "";  // ADD THIS
        }

        public frmLogin()
        {
            InitializeComponent();
            _client.BaseAddress = new Uri("https://localhost:7208");
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var loginData = new { Username = txtUsername.Text, Password = txtPassword.Text };
                var response = await _client.PostAsJsonAsync("api/auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = false };
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>(options);

                    // STORE USER SESSION
                    UserSession.Username = txtUsername.Text;
                    UserSession.Email = result?.email ?? "";
                    UserSession.Role = result?.role ?? "";

                    string role = result?.role ?? "Unknown";
                    string fullName = result?.fullName ?? "Unknown";

                    MessageBox.Show($"Welcome {fullName}!\nRole: {role}", "Login Successful");
                    this.Hide();

                    if (role == "Candidate")
                    {
                        new frmCandidateApplication().Show();
                    }
                    else if (role == "Voter")
                    {
                        new frmVoterDashboard().Show();
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

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new frmRegister().Show();
        }
    }
}