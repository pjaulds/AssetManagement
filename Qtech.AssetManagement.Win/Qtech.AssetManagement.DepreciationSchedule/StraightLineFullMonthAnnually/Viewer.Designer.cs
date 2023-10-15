namespace Qtech.AssetManagement.DepreciationSchedule.StraightLineFullMonthAnnually
{
    partial class Viewer
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.Fields = new Qtech.AssetManagement.DepreciationSchedule.StraightLineFullMonthAnnually.Fields();
            this.FieldsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Fields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "Fields";
            reportDataSource1.Value = this.FieldsBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Qtech.AssetManagement.DepreciationSchedule.StraightLineFullMonthAnnually.Straight" +
    "LineFullMonthAnnuallyRpt.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(284, 261);
            this.reportViewer1.TabIndex = 0;
            // 
            // Fields
            // 
            this.Fields.DataSetName = "Fields";
            this.Fields.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FieldsBindingSource
            // 
            this.FieldsBindingSource.DataMember = "Fields";
            this.FieldsBindingSource.DataSource = this.Fields;
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.reportViewer1);
            this.KeyPreview = true;
            this.Name = "Viewer";
            this.Text = "Straight Line-Full Month-Annually";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Viewer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Viewer_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Fields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FieldsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource FieldsBindingSource;
        private Fields Fields;
    }
}