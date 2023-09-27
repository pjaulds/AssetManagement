using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PurchaseRequestDetailDB
    {
        public static PurchaseRequestDetail GetItem(int purchaserequestdetailId)
        {
            PurchaseRequestDetail purchaserequestdetail = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestDetailSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", purchaserequestdetailId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        purchaserequestdetail = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return purchaserequestdetail;
        }

        public static PurchaseRequestDetailCollection GetList(PurchaseRequestDetailCriteria purchaserequestdetailCriteria)
        {
            PurchaseRequestDetailCollection tempList = new PurchaseRequestDetailCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestDetailSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", purchaserequestdetailCriteria.mPurchaseRequestId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PurchaseRequestDetailCollection();
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

        public static int SelectCountForGetList(PurchaseRequestDetailCriteria purchaserequestdetailCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestDetailSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", purchaserequestdetailCriteria.mPurchaseRequestId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(PurchaseRequestDetail myPurchaseRequestDetail)
        {
            if (!myPurchaseRequestDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a purchaserequestdetail in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseRequestDetailInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_id", myPurchaseRequestDetail.mPurchaseRequestId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@product_id", myPurchaseRequestDetail.mProductId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@quantity", myPurchaseRequestDetail.mQuantity);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@cost", myPurchaseRequestDetail.mCost);

                Helpers.SetSaveParameters(myCommand, myPurchaseRequestDetail);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update purchaserequestdetail as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPurchaseRequestDetailDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static PurchaseRequestDetail FillDataRecord(IDataRecord myDataRecord)
        {
            PurchaseRequestDetail purchaserequestdetail = new PurchaseRequestDetail();

            purchaserequestdetail.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            purchaserequestdetail.mPurchaseRequestId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("purchase_request_id"));
            purchaserequestdetail.mUnitName = myDataRecord.GetString(myDataRecord.GetOrdinal("unit_name"));
            purchaserequestdetail.mProductName = myDataRecord.GetString(myDataRecord.GetOrdinal("product_name"));
            purchaserequestdetail.mProductId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("product_id"));
            purchaserequestdetail.mQuantity = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("quantity"));
            purchaserequestdetail.mCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("cost"));
            return purchaserequestdetail;
        }
    }
}
