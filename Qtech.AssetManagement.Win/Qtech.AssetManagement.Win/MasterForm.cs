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
                        fixedAssetFormChild.mIsDraft = true;
                        fixedAssetFormChild.Show();
                    }
                    else
                    {
                        fixedAssetForm.mIsDraft = true;
                        fixedAssetForm.mIsRegistered = false;
                        fixedAssetForm.LoadFixedAsset();
                        fixedAssetForm.Select();
                    }
                    break;
                case "FixedAssetRegistered":
                    if (!AllowToAccess((Int32)Modules.FixedAsset)) return;
                    FixedAsset.Default fixedAssetRegisteredForm = null;
                    if ((fixedAssetRegisteredForm = (FixedAsset.Default)IsFormAlreadyOpen(typeof(FixedAsset.Default))) == null)
                    {
                        FixedAsset.Default fixedAssetRegisteredFormChild = new FixedAsset.Default();
                        fixedAssetRegisteredFormChild.MdiParent = this;
                        fixedAssetRegisteredFormChild.mIsRegistered = true;
                        fixedAssetRegisteredFormChild.Show();
                    }
                    else
                    {
                        fixedAssetRegisteredForm.mIsDraft = false;
                        fixedAssetRegisteredForm.mIsRegistered = true;
                        fixedAssetRegisteredForm.LoadFixedAsset();
                        fixedAssetRegisteredForm.Select();
                    }
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
                case "PurchaseVoucher":
                    if (!AllowToAccess((Int32)Modules.PurchaseVoucher)) return;
                    Purchasing.PurchaseVoucher.Default purchaseVoucherForm = null;
                    if ((purchaseVoucherForm = (Purchasing.PurchaseVoucher.Default)IsFormAlreadyOpen(typeof(Purchasing.PurchaseVoucher.Default))) == null)
                    {
                        Purchasing.PurchaseVoucher.Default purchaseVoucherFormChild = new Purchasing.PurchaseVoucher.Default();
                        purchaseVoucherFormChild.MdiParent = this;
                        purchaseVoucherFormChild.Show();
                    }
                    else purchaseVoucherForm.Select();
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
                case "PaymentMode":
                    if (!AllowToAccess((Int32)Modules.PaymentMode)) return;
                    Setup.PaymentMode.Default paymentModeForm = null;
                    if ((paymentModeForm = (Setup.PaymentMode.Default)IsFormAlreadyOpen(typeof(Setup.PaymentMode.Default))) == null)
                    {
                        Setup.PaymentMode.Default paymentModeFormChild = new Setup.PaymentMode.Default();
                        paymentModeFormChild.MdiParent = this;
                        paymentModeFormChild.Show();
                    }
                    else paymentModeForm.Select();
                    break;
                case "AssetType":
                    if (!AllowToAccess((Int32)Modules.AssetType)) return;
                    Maintenance.AssetType.Default assetTypeForm = null;
                    if ((assetTypeForm = (Maintenance.AssetType.Default)IsFormAlreadyOpen(typeof(Maintenance.AssetType.Default))) == null)
                    {
                        Maintenance.AssetType.Default assetTypeFormChild = new Maintenance.AssetType.Default();
                        assetTypeFormChild.MdiParent = this;
                        assetTypeFormChild.Show();
                    }
                    else assetTypeForm.Select();
                    break;
                case "DepreciationSchedule":
                    if (!AllowToAccess((Int32)Modules.DepreciationSchedule)) return;
                    DepreciationSchedule.Default depreciationScheduleForm = null;
                    if ((depreciationScheduleForm = (DepreciationSchedule.Default)IsFormAlreadyOpen(typeof(DepreciationSchedule.Default))) == null)
                    {
                        DepreciationSchedule.Default depreciationScheduleFormChild = new DepreciationSchedule.Default();
                        depreciationScheduleFormChild.MdiParent = this;
                        depreciationScheduleFormChild.Show();
                    }
                    else depreciationScheduleForm.Select();
                    break;
                case "MaintenanceRequestType":
                    if (!AllowToAccess((Int32)Modules.MaintenanceRequestType)) return;
                    RepairAndMaintenance.MaintenanceRequestType.Default maintenanceRequestTypeForm = null;
                    if ((maintenanceRequestTypeForm = (RepairAndMaintenance.MaintenanceRequestType.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.MaintenanceRequestType.Default))) == null)
                    {
                        RepairAndMaintenance.MaintenanceRequestType.Default maintenanceRequestTypeFormChild = new RepairAndMaintenance.MaintenanceRequestType.Default();
                        maintenanceRequestTypeFormChild.MdiParent = this;
                        maintenanceRequestTypeFormChild.Show();
                    }
                    else maintenanceRequestTypeForm.Select();
                    break;
                case "ServiceLevel":
                    if (!AllowToAccess((Int32)Modules.ServiceLevel)) return;
                    RepairAndMaintenance.ServiceLevel.Default serviceLevelForm = null;
                    if ((serviceLevelForm = (RepairAndMaintenance.ServiceLevel.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.ServiceLevel.Default))) == null)
                    {
                        RepairAndMaintenance.ServiceLevel.Default serviceLevelFormChild = new RepairAndMaintenance.ServiceLevel.Default();
                        serviceLevelFormChild.MdiParent = this;
                        serviceLevelFormChild.Show();
                    }
                    else serviceLevelForm.Select();
                    break;
                case "FaultSymptoms":
                    if (!AllowToAccess((Int32)Modules.FaultSymptoms)) return;
                    RepairAndMaintenance.FaultSymptoms.Default faultSymptomsForm = null;
                    if ((faultSymptomsForm = (RepairAndMaintenance.FaultSymptoms.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.FaultSymptoms.Default))) == null)
                    {
                        RepairAndMaintenance.FaultSymptoms.Default faultSymptomsFormChild = new RepairAndMaintenance.FaultSymptoms.Default();
                        faultSymptomsFormChild.MdiParent = this;
                        faultSymptomsFormChild.Show();
                    }
                    else faultSymptomsForm.Select();
                    break;
                case "FaultArea":
                    if (!AllowToAccess((Int32)Modules.FaultArea)) return;
                    RepairAndMaintenance.FaultArea.Default faultAreaForm = null;
                    if ((faultAreaForm = (RepairAndMaintenance.FaultArea.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.FaultArea.Default))) == null)
                    {
                        RepairAndMaintenance.FaultArea.Default faultAreaFormChild = new RepairAndMaintenance.FaultArea.Default();
                        faultAreaFormChild.MdiParent = this;
                        faultAreaFormChild.Show();
                    }
                    else faultAreaForm.Select();
                    break;
                case "MaintenanceRequest":
                    if (!AllowToAccess((Int32)Modules.MaintenanceRequest)) return;
                    RepairAndMaintenance.MaintenanceRequest.Default maintenanceRequestForm = null;
                    if ((maintenanceRequestForm = (RepairAndMaintenance.MaintenanceRequest.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.MaintenanceRequest.Default))) == null)
                    {
                        RepairAndMaintenance.MaintenanceRequest.Default maintenanceRequestFormChild = new RepairAndMaintenance.MaintenanceRequest.Default();
                        maintenanceRequestFormChild.MdiParent = this;
                        maintenanceRequestFormChild.Show();
                    }
                    else maintenanceRequestForm.Select();
                    break;
                case "WorkOrderType":
                    if (!AllowToAccess((Int32)Modules.WorkOrderType)) return;
                    RepairAndMaintenance.WorkOrderType.Default workOrderTypeForm = null;
                    if ((workOrderTypeForm = (RepairAndMaintenance.WorkOrderType.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.WorkOrderType.Default))) == null)
                    {
                        RepairAndMaintenance.WorkOrderType.Default workOrderTypeFormChild = new RepairAndMaintenance.WorkOrderType.Default();
                        workOrderTypeFormChild.MdiParent = this;
                        workOrderTypeFormChild.Show();
                    }
                    else workOrderTypeForm.Select();
                    break;
                case "MaintenanceJobType":
                    if (!AllowToAccess((Int32)Modules.MaintenanceJobType)) return;
                    RepairAndMaintenance.MaintenanceJobType.Default maintenanceJobTypeForm = null;
                    if ((maintenanceJobTypeForm = (RepairAndMaintenance.MaintenanceJobType.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.MaintenanceJobType.Default))) == null)
                    {
                        RepairAndMaintenance.MaintenanceJobType.Default maintenanceJobTypeFormChild = new RepairAndMaintenance.MaintenanceJobType.Default();
                        maintenanceJobTypeFormChild.MdiParent = this;
                        maintenanceJobTypeFormChild.Show();
                    }
                    else maintenanceJobTypeForm.Select();
                    break;
                case "Trade":
                    if (!AllowToAccess((Int32)Modules.Trade)) return;
                    RepairAndMaintenance.Trade.Default tradeForm = null;
                    if ((tradeForm = (RepairAndMaintenance.Trade.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.Trade.Default))) == null)
                    {
                        RepairAndMaintenance.Trade.Default tradeFormChild = new RepairAndMaintenance.Trade.Default();
                        tradeFormChild.MdiParent = this;
                        tradeFormChild.Show();
                    }
                    else tradeForm.Select();
                    break;
                case "WorkOrder":
                    if (!AllowToAccess((Int32)Modules.WorkOrder)) return;
                    RepairAndMaintenance.WorkOrder.Default workOrderForm = null;
                    if ((workOrderForm = (RepairAndMaintenance.WorkOrder.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.WorkOrder.Default))) == null)
                    {
                        RepairAndMaintenance.WorkOrder.Default workOrderFormChild = new RepairAndMaintenance.WorkOrder.Default();
                        workOrderFormChild.MdiParent = this;
                        workOrderFormChild.Show();
                    }
                    else workOrderForm.Select();
                    break;
                case "ExpenseCategory":
                    if (!AllowToAccess((Int32)Modules.ExpenseCategory)) return;
                    RepairAndMaintenance.ExpenseCategory.Default expenseCategoryForm = null;
                    if ((expenseCategoryForm = (RepairAndMaintenance.ExpenseCategory.Default)IsFormAlreadyOpen(typeof(RepairAndMaintenance.ExpenseCategory.Default))) == null)
                    {
                        RepairAndMaintenance.ExpenseCategory.Default expenseCategoryFormChild = new RepairAndMaintenance.ExpenseCategory.Default();
                        expenseCategoryFormChild.MdiParent = this;
                        expenseCategoryFormChild.Show();
                    }
                    else expenseCategoryForm.Select();
                    break;
                case "Currency":
                    if (!AllowToAccess((Int32)Modules.Currency)) return;
                    Maintenance.Currency.Default currencyForm = null;
                    if ((currencyForm = (Maintenance.Currency.Default)IsFormAlreadyOpen(typeof(Maintenance.Currency.Default))) == null)
                    {
                        Maintenance.Currency.Default currencyFormChild = new Maintenance.Currency.Default();
                        currencyFormChild.MdiParent = this;
                        currencyFormChild.Show();
                    }
                    else currencyForm.Select();
                    break;
                case "PaymentTerms":
                    if (!AllowToAccess((Int32)Modules.PaymentTerms)) return;
                    Maintenance.PaymentTerms.Default paymentTermsForm = null;
                    if ((paymentTermsForm = (Maintenance.PaymentTerms.Default)IsFormAlreadyOpen(typeof(Maintenance.PaymentTerms.Default))) == null)
                    {
                        Maintenance.PaymentTerms.Default paymentTermsFormChild = new Maintenance.PaymentTerms.Default();
                        paymentTermsFormChild.MdiParent = this;
                        paymentTermsFormChild.Show();
                    }
                    else paymentTermsForm.Select();
                    break;
                case "AssetCategory":
                    if (!AllowToAccess((Int32)Modules.AssetCategory)) return;
                    Maintenance.AssetCategory.Default assetCategoryForm = null;
                    if ((assetCategoryForm = (Maintenance.AssetCategory.Default)IsFormAlreadyOpen(typeof(Maintenance.AssetCategory.Default))) == null)
                    {
                        Maintenance.AssetCategory.Default assetCategoryFormChild = new Maintenance.AssetCategory.Default();
                        assetCategoryFormChild.MdiParent = this;
                        assetCategoryFormChild.Show();
                    }
                    else assetCategoryForm.Select();
                    break;
                case "AssetClass":
                    if (!AllowToAccess((Int32)Modules.AssetClass)) return;
                    Maintenance.AssetClass.Default assetClassForm = null;
                    if ((assetClassForm = (Maintenance.AssetClass.Default)IsFormAlreadyOpen(typeof(Maintenance.AssetClass.Default))) == null)
                    {
                        Maintenance.AssetClass.Default assetClassFormChild = new Maintenance.AssetClass.Default();
                        assetClassFormChild.MdiParent = this;
                        assetClassFormChild.Show();
                    }
                    else assetClassForm.Select();
                    break;
                case "AccountType":
                    if (!AllowToAccess((Int32)Modules.AccountType)) return;
                    Maintenance.AccountType.Default accountTypeForm = null;
                    if ((accountTypeForm = (Maintenance.AccountType.Default)IsFormAlreadyOpen(typeof(Maintenance.AccountType.Default))) == null)
                    {
                        Maintenance.AccountType.Default accountTypeFormChild = new Maintenance.AccountType.Default();
                        accountTypeFormChild.MdiParent = this;
                        accountTypeFormChild.Show();
                    }
                    else accountTypeForm.Select();
                    break;
                case "AccountClassification":
                    if (!AllowToAccess((Int32)Modules.AccountClassification)) return;
                    Maintenance.AccountClassification.Default accountClassificationForm = null;
                    if ((accountClassificationForm = (Maintenance.AccountClassification.Default)IsFormAlreadyOpen(typeof(Maintenance.AccountClassification.Default))) == null)
                    {
                        Maintenance.AccountClassification.Default accountClassificationFormChild = new Maintenance.AccountClassification.Default();
                        accountClassificationFormChild.MdiParent = this;
                        accountClassificationFormChild.Show();
                    }
                    else accountClassificationForm.Select();
                    break;
                case "AccountGroup":
                    if (!AllowToAccess((Int32)Modules.AccountGroup)) return;
                    Maintenance.AccountGroup.Default accountGroupForm = null;
                    if ((accountGroupForm = (Maintenance.AccountGroup.Default)IsFormAlreadyOpen(typeof(Maintenance.AccountGroup.Default))) == null)
                    {
                        Maintenance.AccountGroup.Default accountGroupFormChild = new Maintenance.AccountGroup.Default();
                        accountGroupFormChild.MdiParent = this;
                        accountGroupFormChild.Show();
                    }
                    else accountGroupForm.Select();
                    break;
                case "CapitalizedCost":
                    if (!AllowToAccess((Int32)Modules.CapitalizedCost)) return;
                    Maintenance.CapitalizedCost.Default capitalizedCostForm = null;
                    if ((capitalizedCostForm = (Maintenance.CapitalizedCost.Default)IsFormAlreadyOpen(typeof(Maintenance.CapitalizedCost.Default))) == null)
                    {
                        Maintenance.CapitalizedCost.Default capitalizedCostFormChild = new Maintenance.CapitalizedCost.Default();
                        capitalizedCostFormChild.MdiParent = this;
                        capitalizedCostFormChild.Show();
                    }
                    else capitalizedCostForm.Select();
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
