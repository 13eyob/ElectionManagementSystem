using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace Election.UI.Forms
{
    public partial class frmRegister : Form
    {
        private readonly HttpClient _client = new HttpClient();

        public frmRegister()
        {
            InitializeComponent();
            // SAME PORT AS frmLogin
            _client.BaseAddress = new Uri("https://localhost:7208");

            // Set defaults
            rbVoter.Checked = true;
            numAge.Value = 18;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get selected role
                string role = rbVoter.Checked ? "Voter" : "Candidate";

                var userData = new
                {
                    FullName = txtFullName.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    Age = (int)numAge.Value,
                    Region = textBox3.Text,
                    Role = role
                };

                var response = await _client.PostAsJsonAsync("api/auth/register", userData);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Registration successful!\n\n" +
                                   $"Name: {userData.FullName}\n" +
                                   $"Username: {userData.Username}\n" +
                                   $"Role: {userData.Role}",
                                   "Success");

                    new frmLogin().Show();
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Registration failed: {error}", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot connect to API!\n\nError: {ex.Message}", "Error");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Close();
        }
    }
}