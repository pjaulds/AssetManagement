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

namespace Qtech.AssetManagement.Maintenance.AssetType
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

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
                    return ((BusinessEntities.AssetType)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadAssetType()
        {
            try
            {
                ultraGrid1.SetDataBinding(AssetTypeManager.GetList(), null, true);
                ultraGrid1.Refresh();
            }
            catch {
            }
        }

        private int SaveAssetType()
        {
            try
            {
                BusinessEntities.AssetType item = new BusinessEntities.AssetType();
                LoadAssetTypeFromFormControls(item);

                //validate if all the rules of Status has been meet
                if (item.Validate())
                {
                    Int32 id = AssetTypeManager.Save(item);

                    if (Idlabel.Text == "0") MessageUtil.Created(item.mCode);
                    else MessageUtil.Updated(item.mCode);

                    EndEditing();
                    LoadAssetType();

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
            catch (Exception ex)
            {
                if (SessionUtil.mUser.mUsername.ToUpper() == "ADMIN")
                    MessageUtil.Error(ex.Message);
                else
                    MessageUtil.Error();
                return 0;
            }
        }

        private void LoadAssetTypeFromFormControls(BusinessEntities.AssetType myAssetType)
        {
            myAssetType.mId = int.Parse(Idlabel.Text);
            myAssetType.mCode = CodetextBox.Text;
            myAssetType.mName = NametextBox.Text;
            myAssetType.mPost = ActivecheckBox.Checked;
            myAssetType.mAssetAccountId = ControlUtil.UltraComboReturnValue(AssetAccountultraCombo);
            myAssetType.mAssetAccountName = AssetAccountultraCombo.Text;

            myAssetType.mAccumulatedDepreciationAccountId = ControlUtil.UltraComboReturnValue(AccumulatedDepreciationAccountultraCombo);
            myAssetType.mAccumulatedDepreciationAccountName = AccumulatedDepreciationAccountultraCombo.Text;

            myAssetType.mProductionDepreciationExpenseAccountId = ControlUtil.UltraComboReturnValue(ProductionDepreciationExpenseAccountultraCombo);
            myAssetType.mProductionDepreciationExpenseAccountName = ProductionDepreciationExpenseAccountultraCombo.Text;
            myAssetType.mProductionDepreciationExpenseAccountValue = ControlUtil.TextBoxDecimal(DepAccountProductionValuetextBox);

            myAssetType.mAdminDepreciationExpenseAccountId = ControlUtil.UltraComboReturnValue(AdminDepreciationExpenseAccountultraCombo);
            myAssetType.mAdminDepreciationExpenseAccountName = AdminDepreciationExpenseAccountultraCombo.Text;
            myAssetType.mAdminDepreciationExpenseAccountValue = ControlUtil.TextBoxDecimal(DepAccountAdminValuetextBox);

            myAssetType.mDepreciationMethodId = ControlUtil.UltraComboReturnValue(DepreciationMethodultraCombo);
            myAssetType.mDepreciationMethodName = DepreciationMethodultraCombo.Text;

            myAssetType.mAveragingMethodId = ControlUtil.UltraComboReturnValue(AveragingMethodultraCombo);
            myAssetType.mAveragingMethodName = AveragingMethodultraCombo.Text;

            myAssetType.mMonths = ControlUtil.TextBoxDecimal(MonthstextBox);
            myAssetType.mUsefulLifeYears = ControlUtil.TextBoxDecimal(UsefulLifeYearstextBox);
            myAssetType.mDepreciable = DepreciablecheckBox.Checked;
            myAssetType.mActive = ActivecheckBox.Checked;

            myAssetType.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.AssetType myAssetType)
        {
            Idlabel.Text = myAssetType.mId.ToString();
            CodetextBox.Text = myAssetType.mCode;
            NametextBox.Text = myAssetType.mName;

            AssetAccountultraCombo.Value = myAssetType.mAssetAccountId;
            AccumulatedDepreciationAccountultraCombo.Value = myAssetType.mAccumulatedDepreciationAccountId;
            ProductionDepreciationExpenseAccountultraCombo.Value = myAssetType.mProductionDepreciationExpenseAccountId;
            DepAccountProductionValuetextBox.Text = myAssetType.mProductionDepreciationExpenseAccountValue.ToString();
            AdminDepreciationExpenseAccountultraCombo.Value = myAssetType.mAdminDepreciationExpenseAccountId;
            DepAccountAdminValuetextBox.Text = myAssetType.mAdminDepreciationExpenseAccountValue.ToString();
            DepreciationMethodultraCombo.Value = myAssetType.mDepreciationMethodId;
            AveragingMethodultraCombo.Value = myAssetType.mAveragingMethodId;
            MonthstextBox.Text = myAssetType.mMonths.ToString("N");
            UsefulLifeYearstextBox.Text = myAssetType.mUsefulLifeYears.ToString("N");
            DepreciablecheckBox.Checked = myAssetType.mDepreciable;
            ActivecheckBox.Checked = myAssetType.mActive;
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
        }
        #endregion

        #region ICRUD Members

        public void NewRecord()
        {
            if (!allow_insert)
            {
                MessageUtil.NotAllowedInsertAccess(" asset type");
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            CodetextBox.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            AssetTypeCriteria criteria = new AssetTypeCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (AssetTypeManager.SelectCountForGetList(criteria) > 0)
            {
                MessageUtil.Message(criteria.mCode + " already exists. Please use a different, unique code.");
                return 0;
            }

            criteria = new AssetTypeCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (AssetTypeManager.SelectCountForGetList(criteria) > 0)
            {
                MessageUtil.Message(criteria.mName + " already exists. Please use a different, unique name.");
                return 0;
            }

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }

            if (!MessageUtil.SaveConfirm("asset type")) return 0;

            return SaveAssetType();
        }

        public void CancelTransaction()
        {
            if (MessageUtil.CancelUpdateConfirm())
                EndEditing();
        }

        public void DeleteRecords()
        {
            BusinessEntities.AssetType item = AssetTypeManager.GetItem(_mId);

            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess(" asset type " + item.mCode);
                return;
            }
            
            if (MessageUtil.DeleteConfirm(item.mCode))
            {
                item.mUserId = SessionUtil.mUser.mId;
                AssetTypeManager.Delete(item);
                MessageUtil.DeletedSuccessfully(item.mCode);
                LoadAssetType();
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

            BusinessEntities.AssetType item = AssetTypeManager.GetItem(_mId);

            if (!allow_update)
            {
                MessageUtil.NotAllowedUpdateAccess(" asset type " + item.mCode);
                return;
            }
            
            LoadFormControlsFromUser(item);

            ControlUtil.ExpandPanel(splitContainer1);
            CodetextBox.Focus();
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
                (int)Modules.AssetType);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadAssetType();
            RefreshAllSelection();

            if (FixedAssetSettingDateManager.SelectCountForGetList(new FixedAssetSettingDateCriteria()) > 0)
                StartDatetextBox.Text = FixedAssetSettingDateManager.GetList().First().mDate.ToString("D");
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveRecords();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        public void RefreshAllSelection()
        {
            UltraComboUtil.ChartOfAccountFixedAsset(AssetAccountultraCombo);
            UltraComboUtil.ChartOfAccountAccumulatedDepreciation(AccumulatedDepreciationAccountultraCombo);
            UltraComboUtil.ChartOfAccountDepreciationExpenseAccount(ProductionDepreciationExpenseAccountultraCombo);
            UltraComboUtil.ChartOfAccountDepreciationExpenseAccount(AdminDepreciationExpenseAccountultraCombo);
            UltraComboUtil.DepreciationMethod(DepreciationMethodultraCombo);
            UltraComboUtil.AveragingMethod(AveragingMethodultraCombo);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SaveRecords() > 0) NewRecord();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = Idlabel.Text;
            if (MessageUtil.ResetConfirm()) ControlUtil.ClearConent(splitContainer1.Panel2);

            Idlabel.Text = id;//reassigned
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartDateForm startDateForm = new StartDateForm();
            startDateForm.ShowDialog();

            if (FixedAssetSettingDateManager.SelectCountForGetList(new FixedAssetSettingDateCriteria()) > 0)
                StartDatetextBox.Text = FixedAssetSettingDateManager.GetList().First().mDate.ToString("D");
        }

        private void DepreciablecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AccumulatedDepreciationAccountultraCombo.Enabled = DepreciablecheckBox.Checked;
            ProductionDepreciationExpenseAccountultraCombo.Enabled = DepreciablecheckBox.Checked;
            AdminDepreciationExpenseAccountultraCombo.Enabled = DepreciablecheckBox.Checked;
            DepAccountAdminValuetextBox.Enabled = DepreciablecheckBox.Checked;
            DepAccountProductionValuetextBox.Enabled = DepreciablecheckBox.Checked;

            DepreciationMethodultraCombo.Enabled = DepreciablecheckBox.Checked;
            AveragingMethodultraCombo.Enabled = DepreciablecheckBox.Checked;

            MonthstextBox.Enabled = DepreciablecheckBox.Checked;
            UsefulLifeYearstextBox.Enabled = DepreciablecheckBox.Checked;
        }

        private void UsefulLifeYearstextBox_TextChanged(object sender, EventArgs e)
        {
            int months = 0;
            int.TryParse(MonthstextBox.Text, out months);

            UsefulLifeYearstextBox.Text = (months / 12.0).ToString();
        }
    }
}
