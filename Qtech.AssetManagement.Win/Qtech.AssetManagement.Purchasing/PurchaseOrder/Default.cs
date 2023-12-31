﻿using Infragistics.Win.UltraWinGrid;
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

namespace Qtech.AssetManagement.Purchasing.PurchaseOrder
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

        public bool mForReceiving { get; set; }
        
        public BusinessEntities.PurchaseOrder mPurchaseOrder { get; set; }
        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;

        PurchaseOrderDetailCollection deleted_items;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.PurchaseOrder)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadPurchaseOrder()
        {
            PurchaseOrderCriteria criteria = new PurchaseOrderCriteria();
            if (StartdateTimePicker.Checked && EnddateTimePicker.Checked)
            {
                criteria.mStartDate = StartdateTimePicker.Value.Date;
                criteria.mEndDate = EnddateTimePicker.Value.Date;
            }

            if(mForReceiving)
            {
                criteria = new PurchaseOrderCriteria();
                criteria.mForReceiving = true;
            }

            ultraGrid1.SetDataBinding(PurchaseOrderManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();

            criteria = new PurchaseOrderCriteria();
            criteria.mForReceiving = true;
            ForApvbutton.Text = "For Receiving: " + PurchaseOrderManager.SelectCountForGetList(criteria).ToString();

            criteria = new PurchaseOrderCriteria();
            criteria.mForApproval = true;
            ForApprovalbutton.Text = "For Approval: " + PurchaseOrderManager.SelectCountForGetList(criteria).ToString();
        }

        private int SavePurchaseOrder()
        {
            BusinessEntities.PurchaseOrder item = new BusinessEntities.PurchaseOrder();
            LoadPurchaseOrderFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = PurchaseOrderManager.Save(item);
                EndEditing();
                LoadPurchaseOrder();

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

        private void LoadPurchaseOrderFromFormControls(BusinessEntities.PurchaseOrder myPurchaseOrder)
        {
            myPurchaseOrder.mId = int.Parse(Idlabel.Text);
            myPurchaseOrder.mQuotationId = int.Parse(QuotationIdtIdlabel.Text);
            myPurchaseOrder.mDateOfDelivery = DateOfDeliverydateTimePicker.Checked ? DateOfDeliverydateTimePicker.Value.Date : DateTime.MinValue;
            myPurchaseOrder.mTerms = TermstextBox.Text;
            myPurchaseOrder.mPreparedById = ControlUtil.UltraComboReturnValue(PreparedByutraCombo);
            myPurchaseOrder.mPreparedByName = PreparedByutraCombo.Text;
            myPurchaseOrder.mNotedById = int.Parse(NotedByIdlabel.Text);
            myPurchaseOrder.mNotedByName = NotedBytextBox.Text;
            myPurchaseOrder.mRevised = RevisedcheckBox.Checked;
            myPurchaseOrder.mCancelled = CancelledcheckBox.Checked;
            myPurchaseOrder.mCurrencyId = ControlUtil.UltraComboReturnValue(CurrencyultraCombo);
            myPurchaseOrder.mCurrencyName = CurrencyultraCombo.Text;

            myPurchaseOrder.mApprovedById = int.Parse(ApprovedBylabel.Text);
            myPurchaseOrder.mApprovedByName = ApprovedBytextBox.Text;
            myPurchaseOrder.mUserId = SessionUtil.mUser.mId;

            LoadPurchaseOrderDetailFromFormControls(myPurchaseOrder);
            myPurchaseOrder.mDeletedPurchaseOrderDetailCollection = deleted_items;
        }

        private void LoadPurchaseOrderDetailFromFormControls(BusinessEntities.PurchaseOrder myPurchaseOrder)
        {
            PurchaseOrderDetailCollection items = new PurchaseOrderDetailCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
                items.Add((PurchaseOrderDetail)row.DataBoundItem);
            myPurchaseOrder.mPurchaseOrderDetailCollection = items;
        }

        private void LoadFormControlsFromPurchaseOrder(BusinessEntities.PurchaseOrder myPurchaseOrder)
        {
            Idlabel.Text = myPurchaseOrder.mId.ToString();
            TransactionNotextBox.Text = myPurchaseOrder.mTransactionNo;
            dateTimePicker.Value = myPurchaseOrder.mDate;
            QuotationIdtIdlabel.Text = myPurchaseOrder.mQuotationId.ToString();
            QuotationNotextBox.Text = myPurchaseOrder.mQuotationNo;
            if (myPurchaseOrder.mDateOfDelivery != DateTime.MinValue)
                DateOfDeliverydateTimePicker.Value = myPurchaseOrder.mDateOfDelivery;
            TermstextBox.Text = myPurchaseOrder.mTerms;
            PreparedByutraCombo.Value = myPurchaseOrder.mPreparedById;
            NotedByIdlabel.Text = myPurchaseOrder.mNotedById.ToString();
            NotedBytextBox.Text = myPurchaseOrder.mNotedByName;
            RevisedcheckBox.Checked = myPurchaseOrder.mRevised;
            CancelledcheckBox.Checked = myPurchaseOrder.mCancelled;
            CurrencyultraCombo.Value = myPurchaseOrder.mCurrencyId;

            ApprovedBylabel.Text = myPurchaseOrder.mApprovedById.ToString();
            ApprovedBytextBox.Text = myPurchaseOrder.mApprovedByName;

            LoadFormControlsFromPurchaseOrderDetail(myPurchaseOrder);
        }

        private void LoadFormControlsFromPurchaseOrderDetail(BusinessEntities.PurchaseOrder myPurchaseOrder)
        {
            PurchaseOrderDetailCriteria criteria = new PurchaseOrderDetailCriteria();
            criteria.mPurchaseOrderId = myPurchaseOrder.mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<PurchaseOrderDetail>(PurchaseOrderDetailManager.GetList(criteria));
            ItemsdataGridView.Refresh();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<PurchaseOrderDetail>();
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
            linkLabel1_LinkClicked(this, new LinkLabelLinkClickedEventArgs(linkLabel1.Links[0]));
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

            //PurchaseOrderCriteria criteria = new PurchaseOrderCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mName = NametextBox.Text;
            //if (PurchaseOrderManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Name already exists."));

            //criteria = new PurchaseOrderCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mCode = CodetextBox.Text;
            //if (PurchaseOrderManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SavePurchaseOrder();
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
                BusinessEntities.PurchaseOrder item = PurchaseOrderManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                PurchaseOrderManager.Delete(item);

                LoadPurchaseOrder();

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

            BusinessEntities.PurchaseOrder item = PurchaseOrderManager.GetItem(_mId);

            if (mForReceiving)
            {
                mPurchaseOrder = item;
                Close();
                return;
            }
            
            EndEditing();            
            LoadFormControlsFromPurchaseOrder(item);
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
                (int)Modules.PurchaseOrder);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadPurchaseOrder();
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
            UltraComboUtil.Currency(CurrencyultraCombo);
        }

        
        private void ApprovedBybutton_Click(object sender, EventArgs e)
        {
            if (!allow_delete) //not admin
            {
                User.LogInForm logIn = new User.LogInForm();
                logIn.mForOverride = true;
                logIn.FormClosing += LogInApproval_FormClosing;
                logIn.ShowDialog();
            }
            else
            {
                ApprovedBylabel.Text = SessionUtil.mUser.mId.ToString();
                ApprovedBytextBox.Text = PersonnelManager.GetItem(SessionUtil.mUser.mPersonnelId).mName;
            }
        }

        private void LogInApproval_FormClosing(object sender, FormClosingEventArgs e)
        {
            User.LogInForm logIn = (User.LogInForm)sender;
            if (logIn.mUser == null) return;

            if (!SessionUtil.UserAllowApprove(logIn.mUser, (int)Modules.PurchaseOrder))
            {
                MessageBox.Show("You are not allowed to override transaction (approval).", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ApprovedBylabel.Text = logIn.mUser.mId.ToString();
            ApprovedBytextBox.Text = PersonnelManager.GetItem(logIn.mUser.mPersonnelId).mName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPurchaseOrder();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            QuotationCriteria criteria = new QuotationCriteria();
            criteria.mForPo = true;
            if (QuotationManager.SelectCountForGetList(criteria) == 0)
            {
                MessageBox.Show("No quotation for P.O. found.\nQuotation request must be approved to be able to browse for P.O.", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Quotation.Default qForm = new Quotation.Default();
            qForm.mForPo = true;
            qForm.FormClosing += qForm_FormClosing;
            qForm.ShowDialog();
        }

        private void qForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Quotation.Default prForm = (Quotation.Default)sender;
            if (prForm.mQuotation == null) return;

            if (prForm.mQuotation.mApprovedById == 0)
            {
                MessageBox.Show("Selected record is not yet approved.", "Quotation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BusinessEntities.Quotation q = prForm.mQuotation;

            QuotationIdtIdlabel.Text = q.mId.ToString();
            QuotationNotextBox.Text = q.mTransactionNo;

            QuotationDetailCriteria criteria = new QuotationDetailCriteria();
            criteria.mQuotationId = q.mId;
            decimal quantity = 0;

            PurchaseOrderDetailCollection items = new PurchaseOrderDetailCollection();
            foreach (QuotationDetail qItem in QuotationDetailManager.GetList(criteria))
            {
                quantity = qItem.mQuantity;
                //check if still allowed for po
                PurchaseOrderDetailCriteria orderCriteria = new PurchaseOrderDetailCriteria();
                orderCriteria.mQuotationDetailId = qItem.mId;
                if(PurchaseOrderDetailManager.SelectCountForGetList(orderCriteria) >0)
                {
                    //check if still allowed balance must be greater than zero vs all ordered
                    quantity = qItem.mQuantity - PurchaseOrderDetailManager.GetList(orderCriteria).Sum(x => x.mQuantity);
                    if (quantity <= 0) continue;
                }

                PurchaseOrderDetail item = new PurchaseOrderDetail();
                item.mQuotationDetailId = qItem.mId;
                item.mUnitName = qItem.mUnitName;
                item.mProductName = qItem.mProductName;
                item.mQuantity = quantity; //balance
                item.mCost = qItem.mCost;
                item.mTotalCost = qItem.mTotalCost;
                items.Add(item);
            }

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<PurchaseOrderDetail>(items);
            ItemsdataGridView.Refresh();
        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            PurchaseOrderDetail item = (PurchaseOrderDetail)ItemsdataGridView.CurrentRow.DataBoundItem;

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
                    deleted_items = new  PurchaseOrderDetailCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
        }

        private void ItemsdataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            PurchaseOrderDetail item = (PurchaseOrderDetail)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem;

            string name = ((DataGridView)sender).Columns[e.ColumnIndex].Name;
            if (name == "mQuantity")
            {
                decimal inputValue = e.FormattedValue.ToString() == string.Empty ? 0 : Convert.ToDecimal(e.FormattedValue);
                if (inputValue != item.mQuantity)//editing qty
                {
                    //check if user is trying to over quantity vs quotation

                    //existing po of this item
                    PurchaseOrderDetailCriteria poDetailCriteria = new PurchaseOrderDetailCriteria();
                    poDetailCriteria.mQuotationDetailId = item.mQuotationDetailId;

                    decimal totalPoQuantity = inputValue;
                    if (PurchaseOrderDetailManager.SelectCountForGetList(poDetailCriteria) > 0)
                        totalPoQuantity += PurchaseOrderDetailManager.GetList(poDetailCriteria).Sum(x => x.mQuantity);

                    //if editing existing PO deduct the old qty
                    if (item.mId > 0) totalPoQuantity -= PurchaseOrderDetailManager.GetItem(item.mId).mQuantity;

                    decimal quotationQty = QuotationDetailManager.GetItem(item.mQuotationDetailId).mQuantity;
                    if (totalPoQuantity > quotationQty)
                    {
                        MessageBox.Show("Invalid quantity, will result in over PO.\nQuotation Qty:" + quotationQty.ToString() + "\nPO Qty: " + totalPoQuantity.ToString(), "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void ForApprovalbutton_Click(object sender, EventArgs e)
        {
            PurchaseOrderCriteria criteria = new PurchaseOrderCriteria();
            criteria.mForApproval = true;
            ultraGrid1.SetDataBinding(PurchaseOrderManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private void ForReceivingbutton_Click(object sender, EventArgs e)
        {
            PurchaseOrderCriteria criteria = new PurchaseOrderCriteria();
            criteria.mForReceiving = true;
            ultraGrid1.SetDataBinding(PurchaseOrderManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private void Notedbutton_Click(object sender, EventArgs e)
        {
            if (!allow_delete) //not admin
            {
                User.LogInForm logIn = new User.LogInForm();
                logIn.mForOverride = true;
                logIn.FormClosing += LogIn_FormClosing;
                logIn.ShowDialog();
            }
            else
            {
                NotedByIdlabel.Text = SessionUtil.mUser.mPersonnelId.ToString();
                NotedBytextBox.Text = PersonnelManager.GetItem(SessionUtil.mUser.mPersonnelId).mName;
            }
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            User.LogInForm logIn = (User.LogInForm)sender;
            if (logIn.mUser == null) return;

            if (!SessionUtil.UserAllowNotedBy(logIn.mUser, (int)Modules.PurchaseOrder))
            {
                MessageBox.Show("You are not allowed to override transaction (checked by).", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            NotedByIdlabel.Text = logIn.mUser.mPersonnelId.ToString();
            NotedBytextBox.Text = PersonnelManager.GetItem(logIn.mUser.mPersonnelId).mName;
        }
    }
}
