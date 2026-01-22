// Location: Election.UI/Forms/FrmCandidateProfile.cs
#nullable enable
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Election.UI.Forms
{
    public partial class FrmCandidateProfile : Form
    {
        private readonly HttpClient _client;
        private readonly string _email;
        private int _candidateId;
        private CandidateDto? _currentCandidate;

        public FrmCandidateProfile(string email)
        {
            InitializeComponent();
            _email = email;
            _client = new() { BaseAddress = new("https://localhost:7208") };

            // Set form title
            this.Text = "My Candidate Profile - View, Update & Delete";
        }

        private async void FrmCandidateProfile_Load(object? sender, EventArgs e) => await LoadCandidateData();

        private async Task LoadCandidateData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // ✅ FIXED: Use the new endpoint that returns correct format
                var response = await _client.GetAsync($"api/candidate/profile/{_email}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    // ✅ Parse the API response wrapper
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<CandidateDto>>(jsonString, options);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        _candidateId = apiResponse.Data.Id;
                        _currentCandidate = apiResponse.Data;
                        UpdateUI(apiResponse.Data);
                    }
                    else
                    {
                        MessageBox.Show(
                            apiResponse?.Message ?? "No candidate application found for your account.",
                            "No Application Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show(
                        "Could not load your candidate profile.\n\nYou may not have submitted an application yet.",
                        "Profile Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Error loading profile: {errorContent}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading profile data:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateUI(CandidateDto c)
        {
            // ✅ Fill all fields
            if (txtFullName != null) txtFullName.Text = c.Name;
            if (txtAge != null) txtAge.Text = c.Age.ToString();
            if (txtRegion != null) txtRegion.Text = c.Region;
            if (txtParty != null) txtParty.Text = c.Party;
            if (txtEmail != null) txtEmail.Text = c.Email;
            if (txtPhone != null) txtPhone.Text = c.Phone;

            // ✅ Display status with color coding
            if (lblStatusValue != null)
            {
                lblStatusValue.Text = c.Status;
                lblStatusValue.ForeColor = c.Status switch
                {
                    "Approved" => Color.FromArgb(40, 167, 69),    // Green
                    "Rejected" => Color.FromArgb(220, 53, 69),     // Red
                    _ => Color.FromArgb(255, 193, 7)               // Yellow/Orange for Pending
                };
            }

            // ✅ Display application date
            if (lblDateValue != null)
            {
                lblDateValue.Text = c.ApplicationDate.ToString("MMM dd, yyyy");
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            // ✅ Validate inputs
            if (!ValidateInputs())
                return;

            // ✅ Confirm update
            var confirmResult = MessageBox.Show(
                "Are you sure you want to update your candidate profile?\n\n" +
                "This will update your application information.",
                "Confirm Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                btnUpdate.Enabled = false;
                btnUpdate.Text = "Updating...";

                // ✅ FIXED: Prepare update data matching UpdateCandidateRequest format
                var updateData = new
                {
                    Id = _candidateId,
                    FullName = txtFullName?.Text?.Trim(),
                    Age = int.Parse(txtAge?.Text?.Trim() ?? "0"),
                    Region = txtRegion?.Text?.Trim(),
                    PartyAffiliation = txtParty?.Text?.Trim(), // Note: API expects PartyAffiliation not Party
                    Phone = txtPhone?.Text?.Trim()
                };

                // ✅ FIXED: Use correct endpoint
                var resp = await _client.PutAsJsonAsync($"api/candidate/update", updateData);

                if (resp.IsSuccessStatusCode)
                {
                    var responseString = await resp.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, options);

                    if (apiResponse?.Success == true)
                    {
                        MessageBox.Show(
                            "✅ Profile updated successfully!\n\n" +
                            "Your candidate application has been updated.",
                            "Update Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Reload data to show updated info
                        await LoadCandidateData();
                    }
                    else
                    {
                        MessageBox.Show(
                            apiResponse?.Message ?? "Update failed",
                            "Update Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var errorContent = await resp.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Failed to update profile.\n\n{errorContent}",
                        "Update Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error updating profile:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnUpdate.Enabled = true;
                btnUpdate.Text = "Update";
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            // ✅ Strong confirmation for delete
            var confirmResult = MessageBox.Show(
                "⚠️ WARNING: This action cannot be undone!\n\n" +
                "Are you sure you want to DELETE your candidate application?\n\n" +
                "This will permanently remove:\n" +
                "• Your candidate profile\n" +
                "• Your application data\n" +
                "• Your approval status\n\n" +
                "You will need to reapply if you want to be a candidate again.",
                "Confirm Delete - Permanent Action",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
                return;

            // ✅ Double confirmation
            var doubleConfirm = MessageBox.Show(
                "This is your FINAL confirmation.\n\n" +
                "Click YES to permanently delete your application.\n" +
                "Click NO to cancel.",
                "Final Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation);

            if (doubleConfirm != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                btnDelete.Enabled = false;
                btnDelete.Text = "Deleting...";

                // ✅ FIXED: Use correct endpoint
                var resp = await _client.DeleteAsync($"api/candidate/delete/{_candidateId}");

                if (resp.IsSuccessStatusCode)
                {
                    var responseString = await resp.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, options);

                    if (apiResponse?.Success == true)
                    {
                        MessageBox.Show(
                            "✅ Your candidate application has been deleted successfully.\n\n" +
                            "You can submit a new application anytime.",
                            "Application Deleted",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(
                            apiResponse?.Message ?? "Delete failed",
                            "Delete Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var errorContent = await resp.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Failed to delete application.\n\n{errorContent}",
                        "Delete Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error deleting application:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnDelete.Enabled = true;
                btnDelete.Text = "Delete";
            }
        }

        private bool ValidateInputs()
        {
            // ✅ Validate Full Name
            if (string.IsNullOrWhiteSpace(txtFullName?.Text))
            {
                MessageBox.Show(
                    "Full Name is required.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtFullName?.Focus();
                return false;
            }

            // ✅ Validate Age
            if (!int.TryParse(txtAge?.Text, out int age) || age < 21 || age > 100)
            {
                MessageBox.Show(
                    "Please enter a valid age (21-100).",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtAge?.Focus();
                return false;
            }

            // ✅ Validate Region
            if (string.IsNullOrWhiteSpace(txtRegion?.Text))
            {
                MessageBox.Show(
                    "Region is required.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtRegion?.Focus();
                return false;
            }

            // ✅ Validate Party
            if (string.IsNullOrWhiteSpace(txtParty?.Text))
            {
                MessageBox.Show(
                    "Party/Affiliation is required.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtParty?.Focus();
                return false;
            }

            // ✅ Validate Phone
            if (string.IsNullOrWhiteSpace(txtPhone?.Text))
            {
                MessageBox.Show(
                    "Phone number is required.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtPhone?.Focus();
                return false;
            }

            return true;
        }

        private void BtnClose_Click(object? sender, EventArgs e) => this.Close();

        // ✅ DTO for API response parsing
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public T? Data { get; set; }
            public string? Message { get; set; }
        }

        // ✅ DTO for candidate data (matches API response from /api/candidate/profile/{email})
        public class CandidateDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public string Region { get; set; } = "";
            public string Party { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Status { get; set; } = "Pending";
            public DateTime ApplicationDate { get; set; }
            public bool IsApproved { get; set; }
            public bool IsRejected { get; set; }
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            // This method can stay empty
        }
    }
}