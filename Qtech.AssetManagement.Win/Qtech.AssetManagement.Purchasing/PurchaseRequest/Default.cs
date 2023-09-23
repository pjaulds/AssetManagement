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

namespace Qtech.AssetManagement.Purchasing.PurchaseRequest
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

        public bool mForQuotation { get; set; }
        public BusinessEntities.PurchaseRequest mPurchaseRequest { get; set; }
        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;

        PurchaseRequestDetailCollection deleted_items;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.PurchaseRequest)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadPurchaseRequest()
        {
            PurchaseRequestCriteria criteria = new PurchaseRequestCriteria();
            if (StartdateTimePicker.Checked && EnddateTimePicker.Checked)
            {
                criteria.mStartDate = StartdateTimePicker.Value.Date;
                criteria.mEndDate = EnddateTimePicker.Value.Date;
            }

            if(mForQuotation)
            {
                criteria = new PurchaseRequestCriteria();
                criteria.mForQuotation = mForQuotation;
            }
            
            ultraGrid1.SetDataBinding(PurchaseRequestManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();

            criteria = new PurchaseRequestCriteria();
            criteria.mForQuotation = true;
            ForQuotationbutton.Text = "For Quotation: " + PurchaseRequestManager.SelectCountForGetList(criteria).ToString();

            criteria = new PurchaseRequestCriteria();
            criteria.mForApproval = true;
            ForApprovalbutton.Text = "For Approval: " + PurchaseRequestManager.SelectCountForGetList(criteria).ToString();
        }

        private int SavePurchaseRequest()
        {
            BusinessEntities.PurchaseRequest item = new BusinessEntities.PurchaseRequest();
            LoadPurchaseRequestFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = PurchaseRequestManager.Save(item);
                EndEditing();
                LoadPurchaseRequest();

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

        private void LoadPurchaseRequestFromFormControls(BusinessEntities.PurchaseRequest myPurchaseRequest)
        {
            myPurchaseRequest.mId = int.Parse(Idlabel.Text);
            myPurchaseRequest.mRequestedById = ControlUtil.UltraComboReturnValue(RequestedByutraCombo);
            myPurchaseRequest.mRequestedByName = RequestedByutraCombo.Text;
            myPurchaseRequest.mDateRequired = DateRequireddateTimePicker.Value.Date;
            myPurchaseRequest.mSupplier1Id = ControlUtil.UltraComboReturnValue(Supplier1ultraCombo);
            myPurchaseRequest.mSupplier1Name = Supplier1ultraCombo.Text;
            myPurchaseRequest.mSupplier2Id = ControlUtil.UltraComboReturnValue(Supplier2ultraCombo);
            myPurchaseRequest.mSupplier2Name = Supplier2ultraCombo.Text;
            myPurchaseRequest.mSupplier3Id = ControlUtil.UltraComboReturnValue(Supplier3ultraCombo);
            myPurchaseRequest.mSupplier3Name = Supplier3ultraCombo.Text;
            myPurchaseRequest.mRemarks = RemarksultraTextEditor.Text;
            myPurchaseRequest.mApprovedById = int.Parse(ApprovedBylabel.Text);
            myPurchaseRequest.mApprovedByName = ApprovedBytextBox.Text;
            myPurchaseRequest.mUserId = SessionUtil.mUser.mId;

            LoadPurchaseRequestDetailFromFormControls(myPurchaseRequest);
            myPurchaseRequest.mDeletedPurchaseRequestDetailCollection = deleted_items;
        }

        private void LoadPurchaseRequestDetailFromFormControls(BusinessEntities.PurchaseRequest myPurchaseRequest)
        {
            PurchaseRequestDetailCollection items = new PurchaseRequestDetailCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
                items.Add((PurchaseRequestDetail)row.DataBoundItem);
            myPurchaseRequest.mPurchaseRequestDetailCollection = items;
        }

        private void LoadFormControlsFromPurchaseRequest(BusinessEntities.PurchaseRequest myPurchaseRequest)
        {
            Idlabel.Text = myPurchaseRequest.mId.ToString();
            dateTimePicker.Value = myPurchaseRequest.mDate;
            TransactionNotextBox.Text = myPurchaseRequest.mTransactionNo;
            RequestedByutraCombo.Value = myPurchaseRequest.mRequestedById;
            DateRequireddateTimePicker.Value = myPurchaseRequest.mDateRequired;
            Supplier1ultraCombo.Value = myPurchaseRequest.mSupplier1Id;
            Supplier2ultraCombo.Value = myPurchaseRequest.mSupplier2Id;
            Supplier3ultraCombo.Value = myPurchaseRequest.mSupplier3Id;
            RemarksultraTextEditor.Text = myPurchaseRequest.mRemarks;
            ApprovedBylabel.Text = myPurchaseRequest.mApprovedById.ToString();
            ApprovedBytextBox.Text = myPurchaseRequest.mApprovedByName;

            LoadFormControlsFromPurchaseRequestDetail(myPurchaseRequest);
        }

        private void LoadFormControlsFromPurchaseRequestDetail(BusinessEntities.PurchaseRequest myPurchaseRequest)
        {
            PurchaseRequestDetailCriteria criteria = new PurchaseRequestDetailCriteria();
            criteria.mPurchaseRequestId = myPurchaseRequest.mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<PurchaseRequestDetail>(PurchaseRequestDetailManager.GetList(criteria));
            ItemsdataGridView.Refresh();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
            ApprovedBylabel.Text = "0";

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<PurchaseRequestDetail>();
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

            RequestedByutraCombo.Focus();
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
                if (!allow_delete)
                {
                    QuotationCriteria quotationCriteria = new QuotationCriteria();
                    quotationCriteria.mPurchaseRequestId = int.Parse(Idlabel.Text);
                    if (QuotationManager.SelectCountForGetList(quotationCriteria) > 0)
                    {
                        MessageBox.Show("This purchase request already has quotation record only admin can update this record.", "Purchase Request", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return 0;
                    }
                }
            }

            BrokenRulesCollection rules = new BrokenRulesCollection();

            //PurchaseRequestCriteria criteria = new PurchaseRequestCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mName = NametextBox.Text;
            //if (PurchaseRequestManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Name already exists."));

            //criteria = new PurchaseRequestCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mCode = CodetextBox.Text;
            //if (PurchaseRequestManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SavePurchaseRequest();
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
                BusinessEntities.PurchaseRequest item = PurchaseRequestManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                PurchaseRequestManager.Delete(item);

                LoadPurchaseRequest();

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


            BusinessEntities.PurchaseRequest item = PurchaseRequestManager.GetItem(_mId);
            if (mForQuotation)
            {
                mPurchaseRequest = item;
                Close();
                return;
            }

            LoadFormControlsFromPurchaseRequest(item);

            ControlUtil.ExpandPanel(splitContainer1);

            RequestedByutraCombo.Focus();
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
                (int)Modules.PurchaseRequest);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadPurchaseRequest();
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
            UltraComboUtil.Personnel(RequestedByutraCombo);
            UltraComboUtil.Supplier(Supplier1ultraCombo);
            UltraComboUtil.Supplier(Supplier2ultraCombo);
            UltraComboUtil.Supplier(Supplier3ultraCombo);
        }

        private void searchProductControl1__SearchingProduct(object sender, EventArgs e)
        {
            if (searchProductUserControl1.mUltraGrid.Visible)
            {
                searchProductUserControl1.Dock = DockStyle.Fill;
                searchProductUserControl1.BringToFront();
            }
            else
            {
                searchProductUserControl1.Dock = DockStyle.Top;
                ItemsdataGridView.BringToFront();
            }
        }

        decimal quantity = 0;
        private void searchProductControl1__GetProduct(object sender, EventArgs e)
        {
            searchProductUserControl1.Dock = DockStyle.Top;
            ItemsdataGridView.BringToFront();

            quantity = searchProductUserControl1.mQuantity;
            if (searchProductUserControl1.mProduct != null && searchProductUserControl1.mQuantity > 0)
                AddProduct(searchProductUserControl1.mProduct);
        }
        private void AddProduct(BusinessEntities.Product myProduct)
        {
            SortableBindingList<PurchaseRequestDetail> items = new SortableBindingList<PurchaseRequestDetail>();

            if (ItemsdataGridView.DataSource != null)
                items = (SortableBindingList<PurchaseRequestDetail>)ItemsdataGridView.DataSource;

            if (items.Where(x => x.mProductId == myProduct.mId).ToList().Count > 0)
            {
                MessageBox.Show("Product " + myProduct.mName + " is already exists on the list");
                return;
            }

            PurchaseRequestDetail item = new PurchaseRequestDetail();
            item.mQuantity = quantity;
            item.mProductId = myProduct.mId;
            item.mProductName = myProduct.mName;
            items.Add(item);
            quantity = 0;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = items;
            ItemsdataGridView.Refresh();
        }

        private void ApprovedBybutton_Click(object sender, EventArgs e)
        {
            ApprovedBylabel.Text = SessionUtil.mUser.mId.ToString();
            ApprovedBytextBox.Text = PersonnelManager.GetItem(SessionUtil.mUser.mPersonnelId).mName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPurchaseRequest();
        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            PurchaseRequestDetail item = (PurchaseRequestDetail)ItemsdataGridView.CurrentRow.DataBoundItem;

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
                    deleted_items = new  PurchaseRequestDetailCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
        }

        private void ForApprovalbutton_Click(object sender, EventArgs e)
        {
            PurchaseRequestCriteria criteria = new PurchaseRequestCriteria();
            criteria.mForApproval = true;
            ultraGrid1.SetDataBinding(PurchaseRequestManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private void ForQuotationbutton_Click(object sender, EventArgs e)
        {
            PurchaseRequestCriteria criteria = new PurchaseRequestCriteria();
            criteria.mForQuotation = true;
            ultraGrid1.SetDataBinding(PurchaseRequestManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }
    }
}
