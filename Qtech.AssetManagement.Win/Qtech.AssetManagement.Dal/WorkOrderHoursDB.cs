using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class WorkOrderHoursDB
    {
        public static WorkOrderHours GetItem(int workorderhoursId)
        {
            WorkOrderHours workorderhours = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderHoursSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", workorderhoursId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        workorderhours = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return workorderhours;
        }

        public static WorkOrderHoursCollection GetList(WorkOrderHoursCriteria workorderhoursCriteria)
        {
            WorkOrderHoursCollection tempList = new WorkOrderHoursCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderHoursSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@work_order_id", workorderhoursCriteria.mWorkOrderId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new WorkOrderHoursCollection();
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

        public static int SelectCountForGetList(WorkOrderHoursCriteria workorderhoursCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderHoursSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@work_order_id", workorderhoursCriteria.mWorkOrderId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(WorkOrderHours myWorkOrderHours)
        {
            if (!myWorkOrderHours.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a workorderhours in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spWorkOrderHoursInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@work_order_id", myWorkOrderHours.mWorkOrderId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@expense_category_id", myWorkOrderHours.mExpenseCategoryId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@hours", myWorkOrderHours.mHours);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@rate_per_hour", myWorkOrderHours.mRatePerHour);

                Helpers.SetSaveParameters(myCommand, myWorkOrderHours);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update workorderhours as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spWorkOrderHoursDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static WorkOrderHours FillDataRecord(IDataRecord myDataRecord)
        {
            WorkOrderHours workorderhours = new WorkOrderHours();

            workorderhours.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            workorderhours.mWorkOrderId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("work_order_id"));
            workorderhours.mExpenseCategoryName = myDataRecord.GetString(myDataRecord.GetOrdinal("expense_category_name"));
            workorderhours.mExpenseCategoryId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("expense_category_id"));
            workorderhours.mHours = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("hours"));
            workorderhours.mRatePerHour = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("rate_per_hour"));
            return workorderhours;
        }
    }
}
