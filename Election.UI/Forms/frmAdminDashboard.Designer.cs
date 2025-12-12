namespace Election.UI.Forms
{
    partial class frmAdminDashboard
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
            panel1 = new Panel();
            labeladmin = new Label();
            panelhome = new Panel();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            panelvoters = new Panel();
            pictureBox2 = new PictureBox();
            label3 = new Label();
            panelcandidate = new Panel();
            pictureBox3 = new PictureBox();
            label4 = new Label();
            button1 = new Button();
            panelresult = new Panel();
            pictureBox4 = new PictureBox();
            label5 = new Label();
            pictureBox5 = new PictureBox();
            panelstart = new Panel();
            label6 = new Label();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            panel1.SuspendLayout();
            panelhome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelvoters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panelcandidate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panelresult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panelstart.SuspendLayout();
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
            pnlHeader.Size = new Size(1275, 75);
            pnlHeader.TabIndex = 2;
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
            lblSystemTitle.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSystemTitle.ForeColor = Color.White;
            lblSystemTitle.Location = new Point(81, 28);
            lblSystemTitle.Margin = new Padding(4, 0, 4, 0);
            lblSystemTitle.Name = "lblSystemTitle";
            lblSystemTitle.Size = new Size(339, 30);
            lblSystemTitle.TabIndex = 1;
            lblSystemTitle.Text = "ETH Election Management System";
            // 
            // picLogo
            // 
            picLogo.BackgroundImage = Properties.Resources._13;
            picLogo.BackgroundImageLayout = ImageLayout.Zoom;
            picLogo.Location = new Point(13, 17);
            picLogo.Margin = new Padding(4);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(60, 49);
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(15, 22, 40);
            panel1.Controls.Add(panelstart);
            panel1.Controls.Add(panelresult);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panelcandidate);
            panel1.Controls.Add(panelvoters);
            panel1.Controls.Add(panelhome);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(162, 659);
            panel1.TabIndex = 3;
            // 
            // labeladmin
            // 
            labeladmin.AutoSize = true;
            labeladmin.BackColor = Color.SlateGray;
            labeladmin.Font = new Font("Times New Roman", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labeladmin.ForeColor = Color.Navy;
            labeladmin.Location = new Point(492, 79);
            labeladmin.Name = "labeladmin";
            labeladmin.Size = new Size(264, 26);
            labeladmin.TabIndex = 4;
            labeladmin.Text = "Wellcome Administrator";
            // 
            // panelhome
            // 
            panelhome.Controls.Add(label2);
            panelhome.Controls.Add(pictureBox1);
            panelhome.Location = new Point(0, 44);
            panelhome.Name = "panelhome";
            panelhome.Size = new Size(162, 54);
            panelhome.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources._12;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(0, 4);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(51, 48);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(15, 22, 40);
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(58, 17);
            label2.Name = "label2";
            label2.Size = new Size(56, 21);
            label2.TabIndex = 5;
            label2.Text = "Home";
            // 
            // panelvoters
            // 
            panelvoters.Controls.Add(label3);
            panelvoters.Controls.Add(pictureBox2);
            panelvoters.Location = new Point(3, 141);
            panelvoters.Name = "panelvoters";
            panelvoters.Size = new Size(156, 52);
            panelvoters.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = Properties.Resources._19;
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Location = new Point(0, 5);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(48, 48);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(15, 22, 40);
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(58, 18);
            label3.Name = "label3";
            label3.Size = new Size(58, 21);
            label3.TabIndex = 6;
            label3.Text = "Voters";
            label3.Click += label3_Click_1;
            // 
            // panelcandidate
            // 
            panelcandidate.Controls.Add(label4);
            panelcandidate.Controls.Add(pictureBox3);
            panelcandidate.Location = new Point(0, 237);
            panelcandidate.Name = "panelcandidate";
            panelcandidate.Size = new Size(162, 51);
            panelcandidate.TabIndex = 2;
            // 
            // pictureBox3
            // 
            pictureBox3.BackgroundImage = Properties.Resources._14;
            pictureBox3.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox3.Location = new Point(4, 5);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 48);
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(15, 22, 40);
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(58, 19);
            label4.Name = "label4";
            label4.Size = new Size(95, 21);
            label4.TabIndex = 7;
            label4.Text = "Candidates";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Red;
            button1.Location = new Point(1, 589);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(158, 47);
            button1.TabIndex = 4;
            button1.Text = "Lgout";
            button1.UseVisualStyleBackColor = true;
            // 
            // panelresult
            // 
            panelresult.Controls.Add(label5);
            panelresult.Controls.Add(pictureBox4);
            panelresult.Location = new Point(4, 347);
            panelresult.Name = "panelresult";
            panelresult.Size = new Size(158, 50);
            panelresult.TabIndex = 5;
            // 
            // pictureBox4
            // 
            pictureBox4.BackgroundImage = Properties.Resources._17;
            pictureBox4.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox4.Location = new Point(0, 4);
            pictureBox4.Margin = new Padding(4);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(47, 48);
            pictureBox4.TabIndex = 4;
            pictureBox4.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(15, 22, 40);
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(59, 19);
            label5.Name = "label5";
            label5.Size = new Size(64, 21);
            label5.TabIndex = 8;
            label5.Text = "Results";
            label5.Click += label5_Click;
            // 
            // pictureBox5
            // 
            pictureBox5.BackgroundImage = Properties.Resources.tt;
            pictureBox5.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox5.Location = new Point(-1, 4);
            pictureBox5.Margin = new Padding(4);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(51, 51);
            pictureBox5.TabIndex = 5;
            pictureBox5.TabStop = false;
            // 
            // panelstart
            // 
            panelstart.Controls.Add(label6);
            panelstart.Controls.Add(pictureBox5);
            panelstart.Location = new Point(1, 440);
            panelstart.Name = "panelstart";
            panelstart.Size = new Size(161, 60);
            panelstart.TabIndex = 6;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(15, 22, 40);
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(49, 24);
            label6.Name = "label6";
            label6.Size = new Size(107, 21);
            label6.TabIndex = 9;
            label6.Text = "Start Election";
            // 
            // frmAdminDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateGray;
            ClientSize = new Size(1275, 734);
            Controls.Add(labeladmin);
            Controls.Add(panel1);
            Controls.Add(pnlHeader);
            Name = "frmAdminDashboard";
            Text = "frmAdminDashboard";
            Load += frmAdminDashboard_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            panel1.ResumeLayout(false);
            panelhome.ResumeLayout(false);
            panelhome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelvoters.ResumeLayout(false);
            panelvoters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panelcandidate.ResumeLayout(false);
            panelcandidate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panelresult.ResumeLayout(false);
            panelresult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            panelstart.ResumeLayout(false);
            panelstart.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private LinkLabel lnkMyProfile;
        private LinkLabel lnkHome;
        private Button btnLogout;
        private Label lblSystemTitle;
        private PictureBox picLogo;
        private Panel panel1;
        private Label labeladmin;
        private Panel panelhome;
        private PictureBox pictureBox1;
        private Label label2;
        private Panel panelvoters;
        private Label label3;
        private PictureBox pictureBox2;
        private Button button1;
        private Panel panelcandidate;
        private Label label4;
        private PictureBox pictureBox3;
        private Panel panelresult;
        private Label label5;
        private PictureBox pictureBox4;
        private Panel panelstart;
        private Label label6;
        private PictureBox pictureBox5;
    }
}