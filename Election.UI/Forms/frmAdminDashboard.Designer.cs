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
            panelstart = new Panel();
            label6 = new Label();
            pictureBox5 = new PictureBox();
            panelresult = new Panel();
            label5 = new Label();
            pictureBox4 = new PictureBox();
            button1 = new Button();
            panelcandidate = new Panel();
            label4 = new Label();
            pictureBox3 = new PictureBox();
            panelvoters = new Panel();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            panelhome = new Panel();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            labeladmin = new Label();
            pnlMainContent = new Panel();
            btnRefresh = new Button();
            lblCandidatesTitle = new Label();
            dgvCandidates = new DataGridView();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            panel1.SuspendLayout();
            panelstart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panelresult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panelcandidate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panelvoters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panelhome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlMainContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCandidates).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHeader.BackColor = Color.FromArgb(15, 22, 40);
            pnlHeader.Controls.Add(lnkMyProfile);
            pnlHeader.Controls.Add(lnkHome);
            pnlHeader.Controls.Add(btnLogout);
            pnlHeader.Controls.Add(lblSystemTitle);
            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1275, 75);
            pnlHeader.TabIndex = 2;
            // 
            // lnkMyProfile
            // 
            lnkMyProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkMyProfile.ActiveLinkColor = Color.DarkBlue;
            lnkMyProfile.AutoSize = true;
            lnkMyProfile.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lnkMyProfile.LinkColor = Color.White;
            lnkMyProfile.Location = new Point(1086, 45);
            lnkMyProfile.Name = "lnkMyProfile";
            lnkMyProfile.Size = new Size(89, 21);
            lnkMyProfile.TabIndex = 5;
            lnkMyProfile.TabStop = true;
            lnkMyProfile.Text = "My Profile";
            // 
            // lnkHome
            // 
            lnkHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkHome.ActiveLinkColor = Color.DarkBlue;
            lnkHome.AutoSize = true;
            lnkHome.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lnkHome.LinkColor = Color.White;
            lnkHome.Location = new Point(983, 43);
            lnkHome.Name = "lnkHome";
            lnkHome.Size = new Size(56, 21);
            lnkHome.TabIndex = 4;
            lnkHome.TabStop = true;
            lnkHome.Text = "Home";
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = Color.Red;
            btnLogout.Location = new Point(1211, 28);
            btnLogout.Margin = new Padding(4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(123, 36);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            // 
            // lblSystemTitle
            // 
            lblSystemTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
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
            picLogo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
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
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.BackColor = Color.FromArgb(15, 22, 40);
            panel1.Controls.Add(panelstart);
            panel1.Controls.Add(panelresult);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panelcandidate);
            panel1.Controls.Add(panelvoters);
            panel1.Controls.Add(panelhome);
            panel1.Location = new Point(0, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(162, 659);
            panel1.TabIndex = 3;
            // 
            // panelstart
            // 
            panelstart.Controls.Add(label6);
            panelstart.Controls.Add(pictureBox5);
            panelstart.Cursor = Cursors.Hand;
            panelstart.Location = new Point(1, 440);
            panelstart.Name = "panelstart";
            panelstart.Size = new Size(161, 60);
            panelstart.TabIndex = 6;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(15, 22, 40);
            label6.Cursor = Cursors.Hand;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(49, 24);
            label6.Name = "label6";
            label6.Size = new Size(107, 21);
            label6.TabIndex = 9;
            label6.Text = "Start Election";
            // 
            // pictureBox5
            // 
            pictureBox5.BackgroundImage = Properties.Resources.tt;
            pictureBox5.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox5.Cursor = Cursors.Hand;
            pictureBox5.Location = new Point(-1, 4);
            pictureBox5.Margin = new Padding(4);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(51, 51);
            pictureBox5.TabIndex = 5;
            pictureBox5.TabStop = false;
            // 
            // panelresult
            // 
            panelresult.Controls.Add(label5);
            panelresult.Controls.Add(pictureBox4);
            panelresult.Cursor = Cursors.Hand;
            panelresult.Location = new Point(4, 347);
            panelresult.Name = "panelresult";
            panelresult.Size = new Size(158, 50);
            panelresult.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(15, 22, 40);
            label5.Cursor = Cursors.Hand;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(59, 19);
            label5.Name = "label5";
            label5.Size = new Size(64, 21);
            label5.TabIndex = 8;
            label5.Text = "Results";
            // 
            // pictureBox4
            // 
            pictureBox4.BackgroundImage = Properties.Resources._17;
            pictureBox4.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox4.Cursor = Cursors.Hand;
            pictureBox4.Location = new Point(0, 4);
            pictureBox4.Margin = new Padding(4);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(47, 48);
            pictureBox4.TabIndex = 4;
            pictureBox4.TabStop = false;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Red;
            button1.Location = new Point(0, 612);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(162, 47);
            button1.TabIndex = 4;
            button1.Text = "Logout";
            button1.UseVisualStyleBackColor = true;
            // 
            // panelcandidate
            // 
            panelcandidate.Controls.Add(label4);
            panelcandidate.Controls.Add(pictureBox3);
            panelcandidate.Cursor = Cursors.Hand;
            panelcandidate.Location = new Point(0, 237);
            panelcandidate.Name = "panelcandidate";
            panelcandidate.Size = new Size(162, 51);
            panelcandidate.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(15, 22, 40);
            label4.Cursor = Cursors.Hand;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(58, 19);
            label4.Name = "label4";
            label4.Size = new Size(95, 21);
            label4.TabIndex = 7;
            label4.Text = "Candidates";
            // 
            // pictureBox3
            // 
            pictureBox3.BackgroundImage = Properties.Resources._14;
            pictureBox3.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Location = new Point(4, 5);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 48);
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            // 
            // panelvoters
            // 
            panelvoters.Controls.Add(label3);
            panelvoters.Controls.Add(pictureBox2);
            panelvoters.Cursor = Cursors.Hand;
            panelvoters.Location = new Point(3, 141);
            panelvoters.Name = "panelvoters";
            panelvoters.Size = new Size(156, 52);
            panelvoters.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(15, 22, 40);
            label3.Cursor = Cursors.Hand;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(58, 18);
            label3.Name = "label3";
            label3.Size = new Size(58, 21);
            label3.TabIndex = 6;
            label3.Text = "Voters";
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = Properties.Resources._19;
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Location = new Point(0, 5);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(48, 48);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // panelhome
            // 
            panelhome.Controls.Add(label2);
            panelhome.Controls.Add(pictureBox1);
            panelhome.Cursor = Cursors.Hand;
            panelhome.Location = new Point(0, 44);
            panelhome.Name = "panelhome";
            panelhome.Size = new Size(162, 54);
            panelhome.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(15, 22, 40);
            label2.Cursor = Cursors.Hand;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(58, 17);
            label2.Name = "label2";
            label2.Size = new Size(56, 21);
            label2.TabIndex = 5;
            label2.Text = "Home";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources._12;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Location = new Point(0, 4);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(51, 48);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // labeladmin
            // 
            labeladmin.Anchor = AnchorStyles.Top;
            labeladmin.AutoSize = true;
            labeladmin.BackColor = Color.SlateGray;
            labeladmin.Font = new Font("Times New Roman", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labeladmin.ForeColor = Color.Navy;
            labeladmin.Location = new Point(492, 79);
            labeladmin.Name = "labeladmin";
            labeladmin.Size = new Size(264, 26);
            labeladmin.TabIndex = 4;
            labeladmin.Text = "Welcome Administrator";
            // 
            // pnlMainContent
            // 
            pnlMainContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlMainContent.BackColor = Color.White;
            pnlMainContent.Controls.Add(btnRefresh);
            pnlMainContent.Controls.Add(lblCandidatesTitle);
            pnlMainContent.Controls.Add(dgvCandidates);
            pnlMainContent.Location = new Point(162, 108);
            pnlMainContent.Name = "pnlMainContent";
            pnlMainContent.Size = new Size(1113, 626);
            pnlMainContent.TabIndex = 5;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.DodgerBlue;
            btnRefresh.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(950, 20);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 35);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblCandidatesTitle
            // 
            lblCandidatesTitle.AutoSize = true;
            lblCandidatesTitle.Font = new Font("Times New Roman", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCandidatesTitle.ForeColor = Color.Navy;
            lblCandidatesTitle.Location = new Point(20, 20);
            lblCandidatesTitle.Name = "lblCandidatesTitle";
            lblCandidatesTitle.Size = new Size(237, 31);
            lblCandidatesTitle.TabIndex = 1;
            lblCandidatesTitle.Text = "Dashboard Overview";
            // 
            // dgvCandidates
            // 
            dgvCandidates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCandidates.AllowUserToAddRows = false;
            dgvCandidates.AllowUserToDeleteRows = false;
            dgvCandidates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCandidates.BackgroundColor = Color.White;
            dgvCandidates.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCandidates.Location = new Point(20, 70);
            dgvCandidates.Name = "dgvCandidates";
            dgvCandidates.ReadOnly = true;
            dgvCandidates.RowHeadersVisible = false;
            dgvCandidates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCandidates.Size = new Size(1070, 540);
            dgvCandidates.TabIndex = 0;
            dgvCandidates.Visible = false;
            // 
            // frmAdminDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateGray;
            ClientSize = new Size(1275, 734);
            Controls.Add(pnlMainContent);
            Controls.Add(labeladmin);
            Controls.Add(panel1);
            Controls.Add(pnlHeader);

            // Form properties for maximize/minimize/resize
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;
            MinimumSize = new Size(900, 600);

            Name = "frmAdminDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Dashboard - Election System";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            panel1.ResumeLayout(false);
            panelstart.ResumeLayout(false);
            panelstart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            panelresult.ResumeLayout(false);
            panelresult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panelcandidate.ResumeLayout(false);
            panelcandidate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panelvoters.ResumeLayout(false);
            panelvoters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panelhome.ResumeLayout(false);
            panelhome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlMainContent.ResumeLayout(false);
            pnlMainContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCandidates).EndInit();
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
        private Panel pnlMainContent;
        private DataGridView dgvCandidates;
        private Label lblCandidatesTitle;
        private Button btnRefresh;
    }
}