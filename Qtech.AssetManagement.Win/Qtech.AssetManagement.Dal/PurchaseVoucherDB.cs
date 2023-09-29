using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PurchaseVoucherDB
    {
        public static PurchaseVoucher GetItem(int purchasevoucherId)
        {
            PurchaseVoucher purchasevoucher = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseVoucherSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", purchasevoucherId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        purchasevoucher = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return purchasevoucher;
        }

        public static PurchaseVoucherCollection GetList(PurchaseVoucherCriteria purchasevoucherCriteria)
        {
            PurchaseVoucherCollection tempList = new PurchaseVoucherCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseVoucherSearchList";

                if (purchasevoucherCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", purchasevoucherCriteria.mStartDate);

                if (purchasevoucherCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", purchasevoucherCriteria.mEndDate);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PurchaseVoucherCollection();
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

        public static int SelectCountForGetList(PurchaseVoucherCriteria purchasevoucherCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseVoucherSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                if (purchasevoucherCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", purchasevoucherCriteria.mStartDate);

                if (purchasevoucherCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", purchasevoucherCriteria.mEndDate);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(PurchaseVoucher myPurchaseVoucher)
        {
            if (!myPurchaseVoucher.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a purchasevoucher in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseVoucherInsertUpdateSingleItem";
                
                Helpers.CreateParameter(myCommand, DbType.Int32, "@receiving_id", myPurchaseVoucher.mReceivingId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@payment_mode_id", myPurchaseVoucher.mPaymentModeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@prepared_by_id", myPurchaseVoucher.mPreparedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@checked_by_id", myPurchaseVoucher.mCheckedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@approved_by_id", myPurchaseVoucher.mApprovedById);

                Helpers.SetSaveParameters(myCommand, myPurchaseVoucher);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update purchasevoucher as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPurchaseVoucherDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static PurchaseVoucher FillDataRecord(IDataRecord myDataRecord)
        {
            PurchaseVoucher purchasevoucher = new PurchaseVoucher();

            purchasevoucher.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                purchasevoucher.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            purchasevoucher.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            purchasevoucher.mTransactionNo = myDataRecord.GetString(myDataRecord.GetOrdinal("transaction_no"));
            purchasevoucher.mReceivingNo = myDataRecord.GetString(myDataRecord.GetOrdinal("receiving_no"));
            purchasevoucher.mReceivingId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("receiving_id"));
            purchasevoucher.mPaymentModeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("payment_mode_id"));
            purchasevoucher.mPreparedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("prepared_by_name"));
            purchasevoucher.mPreparedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("prepared_by_id"));
            purchasevoucher.mCheckedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("checked_by_name"));
            purchasevoucher.mCheckedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("checked_by_id"));
            purchasevoucher.mApprovedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("approved_by_name"));
            purchasevoucher.mApprovedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("approved_by_id"));

            purchasevoucher.mInvoiceNo = myDataRecord.GetString(myDataRecord.GetOrdinal("invoice_no"));
            purchasevoucher.mPurchaseOrderNo = myDataRecord.GetString(myDataRecord.GetOrdinal("purchase_order_no"));
            purchasevoucher.mSupplierName = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_name"));
            purchasevoucher.mAmount = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("amount"));
            return purchasevoucher;
        }
    }
}