using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class ReceivingDetailDB
    {
        public static ReceivingDetail GetItem(int receivingdetailId)
        {
            ReceivingDetail receivingdetail = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingDetailSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", receivingdetailId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        receivingdetail = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return receivingdetail;
        }

        public static ReceivingDetailCollection GetList(ReceivingDetailCriteria receivingdetailCriteria)
        {
            ReceivingDetailCollection tempList = new ReceivingDetailCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingDetailSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@receiving_id", receivingdetailCriteria.mReceivingId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_detail_id", receivingdetailCriteria.mPurchaseOrderDetailId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new ReceivingDetailCollection();
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

        public static int SelectCountForGetList(ReceivingDetailCriteria receivingdetailCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingDetailSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@receiving_id", receivingdetailCriteria.mReceivingId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_detail_id", receivingdetailCriteria.mPurchaseOrderDetailId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(ReceivingDetail myReceivingDetail)
        {
            if (!myReceivingDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a receivingdetail in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spReceivingDetailInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@receiving_id", myReceivingDetail.mReceivingId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_order_detail_id", myReceivingDetail.mPurchaseOrderDetailId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@quantity", myReceivingDetail.mQuantity);

                Helpers.SetSaveParameters(myCommand, myReceivingDetail);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update receivingdetail as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spReceivingDetailDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static ReceivingDetail FillDataRecord(IDataRecord myDataRecord)
        {
            ReceivingDetail receivingdetail = new ReceivingDetail();

            receivingdetail.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            receivingdetail.mReceivingId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("receiving_id"));
            receivingdetail.mPurchaseOrderDetailId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("purchase_order_detail_id"));
            receivingdetail.mQuantity = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("quantity"));

            receivingdetail.mUnitName = myDataRecord.GetString(myDataRecord.GetOrdinal("unit_name"));
            receivingdetail.mProductName = myDataRecord.GetString(myDataRecord.GetOrdinal("product_name"));
            receivingdetail.mCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("cost"));
            receivingdetail.mTotalCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("total_cost"));

            return receivingdetail;
        }
    }
}