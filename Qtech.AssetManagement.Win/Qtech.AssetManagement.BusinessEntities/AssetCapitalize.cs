using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class AssetCapitalize : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }
        public Int32 mAssetId { get; set; }
        public String mAssetName { get; set; }
        public DateTime mDate { get; set; }
        public Int32 mNumber { get; set; }
        public Int32 mCapitalizedCostId { get; set; }
        public String mCapitalizedCostName { get; set; }
        public String mDescription { get; set; }
        public Decimal mAmount { get; set; }
        public Decimal mUsefulLife { get; set; }
        public Boolean mJournalized { get; set; }

        #endregion
    }
}