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
                Helpers.CreateParameter(myCommand, DbType.Int32, "@year", reportCriteria.mYear);

                myCommand.Connection.Open();
                dt.Load(myCommand.ExecuteReader());
                myCommand.Connection.Close();

            }
            return dt;
        }
    }
}
