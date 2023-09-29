using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class ReceivingDB
    {
        public static Receiving GetItem(int receivingId)
        {
            Receiving receiving = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", receivingId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        receiving = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return receiving;
        }

        public static ReceivingCollection GetList(ReceivingCriteria receivingCriteria)
        {
            ReceivingCollection tempList = new ReceivingCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingSearchList";

                if (receivingCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", receivingCriteria.mStartDate);

                if (receivingCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", receivingCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", receivingCriteria.mQuotationId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_id", receivingCriteria.mPurchaseOrderId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_approval", receivingCriteria.mForApproval);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_purchase_voucher", receivingCriteria.mForPurchaseVoucher);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new ReceivingCollection();
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

        public static int SelectCountForGetList(ReceivingCriteria receivingCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                if (receivingCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", receivingCriteria.mStartDate);

                if (receivingCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", receivingCriteria.mEndDate);
                
                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", receivingCriteria.mQuotationId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_id", receivingCriteria.mPurchaseOrderId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_approval", receivingCriteria.mForApproval);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_purchase_voucher", receivingCriteria.mForPurchaseVoucher);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Receiving myReceiving)
        {
            if (!myReceiving.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a receiving in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingInsertUpdateSingleItem";
                
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_id", myReceiving.mPurchaseOrderId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@prepared_by_id", myReceiving.mPreparedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@checked_by_id", myReceiving.mCheckedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@approved_by_id", myReceiving.mApprovedById);
                Helpers.CreateParameter(myCommand, DbType.String, "@invoice_no", myReceiving.mInvoiceNo);
                Helpers.CreateParameter(myCommand, DbType.String, "@dr_no", myReceiving.mDrNo);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@amount", myReceiving.mAmount);
                Helpers.CreateParameter(myCommand, DbType.String, "@remarks", myReceiving.mRemarks);

                Helpers.SetSaveParameters(myCommand, myReceiving);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update receiving as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spReceivingDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Receiving FillDataRecord(IDataRecord myDataRecord)
        {
            Receiving receiving = new Receiving();

            receiving.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                receiving.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            receiving.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            receiving.mPurchaseOrderId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("purchase_order_id"));
            receiving.mPreparedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("prepared_by_name"));
            receiving.mPreparedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("prepared_by_id"));
            receiving.mCheckedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("checked_by_name"));
            receiving.mCheckedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("checked_by_id"));
            receiving.mApprovedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("approved_by_name"));
            receiving.mApprovedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("approved_by_id"));
            receiving.mInvoiceNo = myDataRecord.GetString(myDataRecord.GetOrdinal("invoice_no"));
            receiving.mDrNo = myDataRecord.GetString(myDataRecord.GetOrdinal("dr_no"));
            receiving.mAmount = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("amount"));
            receiving.mRemarks = myDataRecord.GetString(myDataRecord.GetOrdinal("remarks"));

            receiving.mTransactionNo = myDataRecord.GetString(myDataRecord.GetOrdinal("receiving_no"));
            receiving.mPurchaseOrderNo = myDataRecord.GetString(myDataRecord.GetOrdinal("purchase_order_no"));
            receiving.mQuotationtNo = myDataRecord.GetString(myDataRecord.GetOrdinal("quotation_no"));
            receiving.mPurchaseRequestNo = myDataRecord.GetString(myDataRecord.GetOrdinal("purchase_request_no"));
            receiving.mSupplierId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("supplier_id"));
            receiving.mSupplierName = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_name"));
            return receiving;
        }
    }
}
