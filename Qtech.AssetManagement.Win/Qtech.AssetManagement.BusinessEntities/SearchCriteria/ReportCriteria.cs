using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class ReportCriteria
    {
        public int mId { get; set; }
        public DateTime mPurchaseDate { get; set; }
        public decimal mPurchaseCost { get; set; }
        public decimal mResidualValue { get; set; }
        public int mUsefulLifeYears { get; set; }
        public short mYear { get; set; }
        public int mAssetTypeId { get; set; }

        public DateTime mEndDate { get; set; }

        public string mTableName { get; set; }
        
    }
}
