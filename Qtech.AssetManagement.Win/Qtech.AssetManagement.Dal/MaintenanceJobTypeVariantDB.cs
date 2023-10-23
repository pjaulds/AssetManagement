using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class MaintenanceJobTypeVariantDB
    {
        public static MaintenanceJobTypeVariant GetItem(int maintenanceJobTypeVariantId)
        {
            MaintenanceJobTypeVariant maintenanceJobTypeVariant = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeVariantSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceJobTypeVariantId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        maintenanceJobTypeVariant = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return maintenanceJobTypeVariant;
        }

        public static MaintenanceJobTypeVariantCollection GetList(MaintenanceJobTypeVariantCriteria maintenanceJobTypeVariantCriteria)
        {
            MaintenanceJobTypeVariantCollection tempList = new MaintenanceJobTypeVariantCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeVariantSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceJobTypeVariantCriteria.mId);

                if (!string.IsNullOrEmpty(maintenanceJobTypeVariantCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", maintenanceJobTypeVariantCriteria.mCode);

                if (!string.IsNullOrEmpty(maintenanceJobTypeVariantCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", maintenanceJobTypeVariantCriteria.mName);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@maintenance_job_type_id", maintenanceJobTypeVariantCriteria.mMaintenanceJobTypeId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new MaintenanceJobTypeVariantCollection();
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

        public static int SelectCountForGetList(MaintenanceJobTypeVariantCriteria maintenanceJobTypeVariantCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeVariantSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenanceJobTypeVariantCriteria.mId);

                if (!string.IsNullOrEmpty(maintenanceJobTypeVariantCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", maintenanceJobTypeVariantCriteria.mCode);

                if (!string.IsNullOrEmpty(maintenanceJobTypeVariantCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", maintenanceJobTypeVariantCriteria.mName);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@maintenance_job_type_id", maintenanceJobTypeVariantCriteria.mMaintenanceJobTypeId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(MaintenanceJobTypeVariant myMaintenanceJobTypeVariant)
        {
            if (!myMaintenanceJobTypeVariant.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a maintenanceJobTypeVariant in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceJobTypeVariantInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@maintenance_job_type_id", myMaintenanceJobTypeVariant.mMaintenanceJobTypeId);
                Helpers.CreateParameter(myCommand, DbType.String, "@code", myMaintenanceJobTypeVariant.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myMaintenanceJobTypeVariant.mName);

                Helpers.SetSaveParameters(myCommand, myMaintenanceJobTypeVariant);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update maintenanceJobTypeVariant as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spMaintenanceJobTypeVariantDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static MaintenanceJobTypeVariant FillDataRecord(IDataRecord myDataRecord)
        {
            MaintenanceJobTypeVariant maintenanceJobTypeVariant = new MaintenanceJobTypeVariant();

            maintenanceJobTypeVariant.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            maintenanceJobTypeVariant.mMaintenanceJobTypeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("maintenance_job_type_id"));
            maintenanceJobTypeVariant.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            maintenanceJobTypeVariant.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return maintenanceJobTypeVariant;
        }
    }
}