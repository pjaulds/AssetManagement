using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class FixedAssetSettingDateDB
    {
        public static FixedAssetSettingDate GetItem(int fixedassetsettingdateId)
        {
            FixedAssetSettingDate fixedassetsettingdate = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingDateSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", fixedassetsettingdateId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        fixedassetsettingdate = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return fixedassetsettingdate;
        }

        public static FixedAssetSettingDateCollection GetList(FixedAssetSettingDateCriteria fixedassetsettingdateCriteria)
        {
            FixedAssetSettingDateCollection tempList = new FixedAssetSettingDateCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingDateSearchList";


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new FixedAssetSettingDateCollection();
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

        public static int SelectCountForGetList(FixedAssetSettingDateCriteria fixedassetsettingdateCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingDateSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);


                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(FixedAssetSettingDate myFixedAssetSettingDate)
        {
            if (!myFixedAssetSettingDate.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a fixedassetsettingdate in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFixedAssetSettingDateInsertUpdateSingleItem";

                if (myFixedAssetSettingDate.mDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@date", myFixedAssetSettingDate.mDate);

                Helpers.SetSaveParameters(myCommand, myFixedAssetSettingDate);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update fixedassetsettingdate as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spFixedAssetSettingDateDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static FixedAssetSettingDate FillDataRecord(IDataRecord myDataRecord)
        {
            FixedAssetSettingDate fixedassetsettingdate = new FixedAssetSettingDate();

            fixedassetsettingdate.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            if (myDataRecord["date"] != DBNull.Value)
                fixedassetsettingdate.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            return fixedassetsettingdate;
        }
    }
}
