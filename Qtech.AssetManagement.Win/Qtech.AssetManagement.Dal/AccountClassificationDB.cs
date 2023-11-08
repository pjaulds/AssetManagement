using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AccountClassificationDB
    {
        public static AccountClassification GetItem(int accountClassificationId)
        {
            AccountClassification accountClassification = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountClassificationSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountClassificationId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        accountClassification = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return accountClassification;
        }

        public static AccountClassificationCollection GetList(AccountClassificationCriteria accountClassificationCriteria)
        {
            AccountClassificationCollection tempList = new AccountClassificationCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountClassificationSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountClassificationCriteria.mId);

                if (!string.IsNullOrEmpty(accountClassificationCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accountClassificationCriteria.mCode);

                if (!string.IsNullOrEmpty(accountClassificationCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accountClassificationCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AccountClassificationCollection();
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

        public static int SelectCountForGetList(AccountClassificationCriteria accountClassificationCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountClassificationSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountClassificationCriteria.mId);

                if (!string.IsNullOrEmpty(accountClassificationCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accountClassificationCriteria.mCode);

                if (!string.IsNullOrEmpty(accountClassificationCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accountClassificationCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AccountClassification myAccountClassification)
        {
            if (!myAccountClassification.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a accountClassification in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountClassificationInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAccountClassification.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAccountClassification.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myAccountClassification.mPost);

                Helpers.SetSaveParameters(myCommand, myAccountClassification);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update accountClassification as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAccountClassificationDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AccountClassification FillDataRecord(IDataRecord myDataRecord)
        {
            AccountClassification accountClassification = new AccountClassification();

            accountClassification.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            accountClassification.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            accountClassification.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            accountClassification.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            return accountClassification;
        }
    }
}