using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class PurchaseRequestDetail : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Purhcase Request No")]
        [NotEqualTo(Message = "Please select purchase request", mValue = "0")]
        public Int32 mPurchaseRequestId { get; set; }
        public String mPurchaseRequestName { get; set; }

        [Display(Name = "Product")]
        [NotEqualTo(Message = "Please select product", mValue = "0")]
        public Int32 mProductId { get; set; }
        public String mProductName { get; set; }

        [Display(Name = "Qty")]
        [NotEqualTo(Message = "Please enter qty", mValue = "0")]
        public Decimal mQuantity { get; set; }

        [Display(Name = "Cost")]
        public Decimal mCost { get; set; }

        [Display(Name = "Unit")]
        public string mUnitName { get; set; }

        [Display(Name = "Remarks")]
        public string mRemarks { get; set; }
        #endregion
    }
}
