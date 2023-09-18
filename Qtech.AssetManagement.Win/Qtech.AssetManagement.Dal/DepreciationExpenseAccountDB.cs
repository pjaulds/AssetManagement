using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class DepreciationExpenseAccountDB
    {
        public static DepreciationExpenseAccount GetItem(int depreciationexpenseaccountId)
        {
            DepreciationExpenseAccount depreciationexpenseaccount = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationExpenseAccountSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", depreciationexpenseaccountId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        depreciationexpenseaccount = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return depreciationexpenseaccount;
        }

        public static DepreciationExpenseAccountCollection GetList(DepreciationExpenseAccountCriteria depreciationexpenseaccountCriteria)
        {
            DepreciationExpenseAccountCollection tempList = new DepreciationExpenseAccountCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationExpenseAccountSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", depreciationexpenseaccountCriteria.mId);

                if (!string.IsNullOrEmpty(depreciationexpenseaccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", depreciationexpenseaccountCriteria.mCode);

                if (!string.IsNullOrEmpty(depreciationexpenseaccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", depreciationexpenseaccountCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new DepreciationExpenseAccountCollection();
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

        public static int SelectCountForGetList(DepreciationExpenseAccountCriteria depreciationexpenseaccountCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationExpenseAccountSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", depreciationexpenseaccountCriteria.mId);

                if (!string.IsNullOrEmpty(depreciationexpenseaccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", depreciationexpenseaccountCriteria.mCode);

                if (!string.IsNullOrEmpty(depreciationexpenseaccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", depreciationexpenseaccountCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(DepreciationExpenseAccount myDepreciationExpenseAccount)
        {
            if (!myDepreciationExpenseAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a depreciationexpenseaccount in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationExpenseAccountInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myDepreciationExpenseAccount.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myDepreciationExpenseAccount.mName);

                Helpers.SetSaveParameters(myCommand, myDepreciationExpenseAccount);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update depreciationexpenseaccount as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spDepreciationExpenseAccountDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static DepreciationExpenseAccount FillDataRecord(IDataRecord myDataRecord)
        {
            DepreciationExpenseAccount depreciationexpenseaccount = new DepreciationExpenseAccount();

            depreciationexpenseaccount.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            depreciationexpenseaccount.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            depreciationexpenseaccount.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return depreciationexpenseaccount;
        }
    }
}