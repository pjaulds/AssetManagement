using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AccountGroupDB
    {
        public static AccountGroup GetItem(int accountGroupId)
        {
            AccountGroup accountGroup = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountGroupSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountGroupId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        accountGroup = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return accountGroup;
        }

        public static AccountGroupCollection GetList(AccountGroupCriteria accountGroupCriteria)
        {
            AccountGroupCollection tempList = new AccountGroupCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountGroupSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountGroupCriteria.mId);

                if (!string.IsNullOrEmpty(accountGroupCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accountGroupCriteria.mCode);

                if (!string.IsNullOrEmpty(accountGroupCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accountGroupCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AccountGroupCollection();
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

        public static int SelectCountForGetList(AccountGroupCriteria accountGroupCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountGroupSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", accountGroupCriteria.mId);

                if (!string.IsNullOrEmpty(accountGroupCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", accountGroupCriteria.mCode);

                if (!string.IsNullOrEmpty(accountGroupCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", accountGroupCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AccountGroup myAccountGroup)
        {
            if (!myAccountGroup.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a accountGroup in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAccountGroupInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAccountGroup.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAccountGroup.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myAccountGroup.mPost);

                Helpers.SetSaveParameters(myCommand, myAccountGroup);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update accountGroup as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAccountGroupDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AccountGroup FillDataRecord(IDataRecord myDataRecord)
        {
            AccountGroup accountGroup = new AccountGroup();

            accountGroup.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            accountGroup.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            accountGroup.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            accountGroup.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            return accountGroup;
        }
    }
}