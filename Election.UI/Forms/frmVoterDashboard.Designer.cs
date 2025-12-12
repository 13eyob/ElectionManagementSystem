namespace Election.UI.Forms
{
    partial class frmVoterDashboard
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
            panelvoter = new Panel();
            btnSubmit = new Button();
            label1 = new Label();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            panelvoter.SuspendLayout();
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
            pnlHeader.Size = new Size(1373, 75);
            pnlHeader.TabIndex = 1;
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
            // panelvoter
            // 
            panelvoter.BackColor = Color.FromArgb(15, 22, 40);
            panelvoter.Controls.Add(btnSubmit);
            panelvoter.Controls.Add(label1);
            panelvoter.ForeColor = Color.White;
            panelvoter.Location = new Point(56, 73);
            panelvoter.Name = "panelvoter";
            panelvoter.Size = new Size(1246, 647);
            panelvoter.TabIndex = 2;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.DodgerBlue;
            btnSubmit.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(510, 540);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(231, 37);
            btnSubmit.TabIndex = 17;
            btnSubmit.Text = "Submit Application";
            btnSubmit.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(523, 35);
            label1.Name = "label1";
            label1.Size = new Size(198, 25);
            label1.TabIndex = 0;
            label1.Text = "Select Your Candidate";
            // 
            // frmVoterDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources._11;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1373, 766);
            Controls.Add(panelvoter);
            Controls.Add(pnlHeader);
            Name = "frmVoterDashboard";
            Text = "frmVoterDashboard";
            //Load += frmVoterDashboard_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            panelvoter.ResumeLayout(false);
            panelvoter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private LinkLabel lnkMyProfile;
        private LinkLabel lnkHome;
        private Button btnLogout;
        private Label lblSystemTitle;
        private PictureBox picLogo;
        private Panel panelvoter;
        private Label label1;
        private Button btnSubmit;
    }
}