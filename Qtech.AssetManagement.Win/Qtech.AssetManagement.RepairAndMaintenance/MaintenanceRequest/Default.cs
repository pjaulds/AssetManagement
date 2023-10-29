using Infragistics.Win.UltraWinGrid;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Utilities;
using Qtech.AssetManagement.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.RepairAndMaintenance.MaintenanceRequest
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

        public bool mFromBrowse { get; set; }
        public BusinessEntities.MaintenanceRequest mMaintenanceRequest { get; set; }
        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.MaintenanceRequest)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadMaintenanceRequest()
        {
            MaintenanceRequestCriteria criteria = new MaintenanceRequestCriteria();
            
            if(SearchStartdateTimePicker.Checked && SearchEnddateTimePicker.Checked)
            {
                criteria.mStartDate = SearchStartdateTimePicker.Value.Date;
                criteria.mEndDate = SearchEnddateTimePicker.Value.Date;
            }

            ultraGrid1.SetDataBinding(MaintenanceRequestManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveMaintenanceRequest()
        {
            BusinessEntities.MaintenanceRequest item = new BusinessEntities.MaintenanceRequest();
            LoadMaintenanceRequestFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = MaintenanceRequestManager.Save(item);
                EndEditing();
                LoadMaintenanceRequest();

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

        private void LoadMaintenanceRequestFromFormControls(BusinessEntities.MaintenanceRequest myMaintenanceRequest)
        {
            myMaintenanceRequest.mId = int.Parse(Idlabel.Text);
            myMaintenanceRequest.mStartDate = StartdateTimePicker.Value.Date;
            myMaintenanceRequest.mEndDate = EnddateTimePicker.Value.Date;

            myMaintenanceRequest.mMaintenanceRequestTypeId = ControlUtil.UltraComboReturnValue(MaintenanceRequestTypeutraCombo);
            myMaintenanceRequest.mMaintenanceRequestTypeName = MaintenanceRequestTypeutraCombo.Text;

            myMaintenanceRequest.mServiceLevelId = ControlUtil.UltraComboReturnValue(ServiceLevelultraCombo);
            myMaintenanceRequest.mServiceLevelName = ServiceLevelultraCombo.Text;

            myMaintenanceRequest.mRequestedById = ControlUtil.UltraComboReturnValue(RequestedByultraCombo);
            myMaintenanceRequest.mRequestedByName = RequestedByultraCombo.Text;

            myMaintenanceRequest.mFunctionalLocationId = ControlUtil.UltraComboReturnValue(FunctionalLocationultraCombo);
            myMaintenanceRequest.mFunctionalLocationName = FunctionalLocationultraCombo.Text;

            myMaintenanceRequest.mFixedAssetId = int.Parse(FixedAssetIdlabel.Text);

            myMaintenanceRequest.mFaultSymptomsId = ControlUtil.UltraComboReturnValue(FaultSymptomsultraCombo);
            myMaintenanceRequest.mFaultSymptomsName = FaultSymptomsultraCombo.Text;

            myMaintenanceRequest.mFaultAreaId = ControlUtil.UltraComboReturnValue(FaultAreaultraCombo);
            myMaintenanceRequest.mFaultAreaName = FaultAreaultraCombo.Text;

            myMaintenanceRequest.mDescription = DescriptiontextBox.Text;
            myMaintenanceRequest.mStatus = StatustextBox.Text;
            myMaintenanceRequest.mActive = ActivecheckBox.Checked;
            myMaintenanceRequest.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromMaintenanceReques(BusinessEntities.MaintenanceRequest myMaintenanceRequest)
        {
            Idlabel.Text = myMaintenanceRequest.mId.ToString();
            DatetextBox.Text = myMaintenanceRequest.mDate.ToString("D");
            StartdateTimePicker.Value = myMaintenanceRequest.mStartDate;
            EnddateTimePicker.Value = myMaintenanceRequest.mEndDate;
            MaintenanceRequestTypeutraCombo.Value = myMaintenanceRequest.mMaintenanceRequestTypeId;
            ServiceLevelultraCombo.Value = myMaintenanceRequest.mServiceLevelId;
            RequestedByultraCombo.Value = myMaintenanceRequest.mRequestedById;
            FunctionalLocationultraCombo.Value = myMaintenanceRequest.mFunctionalLocationId;
            FixedAssetIdlabel.Text = myMaintenanceRequest.mFixedAssetId.ToString();
            FaultSymptomsultraCombo.Value = myMaintenanceRequest.mFaultSymptomsId;
            FaultAreaultraCombo.Value = myMaintenanceRequest.mFaultAreaId;
            DescriptiontextBox.Text = myMaintenanceRequest.mDescription;
            StatustextBox.Text = myMaintenanceRequest.mStatus;
            ActivecheckBox.Checked = myMaintenanceRequest.mActive;

            LoadFormControlsFromFixedAsset(FixedAssetManager.GetItem(myMaintenanceRequest.mFixedAssetId));
        }

        private void LoadFormControlsFromFixedAsset(BusinessEntities.FixedAsset myFixedAsset)
        {
            FixedAssetIdlabel.Text = myFixedAsset.mId.ToString();
            AssetNotextBox.Text = myFixedAsset.mAssetNo;
            AssetNametextBox.Text = myFixedAsset.mProductName;
            AssetTypetextBox.Text = myFixedAsset.mAssetTypeName;
            ModeltextBox.Text = myFixedAsset.mModel;
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

            DatetextBox.Text = AuditManager.GetDateToday().ToString("D");

            ReportCriteria criteria = new ReportCriteria();
            criteria.mTableName = "MaintenanceRequest";
            MrNotextBox.Text = HelpersManager.GetNewTrasactionNo(criteria);

            StatustextBox.Text = "NEW";

            MaintenanceRequestTypeutraCombo.Focus();

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


            if (StartdateTimePicker.Value.Date > EnddateTimePicker.Value.Date)
            {
                MessageBox.Show("Invalid start/end date.", "Maintenance Request", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            return SaveMaintenanceRequest();
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
                BusinessEntities.MaintenanceRequest item = MaintenanceRequestManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                MaintenanceRequestManager.Delete(item);

                LoadMaintenanceRequest();

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

            BusinessEntities.MaintenanceRequest item = MaintenanceRequestManager.GetItem(_mId);

            if(mFromBrowse)
            {
                mMaintenanceRequest = item;
                Close();
                return;
            }

            LoadFormControlsFromMaintenanceReques(item);
            ControlUtil.ExpandPanel(splitContainer1);
            DatetextBox.Focus();
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
                (int)Modules.MaintenanceRequest);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadMaintenanceRequest();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveMaintenanceRequest();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        public void RefreshAllSelection()
        {
            UltraComboUtil.MaintenanceRequestType(MaintenanceRequestTypeutraCombo);
            UltraComboUtil.ServiceLevel(ServiceLevelultraCombo);
            UltraComboUtil.FaultSymptoms(FaultSymptomsultraCombo);
            UltraComboUtil.FaultArea(FaultAreaultraCombo);
            UltraComboUtil.Personnel(RequestedByultraCombo);
            UltraComboUtil.FunctionalLocation(FunctionalLocationultraCombo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadMaintenanceRequest();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FixedAsset.Default browseFa = new FixedAsset.Default();
            browseFa.mFromBrowse = true;
            browseFa.FormClosing += BrowseFa_FormClosing;
            browseFa.ShowDialog();
        }

        private void BrowseFa_FormClosing(object sender, FormClosingEventArgs e)
        {
            FixedAsset.Default faForm = (FixedAsset.Default)sender;
            if (faForm.mFixedAsset == null) return;

            LoadFormControlsFromFixedAsset(faForm.mFixedAsset);
        }
    }
}
