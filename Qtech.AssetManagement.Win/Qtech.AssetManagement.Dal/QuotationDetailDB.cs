using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class QuotationDetailDB
    {
        public static QuotationDetail GetItem(int quotationdetailId)
        {
            QuotationDetail quotationdetail = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationDetailSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", quotationdetailId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        quotationdetail = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return quotationdetail;
        }

        public static QuotationDetailCollection GetList(QuotationDetailCriteria quotationdetailCriteria)
        {
            QuotationDetailCollection tempList = new QuotationDetailCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationDetailSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", quotationdetailCriteria.mQuotationId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new QuotationDetailCollection();
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

        public static int SelectCountForGetList(QuotationDetailCriteria quotationdetailCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationDetailSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", quotationdetailCriteria.mQuotationId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(QuotationDetail myQuotationDetail)
        {
            if (!myQuotationDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a quotationdetail in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spQuotationDetailInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@quotation_id", myQuotationDetail.mQuotationId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@purchase_request_detail_id", myQuotationDetail.mPurchaseRequestDetailId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@cost1", myQuotationDetail.mCost1);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@cost2", myQuotationDetail.mCost2);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@cost3", myQuotationDetail.mCost3);

                Helpers.SetSaveParameters(myCommand, myQuotationDetail);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update quotationdetail as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spQuotationDetailDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static QuotationDetail FillDataRecord(IDataRecord myDataRecord)
        {
            QuotationDetail quotationdetail = new QuotationDetail();

            quotationdetail.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            quotationdetail.mQuotationId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("quotation_id"));
            quotationdetail.mPurchaseRequestDetailId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("purchase_request_detail_id"));
            quotationdetail.mUnitName = myDataRecord.GetString(myDataRecord.GetOrdinal("unit_name"));
            quotationdetail.mProductName = myDataRecord.GetString(myDataRecord.GetOrdinal("product_name"));
            quotationdetail.mQuantity = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("quantity"));
            quotationdetail.mCost1 = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("cost1"));
            quotationdetail.mCost2 = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("cost2"));
            quotationdetail.mCost3 = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("cost3"));
            quotationdetail.mCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("cost"));
            quotationdetail.mTotalCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("total_cost"));
            return quotationdetail;
        }
    }
}