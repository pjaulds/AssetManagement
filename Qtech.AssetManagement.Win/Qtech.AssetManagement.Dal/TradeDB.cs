using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class TradeDB
    {
        public static Trade GetItem(int tradeId)
        {
            Trade trade = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spTradeSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", tradeId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        trade = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return trade;
        }

        public static TradeCollection GetList(TradeCriteria tradeCriteria)
        {
            TradeCollection tempList = new TradeCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spTradeSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", tradeCriteria.mId);

                if (!string.IsNullOrEmpty(tradeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", tradeCriteria.mCode);

                if (!string.IsNullOrEmpty(tradeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", tradeCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new TradeCollection();
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

        public static int SelectCountForGetList(TradeCriteria tradeCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spTradeSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", tradeCriteria.mId);

                if (!string.IsNullOrEmpty(tradeCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", tradeCriteria.mCode);

                if (!string.IsNullOrEmpty(tradeCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", tradeCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Trade myTrade)
        {
            if (!myTrade.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a trade in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spTradeInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myTrade.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myTrade.mName);

                Helpers.SetSaveParameters(myCommand, myTrade);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update trade as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spTradeDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Trade FillDataRecord(IDataRecord myDataRecord)
        {
            Trade trade = new Trade();

            trade.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            trade.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            trade.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return trade;
        }
    }
}