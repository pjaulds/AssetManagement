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

namespace Qtech.AssetManagement.Purchasing.PurchaseVoucher
{
    public partial class Default : Form, ICRUD, IComboSelection
    {
        public Default()
        {
            InitializeComponent();
        }

        public bool mForFixedAsset { get; set; }
        public BusinessEntities.PurchaseVoucher mPurchaseVoucher { get; set; }

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
                    return ((BusinessEntities.PurchaseVoucher)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadPurchaseVoucher()
        {
            PurchaseVoucherCriteria criteria = new PurchaseVoucherCriteria();
            if (StartdateTimePicker.Checked && EnddateTimePicker.Checked)
            {
                criteria.mStartDate = StartdateTimePicker.Value.Date;
                criteria.mEndDate = EnddateTimePicker.Value.Date;
            }

            if(mForFixedAsset)
            {
                criteria = new PurchaseVoucherCriteria();
                criteria.mForFixedAsset = true;
            }

            ultraGrid1.SetDataBinding(PurchaseVoucherManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private int SavePurchaseVoucher()
        {
            BusinessEntities.PurchaseVoucher item = new BusinessEntities.PurchaseVoucher();
            LoadPurchaseVoucherFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = PurchaseVoucherManager.Save(item);
                EndEditing();
                LoadPurchaseVoucher();

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

        private void LoadPurchaseVoucherFromFormControls(BusinessEntities.PurchaseVoucher myPurchaseVoucher)
        {
            myPurchaseVoucher.mId = int.Parse(Idlabel.Text);
            myPurchaseVoucher.mReceivingId = int.Parse(ReceivingIdIdlabel.Text);
            myPurchaseVoucher.mPaymentModeId = ControlUtil.UltraComboReturnValue(PaymentModeultraCombo);
            myPurchaseVoucher.mPreparedById = ControlUtil.UltraComboReturnValue(PreparedByutraCombo);
            myPurchaseVoucher.mPreparedByName = PreparedByutraCombo.Text;
            myPurchaseVoucher.mCheckedById = ControlUtil.UltraComboReturnValue(CheckedByultraCombo);
            myPurchaseVoucher.mCheckedByName = CheckedByultraCombo.Text;
            myPurchaseVoucher.mApprovedById = int.Parse(ApprovedBylabel.Text);
            myPurchaseVoucher.mApprovedByName = ApprovedBytextBox.Text;
            myPurchaseVoucher.mUserId = SessionUtil.mUser.mId;
            
        }

        private void LoadFormControlsFromPurchaseVoucher(BusinessEntities.PurchaseVoucher myPurchaseVoucher)
        {
            Idlabel.Text = myPurchaseVoucher.mId.ToString();
            TransactiontextBox.Text = myPurchaseVoucher.mTransactionNo;
            dateTimePicker.Value = myPurchaseVoucher.mDate;
            ReceivingIdIdlabel.Text = myPurchaseVoucher.mReceivingId.ToString();
            PaymentModeultraCombo.Value = myPurchaseVoucher.mPaymentModeId;
            PreparedByutraCombo.Value = myPurchaseVoucher.mPreparedById;
            CheckedByultraCombo.Value = myPurchaseVoucher.mCheckedById;
            ApprovedBylabel.Text = myPurchaseVoucher.mApprovedById.ToString();
            ApprovedBytextBox.Text = myPurchaseVoucher.mApprovedByName;

            LoadFormControlsFromReceiving(ReceivingManager.GetItem(myPurchaseVoucher.mReceivingId));
        }

        public void LoadFormControlsFromReceiving(BusinessEntities.Receiving myReceiving)
        {
            ReceivingIdIdlabel.Text = myReceiving.mId.ToString();
            DebittextBox.Text = myReceiving.mAmount.ToString("N");
            CredittextBox.Text = myReceiving.mAmount.ToString("N");
            DifferencetextBox.Text = "-";

            Supplier supplier = SupplierManager.GetItem(myReceiving.mSupplierId);
            SupplierNametextBox.Text = supplier.mName;

            PurchaseOrderNotextBox.Text = myReceiving.mPurchaseOrderNo;
            ReceivingNotextBox.Text = myReceiving.mTransactionNo;
            InvoiceNotextBox.Text = myReceiving.mInvoiceNo;
            PaymentTermstextBox.Text = PurchaseOrderManager.GetItem(myReceiving.mPurchaseOrderId).mTerms;
        }
        

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
            ApprovedBylabel.Text = "0";
            ReceivingIdIdlabel.Text = "0";
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

            Receiving.Default receivingForm = new Receiving.Default();
            receivingForm.mForPurchaseVoucher = true;
            receivingForm.FormClosing += ReceivingForm_FormClosing;
            receivingForm.ShowDialog();
        }

        private void ReceivingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Receiving.Default receivingForm = (Receiving.Default)sender;
            if (receivingForm.mReceiving == null) return;

            if(receivingForm.mReceiving.mApprovedById==0)
            {
                MessageBox.Show("Selected record is not yet approved.", "Receiving", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BusinessEntities.Receiving receiving = receivingForm.mReceiving;
            LoadFormControlsFromReceiving(receiving);            
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

            return SavePurchaseVoucher();
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
                BusinessEntities.PurchaseVoucher item = PurchaseVoucherManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                PurchaseVoucherManager.Delete(item);

                LoadPurchaseVoucher();

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

            BusinessEntities.PurchaseVoucher item = PurchaseVoucherManager.GetItem(_mId);


            if (mForFixedAsset)
            {
                mPurchaseVoucher = item;
                Close();
                return;
            }

            EndEditing();
            LoadFormControlsFromPurchaseVoucher(item);
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
                (int)Modules.PurchaseVoucher);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadPurchaseVoucher();
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
            UltraComboUtil.PaymentMode(PaymentModeultraCombo);
        }

        
        private void ApprovedBybutton_Click(object sender, EventArgs e)
        {
            ApprovedBylabel.Text = SessionUtil.mUser.mId.ToString();
            ApprovedBytextBox.Text = PersonnelManager.GetItem(SessionUtil.mUser.mPersonnelId).mName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPurchaseVoucher();
        }        
    }
}
