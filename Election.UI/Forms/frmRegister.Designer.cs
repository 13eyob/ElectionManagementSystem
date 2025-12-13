namespace Election.UI.Forms
{
    partial class frmRegister
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
            panel1 = new Panel();
            cmbRegion = new ComboBox();
            numAge = new NumericUpDown();
            btnCancel = new Button();
            btnSubmit = new Button();
            rbCandidate = new RadioButton();
            rbVoter = new RadioButton();
            lblRole = new Label();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            txtEmail = new TextBox();
            txtFullName = new TextBox();
            lblPassword = new Label();
            lblUsername = new Label();
            lblEmail = new Label();
            lblRegion = new Label();
            lblAge = new Label();
            lblFullName = new Label();
            label1 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAge).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(15, 22, 40);
            panel1.Controls.Add(cmbRegion);
            panel1.Controls.Add(numAge);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnSubmit);
            panel1.Controls.Add(rbCandidate);
            panel1.Controls.Add(rbVoter);
            panel1.Controls.Add(lblRole);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(txtFullName);
            panel1.Controls.Add(lblPassword);
            panel1.Controls.Add(lblUsername);
            panel1.Controls.Add(lblEmail);
            panel1.Controls.Add(lblRegion);
            panel1.Controls.Add(lblAge);
            panel1.Controls.Add(lblFullName);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(52, 50);
            panel1.Name = "panel1";
            panel1.Size = new Size(748, 703);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint_1;
            // 
            // cmbRegion
            // 
            cmbRegion.BackColor = SystemColors.InactiveCaption;
            cmbRegion.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRegion.FormattingEnabled = true;
            cmbRegion.Location = new Point(200, 262);
            cmbRegion.Name = "cmbRegion";
            cmbRegion.Size = new Size(378, 23);
            cmbRegion.Sorted = true;
            cmbRegion.TabIndex = 19;
            // 
            // numAge
            // 
            numAge.BackColor = SystemColors.InactiveCaption;
            numAge.Location = new Point(200, 211);
            numAge.Minimum = new decimal(new int[] { 18, 0, 0, 0 });
            numAge.Name = "numAge";
            numAge.Size = new Size(378, 23);
            numAge.TabIndex = 18;
            numAge.Value = new decimal(new int[] { 18, 0, 0, 0 });
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(15, 22, 40);
            btnCancel.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.DodgerBlue;
            btnCancel.Location = new Point(225, 612);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(145, 43);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Back to Login";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.DodgerBlue;
            btnSubmit.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(190, 557);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(231, 37);
            btnSubmit.TabIndex = 16;
            btnSubmit.Text = "Submit Application";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // rbCandidate
            // 
            rbCandidate.AutoSize = true;
            rbCandidate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbCandidate.ForeColor = Color.White;
            rbCandidate.Location = new Point(385, 504);
            rbCandidate.Name = "rbCandidate";
            rbCandidate.Size = new Size(98, 25);
            rbCandidate.TabIndex = 15;
            rbCandidate.TabStop = true;
            rbCandidate.Text = "Candidate";
            rbCandidate.UseVisualStyleBackColor = true;
            rbCandidate.CheckedChanged += rbCandidate_CheckedChanged;
            // 
            // rbVoter
            // 
            rbVoter.AutoSize = true;
            rbVoter.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbVoter.ForeColor = Color.White;
            rbVoter.Location = new Point(239, 502);
            rbVoter.Name = "rbVoter";
            rbVoter.Size = new Size(65, 25);
            rbVoter.TabIndex = 14;
            rbVoter.TabStop = true;
            rbVoter.Text = "Voter";
            rbVoter.UseVisualStyleBackColor = true;
            rbVoter.CheckedChanged += rbVoter_CheckedChanged;
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRole.ForeColor = Color.White;
            lblRole.Location = new Point(94, 504);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(41, 21);
            lblRole.TabIndex = 13;
            lblRole.Text = "Role";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = SystemColors.InactiveCaption;
            txtPassword.Location = new Point(200, 417);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(378, 34);
            txtPassword.TabIndex = 12;
            // 
            // txtUsername
            // 
            txtUsername.BackColor = SystemColors.InactiveCaption;
            txtUsername.Location = new Point(200, 358);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(378, 34);
            txtUsername.TabIndex = 11;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = SystemColors.InactiveCaption;
            txtEmail.Location = new Point(200, 308);
            txtEmail.Multiline = true;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(378, 34);
            txtEmail.TabIndex = 10;
            // 
            // txtFullName
            // 
            txtFullName.BackColor = SystemColors.InactiveCaption;
            txtFullName.Location = new Point(200, 160);
            txtFullName.Multiline = true;
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(378, 34);
            txtFullName.TabIndex = 7;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPassword.ForeColor = Color.White;
            lblPassword.Location = new Point(92, 430);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(76, 21);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsername.ForeColor = Color.White;
            lblUsername.Location = new Point(87, 371);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(81, 21);
            lblUsername.TabIndex = 5;
            lblUsername.Text = "Username";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEmail.ForeColor = Color.White;
            lblEmail.Location = new Point(87, 321);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(48, 21);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Email";
            // 
            // lblRegion
            // 
            lblRegion.AutoSize = true;
            lblRegion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRegion.ForeColor = Color.White;
            lblRegion.Location = new Point(87, 264);
            lblRegion.Name = "lblRegion";
            lblRegion.Size = new Size(59, 21);
            lblRegion.TabIndex = 3;
            lblRegion.Text = "Region";
            // 
            // lblAge
            // 
            lblAge.AutoSize = true;
            lblAge.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAge.ForeColor = Color.White;
            lblAge.Location = new Point(87, 213);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(37, 21);
            lblAge.TabIndex = 2;
            lblAge.Text = "Age";
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFullName.ForeColor = Color.White;
            lblFullName.Location = new Point(75, 158);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(81, 21);
            lblFullName.TabIndex = 1;
            lblFullName.Text = "Full Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(225, 34);
            label1.Name = "label1";
            label1.Size = new Size(353, 36);
            label1.TabIndex = 0;
            label1.Text = "REGISTER NEW USER";
            // 
            // frmRegister
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.loginback;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(881, 781);
            Controls.Add(panel1);
            Name = "frmRegister";
            Text = "frmRegister";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAge).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lblFullName;
        private Label label1;
        private Label lblPassword;
        private Label lblUsername;
        private Label lblEmail;
        private Label lblRegion;
        private Label lblAge;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtFullName;
        private RadioButton rbVoter;
        private Label lblRole;
        private Button btnSubmit;
        private RadioButton rbCandidate;
        private Button btnCancel;
        private NumericUpDown numAge;
        private ComboBox cmbRegion;
    }
}