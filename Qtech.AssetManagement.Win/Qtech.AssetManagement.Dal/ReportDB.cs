using Qtech.AssetManagement.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.Dal
{
    public class ReportDB
    {

        #region Straight Line
        public static DataTable DepreciationStraightLineFullMonth(ReportCriteria reportCriteria)
        {
            DataTable dt= new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationStraightLineFullMonth";

                Helpers.CreateParameter(myCommand, DbType.Date, "@purchase_date", reportCriteria.mPurchaseDate);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@purchase_cost", reportCriteria.mPurchaseCost);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@residual_value", reportCriteria.mResidualValue);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@useful_life_years", reportCriteria.mUsefulLifeYears);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        public static DataTable DepreciationScheduleStraightLineFullMonthMonthly(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleStraightLineFullMonthMonthly";
                
                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        public static DataTable DepreciationScheduleStraightLineFullMonthAnnually(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleStraightLineFullMonthAnnually";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        public static DataTable DepreciationScheduleStraightLineActualDaysMonthly(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleStraightLineActualDaysMonthly";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        public static DataTable DepreciationScheduleStraightLineActualDaysAnnually(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleStraightLineActualDaysAnnually";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        #endregion

        #region SYD
        public static DataTable DepreciationScheduleSYDFullMonthMonthly(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleSYDFullMonthMonthly";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        public static DataTable DepreciationScheduleSYDFullMonthAnnually(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleSYDFullMonthAnnually";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        public static DataTable DepreciationScheduleSYDActualDaysMonthly(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleSYDActualDaysMonthly";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }

        public static DataTable DepreciationScheduleSYDActualDaysAnnually(ReportCriteria reportCriteria)
        {
            DataTable dt = new DataTable();
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReportDepreciationScheduleSYDActualDaysAnnually";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", reportCriteria.mId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", reportCriteria.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }
        #endregion
    }
}
