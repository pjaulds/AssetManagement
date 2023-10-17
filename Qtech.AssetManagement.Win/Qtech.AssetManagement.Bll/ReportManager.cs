﻿using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.Bll
{
    public class ReportManager
    {
        public static DataTable DepreciationStraightLineFullMonth(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationStraightLineFullMonth(reportCriteria);
        }

        public static DataTable DepreciationScheduleStraightLineFullMonthMonthly(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleStraightLineFullMonthMonthly(reportCriteria);
        }

        public static DataTable DepreciationScheduleStraightLineFullMonthAnnually(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleStraightLineFullMonthAnnually(reportCriteria);
        }

        public static DataTable DepreciationScheduleStraightLineActualDaysMonthly(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleStraightLineActualDaysMonthly(reportCriteria);
        }

        public static DataTable DepreciationScheduleStraightLineActualDaysAnnually(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleStraightLineActualDaysAnnually(reportCriteria);
        }
    }
}
