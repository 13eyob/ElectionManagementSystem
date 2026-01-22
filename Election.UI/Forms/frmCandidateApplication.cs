#nullable enable
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;  // ✅ Added for ReadFromJsonAsync
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Election.UI;

namespace Election.UI.Forms
{
    public partial class FrmCandidateApplication : Form
    {
        private readonly HttpClient _client;
        private string _manifestoFilePath = "";
        private string _photoFilePath = "";
        private bool _isSubmitting = false;

        private static readonly string[] EthiopianRegions =
        [
            "Addis Ababa", "Afar", "Amhara", "Benishangul-Gumuz", "Dire Dawa",
            "Gambela", "Harari", "Oromia", "Sidama", "Somali",
            "Southern Nations, Nationalities, and Peoples' Region", "SNNPR", "Tigray"
        ];

        private static readonly Regex _digitsOnlyRegex = new(@"[^\d]");

        public FrmCandidateApplication(string? fullName = null)
        {
            InitializeComponent();
            _client = new() { BaseAddress = new("https://localhost:7208"), Timeout = TimeSpan.FromMinutes(5) };
            SetupForm(fullName);

            // ✅ Load existing application data if available
            this.Load += FrmCandidateApplication_Load;
        }

        private async void FrmCandidateApplication_Load(object? sender, EventArgs e)
        {
            // ✅ Auto-load existing candidate application if user has already submitted
            await LoadExistingApplicationData();
        }

        private void SetupForm(string? fullName)
        {
            // Enable maximize/minimize functionality
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1024, 768);
            this.SizeGripStyle = SizeGripStyle.Show;

            // Setup title
            if (lblSystemTitle != null)
                lblSystemTitle.Text = $"ETH Election System | Welcome, {UserSession.Username ?? fullName ?? "User"}";

            // Wire up event handlers
            lnkHome.LinkClicked += LnkHome_LinkClicked;
            lnkMyProfile.LinkClicked += LnkMyProfile_LinkClicked;
            btnLogout.Click += BtnLogout_Click;
            btnUploadFile.Click += BtnUploadFile_Click;
            btnChooseImage.Click += BtnChooseImage_Click;
            btnSubmit.Click += BtnSubmit_Click;

            // Handle form resizing for responsive layout
            this.Resize += FrmCandidateApplication_Resize;

            // Set up name field (optional pre-fill from session)
            if (!string.IsNullOrEmpty(fullName))
                txtFullName.Text = fullName;
            else if (!string.IsNullOrEmpty(UserSession.Username))
                txtFullName.Text = UserSession.Username;

            // Set focus to first field
            txtFullName.Focus();
        }

        // ✅ Load existing candidate application data
        private async Task LoadExistingApplicationData()
        {
            try
            {
                // Get email from the form field
                string email = textEmail?.Text?.Trim() ?? "";
                if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
                    return; // No valid email entered yet, skip loading

                // Show loading indicator
                if (lblSystemTitle != null)
                    lblSystemTitle.Text = $"ETH Election System | Loading your application...";

                Cursor = Cursors.WaitCursor;

                // Try to fetch existing candidate application using the CORRECT endpoint
                var response = await _client.GetAsync($"api/candidate/profile/{email}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<ExistingCandidateDto>>(jsonString, options);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        // ✅ Fill form with existing data
                        PopulateFormWithExistingData(apiResponse.Data);

                        // Show info message
                        MessageBox.Show(
                            $"📋 Your existing candidate application has been loaded.\n\n" +
                            $"Status: {apiResponse.Data.Status}\n" +
                            $"Applied: {apiResponse.Data.ApplicationDate:MMM dd, yyyy}\n\n" +
                            "You can view, update, or delete your application using the 'My Profile' link.",
                            "Existing Application Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Change submit button text
                        if (btnSubmit != null)
                        {
                            btnSubmit.Text = "Update Application";
                            btnSubmit.BackColor = Color.FromArgb(255, 152, 0); // Orange for update
                        }
                    }
                }
                // If 404 or other error, it means no application exists yet - that's fine
            }
            catch (HttpRequestException)
            {
                // Network error or API not available - ignore and let user fill form normally
            }
            catch (Exception ex)
            {
                // Log error but don't block the user
                Console.WriteLine($"Error loading existing application: {ex.Message}");
            }
            finally
            {
                Cursor = Cursors.Default;

                // Reset title
                if (lblSystemTitle != null)
                    lblSystemTitle.Text = $"ETH Election System | Welcome, {UserSession.Username ?? "User"}";
            }
        }

        // ✅ Populate form fields with existing candidate data
        private void PopulateFormWithExistingData(ExistingCandidateDto candidate)
        {
            try
            {
                // Fill text fields
                if (txtFullName != null) txtFullName.Text = candidate.FullName;
                if (txtAge != null) txtAge.Text = candidate.Age.ToString();
                if (textparty != null) textparty.Text = candidate.PartyAffiliation;
                if (textPhone != null) textPhone.Text = candidate.Phone;

                // Note: Email and Region are already filled and locked from SetupForm

                // Add note about files
                if (txtManifesto != null)
                {
                    txtManifesto.Text = "📄 Previous manifesto file is on server.\r\n" +
                                       "Upload a new file only if you want to replace it.";
                    txtManifesto.ForeColor = Color.DarkBlue;
                }

                // Show info in title or status area
                if (lblSystemTitle != null)
                {
                    lblSystemTitle.Text = $"ETH Election System | Editing Existing Application (Status: {candidate.Status})";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error populating form: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // ✅ DTO for existing candidate data
        private class ExistingCandidateDto
        {
            public int Id { get; set; }
            public string FullName { get; set; } = "";
            public int Age { get; set; }
            public string Region { get; set; } = "";
            public string PartyAffiliation { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Status { get; set; } = "Pending";
            public DateTime ApplicationDate { get; set; }
            public string ManifestoFilePath { get; set; } = "";
            public string PhotoFilePath { get; set; } = "";
        }

        // ✅ DTO for API response parsing
        private class ApiResponse<T>
        {
            public bool Success { get; set; }
            public T? Data { get; set; }
            public string? Message { get; set; }
        }

        private void FrmCandidateApplication_Resize(object? sender, EventArgs e)
        {
            // Adjust control sizes on form resize for better responsiveness
            AdjustLayoutOnResize();
        }

        private void AdjustLayoutOnResize()
        {
            // Adjust photo box size based on form size
            if (picPhoto != null && this.Width > 1200)
            {
                // Increase photo size for larger windows
                int newSize = Math.Min(200, this.Height / 4);
                if (picPhoto.Parent is TableLayoutPanel table)
                {
                    // Keep photo centered in its cell
                    picPhoto.Size = new Size(newSize, newSize);
                    picPhoto.Anchor = AnchorStyles.None;
                }
            }
        }

        private void LnkHome_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            // ✅ Home link: Refresh/Reset the form
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Do you want to refresh and clear the form?",
                    "Confirm Refresh",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No) return;
            }

            // Clear and refresh the form
            ClearForm();

            // Show brief confirmation
            lblSystemTitle.Text = $"ETH Election System | Welcome, {UserSession.Username ?? "User"} - Form Refreshed";

            // Reset title after 2 seconds
            System.Windows.Forms.Timer resetTimer = new() { Interval = 2000 };
            resetTimer.Tick += (s, args) =>
            {
                lblSystemTitle.Text = $"ETH Election System | Welcome, {UserSession.Username ?? "User"}";
                resetTimer.Stop();
                resetTimer.Dispose();
            };
            resetTimer.Start();
        }

        private async void LnkMyProfile_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            // ✅ My Profile link: Show candidate profile based on email in the form
            string email = textEmail?.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show(
                    "Please enter your email address first to view your profile.",
                    "Email Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                textEmail?.Focus();
                return;
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                MessageBox.Show(
                    "Please enter a valid email address.",
                    "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textEmail?.Focus();
                return;
            }

            try
            {
                // Open candidate profile in a dialog using the email from the form
                using var profileForm = new FrmCandidateProfile(email);
                profileForm.ShowDialog(this);

                // After closing profile, reload existing data if available
                await LoadExistingApplicationData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error opening profile: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UserSession.Clear();
                var loginForm = new FrmLogin();
                loginForm.Show();
                this.Close();
            }
        }

        private void BtnUploadFile_Click(object? sender, EventArgs e)
        {
            try
            {
                using var dlg = new OpenFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.doc, *.docx)|*.doc;*.docx|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    Title = "Select Manifesto File",
                    Multiselect = false,
                    CheckFileExists = true
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _manifestoFilePath = dlg.FileName;

                    // Show file info
                    var fileInfo = new FileInfo(_manifestoFilePath);
                    txtManifesto.Text = $"📄 File: {Path.GetFileName(_manifestoFilePath)}\r\n";
                    txtManifesto.Text += $"📍 Location: {Path.GetDirectoryName(_manifestoFilePath)}\r\n";
                    txtManifesto.Text += $"📊 Size: {(fileInfo.Length / 1024):N0} KB\r\n";
                    txtManifesto.Text += $"📅 Selected: {DateTime.Now:yyyy-MM-dd HH:mm}";

                    txtManifesto.ForeColor = Color.DarkGreen;

                    // Update button text
                    btnUploadFile.Text = "✓ Manifesto Selected";
                    btnUploadFile.BackColor = Color.Green;
                    btnUploadFile.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChooseImage_Click(object? sender, EventArgs e)
        {
            try
            {
                using var dlg = new OpenFileDialog
                {
                    Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp, *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files (*.*)|*.*",
                    Title = "Select Profile Photo",
                    Multiselect = false,
                    CheckFileExists = true
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _photoFilePath = dlg.FileName;

                    try
                    {
                        // Load and display image
                        using var originalImage = Image.FromFile(_photoFilePath);

                        // Create a copy to avoid file locking
                        var imageCopy = new Bitmap(originalImage);
                        picPhoto.Image = imageCopy;
                        picPhoto.SizeMode = PictureBoxSizeMode.Zoom;

                        // Update button appearance
                        btnChooseImage.Text = "✓ Photo Selected";
                        btnChooseImage.BackColor = Color.Green;
                        btnChooseImage.ForeColor = Color.White;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}\nPlease select a valid image file.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _photoFilePath = "";
                        picPhoto.Image = null;

                        // Reset button
                        btnChooseImage.Text = "Choose Image.....";
                        btnChooseImage.BackColor = SystemColors.Highlight;
                        btnChooseImage.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSubmit_Click(object? sender, EventArgs e)
        {
            // Prevent multiple submissions
            if (_isSubmitting) return;

            if (!ValidateForm()) return;

            try
            {
                _isSubmitting = true;

                // Update UI to show submission in progress
                btnSubmit.Enabled = false;
                btnSubmit.Text = "Submitting...";
                btnSubmit.BackColor = Color.Gray;
                Cursor = Cursors.WaitCursor;

                // Create form data
                using var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(txtFullName.Text.Trim()), "FullName");
                formData.Add(new StringContent(txtAge.Text.Trim()), "Age");
                formData.Add(new StringContent(textRegion.Text.Trim()), "Region");
                formData.Add(new StringContent(textparty.Text.Trim()), "PartyAffiliation");
                formData.Add(new StringContent(textEmail.Text.Trim()), "Email");
                formData.Add(new StringContent(textPhone.Text.Trim()), "Phone");

                // Add manifesto file
                var manifestoBytes = await File.ReadAllBytesAsync(_manifestoFilePath);
                formData.Add(new ByteArrayContent(manifestoBytes), "ManifestoFile", Path.GetFileName(_manifestoFilePath));

                // Add photo file
                var photoBytes = await File.ReadAllBytesAsync(_photoFilePath);
                formData.Add(new ByteArrayContent(photoBytes), "PhotoFile", Path.GetFileName(_photoFilePath));

                // Send request
                var response = await _client.PostAsync("api/candidate/submit", formData);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = JsonSerializer.Deserialize<JsonElement>(responseString);

                        // Try to parse response
                        bool success = (result.TryGetProperty("success", out var s) && s.GetBoolean()) ||
                                      (result.TryGetProperty("Success", out var S) && S.GetBoolean());

                        string message = (result.TryGetProperty("message", out var m) ? m.GetString() : "") ??
                                        (result.TryGetProperty("Message", out var M) ? M.GetString() : "") ??
                                        "Application submitted successfully!";

                        if (success)
                        {
                            MessageBox.Show($"✅ {message}", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear form after successful submission
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show($"❌ {message}", "Submission Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (JsonException)
                    {
                        // If response is not JSON, assume success
                        MessageBox.Show("✅ Application submitted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                }
                else
                {
                    MessageBox.Show($"Server Error: {response.StatusCode}\n{responseString}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network Error: {ex.Message}\nPlease check your internet connection and try again.",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"File Error: {ex.Message}\nPlease check if the files are accessible.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restore UI state
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Submit Application";
                btnSubmit.BackColor = SystemColors.Highlight;
                Cursor = Cursors.Default;
                _isSubmitting = false;
            }
        }

        private bool ValidateForm()
        {
            // Validate Full Name
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
                return ShowValidationError("Full Name is required.", txtFullName);

            // Validate Age
            if (!int.TryParse(txtAge.Text, out int age))
                return ShowValidationError("Age must be a valid number.", txtAge);

            if (age < 21)
                return ShowValidationError("Candidate must be at least 21 years old.", txtAge);

            if (age > 100)
                return ShowValidationError("Please enter a valid age (21-100).", txtAge);

            // Validate Region (already locked, but check)
            if (string.IsNullOrWhiteSpace(textRegion.Text))
                return ShowValidationError("Region is required.", textRegion);

            // Validate Party
            if (string.IsNullOrWhiteSpace(textparty.Text))
                return ShowValidationError("Party/Affiliation is required.", textparty);

            // Validate Email (already locked, but check)
            if (string.IsNullOrWhiteSpace(textEmail.Text))
                return ShowValidationError("Email address is required.", textEmail);

            if (!IsValidEmail(textEmail.Text))
                return ShowValidationError("Please enter a valid email address (e.g., example@domain.com).", textEmail);

            // Validate Phone
            if (string.IsNullOrWhiteSpace(textPhone.Text))
                return ShowValidationError("Phone number is required.", textPhone);

            if (!IsValidEthiopianPhone(textPhone.Text))
                return ShowValidationError("Please enter a valid Ethiopian phone number (09xxxxxxxx or +2519xxxxxxxxx).", textPhone);

            // Validate Manifesto File
            if (string.IsNullOrEmpty(_manifestoFilePath))
            {
                MessageBox.Show("Please upload your manifesto file.", "Manifesto Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnUploadFile.Focus();
                return false;
            }

            // Validate Photo File
            if (string.IsNullOrEmpty(_photoFilePath))
            {
                MessageBox.Show("Please upload your profile photo.", "Photo Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChooseImage.Focus();
                return false;
            }

            // Validate file sizes
            try
            {
                var manifestoInfo = new FileInfo(_manifestoFilePath);
                var photoInfo = new FileInfo(_photoFilePath);

                if (manifestoInfo.Length > 10 * 1024 * 1024) // 10 MB
                {
                    MessageBox.Show("Manifesto file is too large. Maximum size is 10MB.",
                        "File Too Large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (photoInfo.Length > 5 * 1024 * 1024) // 5 MB
                {
                    MessageBox.Show("Photo file is too large. Maximum size is 5MB.",
                        "File Too Large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Unable to read file information. Please reselect the files.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static bool IsValidEthiopianRegion(string region)
        {
            string trimmedRegion = region.Trim();

            // Check exact matches
            foreach (var validRegion in EthiopianRegions)
            {
                if (string.Equals(trimmedRegion, validRegion, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            // Check for common variations
            if (trimmedRegion.Contains("Southern Nations", StringComparison.OrdinalIgnoreCase) ||
                trimmedRegion.Contains("SNNPR", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private static bool ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            control.Focus();
            if (control is TextBox textBox)
            {
                textBox.SelectAll();
            }

            return false;
        }

        private static bool IsValidEmail(string email)
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

        private static bool IsValidEthiopianPhone(string phone)
        {
            string digitsOnly = _digitsOnlyRegex.Replace(phone, "");

            // Check for Ethiopian mobile formats
            return (digitsOnly.Length == 10 && digitsOnly.StartsWith("09")) || // Local format
                   (digitsOnly.Length == 12 && digitsOnly.StartsWith("2519")) || // International format
                   (digitsOnly.Length == 13 && digitsOnly.StartsWith("+2519")); // With plus sign
        }

        private void ClearForm()
        {
            // Clear editable text fields
            txtFullName.Clear();
            txtAge.Clear();
            textparty.Clear();
            textPhone.Clear();
            txtManifesto.Clear();
            txtManifesto.ForeColor = SystemColors.WindowText;

            // Clear photo
            if (picPhoto.Image != null)
            {
                picPhoto.Image.Dispose();
                picPhoto.Image = null;
            }

            // Reset buttons
            btnUploadFile.Text = "Upload File......";
            btnUploadFile.BackColor = Color.White;
            btnUploadFile.ForeColor = Color.DeepSkyBlue;

            btnChooseImage.Text = "Choose Image.....";
            btnChooseImage.BackColor = SystemColors.Highlight;
            btnChooseImage.ForeColor = Color.White;

            // Reset file paths
            _manifestoFilePath = "";
            _photoFilePath = "";

            // Set focus to first editable field
            txtFullName.Focus();
        }

        private bool HasUnsavedChanges()
        {
            return !string.IsNullOrWhiteSpace(txtFullName.Text) ||
                   !string.IsNullOrWhiteSpace(txtAge.Text) ||
                   !string.IsNullOrWhiteSpace(textparty.Text) ||
                   !string.IsNullOrWhiteSpace(textPhone.Text) ||
                   !string.IsNullOrEmpty(_manifestoFilePath) ||
                   !string.IsNullOrEmpty(_photoFilePath);
        }

        // Keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1:
                    ShowHelp();
                    return true;

                case Keys.Control | Keys.S:
                    if (btnSubmit.Enabled)
                        BtnSubmit_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.C:
                    if (MessageBox.Show("Clear all form data?", "Confirm Clear",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        ClearForm();
                    return true;

                case Keys.Escape:
                    if (MessageBox.Show("Close application form?", "Confirm Close",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        this.Close();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ShowHelp()
        {
            string helpText = @"CANDIDATE APPLICATION - HELP GUIDE

1. PERSONAL DETAILS:
   • Full Name: Your complete legal name
   • Age: Must be 21 years or older
   • Region: ✅ AUTO-FILLED from your registration (Cannot be changed)
   • Party/Affiliation: Your political party
   • Email: ✅ AUTO-FILLED from your registration (Cannot be changed)
   • Phone: Ethiopian mobile number (09xxxxxxxx)

2. DOCUMENTS:
   • Manifesto: Upload your campaign document (PDF/DOC/TXT, max 10MB)
   • Photo: Upload a professional photo (JPG/PNG, max 5MB)

3. SUBMISSION:
   • Click 'Submit Application' when all fields are complete
   • Review all information before submitting

IMPORTANT:
   • Email and Region are LOCKED to match your registration
   • This ensures your candidate profile matches your user account

SHORTCUTS:
   • F1: Show this help
   • Ctrl+S: Submit application
   • Ctrl+C: Clear form
   • Esc: Close form

For assistance, contact the election commission.";

            MessageBox.Show(helpText, "Help - Candidate Application",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Check for unsaved changes when user tries to close
            if (e.CloseReason == CloseReason.UserClosing && HasUnsavedChanges())
            {
                var result = MessageBox.Show("You have unsaved changes. Close anyway?",
                    "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            // Clean up resources
            if (picPhoto.Image != null)
            {
                picPhoto.Image.Dispose();
                picPhoto.Image = null;
            }
        }
    }
}