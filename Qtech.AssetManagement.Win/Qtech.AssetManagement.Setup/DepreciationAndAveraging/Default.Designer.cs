namespace Qtech.AssetManagement.Setup.DepreciationAndAveraging
{
    partial class Default
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.depreciationMethodUserControl1 = new Qtech.AssetManagement.Setup.DepreciationMethod.DepreciationMethodUserControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.averagingMethodUserControl1 = new Qtech.AssetManagement.Setup.AveragingMethod.AveragingMethodUserControl();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.depreciationMethodUserControl1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(118, 1);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(923, 482);
            // 
            // depreciationMethodUserControl1
            // 
            this.depreciationMethodUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.depreciationMethodUserControl1.Location = new System.Drawing.Point(0, 0);
            this.depreciationMethodUserControl1.Name = "depreciationMethodUserControl1";
            this.depreciationMethodUserControl1.Size = new System.Drawing.Size(923, 482);
            this.depreciationMethodUserControl1.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.averagingMethodUserControl1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(923, 482);
            // 
            // averagingMethodUserControl1
            // 
            this.averagingMethodUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.averagingMethodUserControl1.Location = new System.Drawing.Point(0, 0);
            this.averagingMethodUserControl1.Name = "averagingMethodUserControl1";
            this.averagingMethodUserControl1.Size = new System.Drawing.Size(923, 482);
            this.averagingMethodUserControl1.TabIndex = 0;
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(1044, 486);
            this.ultraTabControl1.TabIndex = 0;
            this.ultraTabControl1.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.LeftTop;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Depreciation Method";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Averaging Method";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl1.TextOrientation = Infragistics.Win.UltraWinTabs.TextOrientation.Horizontal;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(923, 482);
            // 
            // Default
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 486);
            this.Controls.Add(this.ultraTabControl1);
            this.Name = "Default";
            this.Text = "Depreciation & Averaging";
            this.Load += new System.EventHandler(this.Default_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private DepreciationMethod.DepreciationMethodUserControl depreciationMethodUserControl1;
        private AveragingMethod.AveragingMethodUserControl averagingMethodUserControl1;
    }
}