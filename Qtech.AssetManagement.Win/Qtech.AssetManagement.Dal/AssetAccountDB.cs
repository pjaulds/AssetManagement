using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AssetAccountDB
    {
        public static AssetAccount GetItem(int assetaccountId)
        {
            AssetAccount assetaccount = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetAccountSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetaccountId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        assetaccount = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return assetaccount;
        }

        public static AssetAccountCollection GetList(AssetAccountCriteria assetaccountCriteria)
        {
            AssetAccountCollection tempList = new AssetAccountCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetAccountSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetaccountCriteria.mId);

                if (!string.IsNullOrEmpty(assetaccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetaccountCriteria.mCode);

                if (!string.IsNullOrEmpty(assetaccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetaccountCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AssetAccountCollection();
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

        public static int SelectCountForGetList(AssetAccountCriteria assetaccountCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetAccountSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", assetaccountCriteria.mId);

                if (!string.IsNullOrEmpty(assetaccountCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", assetaccountCriteria.mCode);

                if (!string.IsNullOrEmpty(assetaccountCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", assetaccountCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AssetAccount myAssetAccount)
        {
            if (!myAssetAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a assetaccount in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAssetAccountInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAssetAccount.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAssetAccount.mName);

                Helpers.SetSaveParameters(myCommand, myAssetAccount);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update assetaccount as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAssetAccountDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AssetAccount FillDataRecord(IDataRecord myDataRecord)
        {
            AssetAccount assetaccount = new AssetAccount();

            assetaccount.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            assetaccount.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            assetaccount.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return assetaccount;
        }
    }
}