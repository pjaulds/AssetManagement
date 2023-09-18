using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class UserAccessDB
    {
        public static UserAccess GetItem(int useraccessId)
        {
            UserAccess useraccess = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUserAccessSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", useraccessId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        useraccess = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return useraccess;
        }

        public static UserAccessCollection GetList(UserAccessCriteria useraccessCriteria)
        {
            UserAccessCollection tempList = new UserAccessCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUserAccessSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@user_id", useraccessCriteria.mUserId);
                Helpers.CreateParameter(myCommand, DbType.Int16, "@module_id", useraccessCriteria.mModuleId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new UserAccessCollection();
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

        public static int SelectCountForGetList(UserAccessCriteria useraccessCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUserAccessSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@user_id", useraccessCriteria.mUserId);
                Helpers.CreateParameter(myCommand, DbType.Int16, "@module_id", useraccessCriteria.mModuleId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(UserAccess myUserAccess)
        {
            if (!myUserAccess.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a useraccess in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUserAccessInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@user_id", myUserAccess.mUserId);
                Helpers.CreateParameter(myCommand, DbType.Int16, "@module_id", myUserAccess.mModuleId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@select", myUserAccess.mSelect);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@insert", myUserAccess.mInsert);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@update", myUserAccess.mUpdate);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@delete", myUserAccess.mDelete);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@print", myUserAccess.mPrint);

                Helpers.SetSaveParameters(myCommand, myUserAccess);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update useraccess as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spUserAccessDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static UserAccess FillDataRecord(IDataRecord myDataRecord)
        {
            UserAccess useraccess = new UserAccess();

            useraccess.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            useraccess.mUserId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("user_id"));
            useraccess.mModuleName = myDataRecord.GetString(myDataRecord.GetOrdinal("module_name"));
            useraccess.mModuleId = myDataRecord.GetInt16(myDataRecord.GetOrdinal("module_id"));
            useraccess.mSelect = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("select"));
            useraccess.mInsert = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("insert"));
            useraccess.mUpdate = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("update"));
            useraccess.mDelete = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("delete"));
            useraccess.mPrint = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("print"));

            useraccess.mModuleGroup = myDataRecord.GetString(myDataRecord.GetOrdinal("module_group"));
            return useraccess;
        }
    }
}