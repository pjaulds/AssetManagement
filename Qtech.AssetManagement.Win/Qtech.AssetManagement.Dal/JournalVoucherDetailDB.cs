using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class JournalVoucherDetailDB
    {
        public static JournalVoucherDetail GetItem(int journalvoucherdetailId)
        {
            JournalVoucherDetail journalvoucherdetail = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherDetailSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", journalvoucherdetailId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        journalvoucherdetail = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return journalvoucherdetail;
        }

        public static JournalVoucherDetailCollection GetList(JournalVoucherDetailCriteria journalvoucherdetailCriteria)
        {
            JournalVoucherDetailCollection tempList = new JournalVoucherDetailCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherDetailSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@journal_voucher_id", journalvoucherdetailCriteria.mJournalVoucherId);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new JournalVoucherDetailCollection();
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

        public static int SelectCountForGetList(JournalVoucherDetailCriteria journalvoucherdetailCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherDetailSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@journal_voucher_id", journalvoucherdetailCriteria.mJournalVoucherId);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(JournalVoucherDetail myJournalVoucherDetail)
        {
            if (!myJournalVoucherDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a journalvoucherdetail in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherDetailInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@journal_voucher_id", myJournalVoucherDetail.mJournalVoucherId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@chart_of_account_id", myJournalVoucherDetail.mChartOfAccountId);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@debit_credit", myJournalVoucherDetail.mDebitCredit);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@amount", myJournalVoucherDetail.mAmount);

                Helpers.SetSaveParameters(myCommand, myJournalVoucherDetail);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update journalvoucherdetail as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spJournalVoucherDetailDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static JournalVoucherDetail FillDataRecord(IDataRecord myDataRecord)
        {
            JournalVoucherDetail journalvoucherdetail = new JournalVoucherDetail();

            journalvoucherdetail.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            journalvoucherdetail.mJournalVoucherId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("journal_voucher_id"));
            journalvoucherdetail.mChartOfAccountCode = myDataRecord.GetString(myDataRecord.GetOrdinal("chart_of_account_code"));
            journalvoucherdetail.mChartOfAccountName = myDataRecord.GetString(myDataRecord.GetOrdinal("chart_of_account_name"));
            journalvoucherdetail.mChartOfAccountId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("chart_of_account_id"));
            journalvoucherdetail.mDebitCredit = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("debit_credit"));
            journalvoucherdetail.mAmount = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("amount"));

            journalvoucherdetail.mDebit = journalvoucherdetail.mDebitCredit ? journalvoucherdetail.mAmount : 0;
            journalvoucherdetail.mCredit = !journalvoucherdetail.mDebitCredit ? journalvoucherdetail.mAmount : 0;

            return journalvoucherdetail;
        }
    }
}