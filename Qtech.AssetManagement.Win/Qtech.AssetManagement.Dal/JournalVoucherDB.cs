using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class JournalVoucherDB
    {
        public static JournalVoucher GetItem(int journalvoucherId)
        {
            JournalVoucher journalvoucher = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", journalvoucherId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        journalvoucher = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return journalvoucher;
        }

        public static JournalVoucherCollection GetList(JournalVoucherCriteria journalvoucherCriteria)
        {
            JournalVoucherCollection tempList = new JournalVoucherCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherSearchList";

                if (journalvoucherCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", journalvoucherCriteria.mStartDate);

                if (journalvoucherCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", journalvoucherCriteria.mEndDate);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new JournalVoucherCollection();
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

        public static int SelectCountForGetList(JournalVoucherCriteria journalvoucherCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                if (journalvoucherCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", journalvoucherCriteria.mStartDate);

                if (journalvoucherCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", journalvoucherCriteria.mEndDate);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(JournalVoucher myJournalVoucher)
        {
            if (!myJournalVoucher.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a journalvoucher in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spJournalVoucherInsertUpdateSingleItem";

                if (myJournalVoucher.mDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date", myJournalVoucher.mDate);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@supplier_id", myJournalVoucher.mSupplierId);
                Helpers.CreateParameter(myCommand, DbType.String, "@type", myJournalVoucher.mType);
                Helpers.CreateParameter(myCommand, DbType.String, "@details", myJournalVoucher.mDetails);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myJournalVoucher.mPost);

                Helpers.SetSaveParameters(myCommand, myJournalVoucher);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update journalvoucher as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spJournalVoucherDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static JournalVoucher FillDataRecord(IDataRecord myDataRecord)
        {
            JournalVoucher journalvoucher = new JournalVoucher();

            journalvoucher.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                journalvoucher.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            journalvoucher.mNumber = myDataRecord.GetInt32(myDataRecord.GetOrdinal("number"));
            journalvoucher.mSupplierName = myDataRecord.GetString(myDataRecord.GetOrdinal("supplier_name"));
            journalvoucher.mSupplierId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("supplier_id"));
            journalvoucher.mType = myDataRecord.GetString(myDataRecord.GetOrdinal("type"));
            journalvoucher.mDetails = myDataRecord.GetString(myDataRecord.GetOrdinal("details"));
            journalvoucher.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            journalvoucher.mTransactionNo = myDataRecord.GetString(myDataRecord.GetOrdinal("transaction_no"));
            return journalvoucher;
        }
    }
}