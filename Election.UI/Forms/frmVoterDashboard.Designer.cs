// Location: Election.UI/Forms/frmVoterDashboard.Designer.cs
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
            lblTitle = new Label();
            panelContainer = new Panel();
            flowCandidates = new FlowLayoutPanel();
            pnlSelection = new Panel();
            panelButtons = new Panel();
            btnCancel = new Button();
            btnSubmit = new Button();
            lblSelectedCandidate = new Label();
            statusBar = new Panel();
            lblVoterId = new Label();
            lblConnectionStatus = new Label();
            panelContainer.SuspendLayout();
            pnlSelection.SuspendLayout();
            panelButtons.SuspendLayout();
            statusBar.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.White;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(0, 20, 0, 20);
            lblTitle.Size = new Size(900, 100);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Select Your Candidate";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelContainer
            // 
            panelContainer.AutoScroll = true;
            panelContainer.BackColor = Color.White;
            panelContainer.Controls.Add(flowCandidates);
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.Location = new Point(0, 100);
            panelContainer.Name = "panelContainer";
            panelContainer.Padding = new Padding(40, 20, 40, 20);
            panelContainer.Size = new Size(900, 380);
            panelContainer.TabIndex = 1;
            // 
            // flowCandidates
            // 
            flowCandidates.AutoScroll = true;
            flowCandidates.BackColor = Color.White;
            flowCandidates.Dock = DockStyle.Fill;
            flowCandidates.FlowDirection = FlowDirection.TopDown;
            flowCandidates.Location = new Point(40, 20);
            flowCandidates.Name = "flowCandidates";
            flowCandidates.Padding = new Padding(0, 0, 20, 0);
            flowCandidates.Size = new Size(820, 340);
            flowCandidates.TabIndex = 0;
            flowCandidates.WrapContents = false;
            flowCandidates.Paint += flowCandidates_Paint;
            // 
            // pnlSelection
            // 
            pnlSelection.BackColor = Color.FromArgb(250, 250, 250);
            pnlSelection.Controls.Add(panelButtons);
            pnlSelection.Controls.Add(lblSelectedCandidate);
            pnlSelection.Dock = DockStyle.Bottom;
            pnlSelection.Location = new Point(0, 480);
            pnlSelection.Name = "pnlSelection";
            pnlSelection.Padding = new Padding(40, 20, 40, 20);
            pnlSelection.Size = new Size(900, 170);
            pnlSelection.TabIndex = 2;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnSubmit);
            panelButtons.Dock = DockStyle.Top;
            panelButtons.Location = new Point(40, 70);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(820, 80);
            panelButtons.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 11F);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(230, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 45);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.FromArgb(52, 152, 219);
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(20, 15);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(200, 45);
            btnSubmit.TabIndex = 0;
            btnSubmit.Text = "Submit My Vote Securely";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // lblSelectedCandidate
            // 
            lblSelectedCandidate.Dock = DockStyle.Top;
            lblSelectedCandidate.Font = new Font("Segoe UI", 14F);
            lblSelectedCandidate.ForeColor = Color.Gray;
            lblSelectedCandidate.Location = new Point(40, 20);
            lblSelectedCandidate.Name = "lblSelectedCandidate";
            lblSelectedCandidate.Size = new Size(820, 50);
            lblSelectedCandidate.TabIndex = 0;
            lblSelectedCandidate.Text = "Your selection: None";
            lblSelectedCandidate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // statusBar
            // 
            statusBar.BackColor = Color.FromArgb(44, 62, 80);
            statusBar.Controls.Add(lblVoterId);
            statusBar.Controls.Add(lblConnectionStatus);
            statusBar.Dock = DockStyle.Bottom;
            statusBar.Location = new Point(0, 650);
            statusBar.Name = "statusBar";
            statusBar.Size = new Size(900, 50);
            statusBar.TabIndex = 3;
            // 
            // lblVoterId
            // 
            lblVoterId.Dock = DockStyle.Right;
            lblVoterId.Font = new Font("Segoe UI", 9F);
            lblVoterId.ForeColor = Color.White;
            lblVoterId.Location = new Point(450, 0);
            lblVoterId.Name = "lblVoterId";
            lblVoterId.Padding = new Padding(0, 0, 20, 0);
            lblVoterId.Size = new Size(450, 50);
            lblVoterId.TabIndex = 1;
            lblVoterId.Text = "Your Voter ID: 873-XXX-120";
            lblVoterId.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Dock = DockStyle.Left;
            lblConnectionStatus.Font = new Font("Segoe UI", 9F);
            lblConnectionStatus.ForeColor = Color.White;
            lblConnectionStatus.Location = new Point(0, 0);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Padding = new Padding(20, 0, 0, 0);
            lblConnectionStatus.Size = new Size(450, 50);
            lblConnectionStatus.TabIndex = 0;
            lblConnectionStatus.Text = "Connection Secured & Encrypted";
            lblConnectionStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmVoterDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(900, 700);
            Controls.Add(panelContainer);
            Controls.Add(pnlSelection);
            Controls.Add(statusBar);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmVoterDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Voter Dashboard - Election Management System";
            Load += frmVoterDashboard_Load;
            panelContainer.ResumeLayout(false);
            pnlSelection.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            statusBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.FlowLayoutPanel flowCandidates;
        private System.Windows.Forms.Panel pnlSelection;
        private System.Windows.Forms.Label lblSelectedCandidate;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Panel statusBar;
        private System.Windows.Forms.Label lblVoterId;
        private System.Windows.Forms.Label lblConnectionStatus;
    }
}