using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AccountTypeDB
    {
        public static AccountType GetItem(int accountTypeId)
        {
            AccountType accountType = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountTypeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountTypeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        accountType = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return accountType;
        }

        public static AccountTypeCollection GetList(AccountTypeCriteria accountTypeCriteria)
        {
            AccountTypeCollection tempList = new AccountTypeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountTypeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountTypeCriteria.mId);

                if (!string.IsNullOrEmpty(accountTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accountTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(accountTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accountTypeCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AccountTypeCollection();
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

        public static int SelectCountForGetList(AccountTypeCriteria accountTypeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountTypeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountTypeCriteria.mId);

                if (!string.IsNullOrEmpty(accountTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accountTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(accountTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accountTypeCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AccountType myAccountType)
        {
            if (!myAccountType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a accountType in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountTypeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAccountType.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAccountType.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myAccountType.mPost);

                Helpers.SetSaveParameters(myCommand, myAccountType);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update accountType as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAccountTypeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AccountType FillDataRecord(IDataRecord myDataRecord)
        {
            AccountType accountType = new AccountType();

            accountType.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            accountType.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            accountType.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            accountType.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            return accountType;
        }
    }
}