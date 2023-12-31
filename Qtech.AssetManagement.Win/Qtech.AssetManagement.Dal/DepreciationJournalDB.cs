﻿using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class DepreciationJournalDB
    {
        public static DepreciationJournal GetItem(int depreciationjournalId)
        {
            DepreciationJournal depreciationjournal = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationJournalSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", depreciationjournalId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        depreciationjournal = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return depreciationjournal;
        }

        public static DepreciationJournalCollection GetList(DepreciationJournalCriteria depreciationjournalCriteria)
        {
            DepreciationJournalCollection tempList = new DepreciationJournalCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationJournalSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", depreciationjournalCriteria.mFixedAssetId);
                Helpers.CreateParameter(myCommand, DbType.Int16, "@year", depreciationjournalCriteria.mYear);
                Helpers.CreateParameter(myCommand, DbType.Byte, "@month", depreciationjournalCriteria.mMonth);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new DepreciationJournalCollection();
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

        public static int SelectCountForGetList(DepreciationJournalCriteria depreciationjournalCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationJournalSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", depreciationjournalCriteria.mFixedAssetId);
                Helpers.CreateParameter(myCommand, DbType.Int16, "@year", depreciationjournalCriteria.mYear);
                Helpers.CreateParameter(myCommand, DbType.Byte, "@month", depreciationjournalCriteria.mMonth);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(DepreciationJournal myDepreciationJournal)
        {
            if (!myDepreciationJournal.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a depreciationjournal in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationJournalInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@fixed_asset_id", myDepreciationJournal.mFixedAssetId);
                Helpers.CreateParameter(myCommand, DbType.Int16, "@year", myDepreciationJournal.mYear);
                Helpers.CreateParameter(myCommand, DbType.Byte, "@month", myDepreciationJournal.mMonth);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@depreciation_expense_account_id", myDepreciationJournal.mDepreciationExpenseAccountId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@depreciation_expense_account_debit_credit", myDepreciationJournal.mDepreciationExpenseAccountDebitCredit);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@accumulated_depreciation_account_id", myDepreciationJournal.mAccumulatedDepreciationAccountId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@accumulated_depreciation_account_debit_credit", myDepreciationJournal.mAccumulatedDepreciationAccountDebitCredit);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@amount", myDepreciationJournal.mAmount);
                Helpers.CreateParameter(myCommand, DbType.String, "@description", myDepreciationJournal.mDescription);

                Helpers.SetSaveParameters(myCommand, myDepreciationJournal);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update depreciationjournal as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spDepreciationJournalDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static DepreciationJournal FillDataRecord(IDataRecord myDataRecord)
        {
            DepreciationJournal depreciationjournal = new DepreciationJournal();

            depreciationjournal.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            depreciationjournal.mFixedAssetId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("fixed_asset_id"));
            depreciationjournal.mYear = myDataRecord.GetInt16(myDataRecord.GetOrdinal("year"));
            depreciationjournal.mMonth = myDataRecord.GetByte(myDataRecord.GetOrdinal("month"));
            depreciationjournal.mDepreciationExpenseAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("depreciation_expense_account_name"));
            depreciationjournal.mDepreciationExpenseAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("depreciation_expense_account_id"));
            depreciationjournal.mDepreciationExpenseAccountDebitCredit = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("depreciation_expense_account_debit_credit"));
            depreciationjournal.mAccumulatedDepreciationAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("accumulated_depreciation_account_name"));
            depreciationjournal.mAccumulatedDepreciationAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("accumulated_depreciation_account_id"));
            depreciationjournal.mAccumulatedDepreciationAccountDebitCredit = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("accumulated_depreciation_account_debit_credit"));
            depreciationjournal.mAmount = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("amount"));
            depreciationjournal.mDescription = myDataRecord.GetString(myDataRecord.GetOrdinal("description"));

            return depreciationjournal;
        }
    }
}