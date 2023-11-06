using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.Utilities;

namespace Qtech.AssetManagement.RepairAndMaintenance.WorkOrder
{
    public partial class HoursUserControl : UserControl
    {
        public HoursUserControl()
        {
            InitializeComponent();
        }

        #region Private Members

        #endregion

        #region Public Members
        public int mWorkOrderId { get; set; }
        public WorkOrderHoursCollection deleted_items;
        public bool allow_delete;
        #endregion

        #region Public Methods
        public void LoadExpenseCategory()
        {
            DataGridViewComboBoxColumn ComboColumn = (DataGridViewComboBoxColumn)(ItemsdataGridView.Columns["mExpenseCategoryId"]);
            ComboColumn.DataSource = ExpenseCategoryManager.GetList();
            ComboColumn.DisplayMember = "mName";
            ComboColumn.ValueMember = "mId";
        }
        public WorkOrderHoursCollection LoadWorkOrderHoursFromFormControls()
        {
            WorkOrderHoursCollection items = new WorkOrderHoursCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                items.Add((WorkOrderHours)row.DataBoundItem);
            }
            return items;
        }
        public void LoadFormControlsFromWorkOrderHours()
        {
            WorkOrderHoursCriteria criteria = new WorkOrderHoursCriteria();
            criteria.mWorkOrderId = mWorkOrderId;
            ItemsdataGridView.DataSource = new SortableBindingList<WorkOrderHours>(WorkOrderHoursManager.GetList(criteria));
            ItemsdataGridView.Refresh();
        }
        public void EndEditing()
        {
            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<WorkOrderHours>();
            ItemsdataGridView.Refresh();

            deleted_items = null;
        }
        #endregion

        private void ItemsdataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (ItemsdataGridView.CurrentRow.IsNewRow) return;

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            WorkOrderHours item = (WorkOrderHours)ItemsdataGridView.CurrentRow.DataBoundItem;

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
                    deleted_items = new WorkOrderHoursCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
        }

        private void HoursUserControl_Load(object sender, EventArgs e)
        {

        }
    }
}
