namespace Election.UI.Forms
{
    partial class frmCandidateApplication
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lnkMyProfile = new LinkLabel();
            lnkHome = new LinkLabel();
            btnLogout = new Button();
            lblSystemTitle = new Label();
            picLogo = new PictureBox();
            lblMainTitle = new Label();
            lblStep1 = new Label();
            txtFullName = new TextBox();
            txtAge = new TextBox();
            textRegion = new TextBox();
            lblStep2 = new Label();
            label5 = new Label();
            lblUploadPhoto = new Label();
            pnlMain = new Panel();
            btnSubmit = new Button();
            btnChooseImage = new Button();
            picPhoto = new PictureBox();
            picCameraIcon = new PictureBox();
            btnUploadFile = new Button();
            txtManifesto = new TextBox();
            textPhone = new TextBox();
            textEmail = new TextBox();
            textparty = new TextBox();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPhoto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCameraIcon).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(15, 22, 40);
            pnlHeader.Controls.Add(lnkMyProfile);
            pnlHeader.Controls.Add(lnkHome);
            pnlHeader.Controls.Add(btnLogout);
            pnlHeader.Controls.Add(lblSystemTitle);
            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1280, 75);
            pnlHeader.TabIndex = 0;
            // 
            // lnkMyProfile
            // 
            lnkMyProfile.ActiveLinkColor = Color.DarkBlue;
            lnkMyProfile.AutoSize = true;
            lnkMyProfile.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lnkMyProfile.LinkColor = Color.White;
            lnkMyProfile.Location = new Point(919, 45);
            lnkMyProfile.Name = "lnkMyProfile";
            lnkMyProfile.Size = new Size(89, 21);
            lnkMyProfile.TabIndex = 5;
            lnkMyProfile.TabStop = true;
            lnkMyProfile.Text = "My Profile";
            // 
            // lnkHome
            // 
            lnkHome.ActiveLinkColor = Color.DarkBlue;
            lnkHome.AutoSize = true;
            lnkHome.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lnkHome.LinkColor = Color.White;
            lnkHome.Location = new Point(816, 43);
            lnkHome.Name = "lnkHome";
            lnkHome.Size = new Size(56, 21);
            lnkHome.TabIndex = 4;
            lnkHome.TabStop = true;
            lnkHome.Text = "Home";
            // 
            // btnLogout
            // 
            btnLogout.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = Color.Red;
            btnLogout.Location = new Point(1144, 28);
            btnLogout.Margin = new Padding(4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(123, 36);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "Lgout";
            btnLogout.UseVisualStyleBackColor = true;
            // 
            // lblSystemTitle
            // 
            lblSystemTitle.AutoSize = true;
            lblSystemTitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSystemTitle.ForeColor = Color.White;
            lblSystemTitle.Location = new Point(112, 21);
            lblSystemTitle.Margin = new Padding(4, 0, 4, 0);
            lblSystemTitle.Name = "lblSystemTitle";
            lblSystemTitle.Size = new Size(246, 21);
            lblSystemTitle.TabIndex = 1;
            lblSystemTitle.Text = "ETH Election Management System";
            // 
            // picLogo
            // 
            picLogo.BackgroundImage = Properties.Resources._13;
            picLogo.BackgroundImageLayout = ImageLayout.Zoom;
            picLogo.Location = new Point(15, 17);
            picLogo.Margin = new Padding(4);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(89, 49);
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // lblMainTitle
            // 
            lblMainTitle.AutoSize = true;
            lblMainTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMainTitle.Location = new Point(87, 20);
            lblMainTitle.Name = "lblMainTitle";
            lblMainTitle.Size = new Size(361, 25);
            lblMainTitle.TabIndex = 0;
            lblMainTitle.Text = "Candidate Registration and Application";
            // 
            // lblStep1
            // 
            lblStep1.AutoSize = true;
            lblStep1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStep1.Location = new Point(87, 92);
            lblStep1.Name = "lblStep1";
            lblStep1.Size = new Size(166, 25);
            lblStep1.TabIndex = 1;
            lblStep1.Text = "1. Personal Details";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(88, 147);
            txtFullName.Multiline = true;
            txtFullName.Name = "txtFullName";
            txtFullName.PlaceholderText = "FullName";
            txtFullName.Size = new Size(361, 34);
            txtFullName.TabIndex = 2;
            // 
            // txtAge
            // 
            txtAge.Location = new Point(87, 201);
            txtAge.Multiline = true;
            txtAge.Name = "txtAge";
            txtAge.PlaceholderText = "Age";
            txtAge.Size = new Size(361, 34);
            txtAge.TabIndex = 3;
            // 
            // textRegion
            // 
            textRegion.Location = new Point(87, 253);
            textRegion.Multiline = true;
            textRegion.Name = "textRegion";
            textRegion.PlaceholderText = "Region";
            textRegion.Size = new Size(361, 34);
            textRegion.TabIndex = 4;
            // 
            // lblStep2
            // 
            lblStep2.AutoSize = true;
            lblStep2.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStep2.Location = new Point(87, 319);
            lblStep2.Name = "lblStep2";
            lblStep2.Size = new Size(181, 25);
            lblStep2.TabIndex = 8;
            lblStep2.Text = "2.Upload Manifesto";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(802, 319);
            label5.Name = "label5";
            label5.Size = new Size(0, 25);
            label5.TabIndex = 9;
            // 
            // lblUploadPhoto
            // 
            lblUploadPhoto.AutoSize = true;
            lblUploadPhoto.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUploadPhoto.Location = new Point(831, 319);
            lblUploadPhoto.Name = "lblUploadPhoto";
            lblUploadPhoto.Size = new Size(129, 25);
            lblUploadPhoto.TabIndex = 10;
            lblUploadPhoto.Text = "Upload Photo";
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(btnSubmit);
            pnlMain.Controls.Add(btnChooseImage);
            pnlMain.Controls.Add(picPhoto);
            pnlMain.Controls.Add(picCameraIcon);
            pnlMain.Controls.Add(btnUploadFile);
            pnlMain.Controls.Add(txtManifesto);
            pnlMain.Controls.Add(textPhone);
            pnlMain.Controls.Add(textEmail);
            pnlMain.Controls.Add(textparty);
            pnlMain.Controls.Add(lblUploadPhoto);
            pnlMain.Controls.Add(label5);
            pnlMain.Controls.Add(lblStep2);
            pnlMain.Controls.Add(textRegion);
            pnlMain.Controls.Add(txtAge);
            pnlMain.Controls.Add(txtFullName);
            pnlMain.Controls.Add(lblStep1);
            pnlMain.Controls.Add(lblMainTitle);
            pnlMain.Location = new Point(70, 73);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1137, 670);
            pnlMain.TabIndex = 1;
       
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = SystemColors.Highlight;
            btnSubmit.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(438, 589);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(277, 48);
            btnSubmit.TabIndex = 19;
            btnSubmit.Text = "Submit Application";
            btnSubmit.UseVisualStyleBackColor = false;
            // 
            // btnChooseImage
            // 
            btnChooseImage.BackColor = SystemColors.Highlight;
            btnChooseImage.ForeColor = Color.White;
            btnChooseImage.Location = new Point(849, 516);
            btnChooseImage.Name = "btnChooseImage";
            btnChooseImage.Size = new Size(148, 38);
            btnChooseImage.TabIndex = 18;
            btnChooseImage.Text = "Choose Image.....";
            btnChooseImage.UseVisualStyleBackColor = false;
            // 
            // picPhoto
            // 
            picPhoto.Location = new Point(849, 391);
            picPhoto.Name = "picPhoto";
            picPhoto.Size = new Size(148, 163);
            picPhoto.TabIndex = 17;
            picPhoto.TabStop = false;
            // 
            // picCameraIcon
            // 
            picCameraIcon.BackgroundImage = Properties.Resources.image_icon;
            picCameraIcon.BackgroundImageLayout = ImageLayout.Stretch;
            picCameraIcon.Location = new Point(754, 319);
            picCameraIcon.Name = "picCameraIcon";
            picCameraIcon.Size = new Size(71, 50);
            picCameraIcon.TabIndex = 16;
            picCameraIcon.TabStop = false;
            // 
            // btnUploadFile
            // 
            btnUploadFile.BackColor = Color.White;
            btnUploadFile.ForeColor = Color.DeepSkyBlue;
            btnUploadFile.Location = new Point(384, 516);
            btnUploadFile.Name = "btnUploadFile";
            btnUploadFile.Size = new Size(134, 38);
            btnUploadFile.TabIndex = 15;
            btnUploadFile.Text = "Upload File......";
            btnUploadFile.UseVisualStyleBackColor = false;
            // 
            // txtManifesto
            // 
            txtManifesto.Location = new Point(53, 363);
            txtManifesto.Multiline = true;
            txtManifesto.Name = "txtManifesto";
            txtManifesto.Size = new Size(465, 191);
            txtManifesto.TabIndex = 14;
            // 
            // textPhone
            // 
            textPhone.Location = new Point(562, 262);
            textPhone.Multiline = true;
            textPhone.Name = "textPhone";
            textPhone.PlaceholderText = "Phone";
            textPhone.Size = new Size(361, 34);
            textPhone.TabIndex = 13;
            // 
            // textEmail
            // 
            textEmail.Location = new Point(562, 201);
            textEmail.Multiline = true;
            textEmail.Name = "textEmail";
            textEmail.PlaceholderText = "Email";
            textEmail.Size = new Size(361, 34);
            textEmail.TabIndex = 12;
            // 
            // textparty
            // 
            textparty.Location = new Point(562, 147);
            textparty.Multiline = true;
            textparty.Name = "textparty";
            textparty.PlaceholderText = "party/Affilation";
            textparty.Size = new Size(361, 34);
            textparty.TabIndex = 11;
            // 
            // frmCandidateApplication
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1280, 792);
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "frmCandidateApplication";
            Text = "frmCandidateApplication";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPhoto).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCameraIcon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblSystemTitle;
        private PictureBox picLogo;
        private Button btnLogout;
        private Label lblMainTitle;
        private Label lblStep1;
        private TextBox txtFullName;
        private TextBox txtAge;
        private TextBox textRegion;
        private Label lblStep2;
        private Label label5;
        private Label lblUploadPhoto;
        private Panel pnlMain;
        private TextBox textPhone;
        private TextBox textEmail;
        private TextBox textparty;
        private TextBox txtManifesto;
        private Button btnUploadFile;
        private Button btnChooseImage;
        private PictureBox picPhoto;
        private PictureBox picCameraIcon;
        private Button btnSubmit;
        private LinkLabel lnkHome;
        private LinkLabel lnkMyProfile;
    }
}