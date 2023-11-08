using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PaymentTermsDB
    {
        public static PaymentTerms GetItem(int paymentTermsId)
        {
            PaymentTerms paymentTerms = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentTermsSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", paymentTermsId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        paymentTerms = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return paymentTerms;
        }

        public static PaymentTermsCollection GetList(PaymentTermsCriteria paymentTermsCriteria)
        {
            PaymentTermsCollection tempList = new PaymentTermsCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentTermsSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", paymentTermsCriteria.mId);
                
                if (!string.IsNullOrEmpty(paymentTermsCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", paymentTermsCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PaymentTermsCollection();
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

        public static int SelectCountForGetList(PaymentTermsCriteria paymentTermsCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentTermsSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", paymentTermsCriteria.mId);
                
                if (!string.IsNullOrEmpty(paymentTermsCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", paymentTermsCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(PaymentTerms myPaymentTerms)
        {
            if (!myPaymentTerms.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a paymentTerms in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPaymentTermsInsertUpdateSingleItem";
                
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myPaymentTerms.mName);
                Helpers.CreateParameter(myCommand, DbType.String, "@remarks", myPaymentTerms.mRemarks);

                Helpers.SetSaveParameters(myCommand, myPaymentTerms);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update paymentTerms as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPaymentTermsDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static PaymentTerms FillDataRecord(IDataRecord myDataRecord)
        {
            PaymentTerms paymentTerms = new PaymentTerms();

            paymentTerms.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            paymentTerms.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            paymentTerms.mRemarks = myDataRecord.GetString(myDataRecord.GetOrdinal("remarks"));
            return paymentTerms;
        }
    }
}