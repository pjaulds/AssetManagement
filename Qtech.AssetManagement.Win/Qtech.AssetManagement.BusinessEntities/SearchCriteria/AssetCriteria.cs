﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class AssetCriteria : Asset
    {
        public DateTime mStartDate { get; set; }
        public DateTime mEndDate { get; set; }
    }
}
