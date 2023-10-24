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

namespace Qtech.AssetManagement.RepairAndMaintenance.MaintenanceJobType
{
    public partial class Default : Form, ICRUD
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

        MaintenanceJobTypeVariantCollection deleted_items;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.MaintenanceJobType)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadMaintenanceJobType()
        {
            ultraGrid1.SetDataBinding(MaintenanceJobTypeManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveMaintenanceJobType()
        {
            BusinessEntities.MaintenanceJobType item = new BusinessEntities.MaintenanceJobType();
            LoadMaintenanceJobTypeFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = MaintenanceJobTypeManager.Save(item);
                EndEditing();
                LoadMaintenanceJobType();

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

        private void LoadMaintenanceJobTypeFromFormControls(BusinessEntities.MaintenanceJobType myMaintenanceJobType)
        {
            myMaintenanceJobType.mId = int.Parse(Idlabel.Text);
            myMaintenanceJobType.mCode = CodetextBox.Text;
            myMaintenanceJobType.mName = NametextBox.Text;
            myMaintenanceJobType.mUserId = SessionUtil.mUser.mId;

            LoadMaintenanceJobTypeVariantFromFormControls(myMaintenanceJobType);
            myMaintenanceJobType.mDeletedMaintenanceJobTypeVariantCollection = deleted_items;
        }

        private void LoadMaintenanceJobTypeVariantFromFormControls(BusinessEntities.MaintenanceJobType myMaintenanceJobType)
        {
            MaintenanceJobTypeVariantCollection items = new MaintenanceJobTypeVariantCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                items.Add((MaintenanceJobTypeVariant)row.DataBoundItem);
            }
            myMaintenanceJobType.mMaintenanceJobTypeVariantCollection = items;
        }

        private void LoadFormControlsFromMaintenanceJobType(BusinessEntities.MaintenanceJobType myMaintenanceJobType)
        {
            Idlabel.Text = myMaintenanceJobType.mId.ToString();
            CodetextBox.Text = myMaintenanceJobType.mCode;
            NametextBox.Text = myMaintenanceJobType.mName;

            LoadFormControlsFromMaintenanceJobTypeVariant(myMaintenanceJobType);
        }

        private void LoadFormControlsFromMaintenanceJobTypeVariant(BusinessEntities.MaintenanceJobType myMaintenanceJobType)
        {
            MaintenanceJobTypeVariantCriteria criteria = new MaintenanceJobTypeVariantCriteria();
            criteria.mMaintenanceJobTypeId = myMaintenanceJobType.mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<MaintenanceJobTypeVariant>(MaintenanceJobTypeVariantManager.GetList(criteria));
            ItemsdataGridView.Refresh();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            deleted_items = null;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<MaintenanceJobTypeVariant>();
            ItemsdataGridView.Refresh();
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

            CodetextBox.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            MaintenanceJobTypeCriteria criteria = new MaintenanceJobTypeCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (MaintenanceJobTypeManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Maintenance job type name already exists."));

            criteria = new MaintenanceJobTypeCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (MaintenanceJobTypeManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Maintenance job type code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveMaintenanceJobType();
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
                BusinessEntities.MaintenanceJobType item = MaintenanceJobTypeManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                MaintenanceJobTypeManager.Delete(item);

                LoadMaintenanceJobType();

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

            BusinessEntities.MaintenanceJobType item = MaintenanceJobTypeManager.GetItem(_mId);
            LoadFormControlsFromMaintenanceJobType(item);

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
                (int)Modules.MaintenanceJobType);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadMaintenanceJobType();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveMaintenanceJobType();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (ItemsdataGridView.CurrentRow.IsNewRow) return;

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            MaintenanceJobTypeVariant item = (MaintenanceJobTypeVariant)ItemsdataGridView.CurrentRow.DataBoundItem;

            if (colName == "mDelete")
            {
                if (item.mId > 0)
                {
                    if (!allow_delete)
                    {
                        MessageUtil.NotAllowedDeleteAccess();
                        return;
                    }
                }

                if (deleted_items == null)
                    deleted_items = new  MaintenanceJobTypeVariantCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
        }

        private void ItemsdataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                MaintenanceJobTypeVariant item = (MaintenanceJobTypeVariant)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem;
                if (item == null) return;

                string name = ((DataGridView)sender).Columns[e.ColumnIndex].Name;
                if (name == "mCode")
                {
                    string inputValue = e.FormattedValue.ToString();
                    if (inputValue != item.mCode)//code
                    {
                        MaintenanceJobTypeVariantCriteria criteria = new MaintenanceJobTypeVariantCriteria();
                        criteria.mId = item.mId;
                        criteria.mCode = inputValue;
                        if (MaintenanceJobTypeVariantManager.SelectCountForGetList(criteria) > 0)
                        {
                            MessageBox.Show("Maintenance job type variant code already exists.", "Job Type Variant", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            e.Cancel = true;
                        }
                    }
                }

                if (name == "mName")
                {
                    string inputValue = e.FormattedValue.ToString();
                    if (inputValue != item.mName)
                    {
                        MaintenanceJobTypeVariantCriteria criteria = new MaintenanceJobTypeVariantCriteria();
                        criteria.mId = item.mId;
                        criteria.mName = inputValue;
                        if (MaintenanceJobTypeVariantManager.SelectCountForGetList(criteria) > 0)
                        {
                            MessageBox.Show("Maintenance job type variant name already exists.", "Job Type Variant", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch { }
        }
    }
}
