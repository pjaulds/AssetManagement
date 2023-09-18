using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PurchaseRequestDB
    {
        public static PurchaseRequest GetItem(int purchaserequestId)
        {
            PurchaseRequest purchaserequest = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", purchaserequestId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        purchaserequest = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return purchaserequest;
        }

        public static PurchaseRequestCollection GetList(PurchaseRequestCriteria purchaserequestCriteria)
        {
            PurchaseRequestCollection tempList = new PurchaseRequestCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestSearchList";

                if (purchaserequestCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", purchaserequestCriteria.mStartDate);

                if (purchaserequestCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", purchaserequestCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_quotation", purchaserequestCriteria.mForQuotation);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PurchaseRequestCollection();
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

        public static int SelectCountForGetList(PurchaseRequestCriteria purchaserequestCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                if (purchaserequestCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", purchaserequestCriteria.mStartDate);

                if (purchaserequestCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", purchaserequestCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Boolean, "@for_quotation", purchaserequestCriteria.mForQuotation);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(PurchaseRequest myPurchaseRequest)
        {
            if (!myPurchaseRequest.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a purchaserequest in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestInsertUpdateSingleItem";

              
                Helpers.CreateParameter(myCommand, DbType.Int32, "@requested_by_id", myPurchaseRequest.mRequestedById);
                if (myPurchaseRequest.mDateRequired != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date_required", myPurchaseRequest.mDateRequired);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@supplier_1_id", myPurchaseRequest.mSupplier1Id);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@supplier_2_id", myPurchaseRequest.mSupplier2Id);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@supplier_3_id", myPurchaseRequest.mSupplier3Id);
                Helpers.CreateParameter(myCommand, DbType.String, "@remarks", myPurchaseRequest.mRemarks);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@approved_by_id", myPurchaseRequest.mApprovedById);

                Helpers.SetSaveParameters(myCommand, myPurchaseRequest);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update purchaserequest as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPurchaseRequestDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static PurchaseRequest FillDataRecord(IDataRecord myDataRecord)
        {
            PurchaseRequest purchaserequest = new PurchaseRequest();

            purchaserequest.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                purchaserequest.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            purchaserequest.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            purchaserequest.mRequestedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("requested_by_name"));
            purchaserequest.mRequestedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("requested_by_id"));
            if (myDataRecord["date_required"] != DBNull.Value)
                purchaserequest.mDateRequired = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date_required"));
            purchaserequest.mSupplier1Name = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_1_name"));
            purchaserequest.mSupplier1Id = myDataRecord.GetInt32(myDataRecord.GetOrdinal("supplier_1_id"));
            purchaserequest.mSupplier2Name = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_2_name"));
            purchaserequest.mSupplier2Id = myDataRecord.GetInt32(myDataRecord.GetOrdinal("supplier_2_id"));
            purchaserequest.mSupplier3Name = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_3_name"));
            purchaserequest.mSupplier3Id = myDataRecord.GetInt32(myDataRecord.GetOrdinal("supplier_3_id"));
            purchaserequest.mRemarks = myDataRecord.GetString(myDataRecord.GetOrdinal("remarks"));
            purchaserequest.mApprovedByName = myDataRecord.GetString(myDataRecord.GetOrdinal("approved_by_name"));
            purchaserequest.mApprovedById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("approved_by_id"));
            
            purchaserequest.mTransactionNo = myDataRecord.GetString(myDataRecord.GetOrdinal("purchase_request_no"));
            return purchaserequest;
        }
    }
}