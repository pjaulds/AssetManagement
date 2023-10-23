using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class MaintenanceJobTypeDB
    {
        public static MaintenanceJobType GetItem(int maintenanceJobTypeId)
        {
            MaintenanceJobType maintenanceJobType = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceJobTypeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        maintenanceJobType = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return maintenanceJobType;
        }

        public static MaintenanceJobTypeCollection GetList(MaintenanceJobTypeCriteria maintenanceJobTypeCriteria)
        {
            MaintenanceJobTypeCollection tempList = new MaintenanceJobTypeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceJobTypeCriteria.mId);

                if (!string.IsNullOrEmpty(maintenanceJobTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", maintenanceJobTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(maintenanceJobTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", maintenanceJobTypeCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new MaintenanceJobTypeCollection();
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

        public static int SelectCountForGetList(MaintenanceJobTypeCriteria maintenanceJobTypeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceJobTypeCriteria.mId);

                if (!string.IsNullOrEmpty(maintenanceJobTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", maintenanceJobTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(maintenanceJobTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", maintenanceJobTypeCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(MaintenanceJobType myMaintenanceJobType)
        {
            if (!myMaintenanceJobType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a maintenanceJobType in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myMaintenanceJobType.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myMaintenanceJobType.mName);

                Helpers.SetSaveParameters(myCommand, myMaintenanceJobType);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update maintenanceJobType as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spMaintenanceJobTypeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static MaintenanceJobType FillDataRecord(IDataRecord myDataRecord)
        {
            MaintenanceJobType maintenanceJobType = new MaintenanceJobType();

            maintenanceJobType.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            maintenanceJobType.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            maintenanceJobType.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return maintenanceJobType;
        }
    }
}