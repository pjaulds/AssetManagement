using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class HelpersDB
    {
        public static string GetNewTrasactionNo(ReportCriteria reportCriteria)
        {
            string newNo = string.Empty;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spGetNewTrasactionNo";

                Helpers.CreateParameter(myCommand, DbType.String, "@table", reportCriteria.mTableName);


                myCommand.Connection.Open();
                newNo = myCommand.ExecuteScalar().ToString();
                myCommand.Connection.Close();
            }

            return newNo;
        }
    }
}