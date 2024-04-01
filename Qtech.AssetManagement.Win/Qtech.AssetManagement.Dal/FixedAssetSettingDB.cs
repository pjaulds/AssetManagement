using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class FixedAssetSettingDB
    {
        public static FixedAssetSetting GetItem(int fixedassetsettingId)
        {
            FixedAssetSetting fixedassetsetting = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", fixedassetsettingId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        fixedassetsetting = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return fixedassetsetting;
        }

        public static FixedAssetSettingCollection GetList(FixedAssetSettingCriteria fixedassetsettingCriteria)
        {
            FixedAssetSettingCollection tempList = new FixedAssetSettingCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", fixedassetsettingCriteria.mAssetTypeId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new FixedAssetSettingCollection();
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

        public static int SelectCountForGetList(FixedAssetSettingCriteria fixedassetsettingCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", fixedassetsettingCriteria.mAssetTypeId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(FixedAssetSetting myFixedAssetSetting)
        {
            if (!myFixedAssetSetting.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a fixedassetsetting in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myFixedAssetSetting.mCode);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_type_id", myFixedAssetSetting.mAssetTypeId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_class_id", myFixedAssetSetting.mAssetClassId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@chart_of_account_id", myFixedAssetSetting.mChartOfAccountId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@accumulated_depreciation_account_id", myFixedAssetSetting.mAccumulatedDepreciationAccountId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@depreciation_expense_account_id", myFixedAssetSetting.mDepreciationExpenseAccountId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@depreciation_method_id", myFixedAssetSetting.mDepreciationMethodId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@averaging_method_id", myFixedAssetSetting.mAveragingMethodId);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@useful_life_years", myFixedAssetSetting.mUsefulLifeYears);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@depreciable", myFixedAssetSetting.mDepreciable);

                Helpers.SetSaveParameters(myCommand, myFixedAssetSetting);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update fixedassetsetting as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spFixedAssetSettingDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static FixedAssetSetting FillDataRecord(IDataRecord myDataRecord)
        {
            FixedAssetSetting fixedassetsetting = new FixedAssetSetting();

            fixedassetsetting.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            fixedassetsetting.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));

            fixedassetsetting.mAssetTypeId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("asset_type_id"));
            fixedassetsetting.mAssetTypeName = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_type_name"));

            fixedassetsetting.mAssetClassId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("asset_class_id"));
            fixedassetsetting.mAssetClassCode = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_class_code"));
            fixedassetsetting.mAssetClassName = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_class_name"));

            fixedassetsetting.mChartOfAccountCode = myDataRecord.GetString(myDataRecord.GetOrdinal("chart_of_account_code"));
            fixedassetsetting.mChartOfAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("chart_of_account_name"));
            fixedassetsetting.mChartOfAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("chart_of_account_id"));

            fixedassetsetting.mAccumulatedDepreciationAccountCode = myDataRecord.GetString(myDataRecord.GetOrdinal("accumulated_depreciation_account_code"));
            fixedassetsetting.mAccumulatedDepreciationAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("accumulated_depreciation_account_name"));
            fixedassetsetting.mAccumulatedDepreciationAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("accumulated_depreciation_account_id"));

            fixedassetsetting.mDepreciationExpenseAccountCode = myDataRecord.GetString(myDataRecord.GetOrdinal("depreciation_expense_account_code"));
            fixedassetsetting.mDepreciationExpenseAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("depreciation_expense_account_name"));
            fixedassetsetting.mDepreciationExpenseAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("depreciation_expense_account_id"));

            fixedassetsetting.mDepreciationMethodCode = myDataRecord.GetString(myDataRecord.GetOrdinal("depreciation_method_code"));
            fixedassetsetting.mDepreciationMethodName = myDataRecord.GetString(myDataRecord.GetOrdinal("depreciation_method_name"));
            fixedassetsetting.mDepreciationMethodId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("depreciation_method_id"));

            fixedassetsetting.mAveragingMethodCode = myDataRecord.GetString(myDataRecord.GetOrdinal("averaging_method_code"));
            fixedassetsetting.mAveragingMethodName = myDataRecord.GetString(myDataRecord.GetOrdinal("averaging_method_name"));
            fixedassetsetting.mAveragingMethodId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("averaging_method_id"));

            fixedassetsetting.mUsefulLifeYears = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("useful_life_years"));
            fixedassetsetting.mDepreciable = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("depreciable"));

            return fixedassetsetting;
        }
    }
}