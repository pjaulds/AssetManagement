namespace Qtech.AssetManagement.FixedAsset
{
    partial class RunDepreciationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunDepreciationForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NametextBox = new System.Windows.Forms.TextBox();
            this.MonthcomboBox = new System.Windows.Forms.ComboBox();
            this.YearnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Savebutton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YearnumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(365, 45);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "RUN DEPRECIATION";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(336, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 68);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Asset Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Month";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 122);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Year";
            // 
            // NametextBox
            // 
            this.NametextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NametextBox.Location = new System.Drawing.Point(103, 65);
            this.NametextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.NametextBox.MaxLength = 150;
            this.NametextBox.Name = "NametextBox";
            this.NametextBox.ReadOnly = true;
            this.NametextBox.Size = new System.Drawing.Size(250, 21);
            this.NametextBox.TabIndex = 12;
            // 
            // MonthcomboBox
            // 
            this.MonthcomboBox.FormattingEnabled = true;
            this.MonthcomboBox.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.MonthcomboBox.Location = new System.Drawing.Point(103, 92);
            this.MonthcomboBox.Name = "MonthcomboBox";
            this.MonthcomboBox.Size = new System.Drawing.Size(250, 21);
            this.MonthcomboBox.TabIndex = 13;
            // 
            // YearnumericUpDown
            // 
            this.YearnumericUpDown.Location = new System.Drawing.Point(103, 122);
            this.YearnumericUpDown.Maximum = new decimal(new int[] {
            2999,
            0,
            0,
            0});
            this.YearnumericUpDown.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.YearnumericUpDown.Name = "YearnumericUpDown";
            this.YearnumericUpDown.Size = new System.Drawing.Size(250, 20);
            this.YearnumericUpDown.TabIndex = 14;
            this.YearnumericUpDown.Value = new decimal(new int[] {
            2023,
            0,
            0,
            0});
            // 
            // Savebutton
            // 
            this.Savebutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(43)))), ((int)(((byte)(137)))));
            this.Savebutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Savebutton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Savebutton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Savebutton.Location = new System.Drawing.Point(233, 148);
            this.Savebutton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(120, 23);
            this.Savebutton.TabIndex = 16;
            this.Savebutton.Text = "Run Depreciation";
            this.Savebutton.UseVisualStyleBackColor = false;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // RunDepreciationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(365, 186);
            this.Controls.Add(this.Savebutton);
            this.Controls.Add(this.YearnumericUpDown);
            this.Controls.Add(this.MonthcomboBox);
            this.Controls.Add(this.NametextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "RunDepreciationForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Run Depreciation";
            this.Load += new System.EventHandler(this.RunDepreciationForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RunDepreciationForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YearnumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NametextBox;
        private System.Windows.Forms.ComboBox MonthcomboBox;
        private System.Windows.Forms.NumericUpDown YearnumericUpDown;
        private System.Windows.Forms.Button Savebutton;
    }
}