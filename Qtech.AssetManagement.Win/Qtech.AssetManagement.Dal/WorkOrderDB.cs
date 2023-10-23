using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class WorkOrderDB
    {
        public static WorkOrder GetItem(int workorderId)
        {
            WorkOrder workorder = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", workorderId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        workorder = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return workorder;
        }

        public static WorkOrderCollection GetList(WorkOrderCriteria workorderCriteria)
        {
            WorkOrderCollection tempList = new WorkOrderCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderSearchList";

                if (workorderCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", workorderCriteria.mStartDate);
                if (workorderCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", workorderCriteria.mEndDate);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new WorkOrderCollection();
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

        public static int SelectCountForGetList(WorkOrderCriteria workorderCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);


                if (workorderCriteria.mStartDate!= DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", workorderCriteria.mStartDate);
                if (workorderCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", workorderCriteria.mEndDate);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(WorkOrder myWorkOrder)
        {
            if (!myWorkOrder.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a workorder in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderInsertUpdateSingleItem";

                if (myWorkOrder.mExpectedStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@expected_start_date", myWorkOrder.mExpectedStartDate);
                if (myWorkOrder.mExpectedEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@expected_end_date", myWorkOrder.mExpectedEndDate);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@number", myWorkOrder.mNumber);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@maintenance_request_id", myWorkOrder.mMaintenanceRequestId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@work_order_type_id", myWorkOrder.mWorkOrderTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@maintenance_job_type_variant_id", myWorkOrder.mMaintenanceJobTypeVariantId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@trade_id", myWorkOrder.mTradeId);

                Helpers.SetSaveParameters(myCommand, myWorkOrder);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update workorder as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spWorkOrderDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static WorkOrder FillDataRecord(IDataRecord myDataRecord)
        {
            WorkOrder workorder = new WorkOrder();

            workorder.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));            
            if (myDataRecord["expected_start_date"] != DBNull.Value)
                workorder.mExpectedStartDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("expected_start_date"));
            if (myDataRecord["expected_end_date"] != DBNull.Value)
                workorder.mExpectedEndDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("expected_end_date"));
            workorder.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            workorder.mMaintenanceRequestNo = myDataRecord.GetString(myDataRecord.GetOrdinal("maintenance_request_no"));
            workorder.mMaintenanceRequestId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("maintenance_request_id"));
            workorder.mWorkOrderTypeName = myDataRecord.GetString(myDataRecord.GetOrdinal("work_order_type_name"));
            workorder.mWorkOrderTypeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("work_order_type_id"));
            workorder.mMaintenanceJobTypeVariantName = myDataRecord.GetString(myDataRecord.GetOrdinal("maintenance_job_type_variant_name"));
            workorder.mMaintenanceJobTypeVariantId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("maintenance_job_type_variant_id"));
            workorder.mTradeName = myDataRecord.GetString(myDataRecord.GetOrdinal("trade_name"));
            workorder.mTradeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("trade_id"));

            workorder.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            workorder.mFixedAssetName = myDataRecord.GetString(myDataRecord.GetOrdinal("fixed_asset_name"));
            workorder.mFunctionalLocationName = myDataRecord.GetString(myDataRecord.GetOrdinal("functional_location_name"));
            workorder.mWorkOrderNo = myDataRecord.GetString(myDataRecord.GetOrdinal("work_order_no"));
            workorder.mDescription = myDataRecord.GetString(myDataRecord.GetOrdinal("description"));
            workorder.mServiceLevelName = myDataRecord.GetString(myDataRecord.GetOrdinal("service_level_name"));
            workorder.mStatus = myDataRecord.GetString(myDataRecord.GetOrdinal("status"));
            workorder.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            return workorder;
        }
    }
}