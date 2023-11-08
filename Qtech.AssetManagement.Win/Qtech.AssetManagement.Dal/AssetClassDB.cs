using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AssetClassDB
    {
        public static AssetClass GetItem(int assetClassId)
        {
            AssetClass assetClass = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetClassSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetClassId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        assetClass = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return assetClass;
        }

        public static AssetClassCollection GetList(AssetClassCriteria assetClassCriteria)
        {
            AssetClassCollection tempList = new AssetClassCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetClassSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetClassCriteria.mId);

                if (!string.IsNullOrEmpty(assetClassCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetClassCriteria.mCode);

                if (!string.IsNullOrEmpty(assetClassCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetClassCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AssetClassCollection();
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

        public static int SelectCountForGetList(AssetClassCriteria assetClassCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetClassSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetClassCriteria.mId);

                if (!string.IsNullOrEmpty(assetClassCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetClassCriteria.mCode);

                if (!string.IsNullOrEmpty(assetClassCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetClassCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AssetClass myAssetClass)
        {
            if (!myAssetClass.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a assetClass in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetClassInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAssetClass.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAssetClass.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myAssetClass.mPost);

                Helpers.SetSaveParameters(myCommand, myAssetClass);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update assetClass as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAssetClassDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AssetClass FillDataRecord(IDataRecord myDataRecord)
        {
            AssetClass assetClass = new AssetClass();

            assetClass.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            assetClass.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            assetClass.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            assetClass.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            return assetClass;
        }
    }
}