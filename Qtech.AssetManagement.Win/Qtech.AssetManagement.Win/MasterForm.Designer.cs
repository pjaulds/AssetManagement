namespace Qtech.AssetManagement.Win
{
    partial class MasterForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maintenanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personnelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supplierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedAssetSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.assetAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accumulatedDepreciationAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depreciationExpenseAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depreciationMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.averagingMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartOfAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedAssetManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedAssetProcurementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetIndentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purchaseOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedAssetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionalLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraTabbedMdiManager1 = new Infragistics.Win.UltraWinTabbedMdi.UltraTabbedMdiManager(this.components);
            this.BackgroundpictureBox = new System.Windows.Forms.PictureBox();
            this.goodReceivedNoteReceivingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundpictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.maintenanceToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.fixedAssetManagementToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(673, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator3,
            this.printToolStripMenuItem,
            this.toolStripSeparator5,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(138, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.printToolStripMenuItem.Text = "&Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(138, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // maintenanceToolStripMenuItem
            // 
            this.maintenanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemUsersToolStripMenuItem,
            this.personnelToolStripMenuItem,
            this.productToolStripMenuItem,
            this.supplierToolStripMenuItem});
            this.maintenanceToolStripMenuItem.Name = "maintenanceToolStripMenuItem";
            this.maintenanceToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.maintenanceToolStripMenuItem.Text = "Maintenance";
            // 
            // systemUsersToolStripMenuItem
            // 
            this.systemUsersToolStripMenuItem.Name = "systemUsersToolStripMenuItem";
            this.systemUsersToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.systemUsersToolStripMenuItem.Tag = "User";
            this.systemUsersToolStripMenuItem.Text = "1. System Users";
            this.systemUsersToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // personnelToolStripMenuItem
            // 
            this.personnelToolStripMenuItem.Name = "personnelToolStripMenuItem";
            this.personnelToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.personnelToolStripMenuItem.Tag = "Personnel";
            this.personnelToolStripMenuItem.Text = "2. Personnel";
            this.personnelToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // productToolStripMenuItem
            // 
            this.productToolStripMenuItem.Name = "productToolStripMenuItem";
            this.productToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.productToolStripMenuItem.Tag = "Product";
            this.productToolStripMenuItem.Text = "3. Product";
            this.productToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // supplierToolStripMenuItem
            // 
            this.supplierToolStripMenuItem.Name = "supplierToolStripMenuItem";
            this.supplierToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.supplierToolStripMenuItem.Tag = "Supplier";
            this.supplierToolStripMenuItem.Text = "4. Supplier";
            this.supplierToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fixedAssetSettingToolStripMenuItem,
            this.toolStripMenuItem1,
            this.assetAccountToolStripMenuItem,
            this.accumulatedDepreciationAccountToolStripMenuItem,
            this.depreciationExpenseAccountToolStripMenuItem,
            this.depreciationMethodToolStripMenuItem,
            this.averagingMethodToolStripMenuItem,
            this.chartOfAccountToolStripMenuItem});
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            // 
            // fixedAssetSettingToolStripMenuItem
            // 
            this.fixedAssetSettingToolStripMenuItem.Name = "fixedAssetSettingToolStripMenuItem";
            this.fixedAssetSettingToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.fixedAssetSettingToolStripMenuItem.Tag = "FixedAssetSetting";
            this.fixedAssetSettingToolStripMenuItem.Text = "1. Fixed Asset Setting";
            this.fixedAssetSettingToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(272, 6);
            // 
            // assetAccountToolStripMenuItem
            // 
            this.assetAccountToolStripMenuItem.Name = "assetAccountToolStripMenuItem";
            this.assetAccountToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.assetAccountToolStripMenuItem.Tag = "AssetAccount";
            this.assetAccountToolStripMenuItem.Text = "2. Asset Account";
            this.assetAccountToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // accumulatedDepreciationAccountToolStripMenuItem
            // 
            this.accumulatedDepreciationAccountToolStripMenuItem.Name = "accumulatedDepreciationAccountToolStripMenuItem";
            this.accumulatedDepreciationAccountToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.accumulatedDepreciationAccountToolStripMenuItem.Tag = "AccumulatedDepreciationAccount";
            this.accumulatedDepreciationAccountToolStripMenuItem.Text = "3. Accumulated Depreciation Account";
            this.accumulatedDepreciationAccountToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // depreciationExpenseAccountToolStripMenuItem
            // 
            this.depreciationExpenseAccountToolStripMenuItem.Name = "depreciationExpenseAccountToolStripMenuItem";
            this.depreciationExpenseAccountToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.depreciationExpenseAccountToolStripMenuItem.Tag = "DepreciationExpenseAccount";
            this.depreciationExpenseAccountToolStripMenuItem.Text = "4. Depreciation Expense Account";
            this.depreciationExpenseAccountToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // depreciationMethodToolStripMenuItem
            // 
            this.depreciationMethodToolStripMenuItem.Name = "depreciationMethodToolStripMenuItem";
            this.depreciationMethodToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.depreciationMethodToolStripMenuItem.Tag = "DepreciationMethod";
            this.depreciationMethodToolStripMenuItem.Text = "5. Depreciation Method";
            this.depreciationMethodToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // averagingMethodToolStripMenuItem
            // 
            this.averagingMethodToolStripMenuItem.Name = "averagingMethodToolStripMenuItem";
            this.averagingMethodToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.averagingMethodToolStripMenuItem.Tag = "AveragingMethod";
            this.averagingMethodToolStripMenuItem.Text = "6. Averaging Method";
            this.averagingMethodToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // chartOfAccountToolStripMenuItem
            // 
            this.chartOfAccountToolStripMenuItem.Name = "chartOfAccountToolStripMenuItem";
            this.chartOfAccountToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.chartOfAccountToolStripMenuItem.Tag = "ChartOfAccount";
            this.chartOfAccountToolStripMenuItem.Text = "7. Chart Of Account";
            this.chartOfAccountToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // fixedAssetManagementToolStripMenuItem
            // 
            this.fixedAssetManagementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fixedAssetProcurementToolStripMenuItem,
            this.fixedAssetsToolStripMenuItem,
            this.functionalLocationsToolStripMenuItem});
            this.fixedAssetManagementToolStripMenuItem.Name = "fixedAssetManagementToolStripMenuItem";
            this.fixedAssetManagementToolStripMenuItem.Size = new System.Drawing.Size(152, 20);
            this.fixedAssetManagementToolStripMenuItem.Text = "Fixed Asset Management";
            // 
            // fixedAssetProcurementToolStripMenuItem
            // 
            this.fixedAssetProcurementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assetIndentToolStripMenuItem,
            this.quotationToolStripMenuItem,
            this.purchaseOrderToolStripMenuItem,
            this.goodReceivedNoteReceivingToolStripMenuItem});
            this.fixedAssetProcurementToolStripMenuItem.Name = "fixedAssetProcurementToolStripMenuItem";
            this.fixedAssetProcurementToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.fixedAssetProcurementToolStripMenuItem.Text = "Fixed Asset Procurement";
            // 
            // assetIndentToolStripMenuItem
            // 
            this.assetIndentToolStripMenuItem.Name = "assetIndentToolStripMenuItem";
            this.assetIndentToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.assetIndentToolStripMenuItem.Tag = "PurchaseRequest";
            this.assetIndentToolStripMenuItem.Text = "1. Purchase Request";
            this.assetIndentToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // quotationToolStripMenuItem
            // 
            this.quotationToolStripMenuItem.Name = "quotationToolStripMenuItem";
            this.quotationToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.quotationToolStripMenuItem.Tag = "Quotation";
            this.quotationToolStripMenuItem.Text = "2. Quotation";
            this.quotationToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // purchaseOrderToolStripMenuItem
            // 
            this.purchaseOrderToolStripMenuItem.Name = "purchaseOrderToolStripMenuItem";
            this.purchaseOrderToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.purchaseOrderToolStripMenuItem.Tag = "PurchaseOrder";
            this.purchaseOrderToolStripMenuItem.Text = "3. Purchase Order";
            this.purchaseOrderToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // fixedAssetsToolStripMenuItem
            // 
            this.fixedAssetsToolStripMenuItem.Name = "fixedAssetsToolStripMenuItem";
            this.fixedAssetsToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.fixedAssetsToolStripMenuItem.Tag = "FixedAsset";
            this.fixedAssetsToolStripMenuItem.Text = "Fixed Assets";
            this.fixedAssetsToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // functionalLocationsToolStripMenuItem
            // 
            this.functionalLocationsToolStripMenuItem.Name = "functionalLocationsToolStripMenuItem";
            this.functionalLocationsToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.functionalLocationsToolStripMenuItem.Tag = "FunctionalLocation";
            this.functionalLocationsToolStripMenuItem.Text = "Functional Locations";
            this.functionalLocationsToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // ultraTabbedMdiManager1
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Segoe UI Semibold";
            appearance1.FontData.SizeInPoints = 10F;
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraTabbedMdiManager1.Appearance = appearance1;
            this.ultraTabbedMdiManager1.MdiParent = this;
            this.ultraTabbedMdiManager1.TabGroupSettings.CloseButtonLocation = Infragistics.Win.UltraWinTabs.TabCloseButtonLocation.Tab;
            this.ultraTabbedMdiManager1.TabGroupSettings.TabStyle = Infragistics.Win.UltraWinTabs.TabStyle.PropertyPage2003;
            this.ultraTabbedMdiManager1.TabNavigationMode = Infragistics.Win.UltraWinTabbedMdi.MdiTabNavigationMode.VisibleOrder;
            this.ultraTabbedMdiManager1.TabSettings.AllowClose = Infragistics.Win.DefaultableBoolean.True;
            this.ultraTabbedMdiManager1.TabSettings.CloseButtonAlignment = Infragistics.Win.UltraWinTabs.TabCloseButtonAlignment.AfterContent;
            this.ultraTabbedMdiManager1.TabSettings.CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Always;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(106)))), ((int)(((byte)(18)))));
            this.ultraTabbedMdiManager1.TabSettings.SelectedTabAppearance = appearance2;
            this.ultraTabbedMdiManager1.TabClosed += new Infragistics.Win.UltraWinTabbedMdi.MdiTabEventHandler(this.ultraTabbedMdiManager1_TabClosed);
            this.ultraTabbedMdiManager1.TabSelected += new Infragistics.Win.UltraWinTabbedMdi.MdiTabEventHandler(this.ultraTabbedMdiManager1_TabSelected);
            this.ultraTabbedMdiManager1.TabDisplayed += new Infragistics.Win.UltraWinTabbedMdi.MdiTabEventHandler(this.ultraTabbedMdiManager1_TabDisplayed);
            // 
            // BackgroundpictureBox
            // 
            this.BackgroundpictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackgroundpictureBox.Location = new System.Drawing.Point(0, 24);
            this.BackgroundpictureBox.Name = "BackgroundpictureBox";
            this.BackgroundpictureBox.Size = new System.Drawing.Size(673, 409);
            this.BackgroundpictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BackgroundpictureBox.TabIndex = 4;
            this.BackgroundpictureBox.TabStop = false;
            this.BackgroundpictureBox.Visible = false;
            // 
            // goodReceivedNoteReceivingToolStripMenuItem
            // 
            this.goodReceivedNoteReceivingToolStripMenuItem.Name = "goodReceivedNoteReceivingToolStripMenuItem";
            this.goodReceivedNoteReceivingToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.goodReceivedNoteReceivingToolStripMenuItem.Tag = "Receiving";
            this.goodReceivedNoteReceivingToolStripMenuItem.Text = "4. Good Received Note (Receiving)";
            this.goodReceivedNoteReceivingToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(673, 433);
            this.Controls.Add(this.BackgroundpictureBox);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Name = "MasterForm";
            this.Text = "ASSET MANAGEMENT SYSTEM";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterForm_FormClosing);
            this.Load += new System.EventHandler(this.MasterForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundpictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PictureBox BackgroundpictureBox;
        private Infragistics.Win.UltraWinTabbedMdi.UltraTabbedMdiManager ultraTabbedMdiManager1;
        private System.Windows.Forms.ToolStripMenuItem maintenanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemUsersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedAssetSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem assetAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accumulatedDepreciationAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depreciationExpenseAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depreciationMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem averagingMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedAssetManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem functionalLocationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedAssetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartOfAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personnelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supplierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedAssetProcurementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assetIndentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quotationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purchaseOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goodReceivedNoteReceivingToolStripMenuItem;
    }
}

