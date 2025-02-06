using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AssetTypeDB
    {
        public static AssetType GetItem(int assetTypeId)
        {
            AssetType assetType = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetTypeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetTypeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        assetType = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return assetType;
        }

        public static AssetTypeCollection GetList(AssetTypeCriteria assetTypeCriteria)
        {
            AssetTypeCollection tempList = new AssetTypeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetTypeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetTypeCriteria.mId);

                if (!string.IsNullOrEmpty(assetTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(assetTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetTypeCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AssetTypeCollection();
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

        public static int SelectCountForGetList(AssetTypeCriteria assetTypeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetTypeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetTypeCriteria.mId);

                if (!string.IsNullOrEmpty(assetTypeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetTypeCriteria.mCode);

                if (!string.IsNullOrEmpty(assetTypeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetTypeCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AssetType myAssetType)
        {
            if (!myAssetType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a assetType in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetTypeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAssetType.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAssetType.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myAssetType.mPost);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_account_id", myAssetType.mAssetAccountId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@accumulated_depreciation_account_id", myAssetType.mAccumulatedDepreciationAccountId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@production_depreciation_expense_account_id", myAssetType.mProductionDepreciationExpenseAccountId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@production_depreciation_expense_account_value", myAssetType.mProductionDepreciationExpenseAccountValue);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@admin_depreciation_expense_account_id", myAssetType.mAdminDepreciationExpenseAccountId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@admin_depreciation_expense_account_value", myAssetType.mAdminDepreciationExpenseAccountValue);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@depreciation_method_id", myAssetType.mDepreciationMethodId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@averaging_method_id", myAssetType.mAveragingMethodId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@months", myAssetType.mMonths);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@useful_life_years", myAssetType.mUsefulLifeYears);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", myAssetType.mActive);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@depreciable", myAssetType.mDepreciable);

                Helpers.SetSaveParameters(myCommand, myAssetType);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update assetType as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAssetTypeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AssetType FillDataRecord(IDataRecord myDataRecord)
        {
            AssetType assetType = new AssetType();

            assetType.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            assetType.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            assetType.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            assetType.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            assetType.mAssetAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_account_name"));
            assetType.mAssetAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("asset_account_id"));
            assetType.mAccumulatedDepreciationAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("accumulated_depreciation_account_name"));
            assetType.mAccumulatedDepreciationAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("accumulated_depreciation_account_id"));
            assetType.mProductionDepreciationExpenseAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("production_depreciation_expense_account_name"));
            assetType.mProductionDepreciationExpenseAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("production_depreciation_expense_account_id"));
            assetType.mProductionDepreciationExpenseAccountValue = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("production_depreciation_expense_account_value"));
            assetType.mAdminDepreciationExpenseAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("admin_depreciation_expense_account_name"));
            assetType.mAdminDepreciationExpenseAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("admin_depreciation_expense_account_id"));
            assetType.mAdminDepreciationExpenseAccountValue = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("admin_depreciation_expense_account_value"));
            assetType.mDepreciationMethodName = myDataRecord.GetString(myDataRecord.GetOrdinal("depreciation_method_name"));
            assetType.mDepreciationMethodId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("depreciation_method_id"));
            assetType.mAveragingMethodName = myDataRecord.GetString(myDataRecord.GetOrdinal("averaging_method_name"));
            assetType.mAveragingMethodId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("averaging_method_id"));
            assetType.mMonths = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("months"));
            assetType.mUsefulLifeYears = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("useful_life_years"));
            assetType.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            assetType.mDepreciable = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("depreciable"));

            return assetType;
        }
    }
}