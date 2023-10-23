using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Dal;
using Qtech.AssetManagement.Validation;
using System.ComponentModel;

using Qtech.AssetManagement.Audit;

namespace Qtech.AssetManagement.Bll
{
    public static class HelpersManager
    {
        public static string GetNewTrasactionNo(ReportCriteria reportCriteria)
        {
            return HelpersDB.GetNewTrasactionNo(reportCriteria);
        }
    }
}