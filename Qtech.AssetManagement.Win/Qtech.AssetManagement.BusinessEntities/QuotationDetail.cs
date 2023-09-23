using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class QuotationDetail : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Quotation No")]
        [NotEqualTo(Message = "Please select quotation", mValue = "0")]
        public Int32 mQuotationId { get; set; }
        public String mQuotationName { get; set; }

        [NotEqualTo(Message = "Please select item", mValue = "0")]
        public int mPurchaseRequestDetailId { get; set; }

        [Display(Name = "Product")]        
        public String mProductName { get; set; }

        [Display(Name = "Qty")]
        public Decimal mQuantity { get; set; }

        [Display(Name = "Cost 1")]
        public Decimal mCost1 { get; set; }

        [Display(Name = "Cost 2")]
        public Decimal mCost2 { get; set; }

        [Display(Name = "Cost 3")]
        public Decimal mCost3 { get; set; }

        /// <summary>
        /// winning cost
        /// </summary>
        [Display(Name = "Cost")]
        public decimal mCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal mTotalCost { get; set; }
        #endregion
    }
}