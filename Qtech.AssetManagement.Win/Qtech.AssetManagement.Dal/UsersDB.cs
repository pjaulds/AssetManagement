using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class UsersDB
    {
        public static Users GetItem(int usersId)
        {
            Users users = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUsersSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", usersId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        users = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return users;
        }

        public static UsersCollection GetList(UsersCriteria usersCriteria)
        {
            UsersCollection tempList = new UsersCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUsersSearchList";

                Helpers.CreateParameter(myCommand, DbType.String, "@username", usersCriteria.mUsername);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@personnel_id", usersCriteria.mPersonnelId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@user_id_qasa", usersCriteria.mUserIdQasaDatabase);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new UsersCollection();
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

        public static int SelectCountForGetList(UsersCriteria usersCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUsersSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.String, "@username", usersCriteria.mUsername);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@personnel_id", usersCriteria.mPersonnelId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Users myUsers)
        {
            if (!myUsers.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a users in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUsersInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@username", myUsers.mUsername);

                if (!string.IsNullOrEmpty(myUsers.mPassword))
                    Helpers.CreateParameter(myCommand, DbType.String, "@password", myUsers.mPassword);

                Helpers.CreateParameter(myCommand, DbType.String, "@hash", myUsers.mHash);
                Helpers.CreateParameter(myCommand, DbType.Binary, "@salt", myUsers.mSalt);

                if (myUsers.mPhoto != null)
                    Helpers.CreateParameter(myCommand, DbType.Binary, "@photo", myUsers.mPhoto);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@personnel_id", myUsers.mPersonnelId);

                Helpers.SetSaveParameters(myCommand, myUsers);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update users as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spUsersDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Users FillDataRecord(IDataRecord myDataRecord)
        {
            Users users = new Users();

            users.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));

            if (FieldNames.GetAllNames(myDataRecord).ContainsKey("username"))
                users.mUsername = myDataRecord.GetString(myDataRecord.GetOrdinal("username"));

            if (FieldNames.GetAllNames(myDataRecord).ContainsKey("hash"))
                users.mHash = myDataRecord.GetString(myDataRecord.GetOrdinal("hash"));

            if (FieldNames.GetAllNames(myDataRecord).ContainsKey("salt"))
                users.mSalt = (byte[]) myDataRecord.GetValue(myDataRecord.GetOrdinal("salt"));

            if (FieldNames.GetAllNames(myDataRecord).ContainsKey("photo"))
            {
                if (myDataRecord["photo"] != DBNull.Value)
                    users.mPhoto = (byte[])myDataRecord.GetValue(myDataRecord.GetOrdinal("photo"));
            }

            users.mPersonnelId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("personnel_id"));

            if (FieldNames.GetAllNames(myDataRecord).ContainsKey("allow_no_schedule"))
                users.mAllowNoSchedule = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("allow_no_schedule"));

            if (FieldNames.GetAllNames(myDataRecord).ContainsKey("disable"))
                users.mDisable = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("disable"));

            return users;
        }
    }
}