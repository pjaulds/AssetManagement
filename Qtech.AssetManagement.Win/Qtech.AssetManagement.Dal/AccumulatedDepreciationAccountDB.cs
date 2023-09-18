using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AccumulatedDepreciationAccountDB
    {
        public static AccumulatedDepreciationAccount GetItem(int accumulateddepreciationaccountId)
        {
            AccumulatedDepreciationAccount accumulateddepreciationaccount = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccumulatedDepreciationAccountSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accumulateddepreciationaccountId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        accumulateddepreciationaccount = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return accumulateddepreciationaccount;
        }

        public static AccumulatedDepreciationAccountCollection GetList(AccumulatedDepreciationAccountCriteria accumulateddepreciationaccountCriteria)
        {
            AccumulatedDepreciationAccountCollection tempList = new AccumulatedDepreciationAccountCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccumulatedDepreciationAccountSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accumulateddepreciationaccountCriteria.mId);

                if (!string.IsNullOrEmpty(accumulateddepreciationaccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accumulateddepreciationaccountCriteria.mCode);

                if (!string.IsNullOrEmpty(accumulateddepreciationaccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accumulateddepreciationaccountCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AccumulatedDepreciationAccountCollection();
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

        public static int SelectCountForGetList(AccumulatedDepreciationAccountCriteria accumulateddepreciationaccountCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccumulatedDepreciationAccountSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accumulateddepreciationaccountCriteria.mId);

                if (!string.IsNullOrEmpty(accumulateddepreciationaccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accumulateddepreciationaccountCriteria.mCode);

                if (!string.IsNullOrEmpty(accumulateddepreciationaccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accumulateddepreciationaccountCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AccumulatedDepreciationAccount myAccumulatedDepreciationAccount)
        {
            if (!myAccumulatedDepreciationAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a accumulateddepreciationaccount in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccumulatedDepreciationAccountInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAccumulatedDepreciationAccount.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAccumulatedDepreciationAccount.mName);

                Helpers.SetSaveParameters(myCommand, myAccumulatedDepreciationAccount);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update accumulateddepreciationaccount as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAccumulatedDepreciationAccountDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AccumulatedDepreciationAccount FillDataRecord(IDataRecord myDataRecord)
        {
            AccumulatedDepreciationAccount accumulateddepreciationaccount = new AccumulatedDepreciationAccount();

            accumulateddepreciationaccount.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            accumulateddepreciationaccount.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            accumulateddepreciationaccount.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return accumulateddepreciationaccount;
        }
    }
}
