using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Election.UI.Forms
{
    public partial class frmCandidateProfile : Form
    {
        private readonly HttpClient _client;
        private readonly string _email;
        private int _candidateId;

        public frmCandidateProfile(string email)
        {
            InitializeComponent();
            _email = email;
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7208")
            };
        }

        private async void frmCandidateProfile_Load(object sender, EventArgs e)
        {
            await LoadCandidateData();
        }

        private async Task LoadCandidateData()
        {
            try
            {
                // Fetch candidate data by email
                var response = await _client.GetAsync($"api/candidate/byemail/{_email}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // ✅ FIX ADDED HERE: Case-insensitive JSON parsing
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true  // THIS LINE FIXES THE ISSUE
                    };

                    var result = JsonSerializer.Deserialize<JsonElement>(jsonString, options);

                    if (result.TryGetProperty("success", out var success) && success.GetBoolean())
                    {
                        // Store candidate ID
                        if (result.TryGetProperty("id", out var id))
                        {
                            _candidateId = id.GetInt32();
                        }

                        // Fill form fields
                        if (result.TryGetProperty("fullName", out var fullName))
                            txtFullName.Text = fullName.ToString();

                        if (result.TryGetProperty("age", out var age))
                            txtAge.Text = age.ToString();

                        if (result.TryGetProperty("region", out var region))
                            txtRegion.Text = region.ToString();

                        if (result.TryGetProperty("partyAffiliation", out var party))
                            txtParty.Text = party.ToString();

                        if (result.TryGetProperty("email", out var email))
                            txtEmail.Text = email.ToString();

                        if (result.TryGetProperty("phone", out var phone))
                            txtPhone.Text = phone.ToString();

                        if (result.TryGetProperty("status", out var status))
                            lblStatusValue.Text = status.ToString();

                        if (result.TryGetProperty("applicationDate", out var date))
                            lblDateValue.Text = date.ToString();
                    }
                    else
                    {
                        MessageBox.Show("No candidate application found.", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("No candidate application found.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (!int.TryParse(txtAge.Text, out int age) || age < 18)
                {
                    MessageBox.Show("Age must be 18 or older.", "Validation Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Full Name is required.", "Validation Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRegion.Text))
                {
                    MessageBox.Show("Region is required.", "Validation Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtParty.Text))
                {
                    MessageBox.Show("Party/Affiliation is required.", "Validation Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPhone.Text))
                {
                    MessageBox.Show("Phone is required.", "Validation Error");
                    return;
                }

                // Prepare update data
                var updateData = new
                {
                    Id = _candidateId,
                    FullName = txtFullName.Text.Trim(),
                    Age = age,
                    Region = txtRegion.Text.Trim(),
                    PartyAffiliation = txtParty.Text.Trim(),
                    Phone = txtPhone.Text.Trim()
                };

                // Send update request
                var response = await _client.PutAsJsonAsync("api/candidate/update", updateData);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // ✅ ALSO FIXED HERE: Case-insensitive for update response
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<JsonElement>(jsonString, options);

                    bool success = result.TryGetProperty("success", out var successProp) && successProp.GetBoolean();

                    if (success)
                    {
                        MessageBox.Show("Profile updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string message = result.TryGetProperty("message", out var msg) ? msg.ToString() : "Update failed";
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Update failed. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete your candidate application?\n\nThis action cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var response = await _client.DeleteAsync($"api/candidate/delete/{_candidateId}");

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Candidate application deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Delete failed. Please try again.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}