using Qtech.AssetManagement.BusinessEntities;
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
        #region Straight Line
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

        #endregion

        #region SYD
        public static DataTable DepreciationScheduleSYDFullMonthMonthly(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleSYDFullMonthMonthly(reportCriteria);
        }

        public static DataTable DepreciationScheduleSYDFullMonthAnnually(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleSYDFullMonthAnnually(reportCriteria);
        }

        public static DataTable DepreciationScheduleSYDActualDaysMonthly(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleSYDActualDaysMonthly(reportCriteria);
        }

        public static DataTable DepreciationScheduleSYDActualDaysAnnually(ReportCriteria reportCriteria)
        {
            return ReportDB.DepreciationScheduleSYDActualDaysAnnually(reportCriteria);
        }

        public static DataTable BrowseReceivingFromOtherDB(ReportCriteria reportCriteria)
        {
            return ReportDB.BrowseReceivingFromOtherDB(reportCriteria);
        }

        public static DataTable BrowseReceivingDetailFromOtherDB(ReportCriteria reportCriteria)
        {
            return ReportDB.BrowseReceivingDetailFromOtherDB(reportCriteria);
        }

        public static DataRow Depreciation(ReportCriteria reportCriteria)
        {
            FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == reportCriteria.mAssetTypeId).First();
            
            DataTable dt = new DataTable();
            if (item.mDepreciationMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if (item.mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
                    dt = DepreciationScheduleStraightLineFullMonthMonthly(reportCriteria);
                else if (item.mAveragingMethodId == (int)AveragingMethodEnum.ActualDays)
                    dt = DepreciationScheduleStraightLineActualDaysMonthly(reportCriteria);

            }

            if (item.mDepreciationMethodId == (int)DepreciationMethodEnum.SYD)
            {
                if (item.mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
                    dt = DepreciationScheduleSYDFullMonthMonthly(reportCriteria);
                else if (item.mAveragingMethodId == (int)AveragingMethodEnum.ActualDays)
                    dt = DepreciationScheduleSYDActualDaysMonthly(reportCriteria);
            }

            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }
        #endregion
    }
}
