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

namespace Qtech.AssetManagement.Setup.FixedAssetSetting
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
                    return ((BusinessEntities.FixedAssetSetting)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadFixedAssetSetting()
        {
            ultraGrid1.SetDataBinding(FixedAssetSettingManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveFixedAssetSetting()
        {
            BusinessEntities.FixedAssetSetting item = new BusinessEntities.FixedAssetSetting();
            LoadFixedAssetSettingFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = FixedAssetSettingManager.Save(item);
                EndEditing();
                LoadFixedAssetSetting();

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

        private void LoadFixedAssetSettingFromFormControls(BusinessEntities.FixedAssetSetting myUser)
        {
            myUser.mId = int.Parse(Idlabel.Text);
            myUser.mCode = CodetextBox.Text;

            myUser.mAssetTypeId = ControlUtil.UltraComboReturnValue(AssetTypeutraCombo);
            myUser.mAssetTypeName = AssetTypeutraCombo.Text;
            
            myUser.mChartOfAccountId = ControlUtil.UltraComboReturnValue(AssetAccountultraCombo);
            myUser.mChartOfAccountName = AssetAccountultraCombo.Text;

            myUser.mAccumulatedDepreciationAccountId = ControlUtil.UltraComboReturnValue(AccumulatedDepreciationAccountultraCombo);
            myUser.mAccumulatedDepreciationAccountName = AccumulatedDepreciationAccountultraCombo.Text;

            myUser.mDepreciationExpenseAccountId = ControlUtil.UltraComboReturnValue(DepreciationExpenseAccountultraCombo);
            myUser.mDepreciationExpenseAccountName = DepreciationExpenseAccountultraCombo.Text;

            myUser.mDepreciationMethodId = ControlUtil.UltraComboReturnValue(DepreciationMethodultraCombo);
            myUser.mDepreciationMethodName = DepreciationMethodultraCombo.Text;

            myUser.mAveragingMethodId = ControlUtil.UltraComboReturnValue(AveragingMethodultraCombo);
            myUser.mAveragingMethodName = AveragingMethodultraCombo.Text;

            myUser.mUsefulLifeYears = ControlUtil.TextBoxDecimal(UsefulLifeYearstextBox);
            myUser.mDepreciable = DepreciablecheckBox.Checked;
            myUser.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.FixedAssetSetting myFixedAssetSetting)
        {
            Idlabel.Text = myFixedAssetSetting.mId.ToString();
            CodetextBox.Text = myFixedAssetSetting.mCode;
            AssetTypeutraCombo.Value = myFixedAssetSetting.mAssetTypeId;
            AssetAccountultraCombo.Value = myFixedAssetSetting.mChartOfAccountId;
            AccumulatedDepreciationAccountultraCombo.Value = myFixedAssetSetting.mAccumulatedDepreciationAccountId;
            DepreciationExpenseAccountultraCombo.Value = myFixedAssetSetting.mDepreciationExpenseAccountId;
            DepreciationMethodultraCombo.Value = myFixedAssetSetting.mDepreciationMethodId;
            AveragingMethodultraCombo.Value = myFixedAssetSetting.mAveragingMethodId;
            UsefulLifeYearstextBox.Text = myFixedAssetSetting.mUsefulLifeYears.ToString("N");
            DepreciablecheckBox.Checked = myFixedAssetSetting.mDepreciable;
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
                MessageUtil.NotAllowedInsertAccess();
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            AssetTypeutraCombo.Focus();

        }

        public int SaveRecords()
        {           
            return SaveFixedAssetSetting();
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
                BusinessEntities.FixedAssetSetting item = FixedAssetSettingManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                FixedAssetSettingManager.Delete(item);

                LoadFixedAssetSetting();

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

            if (!allow_update)
            {
                MessageUtil.NotAllowedUpdateAccess();
                return;
            }

            BusinessEntities.FixedAssetSetting item = FixedAssetSettingManager.GetItem(_mId);
            LoadFormControlsFromUser(item);

            ControlUtil.ExpandPanel(splitContainer1);
            AssetTypeutraCombo.Focus();
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
                (int)Modules.FixedAssetSetting);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadFixedAssetSetting();
            
            if (FixedAssetSettingDateManager.SelectCountForGetList(new FixedAssetSettingDateCriteria()) > 0)
                StartDatelabel.Text = FixedAssetSettingDateManager.GetList().First().mDate.ToString("D");

            RefreshAllSelection();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveFixedAssetSetting();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        public void RefreshAllSelection()
        {
            UltraComboUtil.AssetType(AssetTypeutraCombo);
            UltraComboUtil.ChartOfAccount(AssetAccountultraCombo);
            UltraComboUtil.ChartOfAccount(AccumulatedDepreciationAccountultraCombo);
            UltraComboUtil.ChartOfAccount(DepreciationExpenseAccountultraCombo);
            UltraComboUtil.DepreciationMethod(DepreciationMethodultraCombo);
            UltraComboUtil.AveragingMethod(AveragingMethodultraCombo);
        }

        private void FixedAssetSettingdateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            FixedAssetSettingDate date;
            if (FixedAssetSettingDateManager.SelectCountForGetList(new FixedAssetSettingDateCriteria()) == 0) date = new FixedAssetSettingDate();
            else date = FixedAssetSettingDateManager.GetList().First();

            date.mDate = FixedAssetSettingdateTimePicker.Value.Date;
            date.mUserId = SessionUtil.mUser.mId;
            FixedAssetSettingDateManager.Save(date);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartDateForm startDateForm = new StartDateForm();
            startDateForm.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DepreciationMethodultraCombo.Enabled = DepreciablecheckBox.Checked;
            AveragingMethodultraCombo.Enabled = DepreciablecheckBox.Checked;
            UsefulLifeYearstextBox.Enabled = DepreciablecheckBox.Checked;
        }
    }
}
