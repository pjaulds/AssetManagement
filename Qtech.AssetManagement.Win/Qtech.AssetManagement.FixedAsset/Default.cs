using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Utilities;
using Qtech.AssetManagement.Validation;
using Qtech.Qasa.PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.FixedAsset
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

        public bool mFromBrowse { get; set; }
        public BusinessEntities.FixedAsset mFixedAsset { get; set; }
        public bool mIsDraft { get; set; }
        public bool mIsRegistered { get; set; }

        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;
        #endregion

        public IPluginHost PluginHost { get; set; }
        public IPlugin Plugin { get; set; }

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.FixedAsset)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        public void LoadFixedAsset()
        {
            FixedAssetCriteria criteria = new FixedAssetCriteria();
            criteria.mIsDraft = mIsDraft;
            criteria.mIsRegistered = mIsRegistered;
            ultraGrid1.SetDataBinding(FixedAssetManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();

            criteria = new FixedAssetCriteria();
            criteria.mIsRegistered = true;
            Registeredbutton.Text = "Registered" + Environment.NewLine + FixedAssetManager.SelectCountForGetList(criteria).ToString();
            Totalbutton.Text = "Total" + Environment.NewLine + FixedAssetManager.SelectCountForGetList(new FixedAssetCriteria()).ToString();
        }

        private int SaveFixedAsset()
        {
            BusinessEntities.FixedAsset item = new BusinessEntities.FixedAsset();
            LoadFixedAssetFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = FixedAssetManager.Save(item);
                EndEditing();
                LoadFixedAsset();

                return id;

            }
            else
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = item.BrokenRules;
                validationForm.ShowDialog();
                return 0;
            }
        }

        private void LoadFixedAssetFromFormControls(BusinessEntities.FixedAsset myFixedAsset)
        {
            myFixedAsset.mId = int.Parse(Idlabel.Text);
            myFixedAsset.mAssetNo = AssetNotextBox.Text;
            myFixedAsset.mProductId = int.Parse(ProductIdlabel.Text);
            myFixedAsset.mReceivingDetailId = int.Parse(ReceivingDetailIdlabel.Text);
            myFixedAsset.mAssetTypeId = ControlUtil.UltraComboReturnValue(AssetTypeutraCombo);
            myFixedAsset.mAssetTypeName = AssetTypeutraCombo.Text;
            myFixedAsset.mFunctionalLocationId = ControlUtil.UltraComboReturnValue(FunctionalLocationultraCombo);
            myFixedAsset.mFunctionalLocationName = FunctionalLocationultraCombo.Text;
            myFixedAsset.mPersonnelId = ControlUtil.UltraComboReturnValue(PersonnelultraCombo);
            myFixedAsset.mPersonnelName = PersonnelultraCombo.Text;
            myFixedAsset.mDescription = DescriptiontextBox.Text;
            myFixedAsset.mPurchaseDate = PurchasedateTimePicker.Value.Date;
            myFixedAsset.mPurchasePrice = ControlUtil.TextBoxDecimal(PurchasePricetextBox);
            myFixedAsset.mWarrantyExpiry = WarrantydateTimePicker.Value.Date;
            myFixedAsset.mSerialNo = SerialNotextBox.Text;
            myFixedAsset.mModel = ModeltextBox.Text;
            myFixedAsset.mDepreciationStartDate = DepreciationStartdateTimePicker.Value.Date;
            myFixedAsset.mDepreciationMethodId = ControlUtil.UltraComboReturnValue(DepreciationMethodultraCombo);
            myFixedAsset.mDepreciationMethodName = DepreciationMethodultraCombo.Text;
            myFixedAsset.mAveragingMethodId = ControlUtil.UltraComboReturnValue(AveragingMehodultraCombo);
            myFixedAsset.mAveragingMethodName = AveragingMehodultraCombo.Text;
            myFixedAsset.mAccumulatedDepreciation = ControlUtil.TextBoxDecimal(AccumulatedDepreciationtextBox);
            myFixedAsset.mResidualValue = ControlUtil.TextBoxDecimal(ResidualValuetextBox);
            myFixedAsset.mUsefulLifeYears = ControlUtil.TextBoxDecimal(UsefulLifetextBox);
            myFixedAsset.mIsDraft = DraftcheckBox.Checked;
            myFixedAsset.mIsRegistered = RegisterBytextBox.Text != string.Empty;
            myFixedAsset.mIsDisposed = DisposedcheckBox.Checked;
            myFixedAsset.mRegisterById = int.Parse(RegisterByIdlabel.Text);
            myFixedAsset.mRegisterByName = RegisterBytextBox.Text;
            myFixedAsset.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.FixedAsset myFixedAsset)
        {
            Idlabel.Text = myFixedAsset.mId.ToString();
            AssetNotextBox.Text = myFixedAsset.mAssetNo;
            ProductNametextBox.Text = myFixedAsset.mProductName;
            ProductIdlabel.Text = myFixedAsset.mProductId.ToString();
            ReceivingDetailIdlabel.Text = myFixedAsset.mReceivingDetailId.ToString();
            AssetTypeutraCombo.Value = myFixedAsset.mAssetTypeId;
            FunctionalLocationultraCombo.Value = myFixedAsset.mFunctionalLocationId;
            PersonnelultraCombo.Value = myFixedAsset.mPersonnelId;
            DescriptiontextBox.Text = myFixedAsset.mDescription;
            PurchasedateTimePicker.Value = myFixedAsset.mPurchaseDate;
            PurchasePricetextBox.Text = myFixedAsset.mPurchasePrice.ToString("N");
            WarrantydateTimePicker.Value = myFixedAsset.mWarrantyExpiry;
            SerialNotextBox.Text = myFixedAsset.mSerialNo;
            ModeltextBox.Text = myFixedAsset.mModel;
            DepreciationStartdateTimePicker.Value = myFixedAsset.mDepreciationStartDate;
            DepreciationMethodultraCombo.Value = myFixedAsset.mDepreciationMethodId;
            AveragingMehodultraCombo.Value = myFixedAsset.mAveragingMethodId;
            AccumulatedDepreciationtextBox.Text = myFixedAsset.mAccumulatedDepreciation.ToString("N");
            ResidualValuetextBox.Text = myFixedAsset.mResidualValue.ToString();
            UsefulLifetextBox.Text = myFixedAsset.mUsefulLifeYears.ToString("N");
            DraftcheckBox.Checked = myFixedAsset.mIsDraft;
            RegisteredcheckBox.Checked = myFixedAsset.mIsRegistered;
            DisposedcheckBox.Checked = myFixedAsset.mIsDisposed;
            RegisterByIdlabel.Text = myFixedAsset.mRegisterById.ToString();
            RegisterBytextBox.Text = myFixedAsset.mRegisterByName;
        }

        public void LoadFormControlsFromReceivingDetail(ReceivingDetail myReceivingDetail)
        {
            PurchaseOrderDetail pd = PurchaseOrderDetailManager.GetItem(myReceivingDetail.mPurchaseOrderDetailId);
            QuotationDetail qd = QuotationDetailManager.GetItem(pd.mQuotationDetailId);
            PurchaseRequestDetail prd = PurchaseRequestDetailManager.GetItem(qd.mPurchaseRequestDetailId);

            ReceivingDetailIdlabel.Text = myReceivingDetail.mId.ToString();
            ProductIdlabel.Text = prd.mProductId.ToString();
            ProductNametextBox.Text = myReceivingDetail.mProductName;

            PurchaseOrder po = PurchaseOrderManager.GetItem(pd.mPurchaseOrderId);
            Receiving r = ReceivingManager.GetItem(myReceivingDetail.mReceivingId);
            PurchasedateTimePicker.Value = po.mDate;
            PurchasePricetextBox.Text = qd.mCost.ToString("N");
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
        }
        #endregion

        #region ICRUD Members

        public void NewRecord()
        {
            if (!allow_insert)
            {
                MessageUtil.NotAllowedInsertAccess();
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            ProductNametextBox.Focus();
            DraftcheckBox.Checked = !allow_delete;
            RegisteredcheckBox.Checked = allow_delete;

            DraftcheckBox.Checked = !allow_delete;
            RegisteredcheckBox.Checked = allow_delete;

            if (FixedAssetSettingDateManager.SelectCountForGetList(new FixedAssetSettingDateCriteria()) > 0)
                DepreciationStartdateTimePicker.Value = FixedAssetSettingDateManager.GetList().First().mDate;
        }

        public int SaveRecords()
        {
            if (int.Parse(Idlabel.Text) != 0)
            {
                if (!allow_update)
                {
                    MessageUtil.NotAllowedUpdateAccess();
                    return 0;
                }
            }

            BrokenRulesCollection rules = new BrokenRulesCollection();
            
            decimal purchasePrice = 0;
            if (!decimal.TryParse(PurchasePricetextBox.Text, out purchasePrice))
                rules.Add(new BrokenRule("", "Invalid purchase price."));

            decimal usefulLife = 0;
            if (!decimal.TryParse(UsefulLifetextBox.Text, out usefulLife))
                rules.Add(new BrokenRule("", "Invalid useful life (years)."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveFixedAsset();
        }

        public void CancelTransaction()
        {
            if (MessageUtil.AskCancelEdit())
                EndEditing();
        }

        public void DeleteRecords()
        {
            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess();
                return;
            }

            if (MessageUtil.AskDelete())
            {
                BusinessEntities.FixedAsset item = FixedAssetManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                FixedAssetManager.Delete(item);

                LoadFixedAsset();

            }
        }

        public void PrintRecords()
        {
            if (!allow_print)
            {
                MessageUtil.NotAllowedPrintAccess();
                return;
            }
        }

        #endregion

        #region Ultragrid 
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);
        }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (e.Row.Index == -1)
                return;

          
            BusinessEntities.FixedAsset item = FixedAssetManager.GetItem(_mId);

            if(mFromBrowse)
            {
                mFixedAsset = item;
                Close();
                return;
            }

            LoadFormControlsFromUser(item);
            ControlUtil.ExpandPanel(splitContainer1);
            ProductNametextBox.Focus();
        }

        private void ultraGrid1_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            if (!allow_delete)
                MessageUtil.NotAllowedDeleteAccess();
            else
                DeleteRecords();

            e.Cancel = true;
        }
        #endregion

        private void expandPanelControl1__ExpandPanel(object sender, EventArgs e)
        {
            if (Idlabel.Text == "0" && allow_insert)
                ControlUtil.ExpandPanel(splitContainer1);
            else if (Idlabel.Text != "0" && allow_update)
                ControlUtil.ExpandPanel(splitContainer1);
        }

        private void collapsePanelControl1__HidePanel(object sender, EventArgs e)
        {
            ControlUtil.HidePanel(splitContainer1);
        }

        private void Default_Load(object sender, EventArgs e)
        {
            SessionUtil.UserValidate(ref allow_select, ref allow_insert,
                ref allow_update, ref allow_delete, ref allow_print,
                (int)Modules.FixedAsset);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadFixedAsset();            
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveRecords();
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            User.LogInForm logIn = (User.LogInForm)sender;
            if (logIn.mUser == null) return;

            if (!SessionUtil.UserAllowApprove(logIn.mUser, (int)Modules.FixedAsset))
            {
                MessageBox.Show("You are not allowed to override transaction (register).", "Fixed Asset", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            RegisterByIdlabel.Text = logIn.mUser.mId.ToString();
            RegisterBytextBox.Text = PersonnelManager.GetItem(logIn.mUser.mPersonnelId).mName;
        }


        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        public void RefreshAllSelection()
        {
            UltraComboUtil.AssetType(AssetTypeutraCombo);
            UltraComboUtil.FunctionalLocation(FunctionalLocationultraCombo);
            UltraComboUtil.DepreciationMethod(DepreciationMethodultraCombo);
            UltraComboUtil.AveragingMethod(AveragingMehodultraCombo);
            UltraComboUtil.Personnel(PersonnelultraCombo);
        }

        private void ultraGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            UltraGrid ug = ((UltraGrid)sender);

            UIElement clicked_element = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(Default.MousePosition));
            if (clicked_element == null) return;

            CellUIElement cell_element = (CellUIElement)clicked_element.GetAncestor(typeof(CellUIElement));
            if (cell_element == null) return;

            UltraGridCell cell = (UltraGridCell)cell_element.GetContext(typeof(UltraGridCell));
            if (cell == null) return;

            if (cell.Column.Key == "mProductName")
            {
                BusinessEntities.FixedAsset item = (BusinessEntities.FixedAsset)cell.Row.ListObject;
                RegisterForm frm = new RegisterForm();
                frm.mId = item.mId;
                frm.ShowDialog();
            }
        }

        private void BeginningBalanceradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (Idlabel.Text == "0") DescriptiontextBox.Text = "Beginning Balance";
            AccumulatedDepreciationtextBox.Enabled = BeginningBalanceradioButton.Checked;
        }

        private void NewPurchaseradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (Idlabel.Text == "0") DescriptiontextBox.Text = "New Purchase";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Idlabel.Text == "0")
            {
                SearchProductForm browseProductForm = new SearchProductForm();
                browseProductForm.FormClosing += BrowseProductForm_FormClosing;
                browseProductForm.ShowDialog();
            }
        }

        private void BrowseProductForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SearchProductForm browseProductForm = (SearchProductForm)sender;
            if (browseProductForm.mProduct == null) return;

            Product product = browseProductForm.mProduct;
            ProductIdlabel.Text = product.mId.ToString();
            ProductNametextBox.Text = product.mName;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Idlabel.Text == "0")
            {
                Purchasing.PurchaseVoucher.Default apvForm = new Purchasing.PurchaseVoucher.Default();
                apvForm.mForFixedAsset = true;
                apvForm.FormClosing += ApvForm_FormClosing;
                apvForm.ShowDialog();
            }
        }

        private void ApvForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Purchasing.PurchaseVoucher.Default apvForm = (Purchasing.PurchaseVoucher.Default)sender;
            if (apvForm.mPurchaseVoucher == null) return;

            ReceivingDetailCriteria criteria = new ReceivingDetailCriteria();
            criteria.mReceivingId = apvForm.mPurchaseVoucher.mReceivingId;
            criteria.mForFixedAsset = true;

            //if remaining for fa encoding is 1
            //or mostly the reccord from pr to receiving is always 1
            if (ReceivingDetailManager.SelectCountForGetList(criteria) == 1)
                LoadFormControlsFromReceivingDetail(ReceivingDetailManager.GetList(criteria).First());
            else
            {
                //browse items from receiving detail
            }

        }

        private void AssetTypeutraCombo_RowSelected(object sender, RowSelectedEventArgs e)
        {
            DepreciationMethodultraCombo.Value = 0;
            AveragingMehodultraCombo.Value = 0;

            if (e.Row == null) return;
            if (e.Row.Index == -1) return;

            FixedAssetSettingCriteria criteria = new FixedAssetSettingCriteria();
            criteria.mAssetTypeId = ControlUtil.UltraComboReturnValue(AssetTypeutraCombo);

            if (FixedAssetSettingManager.SelectCountForGetList(criteria) == 0) return;
            FixedAssetSetting item = FixedAssetSettingManager.GetList(criteria).First();

            DepreciationMethodultraCombo.Value = item.mDepreciationMethodId;
            AveragingMehodultraCombo.Value = item.mAveragingMethodId;
        }

        private void Registeredbutton_Click(object sender, EventArgs e)
        {
            FixedAssetCriteria criteria = new FixedAssetCriteria();
            criteria.mIsRegistered = true;
            ultraGrid1.SetDataBinding(FixedAssetManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private void Totalbutton_Click(object sender, EventArgs e)
        {
            ultraGrid1.SetDataBinding(FixedAssetManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private void AccumulatedDepreciationtextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //enter username password
            User.LogInForm logIn = new User.LogInForm();
            logIn.mForOverride = true;
            logIn.FormClosing += LogIn_FormClosing;
            logIn.ShowDialog();
        }
    }
}
