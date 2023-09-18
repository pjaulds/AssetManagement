using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class ChartOfAccountDB
    {
        public static ChartOfAccount GetItem(int chartOfAccountId)
        {
            ChartOfAccount chartOfAccount = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spChartOfAccountSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", chartOfAccountId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        chartOfAccount = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return chartOfAccount;
        }

        public static ChartOfAccountCollection GetList(ChartOfAccountCriteria chartOfAccountCriteria)
        {
            ChartOfAccountCollection tempList = new ChartOfAccountCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spChartOfAccountSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", chartOfAccountCriteria.mId);

                if (!string.IsNullOrEmpty(chartOfAccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", chartOfAccountCriteria.mCode);

                if (!string.IsNullOrEmpty(chartOfAccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", chartOfAccountCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new ChartOfAccountCollection();
                        while (myReader.Read())
                        {
                            tempList.Add(FillDataRecord(myReader));
                        }

                        myReader.Close();
                    }

                }
                myCommand.Connection.Close();
            }

            return tempList;
        }

        public static int SelectCountForGetList(ChartOfAccountCriteria chartOfAccountCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spChartOfAccountSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", chartOfAccountCriteria.mId);

                if (!string.IsNullOrEmpty(chartOfAccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", chartOfAccountCriteria.mCode);

                if (!string.IsNullOrEmpty(chartOfAccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", chartOfAccountCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(ChartOfAccount myChartOfAccount)
        {
            if (!myChartOfAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a chartOfAccount in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spChartOfAccountInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myChartOfAccount.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myChartOfAccount.mName);

                Helpers.SetSaveParameters(myCommand, myChartOfAccount);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update chartOfAccount as it has been updated by someone else");
                }

                result = Helpers.GetBusinessBaseId(myCommand);

                myCommand.Connection.Close();

            }
            return result;
        }



        public static bool Delete(int id)
        {
            int result = 0;
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spChartOfAccountDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static ChartOfAccount FillDataRecord(IDataRecord myDataRecord)
        {
            ChartOfAccount chartOfAccount = new ChartOfAccount();

            chartOfAccount.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            chartOfAccount.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            chartOfAccount.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return chartOfAccount;
        }
    }
}