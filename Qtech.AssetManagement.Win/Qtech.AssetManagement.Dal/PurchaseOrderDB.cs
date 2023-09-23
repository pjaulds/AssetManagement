using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PurchaseOrderDB
    {
        public static PurchaseOrder GetItem(int purchaseorderId)
        {
            PurchaseOrder purchaseorder = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", purchaseorderId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        purchaseorder = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return purchaseorder;
        }

        public static PurchaseOrderCollection GetList(PurchaseOrderCriteria purchaseorderCriteria)
        {
            PurchaseOrderCollection tempList = new PurchaseOrderCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderSearchList";

                if (purchaseorderCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", purchaseorderCriteria.mStartDate);

                if (purchaseorderCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", purchaseorderCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", purchaseorderCriteria.mPurchaseRequestId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", purchaseorderCriteria.mQuotationId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_approval", purchaseorderCriteria.mForApproval);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_receiving", purchaseorderCriteria.mForReceiving);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PurchaseOrderCollection();
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

        public static int SelectCountForGetList(PurchaseOrderCriteria purchaseorderCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                if (purchaseorderCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", purchaseorderCriteria.mStartDate);

                if (purchaseorderCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", purchaseorderCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", purchaseorderCriteria.mPurchaseRequestId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", purchaseorderCriteria.mQuotationId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_approval", purchaseorderCriteria.mForApproval);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_receiving", purchaseorderCriteria.mForReceiving);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(PurchaseOrder myPurchaseOrder)
        {
            if (!myPurchaseOrder.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a purchaseorder in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderInsertUpdateSingleItem";

                if (myPurchaseOrder.mDateOfDelivery != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date_of_delivery", myPurchaseOrder.mDateOfDelivery);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", myPurchaseOrder.mQuotationId);
                Helpers.CreateParameter(myCommand, DbType.String, "@terms", myPurchaseOrder.mTerms);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@prepared_by_id", myPurchaseOrder.mPreparedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@noted_by_id", myPurchaseOrder.mNotedById);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@approved_by_id", myPurchaseOrder.mApprovedById);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@revised", myPurchaseOrder.mRevised);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@cancelled", myPurchaseOrder.mCancelled);

                Helpers.SetSaveParameters(myCommand, myPurchaseOrder);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update purchaseorder as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPurchaseOrderDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static PurchaseOrder FillDataRecord(IDataRecord myDataRecord)
        {
            PurchaseOrder purchaseorder = new PurchaseOrder();

            purchaseorder.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                purchaseorder.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            if (myDataRecord["date_of_delivery"] != DBNull.Value)
                purchaseorder.mDateOfDelivery = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date_of_delivery"));
            purchaseorder.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            purchaseorder.mPurchaseRequestNo = myDataRecord.GetString(myDataRecord.GetOrdinal("purchase_request_no"));
            purchaseorder.mQuotationNo = myDataRecord.GetString(myDataRecord.GetOrdinal("quotation_no"));
            purchaseorder.mTransactionNo = myDataRecord.GetString(myDataRecord.GetOrdinal("purchase_order_no"));
            purchaseorder.mQuotationId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("quotation_id"));
            purchaseorder.mSupplierId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("supplier_id"));
            purchaseorder.mTerms = myDataRecord.GetString(myDataRecord.GetOrdinal("terms"));
            purchaseorder.mPreparedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("prepared_by_name"));
            purchaseorder.mPreparedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("prepared_by_id"));
            purchaseorder.mNotedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("noted_by_name"));
            purchaseorder.mNotedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("noted_by_id"));
            purchaseorder.mApprovedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("approved_by_name"));
            purchaseorder.mApprovedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("approved_by_id"));
            purchaseorder.mRevised = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("revised"));
            purchaseorder.mCancelled = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("cancelled"));
            
            return purchaseorder;
        }
    }
}