using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PurchaseOrderDetailDB
    {
        public static PurchaseOrderDetail GetItem(int purchaseorderdetailId)
        {
            PurchaseOrderDetail purchaseorderdetail = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderDetailSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", purchaseorderdetailId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        purchaseorderdetail = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return purchaseorderdetail;
        }

        public static PurchaseOrderDetailCollection GetList(PurchaseOrderDetailCriteria purchaseorderdetailCriteria)
        {
            PurchaseOrderDetailCollection tempList = new PurchaseOrderDetailCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderDetailSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_id", purchaseorderdetailCriteria.mPurchaseOrderId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_detail_id", purchaseorderdetailCriteria.mQuotationDetailId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PurchaseOrderDetailCollection();
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

        public static int SelectCountForGetList(PurchaseOrderDetailCriteria purchaseorderdetailCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderDetailSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_id", purchaseorderdetailCriteria.mPurchaseOrderId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_detail_id", purchaseorderdetailCriteria.mQuotationDetailId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(PurchaseOrderDetail myPurchaseOrderDetail)
        {
            if (!myPurchaseOrderDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a purchaseorderdetail in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPurchaseOrderDetailInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_id", myPurchaseOrderDetail.mPurchaseOrderId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_detail_id", myPurchaseOrderDetail.mQuotationDetailId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@quantity", myPurchaseOrderDetail.mQuantity);

                Helpers.SetSaveParameters(myCommand, myPurchaseOrderDetail);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update purchaseorderdetail as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPurchaseOrderDetailDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static PurchaseOrderDetail FillDataRecord(IDataRecord myDataRecord)
        {
            PurchaseOrderDetail purchaseorderdetail = new PurchaseOrderDetail();

            purchaseorderdetail.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            purchaseorderdetail.mPurchaseOrderId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("purchase_order_id"));
            purchaseorderdetail.mQuotationDetailId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("quotation_detail_id"));
            purchaseorderdetail.mQuantity = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("quantity"));
            purchaseorderdetail.mUnitName = myDataRecord.GetString(myDataRecord.GetOrdinal("unit_name"));
            purchaseorderdetail.mProductName = myDataRecord.GetString(myDataRecord.GetOrdinal("product_name"));
            purchaseorderdetail.mCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("cost"));
            purchaseorderdetail.mTotalCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("total_cost"));
            return purchaseorderdetail;
        }
    }
}
