namespace Qtech.AssetManagement.RepairAndMaintenance.WorkOrder
{
    partial class HoursUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HoursUserControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ItemsdataGridView = new System.Windows.Forms.DataGridView();
            this.mDelete = new System.Windows.Forms.DataGridViewImageColumn();
            this.mExpenseCategoryId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.mHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mRatePerHour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ItemsdataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ItemsdataGridView
            // 
            this.ItemsdataGridView.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(212)))), ((int)(((byte)(240)))));
            this.ItemsdataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ItemsdataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ItemsdataGridView.BackgroundColor = System.Drawing.Color.White;
            this.ItemsdataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(198)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemsdataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ItemsdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ItemsdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mDelete,
            this.mExpenseCategoryId,
            this.mHours,
            this.mRatePerHour});
            this.ItemsdataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemsdataGridView.EnableHeadersVisualStyles = false;
            this.ItemsdataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(226)))), ((int)(((byte)(243)))));
            this.ItemsdataGridView.Location = new System.Drawing.Point(0, 0);
            this.ItemsdataGridView.Name = "ItemsdataGridView";
            this.ItemsdataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(226)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemsdataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.ItemsdataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(226)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ItemsdataGridView.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.ItemsdataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemsdataGridView.Size = new System.Drawing.Size(916, 235);
            this.ItemsdataGridView.TabIndex = 182;
            this.ItemsdataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ItemsdataGridView_CellClick);
            this.ItemsdataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ItemsdataGridView_DataError);
            // 
            // mDelete
            // 
            this.mDelete.HeaderText = "";
            this.mDelete.Image = ((System.Drawing.Image)(resources.GetObject("mDelete.Image")));
            this.mDelete.Name = "mDelete";
            this.mDelete.Width = 25;
            // 
            // mExpenseCategoryId
            // 
            this.mExpenseCategoryId.DataPropertyName = "mExpenseCategoryId";
            this.mExpenseCategoryId.HeaderText = "Expense Category";
            this.mExpenseCategoryId.Name = "mExpenseCategoryId";
            this.mExpenseCategoryId.Width = 250;
            // 
            // mHours
            // 
            this.mHours.DataPropertyName = "mHours";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.mHours.DefaultCellStyle = dataGridViewCellStyle3;
            this.mHours.HeaderText = "Hours";
            this.mHours.Name = "mHours";
            this.mHours.Width = 150;
            // 
            // mRatePerHour
            // 
            this.mRatePerHour.DataPropertyName = "mRatePerHour";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.mRatePerHour.DefaultCellStyle = dataGridViewCellStyle4;
            this.mRatePerHour.HeaderText = "Rate Per Hour";
            this.mRatePerHour.Name = "mRatePerHour";
            // 
            // HoursUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ItemsdataGridView);
            this.Name = "HoursUserControl";
            this.Size = new System.Drawing.Size(916, 235);
            this.Load += new System.EventHandler(this.HoursUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ItemsdataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ItemsdataGridView;
        private System.Windows.Forms.DataGridViewImageColumn mDelete;
        private System.Windows.Forms.DataGridViewComboBoxColumn mExpenseCategoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn mHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn mRatePerHour;
    }
}
