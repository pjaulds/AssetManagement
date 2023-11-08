using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AssetCategoryDB
    {
        public static AssetCategory GetItem(int assetCategoryId)
        {
            AssetCategory assetCategory = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCategorySelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetCategoryId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        assetCategory = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return assetCategory;
        }

        public static AssetCategoryCollection GetList(AssetCategoryCriteria assetCategoryCriteria)
        {
            AssetCategoryCollection tempList = new AssetCategoryCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCategorySearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetCategoryCriteria.mId);

                if (!string.IsNullOrEmpty(assetCategoryCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetCategoryCriteria.mCode);

                if (!string.IsNullOrEmpty(assetCategoryCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetCategoryCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AssetCategoryCollection();
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

        public static int SelectCountForGetList(AssetCategoryCriteria assetCategoryCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCategorySearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetCategoryCriteria.mId);

                if (!string.IsNullOrEmpty(assetCategoryCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetCategoryCriteria.mCode);

                if (!string.IsNullOrEmpty(assetCategoryCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetCategoryCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AssetCategory myAssetCategory)
        {
            if (!myAssetCategory.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a assetCategory in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetCategoryInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAssetCategory.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAssetCategory.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myAssetCategory.mPost);

                Helpers.SetSaveParameters(myCommand, myAssetCategory);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update assetCategory as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAssetCategoryDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AssetCategory FillDataRecord(IDataRecord myDataRecord)
        {
            AssetCategory assetCategory = new AssetCategory();

            assetCategory.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            assetCategory.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            assetCategory.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            assetCategory.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            return assetCategory;
        }
    }
}