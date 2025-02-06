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

namespace Qtech.AssetManagement.Maintenance.Asset
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
                    return ((BusinessEntities.Asset)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadAsset()
        {
            AssetCriteria criteria = new AssetCriteria();
            if (dateTimePicker1.Checked && dateTimePicker2.Checked)
            {
                criteria.mStartDate = dateTimePicker1.Value.Date;
                criteria.mEndDate = dateTimePicker2.Value.Date;
            }
            criteria.mProjectId = ControlUtil.UltraComboReturnValue(SearchProjectultraCombo);

            ultraGrid1.SetDataBinding(AssetManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveAsset()
        {
            BusinessEntities.Asset item = new BusinessEntities.Asset();
            LoadAssetFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = AssetManager.Save(item);
                EndEditing();
                LoadAsset();

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

        private void LoadAssetFromFormControls(BusinessEntities.Asset myAsset)
        {
            myAsset.mId = int.Parse(Idlabel.Text);
            myAsset.mCode = AssetNotextBox.Text;
            myAsset.mDate = DatedateTimePicker.Value.Date;
            myAsset.mReceivedDate = ReceivedDatedateTimePicker.Value.Date;
            myAsset.mName = NametextBox.Text;
            myAsset.mAssetTypeId = ControlUtil.UltraComboReturnValue(AssetTypeutraCombo);
            myAsset.mAssetTypeName = AssetTypeutraCombo.Text;
            myAsset.mAcquisitionCost = ControlUtil.TextBoxDecimal(AcquisitionCosttextBox);
            myAsset.mWarrantyExpiry = WarrantyExpirydateTimePicker.Value.Date;
            myAsset.mBrand = BrandtextBox.Text;
            myAsset.mModel = ModeltextBox.Text;
            myAsset.mSerialNumber = SerialNotextBox.Text;
            myAsset.mCapacity = CapacitytextBox.Text;
            myAsset.mEngineNumber = EngineNotextBox.Text;
            myAsset.mChassisNumber = ChassisNotextBox.Text;
            myAsset.mPlateNumber = PlateNotextBox.Text;
            myAsset.mFunctionalLocationId = ControlUtil.UltraComboReturnValue(FunctionalLocationultraCombo);
            myAsset.mFunctionalLocationName = FunctionalLocationultraCombo.Text;
            myAsset.mPersonnelId = ControlUtil.UltraComboReturnValue(PersonnelultraCombo);
            myAsset.mPersonnelName = PersonnelultraCombo.Text;
            myAsset.mProjectId = ControlUtil.UltraComboReturnValue(ProjectultraCombo);
            myAsset.mProjectName = ProjectultraCombo.Text;
            myAsset.mRegisteredById = SessionUtil.mUser.mId;
            myAsset.mRemarks = RemarkstextBox.Text;
            myAsset.mActive = ActivecheckBox.Checked;
            myAsset.mResidualValue = ControlUtil.TextBoxDecimal(ResidualValuetextBox);
            myAsset.mUsefulLife = ControlUtil.TextBoxDecimal(UsefulLifetextBox);
            myAsset.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.Asset myAsset)
        {
            Idlabel.Text = myAsset.mId.ToString();
            AssetNotextBox.Text = myAsset.mCode;
            DatedateTimePicker.Value = myAsset.mDate;
            ReceivedDatedateTimePicker.Value = myAsset.mReceivedDate;
            NametextBox.Text = myAsset.mName;
            AssetTypeutraCombo.Value = myAsset.mAssetTypeId;
            AcquisitionCosttextBox.Text = myAsset.mAcquisitionCost.ToString("N");
            WarrantyExpirydateTimePicker.Value = myAsset.mWarrantyExpiry;
            BrandtextBox.Text = myAsset.mBrand;
            ModeltextBox.Text = myAsset.mModel;
            SerialNotextBox.Text = myAsset.mSerialNumber;
            EngineNotextBox.Text = myAsset.mEngineNumber;
            ChassisNotextBox.Text = myAsset.mChassisNumber;
            PlateNotextBox.Text = myAsset.mPlateNumber;
            FunctionalLocationultraCombo.Value = myAsset.mFunctionalLocationId;
            PersonnelultraCombo.Value = myAsset.mPersonnelId;
            ProjectultraCombo.Value = myAsset.mProjectId;
            RegisteredBytextBox.Text = myAsset.mRegisteredByName;
            RemarkstextBox.Text = myAsset.mRemarks;
            ActivecheckBox.Checked = myAsset.mActive;
            ResidualValuetextBox.Text = myAsset.mResidualValue.ToString();

            LoadFormControlsFromAssetType();
            UsefulLifetextBox.Text = myAsset.mUsefulLife.ToString();

            LoadFormControlsFromAssetCapitalize(myAsset);
        }

        private void LoadFormControlsFromAssetType()
        {
            if (AssetTypeutraCombo.ActiveRow == null) return;
            BusinessEntities.AssetType item = (BusinessEntities.AssetType)AssetTypeutraCombo.ActiveRow.ListObject;
            DepreciationMethodtextBox.Text = item.mDepreciationMethodName;
            AveragingMethodtextBox.Text = item.mAveragingMethodName;
            UsefulLifetextBox.Text = item.mUsefulLifeYears.ToString();
        }

        private void LoadFormControlsFromAssetCapitalize(BusinessEntities.Asset myAsset)
        {
            AssetCapitalizeCriteria criteria = new AssetCapitalizeCriteria();
            criteria.mAssetId = myAsset.mId;

            CapitalizedataGridView.DataSource = new SortableBindingList<AssetCapitalize>(AssetCapitalizeManager.GetList(criteria));
            CapitalizedataGridView.Refresh();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";

            CapitalizedataGridView.AutoGenerateColumns = false;
            CapitalizedataGridView.DataSource = new SortableBindingList<AssetCapitalize>();
            CapitalizedataGridView.Refresh();
        }
        #endregion

        #region ICRUD Members

        public void NewRecord()
        {
            if (!allow_insert)
            {
                MessageUtil.NotAllowedInsertAccess(" asset registration");
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            DatedateTimePicker.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            //AssetCriteria criteria = new AssetCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mCode = CodetextBox.Text;
            //if (AssetManager.SelectCountForGetList(criteria) > 0)
            //{
            //    MessageUtil.Message(criteria.mCode + " already exists. Please use a different, unique code.");
            //    return 0;
            //}
            
            //criteria = new AssetCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mName = NametextBox.Text;
            //if (AssetManager.SelectCountForGetList(criteria) > 0)
            //{
            //    MessageUtil.Message(criteria.mName + " already exists. Please use a different, unique name.");
            //    return 0;
            //}

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }

            if (!MessageUtil.SaveConfirm(" asset registration")) return 0;

            bool isNew = Idlabel.Text == "0";

            int id = SaveAsset();

            if (id > 0)
            {
                BusinessEntities.Asset item = AssetManager.GetItem(id);

                if (isNew)
                    MessageUtil.SaveSuccessfully(item.mCode);
                else
                    MessageUtil.UpdatedSuccessfully(item.mCode);
            }

            return id;
        }

        public void CancelTransaction()
        {
            if (MessageUtil.CancelUpdateConfirm())
                EndEditing();
        }

        public void DeleteRecords()
        {
            BusinessEntities.Asset item = AssetManager.GetItem(_mId);

            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess(" asset registration " + item.mCode);
                return;
            }
            
            if (MessageUtil.DeleteConfirm(item.mCode))
            {   
                item.mUserId = SessionUtil.mUser.mId;
                AssetManager.Delete(item);
                MessageUtil.DeletedSuccessfully(item.mCode);
                LoadAsset();
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

            EndEditing();

            if (!allow_update)
            {
                MessageUtil.NotAllowedUpdateAccess(" asset registration");
                return;
            }

            BusinessEntities.Asset item = AssetManager.GetItem(_mId);
            LoadFormControlsFromUser(item);

            ControlUtil.ExpandPanel(splitContainer1);
            DatedateTimePicker.Focus();
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
            RefreshAllSelection();
            LoadAsset();

            DateTime dateToday = AuditManager.GetDateToday();
            dateTimePicker1.Value = new DateTime(dateToday.Year, dateToday.Month, 1);
            dateTimePicker2.Value = dateTimePicker1.Value.AddMonths(1).AddDays(-1);
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
            UltraComboUtil.AssetType(AssetTypeutraCombo);
            UltraComboUtil.FunctionalLocation(FunctionalLocationultraCombo);
            UltraComboUtil.Personnel(PersonnelultraCombo);
            UltraComboUtil.Project(ProjectultraCombo);
            UltraComboUtil.Project(SearchProjectultraCombo);

            DataGridViewComboBoxColumn comboColumn = (DataGridViewComboBoxColumn)(CapitalizedataGridView.Columns["mCapitalizedCost"]);
            comboColumn.DataSource = CapitalizedCostManager.GetList();
            comboColumn.DisplayMember = "mName";
            comboColumn.ValueMember = "mId";
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

        private void AssetTypeutraCombo_RowSelected(object sender, RowSelectedEventArgs e)
        {
            AssetTypeCodetextBox.Text = string.Empty;
            if (e.Row == null) return;
            if (e.Row.Index == -1) return;

            BusinessEntities.AssetType item = (BusinessEntities.AssetType)e.Row.ListObject;
            AssetTypeCodetextBox.Text = item.mCode;

            LoadFormControlsFromAssetType();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadAsset();
        }

        private void CapitalizedataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void CapitalizedataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["mDate"].Value = AuditManager.GetDateToday();
        }
    }
}
