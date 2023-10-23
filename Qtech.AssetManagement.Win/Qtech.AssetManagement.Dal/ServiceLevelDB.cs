using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class ServiceLevelDB
    {
        public static ServiceLevel GetItem(int serviceLevelId)
        {
            ServiceLevel serviceLevel = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spServiceLevelSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", serviceLevelId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        serviceLevel = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return serviceLevel;
        }

        public static ServiceLevelCollection GetList(ServiceLevelCriteria serviceLevelCriteria)
        {
            ServiceLevelCollection tempList = new ServiceLevelCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spServiceLevelSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", serviceLevelCriteria.mId);

                if (!string.IsNullOrEmpty(serviceLevelCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", serviceLevelCriteria.mCode);

                if (!string.IsNullOrEmpty(serviceLevelCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", serviceLevelCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new ServiceLevelCollection();
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

        public static int SelectCountForGetList(ServiceLevelCriteria serviceLevelCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spServiceLevelSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", serviceLevelCriteria.mId);

                if (!string.IsNullOrEmpty(serviceLevelCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", serviceLevelCriteria.mCode);

                if (!string.IsNullOrEmpty(serviceLevelCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", serviceLevelCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(ServiceLevel myServiceLevel)
        {
            if (!myServiceLevel.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a serviceLevel in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spServiceLevelInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myServiceLevel.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myServiceLevel.mName);

                Helpers.SetSaveParameters(myCommand, myServiceLevel);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update serviceLevel as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spServiceLevelDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static ServiceLevel FillDataRecord(IDataRecord myDataRecord)
        {
            ServiceLevel serviceLevel = new ServiceLevel();

            serviceLevel.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            serviceLevel.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            serviceLevel.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return serviceLevel;
        }
    }
}