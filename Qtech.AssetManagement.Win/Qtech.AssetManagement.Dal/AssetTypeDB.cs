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
            return assetType;
        }
    }
}