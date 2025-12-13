using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace Election.UI.Forms
{
    public partial class frmRegister : Form
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly List<string> _ethiopianRegions = new List<string>
        {
            "Addis Ababa", "Afar", "Amhara", "Benishangul-Gumuz", "Dire Dawa",
            "Gambela", "Harari", "Oromia", "Sidama", "Somali",
            "Southern Nations, Nationalities, and Peoples' Region", "Tigray"
        };

        public frmRegister()
        {
            InitializeComponent();
            // SAME PORT AS frmLogin
            _client.BaseAddress = new Uri("https://localhost:7208");

            // Set defaults
            rbVoter.Checked = true;
            numAge.Value = 18;

            // Initialize ComboBox with Ethiopian regions
            InitializeRegionComboBox();
        }

        private void InitializeRegionComboBox()
        {
            // Clear any existing items
            cmbRegion.Items.Clear();

            // Add Ethiopian regions to ComboBox
            foreach (var region in _ethiopianRegions)
            {
                cmbRegion.Items.Add(region);
            }

            // Set ComboBox to force selection from dropdown list
            cmbRegion.DropDownStyle = ComboBoxStyle.DropDownList;

            // Sort regions alphabetically
            cmbRegion.Sorted = true;

            // Set first region as default selection
            if (cmbRegion.Items.Count > 0)
            {
                cmbRegion.SelectedIndex = 0;
            }
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate all inputs before making API call
            if (!ValidateInput())
            {
                return; // Stop if validation fails
            }

            try
            {
                // Get selected role
                string role = rbVoter.Checked ? "Voter" : "Candidate";

                var userData = new
                {
                    FullName = txtFullName.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text,
                    Email = txtEmail.Text.Trim(),
                    Age = (int)numAge.Value,
                    Region = cmbRegion.SelectedItem.ToString(),
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

        /// <summary>
        /// Validates all form inputs before submission
        /// </summary>
        /// <returns>True if all validations pass, False otherwise</returns>
        private bool ValidateInput()
        {
            // 1. Required Fields Validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowValidationError("Full Name is required", txtFullName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowValidationError("Username is required", txtUsername);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowValidationError("Email is required", txtEmail);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowValidationError("Password is required", txtPassword);
                return false;
            }

            // 2. Email Format Validation
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                ShowValidationError("Please enter a valid email address", txtEmail);
                return false;
            }

            // 3. Password Strength Validation (minimum 6 characters)
            if (txtPassword.Text.Length < 6)
            {
                ShowValidationError("Password must be at least 6 characters", txtPassword);
                return false;
            }

            // 4. Age Validation based on Role
            int age = (int)numAge.Value;
            if (rbVoter.Checked && age < 18)
            {
                ShowValidationError("Voters must be 18 years or older", numAge);
                return false;
            }

            if (rbCandidate.Checked && age < 21)
            {
                ShowValidationError("Candidates must be 21 years or older", numAge);
                return false;
            }

            // 5. Region Validation - Must select from ComboBox
            if (cmbRegion.SelectedIndex == -1)
            {
                MessageBox.Show("Please select your region from the dropdown list", "Validation Error");
                cmbRegion.Focus();
                cmbRegion.DroppedDown = true; // Open dropdown for user
                return false;
            }

            // 6. Username Format Validation (no spaces allowed)
            if (txtUsername.Text.Contains(" "))
            {
                ShowValidationError("Username cannot contain spaces", txtUsername);
                return false;
            }

            // 7. Username Length Validation
            if (txtUsername.Text.Trim().Length < 3)
            {
                ShowValidationError("Username must be at least 3 characters", txtUsername);
                return false;
            }

            // 8. Full Name Validation (should have at least first and last name)
            var nameParts = txtFullName.Text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length < 2)
            {
                ShowValidationError("Please enter both first name and last name", txtFullName);
                return false;
            }

            // All validations passed
            return true;
        }

        /// <summary>
        /// Shows validation error message and focuses on the problematic control
        /// </summary>
        /// <param name="message">Error message to display</param>
        /// <param name="control">Control to focus on</param>
        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Validation Error");
            control.Focus();

            // Special handling for ComboBox
            if (control is ComboBox comboBox)
            {
                comboBox.DroppedDown = true;
            }

            // Special handling for TextBox - select all text
            if (control is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        /// <summary>
        /// Validates email format using MailAddress class
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if valid email format, False otherwise</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adjusts minimum age when Voter role is selected
        /// </summary>
        private void rbVoter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbVoter.Checked && numAge.Value < 18)
            {
                numAge.Value = 18;
            }
        }

        /// <summary>
        /// Adjusts minimum age when Candidate role is selected
        /// </summary>
        private void rbCandidate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCandidate.Checked && numAge.Value < 21)
            {
                numAge.Value = 21;
            }
        }

        /// <summary>
        /// Cancel registration and return to login form
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Close();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Original panel paint code remains unchanged
        }
    }
}