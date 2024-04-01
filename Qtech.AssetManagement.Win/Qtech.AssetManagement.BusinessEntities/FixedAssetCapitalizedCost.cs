using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class FixedAssetCapitalizedCost : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Fixed Asset")]
        public Int32 mFixedAssetId { get; set; }
        public String mFixedAssetName { get; set; }
        public DateTime mDate { get; set; }
        public Int32 mNumber { get; set; }

        [Display(Name = "Capitalized Cost")]
        public Int32 mCapitalizedCostId { get; set; }
        public String mCapitalizedCostName { get; set; }
        public String mDescription { get; set; }
        public Decimal mUsefulLife { get; set; }

        [Display(Name = "Amount")]
        public Decimal mAmount { get; set; }

        [Display(Name = "Journalized")]
        public Boolean mIsJournalized { get; set; }
        public Int32 mAssetAccountId { get; set; }
        public String mAssetAccountName { get; set; }
        public Int32 mCashPayableAccountId { get; set; }
        public String mCashPayableAccountName { get; set; }
        public string mTransactionNo { get; set; }
        #endregion
    }
}
