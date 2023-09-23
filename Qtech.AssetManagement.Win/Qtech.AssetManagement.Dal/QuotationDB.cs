using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class QuotationDB
    {
        public static Quotation GetItem(int quotationId)
        {
            Quotation quotation = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", quotationId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        quotation = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return quotation;
        }

        public static QuotationCollection GetList(QuotationCriteria quotationCriteria)
        {
            QuotationCollection tempList = new QuotationCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationSearchList";

                if (quotationCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", quotationCriteria.mStartDate);

                if (quotationCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", quotationCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", quotationCriteria.mPurchaseRequestId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_po", quotationCriteria.mForPo);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_approval", quotationCriteria.mForApproval);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new QuotationCollection();
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

        public static int SelectCountForGetList(QuotationCriteria quotationCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                if (quotationCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", quotationCriteria.mStartDate);

                if (quotationCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", quotationCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", quotationCriteria.mPurchaseRequestId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_po", quotationCriteria.mForPo);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_approval", quotationCriteria.mForApproval);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Quotation myQuotation)
        {
            if (!myQuotation.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a quotation in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationInsertUpdateSingleItem";
                
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", myQuotation.mPurchaseRequestId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@prepared_by_id", myQuotation.mPreparedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@approved_by_id", myQuotation.mApprovedById);
                Helpers.CreateParameter(myCommand, DbType.Byte, "@supplier_no", myQuotation.mSupplierNo);

                Helpers.SetSaveParameters(myCommand, myQuotation);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update quotation as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spQuotationDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Quotation FillDataRecord(IDataRecord myDataRecord)
        {
            Quotation quotation = new Quotation();

            quotation.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                quotation.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            quotation.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            quotation.mPurchaseRequestNo = myDataRecord.GetString(myDataRecord.GetOrdinal("purchase_request_no"));
            quotation.mPurchaseRequestId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("purchase_request_id"));
            quotation.mPreparedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("prepared_by_name"));
            quotation.mPreparedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("prepared_by_id"));
            quotation.mApprovedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("approved_by_name"));
            quotation.mApprovedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("approved_by_id"));
            quotation.mSupplierNo = myDataRecord.GetByte(myDataRecord.GetOrdinal("supplier_no"));

            quotation.mSupplier1Name = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_1_name"));
            quotation.mSupplier2Name = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_2_name"));
            quotation.mSupplier3Name = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_3_name"));

            quotation.mTransactionNo = myDataRecord.GetString(myDataRecord.GetOrdinal("quotation_no"));
            return quotation;
        }
    }
}