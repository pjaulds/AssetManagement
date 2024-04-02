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

namespace Qtech.AssetManagement.JournalVoucher
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

        JournalVoucherDetailCollection deleted_items;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.JournalVoucher)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadJournalVoucher()
        {
            JournalVoucherCriteria criteria = new JournalVoucherCriteria();
            if (StartdateTimePicker.Checked && EnddateTimePicker.Checked)
            {
                criteria.mStartDate = StartdateTimePicker.Value.Date;
                criteria.mEndDate = EnddateTimePicker.Value.Date;
            }
            
            ultraGrid1.SetDataBinding(JournalVoucherManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveJournalVoucher()
        {
            BusinessEntities.JournalVoucher item = new BusinessEntities.JournalVoucher();
            LoadJournalVoucherFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = JournalVoucherManager.Save(item);
                EndEditing();
                LoadJournalVoucher();

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

        private void LoadJournalVoucherFromFormControls(BusinessEntities.JournalVoucher myJournalVoucher)
        {
            myJournalVoucher.mId = int.Parse(Idlabel.Text);
            myJournalVoucher.mDate = dateTimePicker.Value.Date;
            myJournalVoucher.mSupplierId = ControlUtil.UltraComboReturnValue(SupplierutraCombo);

            myJournalVoucher.mType = "Entry";
            myJournalVoucher.mType = BeginningradioButton.Checked ? "Beginning" : myJournalVoucher.mType;
            myJournalVoucher.mType = AdjustingradioButton.Checked ? "Adjusting" : myJournalVoucher.mType;
            myJournalVoucher.mType = ClosingradioButton.Checked ? "Closing" : myJournalVoucher.mType;

            myJournalVoucher.mDetails = DetailstextBox.Text;
            myJournalVoucher.mPost = PostcheckBox.Checked;
            myJournalVoucher.mUserId = SessionUtil.mUser.mId;

            LoadJournalVoucherDetailFromFormControls(myJournalVoucher);
            myJournalVoucher.mDeletedJournalVoucherDetailCollection = deleted_items;
        }

        private void LoadJournalVoucherDetailFromFormControls(BusinessEntities.JournalVoucher myJournalVoucher)
        {
            JournalVoucherDetailCollection items = new JournalVoucherDetailCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
                items.Add((JournalVoucherDetail)row.DataBoundItem);
            myJournalVoucher.mJournalVoucherDetailCollection = items;
        }

        private void LoadFormControlsFromJournalVoucher(BusinessEntities.JournalVoucher myJournalVoucher)
        {
            Idlabel.Text = myJournalVoucher.mId.ToString();
            dateTimePicker.Value = myJournalVoucher.mDate;
            TransactionNotextBox.Text = myJournalVoucher.mTransactionNo;
            SupplierutraCombo.Value = myJournalVoucher.mSupplierId;

            EntryradioButton.Checked = myJournalVoucher.mType == "Entry";
            BeginningradioButton.Checked = myJournalVoucher.mType == "Beginning";
            AdjustingradioButton.Checked = myJournalVoucher.mType == "Adjusting";
            ClosingradioButton.Checked = myJournalVoucher.mType == "Closing";

            PostcheckBox.Checked = myJournalVoucher.mPost;
            DetailstextBox.Text = myJournalVoucher.mDetails;

            LoadFormControlsFromJournalVoucherDetail(myJournalVoucher);
        }

        private void LoadFormControlsFromJournalVoucherDetail(BusinessEntities.JournalVoucher myJournalVoucher)
        {
            JournalVoucherDetailCriteria criteria = new JournalVoucherDetailCriteria();
            criteria.mJournalVoucherId = myJournalVoucher.mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<JournalVoucherDetail>(JournalVoucherDetailManager.GetList(criteria));
            ItemsdataGridView.Refresh();

            ComputeTotalValues();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<JournalVoucherDetail>();
            ItemsdataGridView.Refresh();

            deleted_items = null;

            ComputeTotalValues();
        }

        private void ComputeTotalValues()
        {
            SortableBindingList<JournalVoucherDetail> items = (SortableBindingList<JournalVoucherDetail>)ItemsdataGridView.DataSource;
            Debitlabel.Text = items.Sum(x => x.mDebit).ToString("N");
            Creditlabel.Text = items.Sum(x => x.mCredit).ToString("N");
            Differencelabel.Text = items.Sum(x => x.mDebit - x.mCredit).ToString("N");

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
            SupplierutraCombo.Focus();
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

            decimal diff = 0;
            decimal.TryParse(Differencelabel.Text, out diff);
            if (diff != 0)
            {
                MessageBox.Show("Invalid input, there is " + Differencelabel.Text + " difference.", "Journal Voucher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            BrokenRulesCollection rules = new BrokenRulesCollection();

            //JournalVoucherCriteria criteria = new JournalVoucherCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mName = NametextBox.Text;
            //if (JournalVoucherManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Name already exists."));

            //criteria = new JournalVoucherCriteria();
            //criteria.mId = int.Parse(Idlabel.Text);
            //criteria.mCode = CodetextBox.Text;
            //if (JournalVoucherManager.SelectCountForGetList(criteria) > 0)
            //    rules.Add(new BrokenRule("", "Code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveJournalVoucher();
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
                BusinessEntities.JournalVoucher item = JournalVoucherManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                JournalVoucherManager.Delete(item);

                LoadJournalVoucher();

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


            BusinessEntities.JournalVoucher item = JournalVoucherManager.GetItem(_mId);
            LoadFormControlsFromJournalVoucher(item);
            ControlUtil.ExpandPanel(splitContainer1);
            SupplierutraCombo.Focus();
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
                (int)Modules.JournalVoucher);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadJournalVoucher();
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
            UltraComboUtil.Supplier(SupplierutraCombo);
        }

        private void searchProductControl1__SearchingProduct(object sender, EventArgs e)
        {
            if (searchChartOfAccountUserControl1.mUltraGrid.Visible)
            {
                searchChartOfAccountUserControl1.Dock = DockStyle.Fill;
                searchChartOfAccountUserControl1.BringToFront();
            }
            else
            {
                searchChartOfAccountUserControl1.Dock = DockStyle.Top;
                ItemsdataGridView.BringToFront();
            }
        }
        
        private void searchProductControl1__GetProduct(object sender, EventArgs e)
        {
            searchChartOfAccountUserControl1.Dock = DockStyle.Top;
            ItemsdataGridView.BringToFront();
            
            if (searchChartOfAccountUserControl1.mChartOfAccount != null)
                AddProduct(searchChartOfAccountUserControl1.mChartOfAccount);
        }
        private void AddProduct(ChartOfAccount myChartOfAccount)
        {
            SortableBindingList<JournalVoucherDetail> items = new SortableBindingList<JournalVoucherDetail>();

            if (ItemsdataGridView.DataSource != null)
                items = (SortableBindingList<JournalVoucherDetail>)ItemsdataGridView.DataSource;

            if (items.Where(x => x.mChartOfAccountId == myChartOfAccount.mId).ToList().Count > 0)
            {
                MessageBox.Show("Chart of account " + myChartOfAccount.mName + " is already exists on the list");
                return;
            }

            JournalVoucherDetail item = new JournalVoucherDetail();
            item.mChartOfAccountId = myChartOfAccount.mId;
            item.mChartOfAccountCode = myChartOfAccount.mCode;
            item.mChartOfAccountName = myChartOfAccount.mName;
            items.Add(item);

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = items;
            ItemsdataGridView.Refresh();

            ComputeTotalValues();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadJournalVoucher();
        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            JournalVoucherDetail item = (JournalVoucherDetail)ItemsdataGridView.CurrentRow.DataBoundItem;

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
                    deleted_items = new JournalVoucherDetailCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
        }
        
        private void ItemsdataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void CheckedBybutton_Click(object sender, EventArgs e)
        {
            User.OverrideLogInForm logIn = new User.OverrideLogInForm();
            logIn.mForOverride = true;
            logIn.FormClosing += LogIn_FormClosing;
            logIn.ShowDialog();
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            User.OverrideLogInForm logIn = (User.OverrideLogInForm)sender;
            if (logIn.mUser == null) return;

            if(!SessionUtil.UserAllowDelete(logIn.mUser, (int)Modules.JournalVoucher))
            {
                MessageBox.Show("You are not allowed to override transaction posting.", "Journal Voucher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            PostcheckBox.Checked = true;
        }

        private void ItemsdataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 4)
                ComputeTotalValues();
        }
    }
}
