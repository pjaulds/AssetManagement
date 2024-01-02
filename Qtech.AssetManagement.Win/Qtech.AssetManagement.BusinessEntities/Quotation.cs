using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Quotation : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDate { get; set; }

        [Display(Name = "Transaction No")]
        public Int32 mNumber { get; set; }

        [Display(Name = "P.R. No.")]
        [NotEqualTo(Message = "Please select purchase request", mValue = "0")]
        public Int32 mPurchaseRequestId { get; set; }
        public String mPurchaseRequestNo { get; set; }

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

        [NotEqualTo(Message = "Please select supplier", mValue = "0")]
        public Byte mSupplierNo { get; set; }

        [Display(Name = "Supplier 1")]
        public String mSupplier1Name { get; set; }

        [Display(Name = "Supplier 2")]
        public String mSupplier2Name { get; set; }

        [Display(Name = "Supplier 3")]
        public String mSupplier3Name { get; set; }

        public string mTransactionNo { get; set; }

        public QuotationDetailCollection mQuotationDetailCollection { get; set; }
        public QuotationDetailCollection mDeletedQuotationDetailCollection { get; set; }
        #endregion
    }
}
