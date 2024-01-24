using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Disposal : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Asset")]
        public Int32 mFixedAssetId { get; set; }
        public String mFixedAssetName { get; set; }

        [Display(Name = "Asset No")]
        public string mAssetNo { get; set; }

        [Display(Name = "Asset Type")]
        public string mAssetTypeName { get; set; }

        [Display(Name = "Date Disposed")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDateDisposed { get; set; }

        [Display(Name = "Sales Proceeds")]
        public Decimal mSalesProceeds { get; set; }

        [Display(Name = "Gain/Loss")]
        public decimal mGainLosses { get; set; }

        [Display(Name = "Cash Account")]
        [NotEqualTo(Message = "Please select cash account", mValue = "0")]
        public Int32 mCashAccountId { get; set; }
        public String mCashAccountName { get; set; }

        [Display(Name = "Gain (Loss) Account")]
        [NotEqualTo(Message = "Please select gain (loss) account", mValue = "0")]
        public Int32 mGainLossAccountId { get; set; }
        public String mGainLossAccountName { get; set; }
        
        public DateTime mPurchaseDate { get; set; }
        #endregion
    }
}
