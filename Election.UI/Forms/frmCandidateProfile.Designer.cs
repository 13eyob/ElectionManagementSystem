namespace Election.UI.Forms
{
    partial class frmCandidateProfile
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Panel panelMain;
        private Label lblFullName;
        private TextBox txtFullName;
        private Label lblAge;
        private TextBox txtAge;
        private Label lblRegion;
        private TextBox txtRegion;
        private Label lblParty;
        private TextBox txtParty;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblStatus;
        private Label lblStatusValue;
        private Label lblAppliedDate;
        private Label lblDateValue;
        private Panel panelButtons;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            panelMain = new Panel();
            lblDateValue = new Label();
            lblAppliedDate = new Label();
            lblStatusValue = new Label();
            lblStatus = new Label();
            txtPhone = new TextBox();
            lblPhone = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtParty = new TextBox();
            lblParty = new Label();
            txtRegion = new TextBox();
            lblRegion = new Label();
            txtAge = new TextBox();
            lblAge = new Label();
            txtFullName = new TextBox();
            lblFullName = new Label();
            panelButtons = new Panel();
            btnClose = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            panelMain.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 22, 40);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(232, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "My Candidate Profile";
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.BorderStyle = BorderStyle.FixedSingle;
            panelMain.Controls.Add(lblDateValue);
            panelMain.Controls.Add(lblAppliedDate);
            panelMain.Controls.Add(lblStatusValue);
            panelMain.Controls.Add(lblStatus);
            panelMain.Controls.Add(txtPhone);
            panelMain.Controls.Add(lblPhone);
            panelMain.Controls.Add(txtEmail);
            panelMain.Controls.Add(lblEmail);
            panelMain.Controls.Add(txtParty);
            panelMain.Controls.Add(lblParty);
            panelMain.Controls.Add(txtRegion);
            panelMain.Controls.Add(lblRegion);
            panelMain.Controls.Add(txtAge);
            panelMain.Controls.Add(lblAge);
            panelMain.Controls.Add(txtFullName);
            panelMain.Controls.Add(lblFullName);
            panelMain.Location = new Point(20, 70);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(500, 350);
            panelMain.TabIndex = 1;
          
            // 
            // lblDateValue
            // 
            lblDateValue.AutoSize = true;
            lblDateValue.Font = new Font("Segoe UI", 9.5F);
            lblDateValue.Location = new Point(150, 290);
            lblDateValue.Name = "lblDateValue";
            lblDateValue.Size = new Size(85, 17);
            lblDateValue.TabIndex = 15;
            lblDateValue.Text = "Not available";
            // 
            // lblAppliedDate
            // 
            lblAppliedDate.AutoSize = true;
            lblAppliedDate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblAppliedDate.Location = new Point(30, 290);
            lblAppliedDate.Name = "lblAppliedDate";
            lblAppliedDate.Size = new Size(93, 17);
            lblAppliedDate.TabIndex = 14;
            lblAppliedDate.Text = "Applied Date:";
            // 
            // lblStatusValue
            // 
            lblStatusValue.AutoSize = true;
            lblStatusValue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblStatusValue.ForeColor = Color.FromArgb(0, 123, 255);
            lblStatusValue.Location = new Point(150, 260);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new Size(59, 17);
            lblStatusValue.TabIndex = 13;
            lblStatusValue.Text = "Pending";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblStatus.Location = new Point(30, 260);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 17);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "Status:";
            // 
            // txtPhone
            // 
            txtPhone.Font = new Font("Segoe UI", 10F);
            txtPhone.Location = new Point(150, 210);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(250, 25);
            txtPhone.TabIndex = 11;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPhone.Location = new Point(30, 213);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(55, 19);
            lblPhone.TabIndex = 10;
            lblPhone.Text = "Phone:";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.WhiteSmoke;
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(150, 175);
            txtEmail.Name = "txtEmail";
            txtEmail.ReadOnly = true;
            txtEmail.Size = new Size(250, 25);
            txtEmail.TabIndex = 9;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEmail.Location = new Point(30, 178);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(49, 19);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email:";
            // 
            // txtParty
            // 
            txtParty.Font = new Font("Segoe UI", 10F);
            txtParty.Location = new Point(150, 140);
            txtParty.Name = "txtParty";
            txtParty.Size = new Size(250, 25);
            txtParty.TabIndex = 7;
            // 
            // lblParty
            // 
            lblParty.AutoSize = true;
            lblParty.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblParty.Location = new Point(30, 143);
            lblParty.Name = "lblParty";
            lblParty.Size = new Size(121, 19);
            lblParty.TabIndex = 6;
            lblParty.Text = "Party/Affiliation:";
            // 
            // txtRegion
            // 
            txtRegion.Font = new Font("Segoe UI", 10F);
            txtRegion.Location = new Point(150, 105);
            txtRegion.Name = "txtRegion";
            txtRegion.Size = new Size(250, 25);
            txtRegion.TabIndex = 5;
            // 
            // lblRegion
            // 
            lblRegion.AutoSize = true;
            lblRegion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRegion.Location = new Point(30, 108);
            lblRegion.Name = "lblRegion";
            lblRegion.Size = new Size(60, 19);
            lblRegion.TabIndex = 4;
            lblRegion.Text = "Region:";
            // 
            // txtAge
            // 
            txtAge.Font = new Font("Segoe UI", 10F);
            txtAge.Location = new Point(150, 70);
            txtAge.Name = "txtAge";
            txtAge.Size = new Size(80, 25);
            txtAge.TabIndex = 3;
            // 
            // lblAge
            // 
            lblAge.AutoSize = true;
            lblAge.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAge.Location = new Point(30, 73);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(40, 19);
            lblAge.TabIndex = 2;
            lblAge.Text = "Age:";
            // 
            // txtFullName
            // 
            txtFullName.Font = new Font("Segoe UI", 10F);
            txtFullName.Location = new Point(150, 35);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(250, 25);
            txtFullName.TabIndex = 1;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFullName.Location = new Point(30, 38);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(80, 19);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "Full Name:";
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.FromArgb(240, 240, 245);
            panelButtons.Controls.Add(btnClose);
            panelButtons.Controls.Add(btnDelete);
            panelButtons.Controls.Add(btnUpdate);
            panelButtons.Location = new Point(20, 430);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(500, 60);
            panelButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(380, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(200, 15);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.FromArgb(40, 167, 69);
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(20, 15);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(100, 30);
            btnUpdate.TabIndex = 0;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += BtnUpdate_Click;
            // 
            // frmCandidateProfile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(544, 511);
            Controls.Add(panelButtons);
            Controls.Add(panelMain);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCandidateProfile";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "My Candidate Profile";
            Load += frmCandidateProfile_Load;
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}