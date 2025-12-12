using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;
using Election.UI;

namespace Election.UI.Forms
{
    public partial class frmCandidateApplication : Form
    {
        private readonly HttpClient _client;
        private string _manifestoFilePath = "";
        private string _photoFilePath = "";

        public frmCandidateApplication()
        {
            InitializeComponent();

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
        }

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

        // ============ KEEP YOUR EXISTING METHODS BELOW ============

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

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
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

        private bool ValidateForm()
        {
            // Simple validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                !int.TryParse(txtAge.Text, out int age) || age < 18 ||
                string.IsNullOrWhiteSpace(textRegion.Text) ||
                string.IsNullOrWhiteSpace(textparty.Text) ||
                string.IsNullOrWhiteSpace(textEmail.Text) ||
                string.IsNullOrWhiteSpace(textPhone.Text))
            {
                MessageBox.Show("Please fill all personal details correctly");
                return false;
            }

            if (string.IsNullOrEmpty(_manifestoFilePath))
            {
                MessageBox.Show("Please upload manifesto file");
                return false;
            }

            if (string.IsNullOrEmpty(_photoFilePath))
            {
                MessageBox.Show("Please upload photo");
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtFullName.Clear();
            txtAge.Clear();
            textRegion.Clear();
            textparty.Clear();
            textEmail.Clear();
            textPhone.Clear();
            txtManifesto.Clear();
            picPhoto.Image = null;
            _manifestoFilePath = "";
            _photoFilePath = "";
        }
    }
}