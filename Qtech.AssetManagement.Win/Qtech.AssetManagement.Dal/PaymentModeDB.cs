using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PaymentModeDB
    {
        public static PaymentMode GetItem(int paymentModeId)
        {
            PaymentMode paymentMode = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentModeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", paymentModeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        paymentMode = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return paymentMode;
        }

        public static PaymentModeCollection GetList(PaymentModeCriteria paymentModeCriteria)
        {
            PaymentModeCollection tempList = new PaymentModeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentModeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", paymentModeCriteria.mId);

                if (!string.IsNullOrEmpty(paymentModeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", paymentModeCriteria.mCode);

                if (!string.IsNullOrEmpty(paymentModeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", paymentModeCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PaymentModeCollection();
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

        public static int SelectCountForGetList(PaymentModeCriteria paymentModeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentModeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", paymentModeCriteria.mId);

                if (!string.IsNullOrEmpty(paymentModeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", paymentModeCriteria.mCode);

                if (!string.IsNullOrEmpty(paymentModeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", paymentModeCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(PaymentMode myPaymentMode)
        {
            if (!myPaymentMode.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a paymentMode in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentModeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myPaymentMode.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myPaymentMode.mName);

                Helpers.SetSaveParameters(myCommand, myPaymentMode);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update paymentMode as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPaymentModeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static PaymentMode FillDataRecord(IDataRecord myDataRecord)
        {
            PaymentMode paymentMode = new PaymentMode();

            paymentMode.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            paymentMode.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            paymentMode.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return paymentMode;
        }
    }
}