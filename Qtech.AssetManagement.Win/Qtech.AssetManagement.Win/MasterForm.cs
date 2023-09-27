using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Win
{
    public partial class MasterForm : Form
    {
        public MasterForm()
        {
            InitializeComponent();
        }

        private void ultraTabbedMdiManager1_TabClosed(object sender, Infragistics.Win.UltraWinTabbedMdi.MdiTabEventArgs e)
        {
            if (ultraTabbedMdiManager1.ActiveTab == null)
                BackgroundpictureBox.Visible = true;
        }

        private void ultraTabbedMdiManager1_TabDisplayed(object sender, Infragistics.Win.UltraWinTabbedMdi.MdiTabEventArgs e)
        {
            BackgroundpictureBox.Visible = false;
        }

        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tag = ((ToolStripMenuItem)sender).Tag.ToString();

            switch (tag)
            {
                case "User":
                    if (!AllowToAccess((Int32)Modules.User)) return;
                    User.Default userForm = null;
                    if ((userForm = (User.Default)IsFormAlreadyOpen(typeof(User.Default))) == null)
                    {
                        User.Default userFormChild = new User.Default();
                        userFormChild.MdiParent = this;
                        userFormChild.Show();
                    }
                    else userForm.Select();
                    break;
                case "AssetAccount":
                    if (!AllowToAccess((Int32)Modules.AssetAccount)) return;
                    Setup.AssetAccount.Default assetAccountForm = null;
                    if ((assetAccountForm = (Setup.AssetAccount.Default)IsFormAlreadyOpen(typeof(Setup.AssetAccount.Default))) == null)
                    {
                        Setup.AssetAccount.Default assetAccountFormChild = new Setup.AssetAccount.Default();
                        assetAccountFormChild.MdiParent = this;
                        assetAccountFormChild.Show();
                    }
                    else assetAccountForm.Select();
                    break;
                case "AccumulatedDepreciationAccount":
                    if (!AllowToAccess((Int32)Modules.AccumulatedDepreciationAccount)) return;
                    Setup.AccumulatedDepreciationAccount.Default accumulatedDepreciationAccountForm = null;
                    if ((accumulatedDepreciationAccountForm = (Setup.AccumulatedDepreciationAccount.Default)IsFormAlreadyOpen(typeof(Setup.AccumulatedDepreciationAccount.Default))) == null)
                    {
                        Setup.AccumulatedDepreciationAccount.Default accumulatedDepreciationAccountFormChild = new Setup.AccumulatedDepreciationAccount.Default();
                        accumulatedDepreciationAccountFormChild.MdiParent = this;
                        accumulatedDepreciationAccountFormChild.Show();
                    }
                    else accumulatedDepreciationAccountForm.Select();
                    break;
                case "DepreciationExpenseAccount":
                    if (!AllowToAccess((Int32)Modules.DepreciationExpenseAccount)) return;
                    Setup.DepreciationExpenseAccount.Default depreciationExpenseAccountForm = null;
                    if ((depreciationExpenseAccountForm = (Setup.DepreciationExpenseAccount.Default)IsFormAlreadyOpen(typeof(Setup.DepreciationExpenseAccount.Default))) == null)
                    {
                        Setup.DepreciationExpenseAccount.Default depreciationExpenseAccountFormChild = new Setup.DepreciationExpenseAccount.Default();
                        depreciationExpenseAccountFormChild.MdiParent = this;
                        depreciationExpenseAccountFormChild.Show();
                    }
                    else depreciationExpenseAccountForm.Select();
                    break;
                case "DepreciationMethod":
                    if (!AllowToAccess((Int32)Modules.DepreciationMethod)) return;
                    Setup.DepreciationMethod.Default depreciationMethodForm = null;
                    if ((depreciationMethodForm = (Setup.DepreciationMethod.Default)IsFormAlreadyOpen(typeof(Setup.DepreciationMethod.Default))) == null)
                    {
                        Setup.DepreciationMethod.Default depreciationMethodFormChild = new Setup.DepreciationMethod.Default();
                        depreciationMethodFormChild.MdiParent = this;
                        depreciationMethodFormChild.Show();
                    }
                    else depreciationMethodForm.Select();
                    break;
                case "AveragingMethod":
                    if (!AllowToAccess((Int32)Modules.AveragingMethod)) return;
                    Setup.AveragingMethod.Default averagingMethodForm = null;
                    if ((averagingMethodForm = (Setup.AveragingMethod.Default)IsFormAlreadyOpen(typeof(Setup.AveragingMethod.Default))) == null)
                    {
                        Setup.AveragingMethod.Default averagingMethodFormChild = new Setup.AveragingMethod.Default();
                        averagingMethodFormChild.MdiParent = this;
                        averagingMethodFormChild.Show();
                    }
                    else averagingMethodForm.Select();
                    break;
                case "FixedAssetSetting":
                    if (!AllowToAccess((Int32)Modules.FixedAssetSetting)) return;
                    Setup.FixedAssetSetting.Default fixedAssetSettingForm = null;
                    if ((fixedAssetSettingForm = (Setup.FixedAssetSetting.Default)IsFormAlreadyOpen(typeof(Setup.FixedAssetSetting.Default))) == null)
                    {
                        Setup.FixedAssetSetting.Default fixedAssetSettingFormChild = new Setup.FixedAssetSetting.Default();
                        fixedAssetSettingFormChild.MdiParent = this;
                        fixedAssetSettingFormChild.Show();
                    }
                    else fixedAssetSettingForm.Select();
                    break;
                case "FunctionalLocation":
                    if (!AllowToAccess((Int32)Modules.FunctionalLocation)) return;
                    Setup.FunctionalLocation.Default functionalLocationForm = null;
                    if ((functionalLocationForm = (Setup.FunctionalLocation.Default)IsFormAlreadyOpen(typeof(Setup.FunctionalLocation.Default))) == null)
                    {
                        Setup.FunctionalLocation.Default functionalLocationFormChild = new Setup.FunctionalLocation.Default();
                        functionalLocationFormChild.MdiParent = this;
                        functionalLocationFormChild.Show();
                    }
                    else functionalLocationForm.Select();
                    break;
                case "FixedAsset":
                    if (!AllowToAccess((Int32)Modules.FixedAsset)) return;
                    FixedAsset.Default fixedAssetForm = null;
                    if ((fixedAssetForm = (FixedAsset.Default)IsFormAlreadyOpen(typeof(FixedAsset.Default))) == null)
                    {
                        FixedAsset.Default fixedAssetFormChild = new FixedAsset.Default();
                        fixedAssetFormChild.MdiParent = this;
                        fixedAssetFormChild.Show();
                    }
                    else fixedAssetForm.Select();
                    break;
                case "ChartOfAccount":
                    if (!AllowToAccess((Int32)Modules.ChartOfAccount)) return;
                    Setup.ChartOfAccount.Default chartOfAccountForm = null;
                    if ((chartOfAccountForm = (Setup.ChartOfAccount.Default)IsFormAlreadyOpen(typeof(Setup.ChartOfAccount.Default))) == null)
                    {
                        Setup.ChartOfAccount.Default chartOfAccountFormChild = new Setup.ChartOfAccount.Default();
                        chartOfAccountFormChild.MdiParent = this;
                        chartOfAccountFormChild.Show();
                    }
                    else chartOfAccountForm.Select();
                    break;
                case "Personnel":
                    if (!AllowToAccess((Int32)Modules.Personnel)) return;
                    Maintenance.Personnel.Default personnelForm = null;
                    if ((personnelForm = (Maintenance.Personnel.Default)IsFormAlreadyOpen(typeof(Maintenance.Personnel.Default))) == null)
                    {
                        Maintenance.Personnel.Default personnelFormChild = new Maintenance.Personnel.Default();
                        personnelFormChild.MdiParent = this;
                        personnelFormChild.Show();
                    }
                    else personnelForm.Select();
                    break;
                case "Product":
                    if (!AllowToAccess((Int32)Modules.Product)) return;
                    Maintenance.Product.Default productForm = null;
                    if ((productForm = (Maintenance.Product.Default)IsFormAlreadyOpen(typeof(Maintenance.Product.Default))) == null)
                    {
                        Maintenance.Product.Default productFormChild = new Maintenance.Product.Default();
                        productFormChild.MdiParent = this;
                        productFormChild.Show();
                    }
                    else productForm.Select();
                    break;
                case "Supplier":
                    if (!AllowToAccess((Int32)Modules.Supplier)) return;
                    Maintenance.Supplier.Default supplierForm = null;
                    if ((supplierForm = (Maintenance.Supplier.Default)IsFormAlreadyOpen(typeof(Maintenance.Supplier.Default))) == null)
                    {
                        Maintenance.Supplier.Default supplierFormChild = new Maintenance.Supplier.Default();
                        supplierFormChild.MdiParent = this;
                        supplierFormChild.Show();
                    }
                    else supplierForm.Select();
                    break;
                case "PurchaseRequest":
                    if (!AllowToAccess((Int32)Modules.PurchaseRequest)) return;
                    Purchasing.PurchaseRequest.Default purchaseRequestForm = null;
                    if ((purchaseRequestForm = (Purchasing.PurchaseRequest.Default)IsFormAlreadyOpen(typeof(Purchasing.PurchaseRequest.Default))) == null)
                    {
                        Purchasing.PurchaseRequest.Default purchaseRequestFormChild = new Purchasing.PurchaseRequest.Default();
                        purchaseRequestFormChild.MdiParent = this;
                        purchaseRequestFormChild.Show();
                    }
                    else purchaseRequestForm.Select();
                    break;
                case "Quotation":
                    if (!AllowToAccess((Int32)Modules.Quotation)) return;
                    Purchasing.Quotation.Default quotationForm = null;
                    if ((quotationForm = (Purchasing.Quotation.Default)IsFormAlreadyOpen(typeof(Purchasing.Quotation.Default))) == null)
                    {
                        Purchasing.Quotation.Default quotationFormChild = new Purchasing.Quotation.Default();
                        quotationFormChild.MdiParent = this;
                        quotationFormChild.Show();
                    }
                    else quotationForm.Select();
                    break;
                case "PurchaseOrder":
                    if (!AllowToAccess((Int32)Modules.PurchaseOrder)) return;
                    Purchasing.PurchaseOrder.Default purchaseOrderForm = null;
                    if ((purchaseOrderForm = (Purchasing.PurchaseOrder.Default)IsFormAlreadyOpen(typeof(Purchasing.PurchaseOrder.Default))) == null)
                    {
                        Purchasing.PurchaseOrder.Default purchaseOrderFormChild = new Purchasing.PurchaseOrder.Default();
                        purchaseOrderFormChild.MdiParent = this;
                        purchaseOrderFormChild.Show();
                    }
                    else purchaseOrderForm.Select();
                    break;
                case "Receiving":
                    if (!AllowToAccess((Int32)Modules.Receiving)) return;
                    Purchasing.Receiving.Default receivingForm = null;
                    if ((receivingForm = (Purchasing.Receiving.Default)IsFormAlreadyOpen(typeof(Purchasing.Receiving.Default))) == null)
                    {
                        Purchasing.Receiving.Default receivingFormChild = new Purchasing.Receiving.Default();
                        receivingFormChild.MdiParent = this;
                        receivingFormChild.Show();
                    }
                    else receivingForm.Select();
                    break;
                case "Unit":
                    if (!AllowToAccess((Int32)Modules.Unit)) return;
                    Maintenance.Unit.Default unitForm = null;
                    if ((unitForm = (Maintenance.Unit.Default)IsFormAlreadyOpen(typeof(Maintenance.Unit.Default))) == null)
                    {
                        Maintenance.Unit.Default unitFormChild = new Maintenance.Unit.Default();
                        unitFormChild.MdiParent = this;
                        unitFormChild.Show();
                    }
                    else unitForm.Select();
                    break;
                case "CompanyProfile":
                    if (!AllowToAccess((Int32)Modules.CompanyProfile)) return;
                    Maintenance.CompanyProfile.Default companyProfileForm = null;
                    if ((companyProfileForm = (Maintenance.CompanyProfile.Default)IsFormAlreadyOpen(typeof(Maintenance.CompanyProfile.Default))) == null)
                    {
                        Maintenance.CompanyProfile.Default companyProfileFormChild = new Maintenance.CompanyProfile.Default();
                        companyProfileFormChild.MdiParent = this;
                        companyProfileFormChild.Show();
                    }
                    else companyProfileForm.Select();
                    break;
            }
        }

        private Boolean AllowToAccess(short moduleId)
        {
            UserAccessCriteria criteria = new UserAccessCriteria();
            criteria.mUserId = SessionUtil.mUser.mId;
            criteria.mModuleId = moduleId;

            bool returnValue = UserAccessManager.GetList(criteria)[0].mSelect;
            if (!returnValue)
                MessageBox.Show("You are not allowed to access.");

            return returnValue;
        }

        private void MasterForm_Load(object sender, EventArgs e)
        {
            User.LogInForm logInForm = new User.LogInForm();
            logInForm.ShowDialog();

            if (SessionUtil.mUser == null) Application.Exit();
            BackgroundpictureBox.Visible = true;
        }

        private void LogInForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICRUD frm_activeform = (ICRUD)this.ActiveMdiChild;
            frm_activeform.SaveRecords();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICRUD frmActiveform = (ICRUD)this.ActiveMdiChild;
            frmActiveform.NewRecord();
        }

        private void MasterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SessionUtil.mUser != null)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Cancel = true;
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICRUD frmActiveform = (ICRUD)this.ActiveMdiChild;
            frmActiveform.PrintRecords();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ultraTabbedMdiManager1_TabSelected(object sender, Infragistics.Win.UltraWinTabbedMdi.MdiTabEventArgs e)
        {
            IComboSelection myChildForm = this.ActiveMdiChild as IComboSelection;
            if (myChildForm != null)
                myChildForm.RefreshAllSelection();
        }
    }
}
