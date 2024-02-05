using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class FixedAssetDB
    {
        public static FixedAsset GetItem(int fixedassetId)
        {
            FixedAsset fixedasset = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", fixedassetId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        fixedasset = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return fixedasset;
        }

        public static FixedAssetCollection GetList(FixedAssetCriteria fixedassetCriteria)
        {
            FixedAssetCollection tempList = new FixedAssetCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSearchList";

                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_draft", fixedassetCriteria.mIsDraft);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_registered", fixedassetCriteria.mIsRegistered);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new FixedAssetCollection();
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

        public static int SelectCountForGetList(FixedAssetCriteria fixedassetCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_draft", fixedassetCriteria.mIsDraft);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_registered", fixedassetCriteria.mIsRegistered);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(FixedAsset myFixedAsset)
        {
            if (!myFixedAsset.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a fixedasset in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@asset_no", myFixedAsset.mAssetNo);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@product_id", myFixedAsset.mProductId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@receiving_detail_id", myFixedAsset.mReceivingDetailId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", myFixedAsset.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@functional_location_id", myFixedAsset.mFunctionalLocationId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@personnel_id", myFixedAsset.mPersonnelId);
                Helpers.CreateParameter(myCommand, DbType.String, "@description", myFixedAsset.mDescription);
                if (myFixedAsset.mPurchaseDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@purchase_date", myFixedAsset.mPurchaseDate);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@purchase_price", myFixedAsset.mPurchasePrice);
                if (myFixedAsset.mWarrantyExpiry != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@warranty_expiry", myFixedAsset.mWarrantyExpiry);
                Helpers.CreateParameter(myCommand, DbType.String, "@serial_no", myFixedAsset.mSerialNo);
                Helpers.CreateParameter(myCommand, DbType.String, "@model", myFixedAsset.mModel);
                if (myFixedAsset.mDepreciationStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@depreciation_start_date", myFixedAsset.mDepreciationStartDate);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@depreciation_method_id", myFixedAsset.mDepreciationMethodId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@averaging_method_id", myFixedAsset.mAveragingMethodId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@accumulated_depreciation", myFixedAsset.mAccumulatedDepreciation);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@residual_value", myFixedAsset.mResidualValue);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@useful_life_years", myFixedAsset.mUsefulLifeYears);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_draft", myFixedAsset.mIsDraft);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_registered", myFixedAsset.mIsRegistered);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_disposed", myFixedAsset.mIsDisposed);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@register_by_id", myFixedAsset.mRegisterById);

                Helpers.SetSaveParameters(myCommand, myFixedAsset);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update fixedasset as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spFixedAssetDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static FixedAsset FillDataRecord(IDataRecord myDataRecord)
        {
            FixedAsset fixedasset = new FixedAsset();

            fixedasset.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            fixedasset.mAssetNo = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_no"));
            fixedasset.mProductId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("product_id"));
            fixedasset.mProductCode = myDataRecord.GetString(myDataRecord.GetOrdinal("product_code"));
            fixedasset.mProductName = myDataRecord.GetString(myDataRecord.GetOrdinal("product_name"));
            fixedasset.mReceivingDetailId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("receiving_detail_id"));
            fixedasset.mAssetTypeName = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_type_name"));
            fixedasset.mAssetTypeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("asset_type_id"));
            fixedasset.mFunctionalLocationName = myDataRecord.GetString(myDataRecord.GetOrdinal("functional_location_name"));
            fixedasset.mFunctionalLocationId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("functional_location_id"));
            fixedasset.mPersonnelName = myDataRecord.GetString(myDataRecord.GetOrdinal("personnel_name"));
            fixedasset.mPersonnelId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("personnel_id"));
            fixedasset.mDescription = myDataRecord.GetString(myDataRecord.GetOrdinal("description"));
            if (myDataRecord["purchase_date"] != DBNull.Value)
                fixedasset.mPurchaseDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("purchase_date"));
            fixedasset.mPurchasePrice = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("purchase_price"));
            if (myDataRecord["warranty_expiry"] != DBNull.Value)
                fixedasset.mWarrantyExpiry = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("warranty_expiry"));
            fixedasset.mSerialNo = myDataRecord.GetString(myDataRecord.GetOrdinal("serial_no"));
            fixedasset.mModel = myDataRecord.GetString(myDataRecord.GetOrdinal("model"));
            if (myDataRecord["depreciation_start_date"] != DBNull.Value)
                fixedasset.mDepreciationStartDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("depreciation_start_date"));
            fixedasset.mDepreciationMethodName = myDataRecord.GetString(myDataRecord.GetOrdinal("depreciation_method_name"));
            fixedasset.mDepreciationMethodId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("depreciation_method_id"));
            fixedasset.mAveragingMethodName = myDataRecord.GetString(myDataRecord.GetOrdinal("averaging_method_name"));
            fixedasset.mAveragingMethodId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("averaging_method_id"));
            fixedasset.mAccumulatedDepreciation = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("accumulated_depreciation"));
            fixedasset.mResidualValue = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("residual_value"));
            fixedasset.mUsefulLifeYears = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("useful_life_years"));
            fixedasset.mIsDraft = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("is_draft"));
            fixedasset.mIsRegistered = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("is_registered"));
            fixedasset.mIsDisposed = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("is_disposed"));
            fixedasset.mRegisterById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("register_by_id"));
            fixedasset.mRegisterByName = myDataRecord.GetString(myDataRecord.GetOrdinal("register_by_name"));
            return fixedasset;
        }
    }
}