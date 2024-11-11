namespace IT12FINALPROJ
{
    partial class Login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            usernameTextbox = new Guna.UI2.WinForms.Guna2TextBox();
            passwordTextbox = new Guna.UI2.WinForms.Guna2TextBox();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            pictureBox2 = new PictureBox();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // guna2DragControl1
            // 
            guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl1.UseTransparentDrag = true;
            // 
            // guna2HtmlLabel3
            // 
            guna2HtmlLabel3.BackColor = Color.White;
            guna2HtmlLabel3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel3.ForeColor = Color.DimGray;
            guna2HtmlLabel3.Location = new Point(146, 33);
            guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            guna2HtmlLabel3.Size = new Size(242, 23);
            guna2HtmlLabel3.TabIndex = 7;
            guna2HtmlLabel3.Text = "Inventory Management System";
            // 
            // usernameTextbox
            // 
            usernameTextbox.BorderColor = Color.FromArgb(136, 113, 205);
            usernameTextbox.BorderRadius = 20;
            usernameTextbox.CustomizableEdges = customizableEdges7;
            usernameTextbox.DefaultText = "";
            usernameTextbox.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            usernameTextbox.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            usernameTextbox.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            usernameTextbox.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            usernameTextbox.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            usernameTextbox.Font = new Font("Segoe UI", 9F);
            usernameTextbox.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            usernameTextbox.IconLeft = (Image)resources.GetObject("usernameTextbox.IconLeft");
            usernameTextbox.IconLeftOffset = new Point(20, 0);
            usernameTextbox.IconLeftSize = new Size(15, 15);
            usernameTextbox.Location = new Point(66, 299);
            usernameTextbox.Name = "usernameTextbox";
            usernameTextbox.PasswordChar = '\0';
            usernameTextbox.PlaceholderText = "Username";
            usernameTextbox.SelectedText = "";
            usernameTextbox.ShadowDecoration.CustomizableEdges = customizableEdges8;
            usernameTextbox.Size = new Size(373, 51);
            usernameTextbox.TabIndex = 0;
            // 
            // passwordTextbox
            // 
            passwordTextbox.BorderColor = Color.FromArgb(136, 113, 205);
            passwordTextbox.BorderRadius = 20;
            passwordTextbox.CustomizableEdges = customizableEdges9;
            passwordTextbox.DefaultText = "";
            passwordTextbox.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            passwordTextbox.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            passwordTextbox.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            passwordTextbox.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            passwordTextbox.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            passwordTextbox.Font = new Font("Segoe UI", 9F);
            passwordTextbox.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            passwordTextbox.IconLeft = (Image)resources.GetObject("passwordTextbox.IconLeft");
            passwordTextbox.IconLeftOffset = new Point(20, 0);
            passwordTextbox.IconLeftSize = new Size(15, 15);
            passwordTextbox.Location = new Point(66, 233);
            passwordTextbox.Name = "passwordTextbox";
            passwordTextbox.PasswordChar = '\0';
            passwordTextbox.PlaceholderText = "Password";
            passwordTextbox.SelectedText = "";
            passwordTextbox.ShadowDecoration.CustomizableEdges = customizableEdges10;
            passwordTextbox.Size = new Size(373, 51);
            passwordTextbox.TabIndex = 1;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 20;
            guna2Button1.BorderThickness = 1;
            guna2Button1.CustomizableEdges = customizableEdges11;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(136, 113, 205);
            guna2Button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(66, 372);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.PressedColor = Color.FloralWhite;
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges12;
            guna2Button1.Size = new Size(373, 45);
            guna2Button1.TabIndex = 2;
            guna2Button1.Text = "Log in";
            guna2Button1.Click += guna2Button1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(66, 17);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(74, 60);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.White;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(66, 132);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(495, 42);
            guna2HtmlLabel1.TabIndex = 5;
            guna2HtmlLabel1.Text = "Welcome! Please log in to continue.";
            // 
            // guna2HtmlLabel2
            // 
            guna2HtmlLabel2.BackColor = Color.White;
            guna2HtmlLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2HtmlLabel2.ForeColor = Color.DimGray;
            guna2HtmlLabel2.Location = new Point(66, 171);
            guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            guna2HtmlLabel2.Size = new Size(194, 23);
            guna2HtmlLabel2.TabIndex = 6;
            guna2HtmlLabel2.Text = "Log in to access the system.";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(6, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1309, 563);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1302, 563);
            ControlBox = false;
            Controls.Add(guna2HtmlLabel3);
            Controls.Add(guna2HtmlLabel2);
            Controls.Add(guna2HtmlLabel1);
            Controls.Add(pictureBox2);
            Controls.Add(guna2Button1);
            Controls.Add(passwordTextbox);
            Controls.Add(usernameTextbox);
            Controls.Add(pictureBox1);
            Name = "Login";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
        private Guna.UI2.WinForms.Guna2TextBox usernameTextbox;
        private Guna.UI2.WinForms.Guna2TextBox passwordTextbox;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private PictureBox pictureBox2;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private PictureBox pictureBox1;
    }
}
