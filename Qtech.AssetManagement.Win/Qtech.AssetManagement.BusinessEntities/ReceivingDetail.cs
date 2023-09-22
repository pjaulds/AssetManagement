using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class ReceivingDetail : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [NotEqualTo(Message = "Please select receiving", mValue = "0")]
        public Int32 mReceivingId { get; set; }

        [NotEqualTo(Message = "Please select purchase order item", mValue = "0")]
        public Int32 mPurchaseOrderDetailId { get; set; }

        [Display(Name = "Quantity")]
        public Decimal mQuantity { get; set; }

        [Display(Name = "Product")]
        public string mProductName { get; set; }

        [Display(Name = "Cost")]
        public decimal mCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal mTotalCost { get; set; }
        #endregion
    }
}
