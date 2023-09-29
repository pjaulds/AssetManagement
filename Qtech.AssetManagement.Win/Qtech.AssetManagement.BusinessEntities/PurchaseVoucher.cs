using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class PurchaseVoucher : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDate { get; set; }

        [Display(Name = "Transaction No.")]
        public Int32 mNumber { get; set; }
        public string mTransactionNo { get; set; }

        [Display(Name = "GRN No.")]
        public Int32 mReceivingId { get; set; }
        public String mReceivingNo { get; set; }

        [Display(Name = "Mode of Payment")]
        [NotEqualTo(Message = "Please select mode of payment", mValue = "0")]
        public Int32 mPaymentModeId { get; set; }
        public String mPaymentModeName { get; set; }

        [Display(Name = "Prepared By")]
        [NotEqualTo(Message = "Please select prepared by", mValue = "0")]
        public Int32 mPreparedById { get; set; }
        public String mPreparedByName { get; set; }

        [Display(Name = "Checked By")]
        [NotEqualTo(Message = "Please select checked by", mValue = "0")]
        public Int32 mCheckedById { get; set; }
        public String mCheckedByName { get; set; }

        [Display(Name = "Approved By")]
        public Int32 mApprovedById { get; set; }
        public String mApprovedByName { get; set; }

        [Display(Name = "PO No.")]
        public string mPurchaseOrderNo { get; set; }

        [Display(Name = "Invoice No.")]
        public string mInvoiceNo { get; set; }

        [Display(Name = "Supplier")]
        public string mSupplierName { get; set; }

        [Display(Name = "Amount")]
        public decimal mAmount { get; set; }
        #endregion
    }
}