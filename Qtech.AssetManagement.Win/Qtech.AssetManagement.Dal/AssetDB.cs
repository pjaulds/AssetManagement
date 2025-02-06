using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AssetDB
    {
        public static Asset GetItem(int assetId)
        {
            Asset asset = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        asset = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return asset;
        }

        public static AssetCollection GetList(AssetCriteria assetCriteria)
        {
            AssetCollection tempList = new AssetCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetSearchList";

                if (assetCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", assetCriteria.mStartDate);

                if (assetCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", assetCriteria.mEndDate);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@project_id", assetCriteria.mProjectId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AssetCollection();
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

        public static int SelectCountForGetList(AssetCriteria assetCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);


                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Asset myAsset)
        {
            if (!myAsset.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a asset in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAsset.mCode);
                if (myAsset.mDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date", myAsset.mDate);
                if (myAsset.mReceivedDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@received_date", myAsset.mReceivedDate);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAsset.mName);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", myAsset.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@acquisition_cost", myAsset.mAcquisitionCost);
                if (myAsset.mWarrantyExpiry != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@warranty_expiry", myAsset.mWarrantyExpiry);
                Helpers.CreateParameter(myCommand, DbType.String, "@brand", myAsset.mBrand);
                Helpers.CreateParameter(myCommand, DbType.String, "@model", myAsset.mModel);
                Helpers.CreateParameter(myCommand, DbType.String, "@serial_number", myAsset.mSerialNumber);
                Helpers.CreateParameter(myCommand, DbType.String, "@capacity", myAsset.mCapacity);
                Helpers.CreateParameter(myCommand, DbType.String, "@engine_number", myAsset.mEngineNumber);
                Helpers.CreateParameter(myCommand, DbType.String, "@chassis_number", myAsset.mChassisNumber);
                Helpers.CreateParameter(myCommand, DbType.String, "@plate_number", myAsset.mPlateNumber);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@functional_location_id", myAsset.mFunctionalLocationId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@personnel_id", myAsset.mPersonnelId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@project_id", myAsset.mProjectId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@registered_by_id", myAsset.mRegisteredById);
                Helpers.CreateParameter(myCommand, DbType.String, "@remarks", myAsset.mRemarks);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", myAsset.mActive);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@residual_value", myAsset.mResidualValue);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@useful_life", myAsset.mUsefulLife);


                Helpers.SetSaveParameters(myCommand, myAsset);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update asset as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAssetDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Asset FillDataRecord(IDataRecord myDataRecord)
        {
            Asset asset = new Asset();

            asset.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            asset.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            if (myDataRecord["date"] != DBNull.Value)
                asset.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            if (myDataRecord["received_date"] != DBNull.Value)
                asset.mReceivedDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("received_date"));
            asset.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            asset.mAssetTypeName = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_type_name"));
            asset.mAssetTypeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("asset_type_id"));
            asset.mAcquisitionCost = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("acquisition_cost"));
            if (myDataRecord["warranty_expiry"] != DBNull.Value)
                asset.mWarrantyExpiry = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("warranty_expiry"));
            asset.mBrand = myDataRecord.GetString(myDataRecord.GetOrdinal("brand"));
            asset.mModel = myDataRecord.GetString(myDataRecord.GetOrdinal("model"));
            asset.mSerialNumber = myDataRecord.GetString(myDataRecord.GetOrdinal("serial_number"));
            asset.mCapacity = myDataRecord.GetString(myDataRecord.GetOrdinal("capacity"));
            asset.mEngineNumber = myDataRecord.GetString(myDataRecord.GetOrdinal("engine_number"));
            asset.mChassisNumber = myDataRecord.GetString(myDataRecord.GetOrdinal("chassis_number"));
            asset.mPlateNumber = myDataRecord.GetString(myDataRecord.GetOrdinal("plate_number"));
            asset.mFunctionalLocationName = myDataRecord.GetString(myDataRecord.GetOrdinal("functional_location_name"));
            asset.mFunctionalLocationId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("functional_location_id"));
            asset.mPersonnelName = myDataRecord.GetString(myDataRecord.GetOrdinal("personnel_name"));
            asset.mPersonnelId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("personnel_id"));
            asset.mProjectName = myDataRecord.GetString(myDataRecord.GetOrdinal("project_name"));
            asset.mProjectId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("project_id"));
            asset.mRegisteredByName = myDataRecord.GetString(myDataRecord.GetOrdinal("registered_by_name"));
            asset.mRegisteredById = myDataRecord.GetInt32(myDataRecord.GetOrdinal("registered_by_id"));
            asset.mRemarks = myDataRecord.GetString(myDataRecord.GetOrdinal("remarks"));
            asset.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            asset.mDisable = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("disable"));
            asset.mResidualValue = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("residual_value"));
            asset.mUsefulLife = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("useful_life"));
            asset.mAssetNo = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_no"));
            return asset;
        }
    }
}
