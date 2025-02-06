namespace Qtech.AssetManagement.Utilities
{
    partial class ValidationListForm
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
            this.dataRepeater1 = new Microsoft.VisualBasic.PowerPacks.DataRepeater();
            this.label1 = new System.Windows.Forms.Label();
            this.Savebutton = new System.Windows.Forms.Button();
            this.Messagelabel = new System.Windows.Forms.Label();
            this.dataRepeater1.ItemTemplate.SuspendLayout();
            this.dataRepeater1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataRepeater1
            // 
            this.dataRepeater1.AllowUserToAddItems = false;
            this.dataRepeater1.AllowUserToDeleteItems = false;
            this.dataRepeater1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataRepeater1.ItemHeaderVisible = false;
            // 
            // dataRepeater1.ItemTemplate
            // 
            this.dataRepeater1.ItemTemplate.Controls.Add(this.label1);
            this.dataRepeater1.ItemTemplate.Size = new System.Drawing.Size(378, 40);
            this.dataRepeater1.Location = new System.Drawing.Point(0, 44);
            this.dataRepeater1.Name = "dataRepeater1";
            this.dataRepeater1.Size = new System.Drawing.Size(386, 240);
            this.dataRepeater1.TabIndex = 1;
            this.dataRepeater1.Text = "dataRepeater1";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Consolas", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(374, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "aaaAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\r\nAAAA\r\nAAA";
            // 
            // Savebutton
            // 
            this.Savebutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(64)))), ((int)(((byte)(245)))));
            this.Savebutton.FlatAppearance.BorderSize = 0;
            this.Savebutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Savebutton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Savebutton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Savebutton.Location = new System.Drawing.Point(299, 289);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(75, 23);
            this.Savebutton.TabIndex = 2;
            this.Savebutton.Text = "&OK";
            this.Savebutton.UseVisualStyleBackColor = false;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // Messagelabel
            // 
            this.Messagelabel.BackColor = System.Drawing.Color.White;
            this.Messagelabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Messagelabel.Font = new System.Drawing.Font("Segoe Condensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Messagelabel.ForeColor = System.Drawing.Color.Black;
            this.Messagelabel.Location = new System.Drawing.Point(0, 0);
            this.Messagelabel.Name = "Messagelabel";
            this.Messagelabel.Size = new System.Drawing.Size(386, 44);
            this.Messagelabel.TabIndex = 32;
            this.Messagelabel.Text = "Please complete all required fields before saving.  The following fields are miss" +
    "ing";
            this.Messagelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ValidationListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(386, 321);
            this.Controls.Add(this.Savebutton);
            this.Controls.Add(this.dataRepeater1);
            this.Controls.Add(this.Messagelabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "ValidationListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validation";
            this.Load += new System.EventHandler(this.ValidationListForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ValidationListForm_KeyDown);
            this.dataRepeater1.ItemTemplate.ResumeLayout(false);
            this.dataRepeater1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.VisualBasic.PowerPacks.DataRepeater dataRepeater1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Savebutton;
        private System.Windows.Forms.Label Messagelabel;
    }
}