using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class DisposalDB
    {
        public static Disposal GetItem(int disposalId)
        {
            Disposal disposal = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDisposalSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", disposalId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        disposal = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return disposal;
        }

        public static DisposalCollection GetList(DisposalCriteria disposalCriteria)
        {
            DisposalCollection tempList = new DisposalCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDisposalSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", disposalCriteria.mFixedAssetId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new DisposalCollection();
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

        public static int SelectCountForGetList(DisposalCriteria disposalCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDisposalSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", disposalCriteria.mFixedAssetId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Disposal myDisposal)
        {
            if (!myDisposal.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a disposal in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDisposalInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", myDisposal.mFixedAssetId);
                if (myDisposal.mDateDisposed != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date_disposed", myDisposal.mDateDisposed);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@sales_proceeds", myDisposal.mSalesProceeds);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@cash_account_id", myDisposal.mCashAccountId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@gain_loss_account_id", myDisposal.mGainLossAccountId);
                if (myDisposal.mDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date", myDisposal.mDate);

                Helpers.SetSaveParameters(myCommand, myDisposal);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update disposal as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spDisposalDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Disposal FillDataRecord(IDataRecord myDataRecord)
        {
            Disposal disposal = new Disposal();

            disposal.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            disposal.mFixedAssetName = myDataRecord.GetString(myDataRecord.GetOrdinal("fixed_asset_name"));
            disposal.mFixedAssetId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("fixed_asset_id"));
            if (myDataRecord["date_disposed"] != DBNull.Value)
                disposal.mDateDisposed = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date_disposed"));
            disposal.mSalesProceeds = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("sales_proceeds"));
            disposal.mCashAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("cash_account_name"));
            disposal.mCashAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("cash_account_id"));
            disposal.mGainLossAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("gain_loss_account_name"));
            disposal.mGainLossAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("gain_loss_account_id"));
            if (myDataRecord["date"] != DBNull.Value)
                disposal.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            return disposal;
        }
    }
}
