using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Election.UI;

namespace Election.UI.Forms
{
    public partial class frmCandidateApplication : Form
    {
        private readonly HttpClient _client;
        private string _manifestoFilePath = "";
        private string _photoFilePath = "";

        // Ethiopian regions for validation
        private readonly string[] _ethiopianRegions = {
            "Addis Ababa", "Afar", "Amhara", "Benishangul-Gumuz", "Dire Dawa",
            "Gambela", "Harari", "Oromia", "Sidama", "Somali",
            "Southern Nations, Nationalities, and Peoples' Region", "SNNPR", "Tigray"
        };

        public frmCandidateApplication()
        {
            InitializeComponent();

            // 🔥 FIXED: Changed to DockStyle.Fill for proper resizing
            pnlMain.Dock = DockStyle.Fill;

            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7208"),
                Timeout = TimeSpan.FromMinutes(5)
            };

            // Show welcome message
            if (!string.IsNullOrEmpty(UserSession.Username))
            {
                lblSystemTitle.Text = $"ETH Election System | Welcome, {UserSession.Username}";
            }

            // Add header event handlers
            lnkHome.LinkClicked += LnkHome_LinkClicked;
            lnkMyProfile.LinkClicked += LnkMyProfile_LinkClicked;
            btnLogout.Click += BtnLogout_Click;

            // Existing event handlers
            btnUploadFile.Click += BtnUploadFile_Click;
            btnChooseImage.Click += BtnChooseImage_Click;
            btnSubmit.Click += BtnSubmit_Click;

            // 🔥 ADDED: Auto-fill email from UserSession
            if (!string.IsNullOrEmpty(UserSession.Email))
            {
                textEmail.Text = UserSession.Email;
                textEmail.ReadOnly = true;
                textEmail.BackColor = System.Drawing.Color.LightGray;
            }

            // 🔥 ADDED: Auto-complete for region textbox (optional)
            InitializeRegionAutoComplete();
        }

        // 🔥 ADDED: Auto-complete for region textbox (helps user type correct regions)
        private void InitializeRegionAutoComplete()
        {
            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(_ethiopianRegions);
            textRegion.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textRegion.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textRegion.AutoCompleteCustomSource = autoComplete;
        }

        // ============ HEADER CONTROLS (UNCHANGED) ============

        // 1. HOME LINKLABEL - Clear form
        private void LnkHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClearForm();
            MessageBox.Show("Form cleared. Ready for new application.", "Home");
        }

        // 2. MY PROFILE LINKLABEL - Open Profile Form
        private void LnkMyProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserSession.Email))
            {
                MessageBox.Show("Please login first.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Open the profile form
            frmCandidateProfile profileForm = new frmCandidateProfile(UserSession.Email);
            profileForm.ShowDialog();
        }

        // 3. LOGOUT BUTTON - Return to login
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UserSession.Clear();
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
                this.Close();
            }
        }

        // ============ FILE UPLOAD METHODS (UNCHANGED) ============

        private void BtnUploadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "PDF Files|*.pdf|Word Documents|*.doc;*.docx|Text Files|*.txt|All Files|*.*";
                dlg.Title = "Select Manifesto File";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _manifestoFilePath = dlg.FileName;
                    var fileInfo = new FileInfo(_manifestoFilePath);

                    txtManifesto.Text = $"📄 File: {Path.GetFileName(_manifestoFilePath)}\n" +
                                       $"📏 Size: {(fileInfo.Length / 1024):0} KB\n" +
                                       $"📅 Type: {Path.GetExtension(_manifestoFilePath).ToUpper()}";
                    txtManifesto.ForeColor = System.Drawing.Color.DarkGreen;
                }
            }
        }

        private void BtnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                dlg.Title = "Select Photo";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _photoFilePath = dlg.FileName;

                    try
                    {
                        picPhoto.Image = System.Drawing.Image.FromFile(_photoFilePath);
                        picPhoto.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch
                    {
                        MessageBox.Show("Cannot load image", "Error");
                        _photoFilePath = "";
                        picPhoto.Image = null;
                    }
                }
            }
        }

        // ============ UPDATED VALIDATION AND SUBMIT ============

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            // 🔥 UPDATED: Comprehensive validation before submission
            if (!ValidateForm())
                return;

            try
            {
                btnSubmit.Enabled = false;
                btnSubmit.Text = "Uploading...";
                Cursor = Cursors.WaitCursor;

                // Create form data
                using var formData = new MultipartFormDataContent();

                // Add text fields
                formData.Add(new StringContent(txtFullName.Text.Trim()), "FullName");
                formData.Add(new StringContent(txtAge.Text.Trim()), "Age");
                formData.Add(new StringContent(textRegion.Text.Trim()), "Region");
                formData.Add(new StringContent(textparty.Text.Trim()), "PartyAffiliation");
                formData.Add(new StringContent(textEmail.Text.Trim()), "Email");
                formData.Add(new StringContent(textPhone.Text.Trim()), "Phone");

                // Add manifesto file
                var manifestoBytes = File.ReadAllBytes(_manifestoFilePath);
                formData.Add(new ByteArrayContent(manifestoBytes), "ManifestoFile",
                    Path.GetFileName(_manifestoFilePath));

                // Add photo file
                var photoBytes = File.ReadAllBytes(_photoFilePath);
                formData.Add(new ByteArrayContent(photoBytes), "PhotoFile",
                    Path.GetFileName(_photoFilePath));

                // Send request
                var response = await _client.PostAsync("api/candidate/submit", formData);

                // Get response as string
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse response
                    var result = JsonSerializer.Deserialize<JsonElement>(responseString);

                    // Check for 'success'
                    bool isSuccess = false;
                    string message = "";

                    if (result.TryGetProperty("success", out var successLower))
                        isSuccess = successLower.GetBoolean();
                    else if (result.TryGetProperty("Success", out var successUpper))
                        isSuccess = successUpper.GetBoolean();

                    if (result.TryGetProperty("message", out var msgLower))
                        message = msgLower.GetString();
                    else if (result.TryGetProperty("Message", out var msgUpper))
                        message = msgUpper.GetString();

                    if (isSuccess)
                    {
                        MessageBox.Show("✅ " + message, "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("❌ " + message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Show the actual error from API
                    MessageBox.Show($"❌ HTTP Error ({response.StatusCode}):\n{responseString}",
                        "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error: {ex.Message}", "Application Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Submit Application";
                Cursor = Cursors.Default;
            }
        }

        // 🔥 UPDATED: Comprehensive Validation Method
        private bool ValidateForm()
        {
            // 1. Full Name Validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowValidationError("Full Name is required", txtFullName);
                return false;
            }

            var nameParts = txtFullName.Text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length < 2)
            {
                ShowValidationError("Please enter both first name and last name", txtFullName);
                return false;
            }

            // 2. Age Validation (Candidate must be 21+)
            if (!int.TryParse(txtAge.Text, out int age))
            {
                ShowValidationError("Please enter a valid age number", txtAge);
                return false;
            }

            if (age < 21) // 🔥 CHANGED: Candidates must be 21+ (not 18+)
            {
                ShowValidationError("Candidates must be 21 years or older", txtAge);
                return false;
            }

            if (age > 120)
            {
                ShowValidationError("Please enter a valid age (1-120)", txtAge);
                return false;
            }

            // 3. Region Validation (Must be Ethiopian region)
            if (string.IsNullOrWhiteSpace(textRegion.Text))
            {
                ShowValidationError("Region is required", textRegion);
                return false;
            }

            // 🔥 CHECK: Region must be a valid Ethiopian region
            if (!IsValidEthiopianRegion(textRegion.Text.Trim()))
            {
                ShowValidationError($"Please enter a valid Ethiopian region.\n\nValid regions: {string.Join(", ", _ethiopianRegions)}", textRegion);
                return false;
            }

            // 4. Party Affiliation Validation
            if (string.IsNullOrWhiteSpace(textparty.Text))
            {
                ShowValidationError("Party Affiliation is required", textparty);
                return false;
            }

            if (textparty.Text.Trim().Length < 2)
            {
                ShowValidationError("Party name must be at least 2 characters", textparty);
                return false;
            }

            // 5. Email Validation
            if (string.IsNullOrWhiteSpace(textEmail.Text))
            {
                ShowValidationError("Email is required", textEmail);
                return false;
            }

            if (!IsValidEmail(textEmail.Text.Trim()))
            {
                ShowValidationError("Please enter a valid email address", textEmail);
                return false;
            }

            // 6. Phone Number Validation
            if (string.IsNullOrWhiteSpace(textPhone.Text))
            {
                ShowValidationError("Phone number is required", textPhone);
                return false;
            }

            if (!IsValidEthiopianPhone(textPhone.Text.Trim()))
            {
                ShowValidationError("Please enter a valid Ethiopian phone number\n\nFormat: 09XXXXXXXX or +2519XXXXXXXX", textPhone);
                return false;
            }

            // 7. Manifesto File Validation
            if (string.IsNullOrEmpty(_manifestoFilePath))
            {
                MessageBox.Show("Please upload manifesto file", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnUploadFile.Focus();
                return false;
            }

            var manifestoFileInfo = new FileInfo(_manifestoFilePath);
            if (manifestoFileInfo.Length > 10 * 1024 * 1024) // 10MB limit
            {
                MessageBox.Show("Manifesto file size must be less than 10MB", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 8. Photo File Validation
            if (string.IsNullOrEmpty(_photoFilePath))
            {
                MessageBox.Show("Please upload photo", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChooseImage.Focus();
                return false;
            }

            var photoFileInfo = new FileInfo(_photoFilePath);
            if (photoFileInfo.Length > 5 * 1024 * 1024) // 5MB limit
            {
                MessageBox.Show("Photo file size must be less than 5MB", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if photo is valid image
            try
            {
                using (var img = System.Drawing.Image.FromFile(_photoFilePath))
                {
                    // Additional image validation
                }
            }
            catch
            {
                MessageBox.Show("Please upload a valid image file", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true; // All validations passed
        }

        // 🔥 ADDED: Ethiopian region validation method
        private bool IsValidEthiopianRegion(string regionInput)
        {
            // Trim and capitalize first letter of each word for better matching
            string normalizedInput = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(regionInput.ToLower());

            foreach (var validRegion in _ethiopianRegions)
            {
                // Case-insensitive comparison
                if (string.Equals(regionInput, validRegion, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                // Also check for "SNNPR" vs full name
                if (validRegion.Equals("SNNPR", StringComparison.OrdinalIgnoreCase) &&
                    regionInput.Contains("Southern Nations", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // 🔥 ADDED: Helper method for validation errors
        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();

            if (control is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        // 🔥 ADDED: Email validation
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

        // 🔥 ADDED: Ethiopian phone number validation
        private bool IsValidEthiopianPhone(string phone)
        {
            // Remove all non-digit characters
            string digitsOnly = Regex.Replace(phone, @"[^\d]", "");

            // Ethiopian phone patterns:
            // 0912345678 (10 digits starting with 09)
            // 251912345678 (12 digits starting with 2519)
            // +251912345678 (with country code)

            if (digitsOnly.Length == 10 && digitsOnly.StartsWith("09"))
                return true;

            if (digitsOnly.Length == 12 && digitsOnly.StartsWith("2519"))
                return true;

            return false;
        }

        // ============ EXISTING METHODS ============

        private void ClearForm()
        {
            txtFullName.Clear();
            txtAge.Clear();
            textRegion.Clear();
            textparty.Clear();
            // Don't clear email if it's from UserSession
            if (!textEmail.ReadOnly)
                textEmail.Clear();
            textPhone.Clear();
            txtManifesto.Clear();
            picPhoto.Image = null;
            _manifestoFilePath = "";
            _photoFilePath = "";
        }

        private void frmCandidateApplication_Load(object sender, EventArgs e)
        {
            // Existing load code
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            // Existing paint code
        }
    }
}