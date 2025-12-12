
namespace Election.UI.Forms
{
    partial class frmLogin
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
            pnllogin = new Panel();
            linkRegister = new LinkLabel();
            button1 = new Button();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            label1 = new Label();
            pnllogin.SuspendLayout();
            SuspendLayout();
            // 
            // pnllogin
            // 
            pnllogin.BackColor = Color.FromArgb(15, 22, 40);
            pnllogin.Controls.Add(linkRegister);
            pnllogin.Controls.Add(button1);
            pnllogin.Controls.Add(txtPassword);
            pnllogin.Controls.Add(txtUsername);
            pnllogin.Controls.Add(label1);
            pnllogin.Location = new Point(76, 32);
            pnllogin.Name = "pnllogin";
            pnllogin.Size = new Size(611, 380);
            pnllogin.TabIndex = 0;
            // 
            // linkRegister
            // 
            linkRegister.AutoSize = true;
            linkRegister.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkRegister.LinkColor = Color.DodgerBlue;
            linkRegister.Location = new Point(203, 316);
            linkRegister.Name = "linkRegister";
            linkRegister.Size = new Size(229, 20);
            linkRegister.TabIndex = 4;
            linkRegister.TabStop = true;
            linkRegister.Text = "Need an account? Register here";
            linkRegister.LinkClicked += linkRegister_LinkClicked;
            // 
            // button1
            // 
            button1.BackColor = Color.DodgerBlue;
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(194, 243);
            button1.Name = "button1";
            button1.Size = new Size(187, 49);
            button1.TabIndex = 3;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(124, 156);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(333, 36);
            txtPassword.TabIndex = 2;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(124, 114);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(333, 36);
            txtUsername.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(203, 26);
            label1.Name = "label1";
            label1.Size = new Size(141, 42);
            label1.TabIndex = 0;
            label1.Text = "LOGIN";
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.loginback;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(800, 450);
            Controls.Add(pnllogin);
            Name = "frmLogin";
            Text = "frmLogin";
            Load += frmLogin_Load;
            pnllogin.ResumeLayout(false);
            pnllogin.PerformLayout();
            ResumeLayout(false);
        }

        private void pnllogin_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Panel pnllogin;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label label1;
        private LinkLabel linkRegister;
        private Button button1;
    }
}