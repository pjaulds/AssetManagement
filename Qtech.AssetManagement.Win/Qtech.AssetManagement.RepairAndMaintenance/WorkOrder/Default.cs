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

namespace Qtech.AssetManagement.RepairAndMaintenance.WorkOrder
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

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.WorkOrder)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadWorkOrder()
        {
            WorkOrderCriteria criteria = new WorkOrderCriteria();

            if (SearchStartdateTimePicker.Checked && SearchEnddateTimePicker.Checked)
            {
                criteria.mStartDate = SearchStartdateTimePicker.Value.Date;
                criteria.mEndDate = SearchEnddateTimePicker.Value.Date;
            }

            ultraGrid1.SetDataBinding(WorkOrderManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveWorkOrder()
        {
            BusinessEntities.WorkOrder item = new BusinessEntities.WorkOrder();
            LoadWorkOrderFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = WorkOrderManager.Save(item);
                EndEditing();
                LoadWorkOrder();

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

        private void LoadWorkOrderFromFormControls(BusinessEntities.WorkOrder myWorkOrder)
        {
            myWorkOrder.mId = int.Parse(Idlabel.Text);
            myWorkOrder.mExpectedStartDate = ExpectedStartdateTimePicker.Value.Date;
            myWorkOrder.mExpectedEndDate = ExpectedEnddateTimePicker.Value.Date;
            myWorkOrder.mMaintenanceRequestId = int.Parse(MaintenanceRequestIdlabel.Text);
            myWorkOrder.mMaintenanceRequestNo = MRNotextBox.Text;
            myWorkOrder.mWorkOrderTypeId = ControlUtil.UltraComboReturnValue(WorkOrderTypeutraCombo);
            myWorkOrder.mWorkOrderTypeName = WorkOrderTypeutraCombo.Text;
            myWorkOrder.mMaintenanceJobTypeVariantId = ControlUtil.UltraComboReturnValue(MaintenanceJobTypeVariantultraCombo);
            myWorkOrder.mMaintenanceJobTypeVariantName = MaintenanceJobTypeVariantultraCombo.Text;
            myWorkOrder.mTradeId = ControlUtil.UltraComboReturnValue(TradeultraCombo);
            myWorkOrder.mTradeName = TradeultraCombo.Text;
            myWorkOrder.mUserId = SessionUtil.mUser.mId;

            myWorkOrder.mWorkOrderHoursCollection = hoursUserControl1.LoadWorkOrderHoursFromFormControls();
            myWorkOrder.mDeletedWorkOrderHoursCollection = hoursUserControl1.deleted_items;
        }

        private void LoadFormControlsFromWorkOrder(BusinessEntities.WorkOrder myWorkOrder)
        {
            Idlabel.Text = myWorkOrder.mId.ToString();
            ExpectedStartdateTimePicker.Value = myWorkOrder.mExpectedStartDate;
            ExpectedEnddateTimePicker.Value = myWorkOrder.mExpectedEndDate;
            WorkOrderTypeutraCombo.Value = myWorkOrder.mWorkOrderTypeId;

            MaintenanceJobTypeultraCombo.Value = MaintenanceJobTypeVariantManager.GetItem(myWorkOrder.mMaintenanceJobTypeVariantId).mMaintenanceJobTypeId;
            MaintenanceJobTypeVariantultraCombo.Value = myWorkOrder.mMaintenanceJobTypeVariantId;
            TradeultraCombo.Value = myWorkOrder.mTradeId;

            hoursUserControl1.mWorkOrderId = myWorkOrder.mId;
            hoursUserControl1.LoadFormControlsFromWorkOrderHours();

            LoadFormControlsFromMaintenanceRequest(MaintenanceRequestManager.GetItem(myWorkOrder.mMaintenanceRequestId));
        }

        private void LoadFormControlsFromMaintenanceRequest(BusinessEntities.MaintenanceRequest myMaintenanceRequest)
        {
            MaintenanceRequestIdlabel.Text = myMaintenanceRequest.mId.ToString();
            MRNotextBox.Text = myMaintenanceRequest.mMaintenanceRequestNo;
            DatetextBox.Text = myMaintenanceRequest.mDate.ToString("D");
            AssetNotextBox.Text = myMaintenanceRequest.mFixedAssetName;
            FunctionalLocationtextBox.Text = myMaintenanceRequest.mFunctionalLocationName;
            DescriptiontextBox.Text = myMaintenanceRequest.mDescription;
            ServiceLeveltextBox.Text = myMaintenanceRequest.mServiceLevelName;
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);

            hoursUserControl1.EndEditing();
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

            ReportCriteria criteria = new ReportCriteria();
            criteria.mTableName = "WorkOrder";
            WorkOrderNotextBox.Text = HelpersManager.GetNewTrasactionNo(criteria);

        }

        public int SaveRecords()
        {
            return SaveWorkOrder();
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
                BusinessEntities.WorkOrder item = WorkOrderManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                WorkOrderManager.Delete(item);

                LoadWorkOrder();

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

            BusinessEntities.WorkOrder item = WorkOrderManager.GetItem(_mId);
            LoadFormControlsFromWorkOrder(item);

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
                (int)Modules.WorkOrder);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadWorkOrder();
            hoursUserControl1.allow_delete = allow_delete;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveWorkOrder();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        public void RefreshAllSelection()
        {
            UltraComboUtil.WorkOrderType(WorkOrderTypeutraCombo);
            UltraComboUtil.MaintenanceJobType(MaintenanceJobTypeultraCombo);
            UltraComboUtil.Trade(TradeultraCombo);
            hoursUserControl1.LoadExpenseCategory();
        }

        private void MaintenanceJobTypeultraCombo_RowSelected(object sender, RowSelectedEventArgs e)
        {
            if (e.Row == null) return;
            if (e.Row.Index == -1) return;

            MaintenanceJobTypeVariantCriteria criteria = new MaintenanceJobTypeVariantCriteria();
            criteria.mMaintenanceJobTypeId = ((BusinessEntities.MaintenanceJobType)e.Row.ListObject).mId;

            UltraComboUtil.MaintenanceJobTypeVariant(MaintenanceJobTypeVariantultraCombo, criteria);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MaintenanceRequest.Default mRForm = new MaintenanceRequest.Default();
            mRForm.mFromBrowse = true;
            mRForm.FormClosing += MRForm_FormClosing;
            mRForm.ShowDialog();
        }

        private void MRForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MaintenanceRequest.Default mRForm = (MaintenanceRequest.Default)sender;
            if (mRForm.mMaintenanceRequest == null) return;

            LoadFormControlsFromMaintenanceRequest(mRForm.mMaintenanceRequest);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadWorkOrder();
        }
    }
}
