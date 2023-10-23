using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class MaintenanceRequestDB
    {
        public static MaintenanceRequest GetItem(int maintenancerequestId)
        {
            MaintenanceRequest maintenancerequest = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", maintenancerequestId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        maintenancerequest = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return maintenancerequest;
        }

        public static MaintenanceRequestCollection GetList(MaintenanceRequestCriteria maintenancerequestCriteria)
        {
            MaintenanceRequestCollection tempList = new MaintenanceRequestCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestSearchList";

                if (maintenancerequestCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", maintenancerequestCriteria.mStartDate);
                if (maintenancerequestCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", maintenancerequestCriteria.mEndDate);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new MaintenanceRequestCollection();
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

        public static int SelectCountForGetList(MaintenanceRequestCriteria maintenancerequestCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                if (maintenancerequestCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", maintenancerequestCriteria.mStartDate);
                if (maintenancerequestCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", maintenancerequestCriteria.mEndDate);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(MaintenanceRequest myMaintenanceRequest)
        {
            if (!myMaintenanceRequest.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a maintenancerequest in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spMaintenanceRequestInsertUpdateSingleItem";
                
                if (myMaintenanceRequest.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", myMaintenanceRequest.mStartDate);
                if (myMaintenanceRequest.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", myMaintenanceRequest.mEndDate);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@maintenance_request_type_id", myMaintenanceRequest.mMaintenanceRequestTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@service_level_id", myMaintenanceRequest.mServiceLevelId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@requested_by_id", myMaintenanceRequest.mRequestedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@functional_location_id", myMaintenanceRequest.mFunctionalLocationId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", myMaintenanceRequest.mFixedAssetId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@fault_symptoms_id", myMaintenanceRequest.mFaultSymptomsId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@fault_area_id", myMaintenanceRequest.mFaultAreaId);
                Helpers.CreateParameter(myCommand, DbType.String, "@description", myMaintenanceRequest.mDescription);
                Helpers.CreateParameter(myCommand, DbType.String, "@status", myMaintenanceRequest.mStatus);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", myMaintenanceRequest.mActive);

                Helpers.SetSaveParameters(myCommand, myMaintenanceRequest);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update maintenancerequest as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spMaintenanceRequestDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static MaintenanceRequest FillDataRecord(IDataRecord myDataRecord)
        {
            MaintenanceRequest maintenancerequest = new MaintenanceRequest();

            maintenancerequest.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                maintenancerequest.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            maintenancerequest.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            maintenancerequest.mMaintenanceRequestNo = myDataRecord.GetString(myDataRecord.GetOrdinal("maintenance_request_no"));
            if (myDataRecord["start_date"] != DBNull.Value)
                maintenancerequest.mStartDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("start_date"));
            if (myDataRecord["end_date"] != DBNull.Value)
                maintenancerequest.mEndDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("end_date"));
            maintenancerequest.mMaintenanceRequestTypeName = myDataRecord.GetString(myDataRecord.GetOrdinal("maintenance_request_type_name"));
            maintenancerequest.mMaintenanceRequestTypeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("maintenance_request_type_id"));
            maintenancerequest.mServiceLevelName = myDataRecord.GetString(myDataRecord.GetOrdinal("service_level_name"));
            maintenancerequest.mServiceLevelId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("service_level_id"));
            maintenancerequest.mRequestedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("requested_by_name"));
            maintenancerequest.mRequestedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("requested_by_id"));
            maintenancerequest.mFunctionalLocationName = myDataRecord.GetString(myDataRecord.GetOrdinal("functional_location_name"));
            maintenancerequest.mFunctionalLocationId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("functional_location_id"));
            maintenancerequest.mFixedAssetName = myDataRecord.GetString(myDataRecord.GetOrdinal("fixed_asset_name"));
            maintenancerequest.mFixedAssetId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("fixed_asset_id"));
            maintenancerequest.mFaultSymptomsName = myDataRecord.GetString(myDataRecord.GetOrdinal("fault_symptoms_name"));
            maintenancerequest.mFaultSymptomsId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("fault_symptoms_id"));
            maintenancerequest.mFaultAreaName = myDataRecord.GetString(myDataRecord.GetOrdinal("fault_area_name"));
            maintenancerequest.mFaultAreaId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("fault_area_id"));
            maintenancerequest.mDescription = myDataRecord.GetString(myDataRecord.GetOrdinal("description"));
            maintenancerequest.mStatus = myDataRecord.GetString(myDataRecord.GetOrdinal("status"));
            maintenancerequest.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            return maintenancerequest;
        }
    }
}
