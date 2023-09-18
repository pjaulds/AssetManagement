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

namespace Qtech.AssetManagement.Purchasing.Quotation
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

        public bool mForPo { get; set; }
        public BusinessEntities.Quotation mQuotation { get; set; }
        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;

        QuotationDetailCollection deleted_items;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.Quotation)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadQuotation()
        {
            QuotationCriteria criteria = new QuotationCriteria();
            if (StartdateTimePicker.Checked && EnddateTimePicker.Checked)
            {
                criteria.mStartDate = StartdateTimePicker.Value.Date;
                criteria.mEndDate = EnddateTimePicker.Value.Date;
            }

            ultraGrid1.SetDataBinding(QuotationManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveQuotation()
        {
            BusinessEntities.Quotation item = new BusinessEntities.Quotation();
            LoadQuotationFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = QuotationManager.Save(item);
                EndEditing();
                LoadQuotation();

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

        private void LoadQuotationFromFormControls(BusinessEntities.Quotation myQuotation)
        {
            myQuotation.mId = int.Parse(Idlabel.Text);
            myQuotation.mPurchaseRequestId = int.Parse(PurchaseRequestIdlabel.Text);
            myQuotation.mPurchaseRequestNo = PRNotextBox.Text;
            myQuotation.mPreparedById = ControlUtil.UltraComboReturnValue(PreparedByutraCombo);
            myQuotation.mPreparedByName = PreparedByutraCombo.Text;

            if (Supplier1radioButton.Checked)
                myQuotation.mSupplierNo = 1;

            if (Supplier2radioButton.Checked)
                myQuotation.mSupplierNo = 2;

            if (Supplier3radioButton.Checked)
                myQuotation.mSupplierNo = 3;

            myQuotation.mApprovedById = int.Parse(ApprovedBylabel.Text);
            myQuotation.mApprovedByName = ApprovedBytextBox.Text;
            myQuotation.mUserId = SessionUtil.mUser.mId;

            LoadQuotationDetailFromFormControls(myQuotation);
            myQuotation.mDeletedQuotationDetailCollection = deleted_items;
        }

        private void LoadQuotationDetailFromFormControls(BusinessEntities.Quotation myQuotation)
        {
            QuotationDetailCollection items = new QuotationDetailCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
                items.Add((QuotationDetail)row.DataBoundItem);
            myQuotation.mQuotationDetailCollection = items;
        }

        private void LoadFormControlsFromQuotation(BusinessEntities.Quotation myQuotation)
        {
            Idlabel.Text = myQuotation.mId.ToString();
            dateTimePicker.Value = myQuotation.mDate;
            PurchaseRequestIdlabel.Text = myQuotation.mPurchaseRequestId.ToString();
            PRNotextBox.Text = myQuotation.mPurchaseRequestNo;

            PreparedByutraCombo.Value = myQuotation.mPreparedById;

            Supplier1radioButton.Checked = myQuotation.mSupplierNo == 1;
            Supplier2radioButton.Checked = myQuotation.mSupplierNo == 2;
            Supplier3radioButton.Checked = myQuotation.mSupplierNo == 3;

            Supplier1textBox.Text = myQuotation.mSupplier1Name;
            Supplier2textBox.Text = myQuotation.mSupplier2Name;
            Supplier3textBox.Text = myQuotation.mSupplier3Name;

            ApprovedBylabel.Text = myQuotation.mApprovedById.ToString();
            ApprovedBytextBox.Text = myQuotation.mApprovedByName;

            LoadFormControlsFromQuotationDetail(myQuotation);
        }

        private void LoadFormControlsFromQuotationDetail(BusinessEntities.Quotation myQuotation)
        {
            QuotationDetailCriteria criteria = new QuotationDetailCriteria();
            criteria.mQuotationId = myQuotation.mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<QuotationDetail>(QuotationDetailManager.GetList(criteria));
            ItemsdataGridView.Refresh();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
            ApprovedBylabel.Text = "0";
            PurchaseRequestIdlabel.Text = "0";

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<QuotationDetail>();
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

                //check if there is existing quoation do not allow save unless admin
                //if (!allow_delete)
                //{
                //    QuotationCriteria quotationCriteria = new QuotationCriteria();
                //    quotationCriteria.mPurchaseRequestId = int.Parse(Idlabel.Text);
                //    if (QuotationManager.SelectCountForGetList(quotationCriteria) > 0)
                //    {
                //        MessageBox.Show("This purchase request already has quotation record only admin can update this record.", "Purchase Request", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        return 0;
                //    }
                //}
            }

            BrokenRulesCollection rules = new BrokenRulesCollection();

            //QuotationCriteria criteria = new QuotationCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mName = NametextBox.Text;
            //if (QuotationManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Name already exists."));

            //criteria = new QuotationCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mCode = CodetextBox.Text;
            //if (QuotationManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveQuotation();
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
                BusinessEntities.Quotation item = QuotationManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                QuotationManager.Delete(item);

                LoadQuotation();

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
            
            BusinessEntities.Quotation item = QuotationManager.GetItem(_mId);

            if (mForPo)
            {
                mQuotation = item;
                Close();
                return;
            }

            LoadFormControlsFromQuotation(item);

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
                (int)Modules.Quotation);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadQuotation();
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
        }

        
        private void ApprovedBybutton_Click(object sender, EventArgs e)
        {
            ApprovedBylabel.Text = SessionUtil.mUser.mId.ToString();
            ApprovedBytextBox.Text = PersonnelManager.GetItem(SessionUtil.mUser.mPersonnelId).mName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadQuotation();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PurchaseRequestCriteria criteria = new PurchaseRequestCriteria();
            criteria.mForQuotation = true;
            if (PurchaseRequestManager.SelectCountForGetList(criteria) == 0)
            {
                MessageBox.Show("No purchase request for quotation found.\nPurchase request must be approved to be able to browse for quotation.", "Quotation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PurchaseRequest.Default prForm = new PurchaseRequest.Default();
            prForm.mForQuotation = true;
            prForm.FormClosing += PrForm_FormClosing;
            prForm.ShowDialog();
        }

        private void PrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PurchaseRequest.Default prForm = (PurchaseRequest.Default)sender;
            if (prForm.mPurchaseRequest == null) return;

            BusinessEntities.PurchaseRequest pr = prForm.mPurchaseRequest;

            PurchaseRequestIdlabel.Text = pr.mId.ToString();
            PRNotextBox.Text = pr.mTransactionNo;
            Supplier1textBox.Text = pr.mSupplier1Name;
            Supplier2textBox.Text = pr.mSupplier2Name;
            Supplier3textBox.Text = pr.mSupplier3Name;

            PurchaseRequestDetailCriteria criteria = new PurchaseRequestDetailCriteria();
            criteria.mPurchaseRequestId = pr.mId;

            QuotationDetailCollection items = new QuotationDetailCollection();
            foreach (PurchaseRequestDetail prItem in PurchaseRequestDetailManager.GetList(criteria))
            {
                QuotationDetail item = new QuotationDetail();
                item.mPurchaseRequestDetailId = prItem.mId;
                item.mProductName = prItem.mProductName;
                item.mQuantity = prItem.mQuantity;
                items.Add(item);
            }

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<QuotationDetail>(items);
            ItemsdataGridView.Refresh();
        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            QuotationDetail item = (QuotationDetail)ItemsdataGridView.CurrentRow.DataBoundItem;

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
                    deleted_items = new  QuotationDetailCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
        }
    }
}
