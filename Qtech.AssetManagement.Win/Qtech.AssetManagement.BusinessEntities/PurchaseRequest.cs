using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class PurchaseRequest : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDate { get; set; }

        [Display(Name = "Transaction No")]
        public Int32 mNumber { get; set; }

        [Display(Name = "Requested By")]
        [NotEqualTo(Message = "Please select requested by", mValue = "0")]
        public Int32 mRequestedById { get; set; }
        public String mRequestedByName { get; set; }

        [Display(Name = "Date Required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDateRequired { get; set; }

        [Display(Name = "Supplier 1")]
        public Int32 mSupplier1Id { get; set; }
        public String mSupplier1Name { get; set; }

        [Display(Name = "Supplier 2")]
        public Int32 mSupplier2Id { get; set; }
        public String mSupplier2Name { get; set; }

        [Display(Name = "Supplier 3")]
        public Int32 mSupplier3Id { get; set; }
        public String mSupplier3Name { get; set; }

        [Display(Name = "Approved By")]
        public Int32 mApprovedById { get; set; }
        public String mApprovedByName { get; set; }

        [Display(Name = "Remarks")]
        public string mRemarks { get; set; }
        public string mTransactionNo { get; set; }

        public PurchaseRequestDetailCollection mPurchaseRequestDetailCollection { get; set; }
        public PurchaseRequestDetailCollection mDeletedPurchaseRequestDetailCollection { get; set; }
        #endregion
    }
}
