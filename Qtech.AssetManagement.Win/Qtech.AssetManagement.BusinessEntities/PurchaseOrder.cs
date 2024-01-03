using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class PurchaseOrder : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDate { get; set; }

        [Display(Name = "Request Date of Delivery")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDateOfDelivery { get; set; }

        [Display(Name = "PR No.")]
        public string mPurchaseRequestNo { get; set; }

        [Display(Name = "PO No.")]
        public Int32 mNumber { get; set; }
        public string mTransactionNo { get; set; }

        [NotEqualTo(Message = "Please select quotation", mValue = "0")]
        public Int32 mQuotationId { get; set; }
        public int mSupplierId { get; set; }

        [Display(Name = "RFQ No.")]
        public string mQuotationNo { get; set; }
        public String mQuotationName { get; set; }

        [Display(Name = "Terms.")]
        public String mTerms { get; set; }

        [Display(Name = "Prepared By")]
        [NotEqualTo(Message = "Please select prepared by", mValue = "0")]
        public Int32 mPreparedById { get; set; }
        public String mPreparedByName { get; set; }

        [Display(Name = "Noted By")]
        public Int32 mNotedById { get; set; }
        public String mNotedByName { get; set; }

        [Display(Name = "Approved By")]
        public Int32 mApprovedById { get; set; }
        public String mApprovedByName { get; set; }

        [Display(Name = "Revised")]
        public Boolean mRevised { get; set; }

        [Display(Name = "Cancelled")]
        public Boolean mCancelled { get; set; }

        public int mCurrencyId { get; set; }
        public string mCurrencyName { get; set; }

        public PurchaseOrderDetailCollection mPurchaseOrderDetailCollection { get; set; }
        public PurchaseOrderDetailCollection mDeletedPurchaseOrderDetailCollection { get; set; }
        #endregion
    }
}