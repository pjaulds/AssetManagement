namespace Qtech.AssetManagement.User
{
    partial class OverrideLogInForm
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
            this.PasswordtextBox = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.UserNametextBox = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordtextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNametextBox)).BeginInit();
            this.panel14.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PasswordtextBox
            // 
            this.PasswordtextBox.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.PasswordtextBox.Location = new System.Drawing.Point(21, 60);
            this.PasswordtextBox.Name = "PasswordtextBox";
            this.PasswordtextBox.NullText = "Password";
            this.PasswordtextBox.PasswordChar = '*';
            this.PasswordtextBox.Size = new System.Drawing.Size(220, 22);
            this.PasswordtextBox.TabIndex = 4;
            this.PasswordtextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PasswordtextBox_KeyPress);
            // 
            // UserNametextBox
            // 
            this.UserNametextBox.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.UserNametextBox.Location = new System.Drawing.Point(21, 33);
            this.UserNametextBox.Name = "UserNametextBox";
            this.UserNametextBox.NullText = "Username";
            this.UserNametextBox.Size = new System.Drawing.Size(220, 22);
            this.UserNametextBox.TabIndex = 3;
            this.UserNametextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserNametextBox_KeyPress);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(43)))), ((int)(((byte)(137)))));
            this.Cancelbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancelbutton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancelbutton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Cancelbutton.Location = new System.Drawing.Point(164, 88);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(77, 23);
            this.Cancelbutton.TabIndex = 17;
            this.Cancelbutton.Text = "&Login";
            this.Cancelbutton.UseVisualStyleBackColor = false;
            this.Cancelbutton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel14.Controls.Add(this.label15);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(288, 21);
            this.panel14.TabIndex = 185;
            this.panel14.Tag = "headerpanel";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label15.Dock = System.Windows.Forms.DockStyle.Right;
            this.label15.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(270, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(14, 13);
            this.label15.TabIndex = 24;
            this.label15.Tag = "";
            this.label15.Text = "X";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UserNametextBox);
            this.groupBox1.Controls.Add(this.PasswordtextBox);
            this.groupBox1.Controls.Add(this.Cancelbutton);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.Gray;
            this.groupBox1.Location = new System.Drawing.Point(12, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 130);
            this.groupBox1.TabIndex = 186;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login Credentials";
            // 
            // OverrideLogInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(288, 178);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "OverrideLogInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogInForm";
            this.Load += new System.EventHandler(this.LogInForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogInForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.PasswordtextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNametextBox)).EndInit();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.UltraWinEditors.UltraTextEditor PasswordtextBox;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor UserNametextBox;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}