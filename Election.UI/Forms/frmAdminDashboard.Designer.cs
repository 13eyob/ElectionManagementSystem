namespace Election.UI.Forms
{
    partial class FrmAdminDashboard
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblElectionStatusHeader = new Label();
            btnLogout = new Button();
            lblSystemTitle = new Label();
            picLogo = new PictureBox();
            panel1 = new Panel();
            panelElectionControl = new Panel();
            label6 = new Label();
            panelResults = new Panel();
            label5 = new Label();
            panelVoteMonitoring = new Panel();
            label8 = new Label();
            panelUserManagement = new Panel();
            label3 = new Label();
            panelCandidate = new Panel();
            label4 = new Label();
            panelDashboard = new Panel();
            label2 = new Label();
            labeladmin = new Label();
            pnlMainContent = new Panel();
            btnRefresh = new Button();
            lblCandidatesTitle = new Label();
            dgvCandidates = new DataGridView();
            toolTip1 = new ToolTip(components);
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            panel1.SuspendLayout();
            panelElectionControl.SuspendLayout();
            panelResults.SuspendLayout();
            panelVoteMonitoring.SuspendLayout();
            panelUserManagement.SuspendLayout();
            panelCandidate.SuspendLayout();
            panelDashboard.SuspendLayout();
            pnlMainContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCandidates).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHeader.BackColor = Color.FromArgb(15, 22, 40);
            pnlHeader.Controls.Add(lblElectionStatusHeader);
            pnlHeader.Controls.Add(btnLogout);
            pnlHeader.Controls.Add(lblSystemTitle);
            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1400, 75);
            pnlHeader.TabIndex = 2;
            // 
            // lblElectionStatusHeader
            // 
            lblElectionStatusHeader.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblElectionStatusHeader.AutoSize = true;
            lblElectionStatusHeader.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblElectionStatusHeader.ForeColor = Color.Orange;
            lblElectionStatusHeader.Location = new Point(1050, 9);
            lblElectionStatusHeader.Name = "lblElectionStatusHeader";
            lblElectionStatusHeader.Size = new Size(149, 20);
            lblElectionStatusHeader.TabIndex = 6;
            lblElectionStatusHeader.Text = "⏸ Election Inactive";
            lblElectionStatusHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = Color.Red;
            btnLogout.Location = new Point(1260, 28);
            btnLogout.Margin = new Padding(4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(123, 36);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "Logout";
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
            panel1.Controls.Add(panelElectionControl);
            panel1.Controls.Add(panelResults);
            panel1.Controls.Add(panelVoteMonitoring);
            panel1.Controls.Add(panelUserManagement);
            panel1.Controls.Add(panelCandidate);
            panel1.Controls.Add(panelDashboard);
            panel1.Location = new Point(0, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(259, 874);
            panel1.TabIndex = 3;
            // 
            // panelElectionControl
            // 
            panelElectionControl.Controls.Add(label6);
            panelElectionControl.Cursor = Cursors.Hand;
            panelElectionControl.Location = new Point(0, 540);
            panelElectionControl.Name = "panelElectionControl";
            panelElectionControl.Size = new Size(223, 60);
            panelElectionControl.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(15, 22, 40);
            label6.Cursor = Cursors.Hand;
            label6.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label6.ForeColor = Color.White;
            label6.Location = new Point(36, 20);
            label6.Name = "label6";
            label6.Size = new Size(146, 20);
            label6.TabIndex = 9;
            label6.Text = "⚙️ Election Control";
            // 
            // panelResults
            // 
            panelResults.Controls.Add(label5);
            panelResults.Cursor = Cursors.Hand;
            panelResults.Location = new Point(0, 460);
            panelResults.Name = "panelResults";
            panelResults.Size = new Size(223, 60);
            panelResults.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(15, 22, 40);
            label5.Cursor = Cursors.Hand;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(36, 20);
            label5.Name = "label5";
            label5.Size = new Size(85, 20);
            label5.TabIndex = 8;
            label5.Text = "📊 Results";
            // 
            // panelVoteMonitoring
            // 
            panelVoteMonitoring.Controls.Add(label8);
            panelVoteMonitoring.Cursor = Cursors.Hand;
            panelVoteMonitoring.Location = new Point(0, 380);
            panelVoteMonitoring.Name = "panelVoteMonitoring";
            panelVoteMonitoring.Size = new Size(223, 60);
            panelVoteMonitoring.TabIndex = 9;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.FromArgb(15, 22, 40);
            label8.Cursor = Cursors.Hand;
            label8.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label8.ForeColor = Color.White;
            label8.Location = new Point(34, 20);
            label8.Name = "label8";
            label8.Size = new Size(148, 20);
            label8.TabIndex = 10;
            label8.Text = "🗳️ Vote Monitoring";
            // 
            // panelUserManagement
            // 
            panelUserManagement.Controls.Add(label3);
            panelUserManagement.Cursor = Cursors.Hand;
            panelUserManagement.Location = new Point(0, 300);
            panelUserManagement.Name = "panelUserManagement";
            panelUserManagement.Size = new Size(223, 60);
            panelUserManagement.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(15, 22, 40);
            label3.Cursor = Cursors.Hand;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(34, 21);
            label3.Name = "label3";
            label3.Size = new Size(163, 20);
            label3.TabIndex = 6;
            label3.Text = "\U0001f9d1 User Management";
            label3.Click += Label3_Click;
            // 
            // panelCandidate
            // 
            panelCandidate.Controls.Add(label4);
            panelCandidate.Cursor = Cursors.Hand;
            panelCandidate.Location = new Point(0, 220);
            panelCandidate.Name = "panelCandidate";
            panelCandidate.Size = new Size(208, 60);
            panelCandidate.TabIndex = 4;
            panelCandidate.Paint += PanelCandidate_Paint;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(15, 22, 40);
            label4.Cursor = Cursors.Hand;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(36, 23);
            label4.Name = "label4";
            label4.Size = new Size(112, 20);
            label4.TabIndex = 7;
            label4.Text = "🎯 Candidates";
            // 
            // panelDashboard
            // 
            panelDashboard.Controls.Add(label2);
            panelDashboard.Cursor = Cursors.Hand;
            panelDashboard.Location = new Point(0, 140);
            panelDashboard.Name = "panelDashboard";
            panelDashboard.Size = new Size(200, 60);
            panelDashboard.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(15, 22, 40);
            label2.Cursor = Cursors.Hand;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(39, 25);
            label2.Name = "label2";
            label2.Size = new Size(109, 20);
            label2.TabIndex = 5;
            label2.Text = "🏠 Dashboard";
            // 
            // labeladmin
            // 
            labeladmin.Anchor = AnchorStyles.Top;
            labeladmin.AutoSize = true;
            labeladmin.BackColor = Color.SlateGray;
            labeladmin.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labeladmin.ForeColor = Color.Navy;
            labeladmin.Location = new Point(550, 79);
            labeladmin.Name = "labeladmin";
            labeladmin.Size = new Size(246, 30);
            labeladmin.TabIndex = 4;
            labeladmin.Text = "Welcome Administrator";
            labeladmin.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlMainContent
            // 
            pnlMainContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlMainContent.BackColor = Color.White;
            pnlMainContent.Controls.Add(btnRefresh);
            pnlMainContent.Controls.Add(lblCandidatesTitle);
            pnlMainContent.Controls.Add(dgvCandidates);
            pnlMainContent.Location = new Point(257, 115);
            pnlMainContent.Name = "pnlMainContent";
            pnlMainContent.Size = new Size(1143, 812);
            pnlMainContent.TabIndex = 5;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(21, 101, 192);
            btnRefresh.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(991, 7);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 35);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "🔄 Refresh";
            toolTip1.SetToolTip(btnRefresh, "Refresh current view (Ctrl+R)");
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblCandidatesTitle
            // 
            lblCandidatesTitle.AutoSize = true;
            lblCandidatesTitle.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCandidatesTitle.ForeColor = Color.FromArgb(15, 22, 40);
            lblCandidatesTitle.Location = new Point(8, 0);
            lblCandidatesTitle.Name = "lblCandidatesTitle";
            lblCandidatesTitle.Size = new Size(286, 37);
            lblCandidatesTitle.TabIndex = 1;
            lblCandidatesTitle.Text = "Dashboard Overview";
            // 
            // dgvCandidates
            // 
            dgvCandidates.AllowUserToAddRows = false;
            dgvCandidates.AllowUserToDeleteRows = false;
            dgvCandidates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCandidates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCandidates.BackgroundColor = Color.White;
            dgvCandidates.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCandidates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCandidates.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(15, 22, 40);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvCandidates.DefaultCellStyle = dataGridViewCellStyle2;
            dgvCandidates.Location = new Point(20, 92);
            dgvCandidates.Name = "dgvCandidates";
            dgvCandidates.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvCandidates.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvCandidates.RowHeadersVisible = false;
            dgvCandidates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCandidates.Size = new Size(1123, 742);
            dgvCandidates.TabIndex = 0;
            dgvCandidates.Visible = false;
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 100;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 20;
            // 
            // frmAdminDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateGray;
            ClientSize = new Size(1400, 927);
            Controls.Add(pnlMainContent);
            Controls.Add(labeladmin);
            Controls.Add(panel1);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(1000, 700);
            Name = "frmAdminDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Election System - Admin Dashboard";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            panel1.ResumeLayout(false);
            panelElectionControl.ResumeLayout(false);
            panelElectionControl.PerformLayout();
            panelResults.ResumeLayout(false);
            panelResults.PerformLayout();
            panelVoteMonitoring.ResumeLayout(false);
            panelVoteMonitoring.PerformLayout();
            panelUserManagement.ResumeLayout(false);
            panelUserManagement.PerformLayout();
            panelCandidate.ResumeLayout(false);
            panelCandidate.PerformLayout();
            panelDashboard.ResumeLayout(false);
            panelDashboard.PerformLayout();
            pnlMainContent.ResumeLayout(false);
            pnlMainContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCandidates).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblSystemTitle;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labeladmin;
        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelUserManagement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelCandidate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelResults;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelElectionControl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlMainContent;
        private System.Windows.Forms.DataGridView dgvCandidates;
        private System.Windows.Forms.Label lblCandidatesTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panelVoteMonitoring;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblElectionStatusHeader;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}