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

namespace Qtech.AssetManagement.Purchasing.Receiving
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

        public bool mForPurchaseVoucher { get; set; }
        public BusinessEntities.Receiving mReceiving { get; set; }
        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;

        ReceivingDetailCollection deleted_items;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.Receiving)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadReceiving()
        {
            ReceivingCriteria criteria = new ReceivingCriteria();
            if (StartdateTimePicker.Checked && EnddateTimePicker.Checked)
            {
                criteria.mStartDate = StartdateTimePicker.Value.Date;
                criteria.mEndDate = EnddateTimePicker.Value.Date;
            }

            if(mForPurchaseVoucher)
            {
                criteria = new ReceivingCriteria();
                criteria.mForPurchaseVoucher = true;
            }

            ultraGrid1.SetDataBinding(ReceivingManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
            
            criteria = new ReceivingCriteria();
            criteria.mForPurchaseVoucher = true;
            ForApvbutton.Text = "For APV: " + ReceivingManager.SelectCountForGetList(criteria).ToString();

            criteria = new ReceivingCriteria();
            criteria.mForApproval = true;
            ForApprovalbutton.Text = "For Approval: " + ReceivingManager.SelectCountForGetList(criteria).ToString();
        }

        private int SaveReceiving()
        {
            BusinessEntities.Receiving item = new BusinessEntities.Receiving();
            LoadReceivingFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = ReceivingManager.Save(item);
                EndEditing();
                LoadReceiving();

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

        private void LoadReceivingFromFormControls(BusinessEntities.Receiving myReceiving)
        {
            myReceiving.mId = int.Parse(Idlabel.Text);
            myReceiving.mPurchaseOrderId = int.Parse(PurchaseOrderIdtIdlabel.Text);
            
            myReceiving.mPreparedById = ControlUtil.UltraComboReturnValue(PreparedByutraCombo);
            myReceiving.mPreparedByName = PreparedByutraCombo.Text;
            myReceiving.mCheckedById = ControlUtil.UltraComboReturnValue(CheckedByultraCombo);
            myReceiving.mCheckedByName = CheckedByultraCombo.Text;
            myReceiving.mApprovedById = int.Parse(ApprovedBylabel.Text);
            myReceiving.mApprovedByName = ApprovedBytextBox.Text;
            myReceiving.mInvoiceNo = InvoiceNotextBox.Text;
            myReceiving.mDrNo = DRNotextBox.Text;
            myReceiving.mRemarks = RemarksultraTextEditor.Text;
            myReceiving.mUserId = SessionUtil.mUser.mId;

            LoadReceivingDetailFromFormControls(myReceiving);
            myReceiving.mDeletedReceivingDetailCollection = deleted_items;
        }

        private void LoadReceivingDetailFromFormControls(BusinessEntities.Receiving myReceiving)
        {
            ReceivingDetailCollection items = new ReceivingDetailCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
                items.Add((ReceivingDetail)row.DataBoundItem);
            myReceiving.mReceivingDetailCollection = items;
        }

        private void LoadFormControlsFromReceiving(BusinessEntities.Receiving myReceiving)
        {
            Idlabel.Text = myReceiving.mId.ToString();
            TransactionNotextBox.Text = myReceiving.mTransactionNo;
            dateTimePicker.Value = myReceiving.mDate;
            PurchaseOrderIdtIdlabel.Text = myReceiving.mPurchaseOrderId.ToString();
            PurchaseOrderNotextBox.Text = myReceiving.mPurchaseOrderNo;
            PreparedByutraCombo.Value = myReceiving.mPreparedById;
            CheckedByultraCombo.Value = myReceiving.mCheckedById;
            ApprovedBylabel.Text = myReceiving.mApprovedById.ToString();
            ApprovedBytextBox.Text = myReceiving.mApprovedByName;
            InvoiceNotextBox.Text = myReceiving.mInvoiceNo;
            DRNotextBox.Text = myReceiving.mDrNo;
            RemarksultraTextEditor.Text = myReceiving.mRemarks;

            LoadFormControlsFromReceivingDetail(myReceiving);
        }

        private void LoadFormControlsFromReceivingDetail(BusinessEntities.Receiving myReceiving)
        {
            ReceivingDetailCriteria criteria = new ReceivingDetailCriteria();
            criteria.mReceivingId = myReceiving.mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<ReceivingDetail>(ReceivingDetailManager.GetList(criteria));
            ItemsdataGridView.Refresh();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
            ApprovedBylabel.Text = "0";
            PurchaseOrderIdtIdlabel.Text = "0";

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<ReceivingDetail>();
            ItemsdataGridView.Refresh();

            deleted_items = null;
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

            PreparedByutraCombo.Focus();
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

            //ReceivingCriteria criteria = new ReceivingCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mName = NametextBox.Text;
            //if (ReceivingManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Name already exists."));

            //criteria = new ReceivingCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mCode = CodetextBox.Text;
            //if (ReceivingManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveReceiving();
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
                BusinessEntities.Receiving item = ReceivingManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                ReceivingManager.Delete(item);

                LoadReceiving();

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
            
            BusinessEntities.Receiving item = ReceivingManager.GetItem(_mId);

            if (mForPurchaseVoucher)
            {
                mReceiving = item;
                Close();
                return;
            }

            EndEditing();
            LoadFormControlsFromReceiving(item);
            ControlUtil.ExpandPanel(splitContainer1);
            PreparedByutraCombo.Focus();
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
                (int)Modules.Receiving);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadReceiving();
            ApprovedBybutton.Enabled = allow_delete;
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
            UltraComboUtil.Personnel(PreparedByutraCombo);
            UltraComboUtil.Personnel(CheckedByultraCombo);
        }

        
        private void ApprovedBybutton_Click(object sender, EventArgs e)
        {
            ApprovedBylabel.Text = SessionUtil.mUser.mId.ToString();
            ApprovedBytextBox.Text = PersonnelManager.GetItem(SessionUtil.mUser.mPersonnelId).mName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadReceiving();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PurchaseOrderCriteria criteria = new PurchaseOrderCriteria();
            criteria.mForReceiving = true;
            if (PurchaseOrderManager.SelectCountForGetList(criteria) == 0)
            {
                MessageBox.Show("No P.O. for receiving found.\nPurchase Order request must be approved to be able to browse for receiving", "Receiving", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PurchaseOrder.Default poForm = new PurchaseOrder.Default();
            poForm.mForReceiving = true;
            poForm.FormClosing += PoForm_FormClosing;
            poForm.ShowDialog();
        }

        private void PoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PurchaseOrder.Default poForm = (PurchaseOrder.Default)sender;
            if (poForm.mPurchaseOrder == null) return;

            if (poForm.mPurchaseOrder.mApprovedById == 0)
            {
                MessageBox.Show("Selected record is not yet approved.", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BusinessEntities.PurchaseOrder po = poForm.mPurchaseOrder;

            PurchaseOrderIdtIdlabel.Text = po.mId.ToString();
            PurchaseOrderNotextBox.Text = po.mTransactionNo;

            PurchaseOrderDetailCriteria criteria = new PurchaseOrderDetailCriteria();
            criteria.mPurchaseOrderId = po.mId;
            decimal quantity = 0;

            ReceivingDetailCollection items = new ReceivingDetailCollection();
            foreach (PurchaseOrderDetail poItem in PurchaseOrderDetailManager.GetList(criteria))
            {
                quantity = poItem.mQuantity;
                //check if still allowed for receiving
                ReceivingDetailCriteria orderCriteria = new ReceivingDetailCriteria();
                orderCriteria.mPurchaseOrderDetailId = poItem.mId;
                if (ReceivingDetailManager.SelectCountForGetList(orderCriteria) > 0)
                {
                    //check if still allowed balance must be greater than zero vs all ordered
                    quantity = poItem.mQuantity - ReceivingDetailManager.GetList(orderCriteria).Sum(x => x.mQuantity);
                    if (quantity <= 0) continue;
                }

                ReceivingDetail item = new ReceivingDetail();
                item.mPurchaseOrderDetailId = poItem.mId;
                item.mUnitName = poItem.mUnitName;
                item.mProductName = poItem.mProductName;
                item.mQuantity = quantity; //balance
                item.mCost = poItem.mCost;
                item.mTotalCost = poItem.mTotalCost;
                items.Add(item);
            }

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<ReceivingDetail>(items);
            ItemsdataGridView.Refresh();
        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            ReceivingDetail item = (ReceivingDetail)ItemsdataGridView.CurrentRow.DataBoundItem;

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
                    deleted_items = new  ReceivingDetailCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
        }

        private void ItemsdataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            ReceivingDetail item = (ReceivingDetail)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem;

            string name = ((DataGridView)sender).Columns[e.ColumnIndex].Name;
            if (name == "mQuantity")
            {
                decimal inputValue = e.FormattedValue.ToString() == string.Empty ? 0 : Convert.ToDecimal(e.FormattedValue);
                if (inputValue != item.mQuantity)//editing qty
                {
                    //check if user is trying to over quantity vs quotation

                    //existing po of this item
                    ReceivingDetailCriteria poDetailCriteria = new ReceivingDetailCriteria();
                    poDetailCriteria.mPurchaseOrderDetailId = item.mPurchaseOrderDetailId;

                    decimal totalReceivedQuantity = inputValue;
                    if (ReceivingDetailManager.SelectCountForGetList(poDetailCriteria) > 0)
                        totalReceivedQuantity += ReceivingDetailManager.GetList(poDetailCriteria).Sum(x => x.mQuantity);

                    //if editing existing receivived deduct the old qty
                    if (item.mId > 0) totalReceivedQuantity -= ReceivingDetailManager.GetItem(item.mId).mQuantity;

                    decimal poQty = PurchaseOrderDetailManager.GetItem(item.mPurchaseOrderDetailId).mQuantity;
                    if (totalReceivedQuantity > poQty)
                    {
                        MessageBox.Show("Invalid quantity, will result in over Receiving.\nPO Qty:" + poQty.ToString() + "\nReceived Qty: " + totalReceivedQuantity.ToString(), "Receiving", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void ForApprovalbutton_Click(object sender, EventArgs e)
        {
            ReceivingCriteria criteria = new ReceivingCriteria();
            criteria.mForApproval = true;
            ultraGrid1.SetDataBinding(ReceivingManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private void ForApvbutton_Click(object sender, EventArgs e)
        {
            ReceivingCriteria criteria = new ReceivingCriteria();
            criteria.mForPurchaseVoucher = true;
            ultraGrid1.SetDataBinding(ReceivingManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }
    }
}
