using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class ReportCriteria
    {
        public DateTime mPurchaseDate { get; set; }
        public decimal mPurchaseCost { get; set; }
        public decimal mResidualValue { get; set; }
        public int mUsefulLifeYears { get; set; }
        public short mYear { get; set; }
    }
}
