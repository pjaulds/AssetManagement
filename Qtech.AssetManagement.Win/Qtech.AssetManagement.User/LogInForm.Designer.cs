namespace Qtech.AssetManagement.User
{
    partial class LogInForm
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
            this.LogInButton = new System.Windows.Forms.Button();
            this.PasswordtextBox = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.UserNametextBox = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordtextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNametextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LogInButton
            // 
            this.LogInButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.LogInButton.FlatAppearance.BorderSize = 0;
            this.LogInButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogInButton.Location = new System.Drawing.Point(359, 211);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(53, 21);
            this.LogInButton.TabIndex = 5;
            this.LogInButton.Text = "Log In";
            this.LogInButton.UseVisualStyleBackColor = false;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // PasswordtextBox
            // 
            this.PasswordtextBox.Location = new System.Drawing.Point(205, 211);
            this.PasswordtextBox.Name = "PasswordtextBox";
            this.PasswordtextBox.NullText = "Password";
            this.PasswordtextBox.PasswordChar = '*';
            this.PasswordtextBox.Size = new System.Drawing.Size(152, 21);
            this.PasswordtextBox.TabIndex = 4;
            this.PasswordtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PasswordtextBox_KeyPress);
            // 
            // UserNametextBox
            // 
            this.UserNametextBox.Location = new System.Drawing.Point(51, 211);
            this.UserNametextBox.Name = "UserNametextBox";
            this.UserNametextBox.NullText = "Username";
            this.UserNametextBox.Size = new System.Drawing.Size(152, 21);
            this.UserNametextBox.TabIndex = 3;
            this.UserNametextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserNametextBox_KeyPress);
            // 
            // LogInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            //this.BackgroundImage = global::Qtech.AssetManagement.User.Properties.Resources.LogInBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(446, 254);
            this.Controls.Add(this.LogInButton);
            this.Controls.Add(this.PasswordtextBox);
            this.Controls.Add(this.UserNametextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "LogInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogInForm";
            this.Load += new System.EventHandler(this.LogInForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogInForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.PasswordtextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNametextBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LogInButton;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor PasswordtextBox;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor UserNametextBox;
    }
}