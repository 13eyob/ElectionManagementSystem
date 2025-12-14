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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.flowCandidates = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblSelectedCandidate = new System.Windows.Forms.Label();
            this.statusBar = new System.Windows.Forms.Panel();
            this.lblVoterId = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.panelContainer.SuspendLayout();
            this.pnlSelection.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            this.lblTitle.Size = new System.Drawing.Size(900, 100);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Select Your Candidate";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelContainer
            // 
            this.panelContainer.AutoScroll = true;
            this.panelContainer.BackColor = System.Drawing.Color.White;
            this.panelContainer.Controls.Add(this.flowCandidates);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 100);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Padding = new System.Windows.Forms.Padding(40, 20, 40, 20);
            this.panelContainer.Size = new System.Drawing.Size(900, 380);
            this.panelContainer.TabIndex = 1;
            // 
            // flowCandidates
            // 
            this.flowCandidates.AutoScroll = true;
            this.flowCandidates.BackColor = System.Drawing.Color.White;
            this.flowCandidates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCandidates.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowCandidates.Location = new System.Drawing.Point(40, 20);
            this.flowCandidates.Name = "flowCandidates";
            this.flowCandidates.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.flowCandidates.Size = new System.Drawing.Size(820, 340);
            this.flowCandidates.TabIndex = 0;
            this.flowCandidates.WrapContents = false;
            // 
            // pnlSelection
            // 
            this.pnlSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlSelection.Controls.Add(this.panelButtons);
            this.pnlSelection.Controls.Add(this.lblSelectedCandidate);
            this.pnlSelection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSelection.Location = new System.Drawing.Point(0, 480);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Padding = new System.Windows.Forms.Padding(40, 20, 40, 20);
            this.pnlSelection.Size = new System.Drawing.Size(900, 170);
            this.pnlSelection.TabIndex = 2;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnSubmit);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(40, 70);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(820, 80);
            this.panelButtons.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(230, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 45);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(20, 15);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(200, 45);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "Submit My Vote Securely";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblSelectedCandidate
            // 
            this.lblSelectedCandidate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSelectedCandidate.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSelectedCandidate.ForeColor = System.Drawing.Color.Gray;
            this.lblSelectedCandidate.Location = new System.Drawing.Point(40, 20);
            this.lblSelectedCandidate.Name = "lblSelectedCandidate";
            this.lblSelectedCandidate.Size = new System.Drawing.Size(820, 50);
            this.lblSelectedCandidate.TabIndex = 0;
            this.lblSelectedCandidate.Text = "Your selection: None";
            this.lblSelectedCandidate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusBar
            // 
            this.statusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.statusBar.Controls.Add(this.lblVoterId);
            this.statusBar.Controls.Add(this.lblConnectionStatus);
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Location = new System.Drawing.Point(0, 650);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(900, 50);
            this.statusBar.TabIndex = 3;
            // 
            // lblVoterId
            // 
            this.lblVoterId.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblVoterId.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblVoterId.ForeColor = System.Drawing.Color.White;
            this.lblVoterId.Location = new System.Drawing.Point(450, 0);
            this.lblVoterId.Name = "lblVoterId";
            this.lblVoterId.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblVoterId.Size = new System.Drawing.Size(450, 50);
            this.lblVoterId.TabIndex = 1;
            this.lblVoterId.Text = "Your Voter ID: 873-XXX-120";
            this.lblVoterId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.White;
            this.lblConnectionStatus.Location = new System.Drawing.Point(0, 0);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblConnectionStatus.Size = new System.Drawing.Size(450, 50);
            this.lblConnectionStatus.TabIndex = 0;
            this.lblConnectionStatus.Text = "Connection Secured & Encrypted";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmVoterDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.pnlSelection);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVoterDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Voter Dashboard - Election Management System";
            this.Load += new System.EventHandler(this.frmVoterDashboard_Load);
            this.panelContainer.ResumeLayout(false);
            this.pnlSelection.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.ResumeLayout(false);
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