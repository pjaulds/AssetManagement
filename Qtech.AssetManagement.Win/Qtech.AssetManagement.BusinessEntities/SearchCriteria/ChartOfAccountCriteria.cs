using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class ChartOfAccountCriteria : ChartOfAccount
    {
        public bool mForAccumulatedDepreciationAccount { get; set; }
        public bool mForDepreciationExpenseAccount { get; set; }
        public bool mForFixedAssetAccount { get; set; }
    }
}
