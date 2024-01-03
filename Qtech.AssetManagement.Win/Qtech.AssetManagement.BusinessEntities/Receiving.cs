using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Receiving : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDate { get; set; }

        [Display(Name = "GRN No.")]
        public Int32 mNumber { get; set; }
        public string mTransactionNo { get; set; }

        [NotEqualTo(Message = "Please select purchase order", mValue = "0")]
        public Int32 mPurchaseOrderId { get; set; }

        [Display(Name = "PO No.")]
        public String mPurchaseOrderNo { get; set; }

        [Display(Name = "Prepared By")]
        [NotEqualTo(Message = "Please select prepared by", mValue = "0")]
        public Int32 mPreparedById { get; set; }
        public String mPreparedByName { get; set; }

        [Display(Name = "Checked By")]
        public Int32 mCheckedById { get; set; }
        public String mCheckedByName { get; set; }

        [Display(Name = "Approved By")]
        public Int32 mApprovedById { get; set; }
        public String mApprovedByName { get; set; }

        [Display(Name = "Remarks.")]
        public String mRemarks { get; set; }

        [Display(Name = "PR No.")]
        public String mPurchaseRequestNo { get; set; }

        [Display(Name = "RFQ No.")]
        public String mQuotationtNo { get; set; }

        [Display(Name = "Invoice No.")]
        public String mInvoiceNo { get; set; }

        [Display(Name = "DR No.")]
        public String mDrNo { get; set; }

        [Display(Name = "Amount")]
        public Decimal mAmount { get; set; }

        [Display(Name = "Supplier")]
        public int mSupplierId { get; set; }
        public string mSupplierName { get; set; }

        public ReceivingDetailCollection mReceivingDetailCollection { get; set; }
        public ReceivingDetailCollection mDeletedReceivingDetailCollection { get; set; }
        #endregion
    }
}