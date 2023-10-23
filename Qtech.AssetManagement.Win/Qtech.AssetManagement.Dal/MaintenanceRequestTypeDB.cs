using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class MaintenanceRequestTypeDB
    {
        public static MaintenanceRequestType GetItem(int maintenanceRequestTypeId)
        {
            MaintenanceRequestType maintenanceRequestType = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestTypeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceRequestTypeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        maintenanceRequestType = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return maintenanceRequestType;
        }

        public static MaintenanceRequestTypeCollection GetList(MaintenanceRequestTypeCriteria maintenanceRequestTypeCriteria)
        {
            MaintenanceRequestTypeCollection tempList = new MaintenanceRequestTypeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestTypeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceRequestTypeCriteria.mId);

                if (!string.IsNullOrEmpty(maintenanceRequestTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", maintenanceRequestTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(maintenanceRequestTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", maintenanceRequestTypeCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new MaintenanceRequestTypeCollection();
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

        public static int SelectCountForGetList(MaintenanceRequestTypeCriteria maintenanceRequestTypeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestTypeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceRequestTypeCriteria.mId);

                if (!string.IsNullOrEmpty(maintenanceRequestTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", maintenanceRequestTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(maintenanceRequestTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", maintenanceRequestTypeCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(MaintenanceRequestType myMaintenanceRequestType)
        {
            if (!myMaintenanceRequestType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a maintenanceRequestType in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestTypeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myMaintenanceRequestType.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myMaintenanceRequestType.mName);

                Helpers.SetSaveParameters(myCommand, myMaintenanceRequestType);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update maintenanceRequestType as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spMaintenanceRequestTypeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static MaintenanceRequestType FillDataRecord(IDataRecord myDataRecord)
        {
            MaintenanceRequestType maintenanceRequestType = new MaintenanceRequestType();

            maintenanceRequestType.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            maintenanceRequestType.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            maintenanceRequestType.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return maintenanceRequestType;
        }
    }
}