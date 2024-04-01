using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class FixedAssetCapitalizedCostDB
    {
        public static FixedAssetCapitalizedCost GetItem(int fixedassetcapitalizedcostId)
        {
            FixedAssetCapitalizedCost fixedassetcapitalizedcost = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetCapitalizedCostSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", fixedassetcapitalizedcostId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        fixedassetcapitalizedcost = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return fixedassetcapitalizedcost;
        }

        public static FixedAssetCapitalizedCostCollection GetList(FixedAssetCapitalizedCostCriteria fixedassetcapitalizedcostCriteria)
        {
            FixedAssetCapitalizedCostCollection tempList = new FixedAssetCapitalizedCostCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetCapitalizedCostSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", fixedassetcapitalizedcostCriteria.mFixedAssetId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new FixedAssetCapitalizedCostCollection();
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

        public static int SelectCountForGetList(FixedAssetCapitalizedCostCriteria fixedassetcapitalizedcostCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetCapitalizedCostSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", fixedassetcapitalizedcostCriteria.mFixedAssetId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(FixedAssetCapitalizedCost myFixedAssetCapitalizedCost)
        {
            if (!myFixedAssetCapitalizedCost.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a fixedassetcapitalizedcost in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetCapitalizedCostInsertUpdateSingleItem";


                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", myFixedAssetCapitalizedCost.mFixedAssetId);
                if (myFixedAssetCapitalizedCost.mDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date", myFixedAssetCapitalizedCost.mDate);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@capitalized_cost_id", myFixedAssetCapitalizedCost.mCapitalizedCostId);
                Helpers.CreateParameter(myCommand, DbType.String, "@description", string.IsNullOrEmpty(myFixedAssetCapitalizedCost.mDescription) ? "" : myFixedAssetCapitalizedCost.mDescription);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@amount", myFixedAssetCapitalizedCost.mAmount);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@useful_life", myFixedAssetCapitalizedCost.mUsefulLife);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@is_journalized", myFixedAssetCapitalizedCost.mIsJournalized);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@asset_account_id", myFixedAssetCapitalizedCost.mAssetAccountId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@cash_payable_account_id", myFixedAssetCapitalizedCost.mCashPayableAccountId);

                Helpers.SetSaveParameters(myCommand, myFixedAssetCapitalizedCost);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update fixedassetcapitalizedcost as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spFixedAssetCapitalizedCostDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static FixedAssetCapitalizedCost FillDataRecord(IDataRecord myDataRecord)
        {
            FixedAssetCapitalizedCost fixedassetcapitalizedcost = new FixedAssetCapitalizedCost();

            fixedassetcapitalizedcost.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            fixedassetcapitalizedcost.mFixedAssetName = myDataRecord.GetString(myDataRecord.GetOrdinal("fixed_asset_name"));
            fixedassetcapitalizedcost.mFixedAssetId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("fixed_asset_id"));
            if (myDataRecord["date"] != DBNull.Value)
                fixedassetcapitalizedcost.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            fixedassetcapitalizedcost.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            fixedassetcapitalizedcost.mCapitalizedCostName = myDataRecord.GetString(myDataRecord.GetOrdinal("capitalized_cost_name"));
            fixedassetcapitalizedcost.mCapitalizedCostId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("capitalized_cost_id"));
            fixedassetcapitalizedcost.mDescription = myDataRecord.GetString(myDataRecord.GetOrdinal("description"));
            fixedassetcapitalizedcost.mAmount = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("amount"));
            fixedassetcapitalizedcost.mUsefulLife = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("useful_life"));
            fixedassetcapitalizedcost.mIsJournalized = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("is_journalized"));
            fixedassetcapitalizedcost.mAssetAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("asset_account_name"));
            fixedassetcapitalizedcost.mAssetAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("asset_account_id"));
            fixedassetcapitalizedcost.mCashPayableAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("cash_payable_account_name"));
            fixedassetcapitalizedcost.mCashPayableAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("cash_payable_account_id"));
            fixedassetcapitalizedcost.mTransactionNo = myDataRecord.GetString(myDataRecord.GetOrdinal("transaction_no"));
            return fixedassetcapitalizedcost;
        }
    }
}