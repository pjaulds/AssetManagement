using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class WorkOrderTypeDB
    {
        public static WorkOrderType GetItem(int workOrderTypeId)
        {
            WorkOrderType workOrderType = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderTypeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", workOrderTypeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        workOrderType = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return workOrderType;
        }

        public static WorkOrderTypeCollection GetList(WorkOrderTypeCriteria workOrderTypeCriteria)
        {
            WorkOrderTypeCollection tempList = new WorkOrderTypeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderTypeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", workOrderTypeCriteria.mId);

                if (!string.IsNullOrEmpty(workOrderTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", workOrderTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(workOrderTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", workOrderTypeCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new WorkOrderTypeCollection();
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

        public static int SelectCountForGetList(WorkOrderTypeCriteria workOrderTypeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderTypeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", workOrderTypeCriteria.mId);

                if (!string.IsNullOrEmpty(workOrderTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", workOrderTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(workOrderTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", workOrderTypeCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(WorkOrderType myWorkOrderType)
        {
            if (!myWorkOrderType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a workOrderType in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderTypeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myWorkOrderType.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myWorkOrderType.mName);

                Helpers.SetSaveParameters(myCommand, myWorkOrderType);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update workOrderType as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spWorkOrderTypeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static WorkOrderType FillDataRecord(IDataRecord myDataRecord)
        {
            WorkOrderType workOrderType = new WorkOrderType();

            workOrderType.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            workOrderType.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            workOrderType.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return workOrderType;
        }
    }
}