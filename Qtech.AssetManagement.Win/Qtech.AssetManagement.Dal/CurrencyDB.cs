﻿using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class CurrencyDB
    {
        public static Currency GetItem(int currencyId)
        {
            Currency currency = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCurrencySelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", currencyId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        currency = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return currency;
        }

        public static CurrencyCollection GetList(CurrencyCriteria currencyCriteria)
        {
            CurrencyCollection tempList = new CurrencyCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCurrencySearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", currencyCriteria.mId);

                if (!string.IsNullOrEmpty(currencyCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", currencyCriteria.mCode);

                if (!string.IsNullOrEmpty(currencyCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", currencyCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new CurrencyCollection();
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

        public static int SelectCountForGetList(CurrencyCriteria currencyCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCurrencySearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", currencyCriteria.mId);

                if (!string.IsNullOrEmpty(currencyCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", currencyCriteria.mCode);

                if (!string.IsNullOrEmpty(currencyCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", currencyCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Currency myCurrency)
        {
            if (!myCurrency.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a currency in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCurrencyInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myCurrency.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myCurrency.mName);

                Helpers.SetSaveParameters(myCommand, myCurrency);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update currency as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spCurrencyDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Currency FillDataRecord(IDataRecord myDataRecord)
        {
            Currency currency = new Currency();

            currency.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            currency.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            currency.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return currency;
        }
    }
}