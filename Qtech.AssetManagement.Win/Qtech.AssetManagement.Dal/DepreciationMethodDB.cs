using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class DepreciationMethodDB
    {
        public static DepreciationMethod GetItem(int depreciationmethodId)
        {
            DepreciationMethod depreciationmethod = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationMethodSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", depreciationmethodId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        depreciationmethod = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return depreciationmethod;
        }

        public static DepreciationMethodCollection GetList(DepreciationMethodCriteria depreciationmethodCriteria)
        {
            DepreciationMethodCollection tempList = new DepreciationMethodCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationMethodSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", depreciationmethodCriteria.mId);

                if (!string.IsNullOrEmpty(depreciationmethodCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", depreciationmethodCriteria.mCode);

                if (!string.IsNullOrEmpty(depreciationmethodCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", depreciationmethodCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new DepreciationMethodCollection();
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

        public static int SelectCountForGetList(DepreciationMethodCriteria depreciationmethodCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationMethodSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", depreciationmethodCriteria.mId);

                if (!string.IsNullOrEmpty(depreciationmethodCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", depreciationmethodCriteria.mCode);

                if (!string.IsNullOrEmpty(depreciationmethodCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", depreciationmethodCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(DepreciationMethod myDepreciationMethod)
        {
            if (!myDepreciationMethod.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a depreciationmethod in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spDepreciationMethodInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myDepreciationMethod.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myDepreciationMethod.mName);
                Helpers.CreateParameter(myCommand, DbType.String, "@active", myDepreciationMethod.mActive);
                Helpers.CreateParameter(myCommand, DbType.String, "@remarks", myDepreciationMethod.mRemarks);

                Helpers.SetSaveParameters(myCommand, myDepreciationMethod);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update depreciationmethod as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spDepreciationMethodDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static DepreciationMethod FillDataRecord(IDataRecord myDataRecord)
        {
            DepreciationMethod depreciationmethod = new DepreciationMethod();

            depreciationmethod.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            depreciationmethod.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            depreciationmethod.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            depreciationmethod.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            depreciationmethod.mRemarks = myDataRecord.GetString(myDataRecord.GetOrdinal("remarks"));
            return depreciationmethod;
        }
    }
}
