
namespace Election.UI.Forms
{
    partial class FrmLogin
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
            lblPassword = new Label();
            lblUsername = new Label();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            linkRegister = new LinkLabel();
            button1 = new Button();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            label1 = new Label();
            pnllogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pnllogin
            // 
            pnllogin.BackColor = Color.FromArgb(15, 22, 40);
            pnllogin.Controls.Add(lblPassword);
            pnllogin.Controls.Add(lblUsername);
            pnllogin.Controls.Add(pictureBox2);
            pnllogin.Controls.Add(pictureBox1);
            pnllogin.Controls.Add(linkRegister);
            pnllogin.Controls.Add(button1);
            pnllogin.Controls.Add(txtPassword);
            pnllogin.Controls.Add(txtUsername);
            pnllogin.Controls.Add(label1);
            pnllogin.Location = new Point(125, 32);
            pnllogin.Name = "pnllogin";
            pnllogin.Size = new Size(719, 418);
            pnllogin.TabIndex = 0;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPassword.ForeColor = Color.White;
            lblPassword.Location = new Point(158, 184);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(76, 21);
            lblPassword.TabIndex = 8;
            lblPassword.Text = "Password";
            lblPassword.Click += LblPassword_Click;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsername.ForeColor = Color.White;
            lblUsername.Location = new Point(153, 129);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(81, 21);
            lblUsername.TabIndex = 7;
            lblUsername.Text = "Username";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(224, 224, 224);
            pictureBox2.BackgroundImage = Properties.Resources._lock;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(89, 169);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(58, 36);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(224, 224, 224);
            pictureBox1.BackgroundImage = Properties.Resources.user;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(89, 114);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(58, 36);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // linkRegister
            // 
            linkRegister.AutoSize = true;
            linkRegister.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkRegister.LinkColor = Color.DodgerBlue;
            linkRegister.Location = new Point(285, 324);
            linkRegister.Name = "linkRegister";
            linkRegister.Size = new Size(229, 20);
            linkRegister.TabIndex = 4;
            linkRegister.TabStop = true;
            linkRegister.Text = "Need an account? Register here";
            // 
            // button1
            // 
            button1.BackColor = Color.DodgerBlue;
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(299, 234);
            button1.Name = "button1";
            button1.Size = new Size(187, 49);
            button1.TabIndex = 3;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = false;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.White;
            txtPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.ForeColor = Color.Gray;
            txtPassword.Location = new Point(240, 169);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(333, 36);
            txtPassword.TabIndex = 2;
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.White;
            txtUsername.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.ForeColor = Color.Gray;
            txtUsername.Location = new Point(240, 114);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(333, 36);
            txtUsername.TabIndex = 1;
            txtUsername.TextChanged += TxtUsername_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(299, 28);
            label1.Name = "label1";
            label1.Size = new Size(141, 42);
            label1.TabIndex = 0;
            label1.Text = "LOGIN";
            label1.Click += Label1_Click;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.loginback;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(939, 602);
            Controls.Add(pnllogin);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login - Election Management System";
            pnllogin.ResumeLayout(false);
            pnllogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnllogin;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label label1;
        private LinkLabel linkRegister;
        private Button button1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label lblUsername;
        private Label lblPassword;
    }
}
