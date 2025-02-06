using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class AveragingMethodDB
    {
        public static AveragingMethod GetItem(int averagingmethodId)
        {
            AveragingMethod averagingmethod = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAveragingMethodSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", averagingmethodId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        averagingmethod = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return averagingmethod;
        }

        public static AveragingMethodCollection GetList(AveragingMethodCriteria averagingmethodCriteria)
        {
            AveragingMethodCollection tempList = new AveragingMethodCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAveragingMethodSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", averagingmethodCriteria.mId);

                if (!string.IsNullOrEmpty(averagingmethodCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", averagingmethodCriteria.mCode);

                if (!string.IsNullOrEmpty(averagingmethodCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", averagingmethodCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AveragingMethodCollection();
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

        public static int SelectCountForGetList(AveragingMethodCriteria averagingmethodCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAveragingMethodSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", averagingmethodCriteria.mId);

                if (!string.IsNullOrEmpty(averagingmethodCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", averagingmethodCriteria.mCode);

                if (!string.IsNullOrEmpty(averagingmethodCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", averagingmethodCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(AveragingMethod myAveragingMethod)
        {
            if (!myAveragingMethod.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a averagingmethod in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spAveragingMethodInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myAveragingMethod.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myAveragingMethod.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", myAveragingMethod.mActive);
                Helpers.CreateParameter(myCommand, DbType.String, "@remarks", myAveragingMethod.mRemarks);

                Helpers.SetSaveParameters(myCommand, myAveragingMethod);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update averagingmethod as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spAveragingMethodDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static AveragingMethod FillDataRecord(IDataRecord myDataRecord)
        {
            AveragingMethod averagingmethod = new AveragingMethod();

            averagingmethod.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            averagingmethod.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            averagingmethod.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            averagingmethod.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            averagingmethod.mRemarks = myDataRecord.GetString(myDataRecord.GetOrdinal("remarks"));
            return averagingmethod;
        }
    }
}