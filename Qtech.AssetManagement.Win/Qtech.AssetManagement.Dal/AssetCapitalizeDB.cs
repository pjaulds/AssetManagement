using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AssetCapitalizeDB
    {
        public static AssetCapitalize GetItem(int assetcapitalizeId)
        {
            AssetCapitalize assetcapitalize = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCapitalizeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetcapitalizeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        assetcapitalize = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return assetcapitalize;
        }

        public static AssetCapitalizeCollection GetList(AssetCapitalizeCriteria assetcapitalizeCriteria)
        {
            AssetCapitalizeCollection tempList = new AssetCapitalizeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCapitalizeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_id", assetcapitalizeCriteria.mAssetId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AssetCapitalizeCollection();
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

        public static int SelectCountForGetList(AssetCapitalizeCriteria assetcapitalizeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCapitalizeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_id", assetcapitalizeCriteria.mAssetId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AssetCapitalize myAssetCapitalize)
        {
            if (!myAssetCapitalize.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a assetcapitalize in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCapitalizeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_id", myAssetCapitalize.mAssetId);
                if (myAssetCapitalize.mDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date", myAssetCapitalize.mDate);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@number", myAssetCapitalize.mNumber);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@capitalized_cost_id", myAssetCapitalize.mCapitalizedCostId);
                Helpers.CreateParameter(myCommand, DbType.String, "@description", string.IsNullOrEmpty(myAssetCapitalize.mDescription) ? "" : myAssetCapitalize.mDescription);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@amount", myAssetCapitalize.mAmount);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@useful_life", myAssetCapitalize.mUsefulLife);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@journalized", myAssetCapitalize.mJournalized);

                Helpers.SetSaveParameters(myCommand, myAssetCapitalize);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update assetcapitalize as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAssetCapitalizeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AssetCapitalize FillDataRecord(IDataRecord myDataRecord)
        {
            AssetCapitalize assetcapitalize = new AssetCapitalize();

            assetcapitalize.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            assetcapitalize.mAssetName = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_name"));
            assetcapitalize.mAssetId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("asset_id"));
            if (myDataRecord["date"] != DBNull.Value)
                assetcapitalize.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            assetcapitalize.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            assetcapitalize.mCapitalizedCostName = myDataRecord.GetString(myDataRecord.GetOrdinal("capitalized_cost_name"));
            assetcapitalize.mCapitalizedCostId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("capitalized_cost_id"));
            assetcapitalize.mDescription = myDataRecord.GetString(myDataRecord.GetOrdinal("description"));
            assetcapitalize.mAmount = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("amount"));
            assetcapitalize.mUsefulLife = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("useful_life"));
            assetcapitalize.mJournalized = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("journalized"));
            
            return assetcapitalize;
        }
    }
}