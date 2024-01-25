using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class FunctionalLocationDB
    {
        public static FunctionalLocation GetItem(int functionallocationId)
        {
            FunctionalLocation functionallocation = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFunctionalLocationSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", functionallocationId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        functionallocation = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return functionallocation;
        }

        public static FunctionalLocationCollection GetList(FunctionalLocationCriteria functionallocationCriteria)
        {
            FunctionalLocationCollection tempList = new FunctionalLocationCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFunctionalLocationSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", functionallocationCriteria.mId);

                if (!string.IsNullOrEmpty(functionallocationCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", functionallocationCriteria.mCode);

                if (!string.IsNullOrEmpty(functionallocationCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", functionallocationCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new FunctionalLocationCollection();
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

        public static int SelectCountForGetList(FunctionalLocationCriteria functionallocationCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFunctionalLocationSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", functionallocationCriteria.mId);

                if (!string.IsNullOrEmpty(functionallocationCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", functionallocationCriteria.mCode);

                if (!string.IsNullOrEmpty(functionallocationCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", functionallocationCriteria.mName);


                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(FunctionalLocation myFunctionalLocation)
        {
            if (!myFunctionalLocation.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a functionallocation in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFunctionalLocationInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myFunctionalLocation.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myFunctionalLocation.mName);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@parent_fl_id", myFunctionalLocation.mParentFlId);
                Helpers.CreateParameter(myCommand, DbType.String, "@fl_status", myFunctionalLocation.mFlStatus);
                Helpers.CreateParameter(myCommand, DbType.String, "@address_name", myFunctionalLocation.mAddressName);
                Helpers.CreateParameter(myCommand, DbType.String, "@street", myFunctionalLocation.mStreet);
                Helpers.CreateParameter(myCommand, DbType.String, "@city", myFunctionalLocation.mCity);
                Helpers.CreateParameter(myCommand, DbType.String, "@province", myFunctionalLocation.mProvince);
                Helpers.CreateParameter(myCommand, DbType.String, "@country", myFunctionalLocation.mCountry);
                Helpers.CreateParameter(myCommand, DbType.String, "@zip_code", myFunctionalLocation.mZipCode);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", myFunctionalLocation.mActive);

                Helpers.SetSaveParameters(myCommand, myFunctionalLocation);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update functionallocation as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spFunctionalLocationDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static FunctionalLocation FillDataRecord(IDataRecord myDataRecord)
        {
            FunctionalLocation functionallocation = new FunctionalLocation();

            functionallocation.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            functionallocation.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            functionallocation.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            functionallocation.mParentFlName = myDataRecord.GetString(myDataRecord.GetOrdinal("parent_fl_name"));
            functionallocation.mParentFlId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("parent_fl_id"));
            functionallocation.mFlStatus = myDataRecord.GetString(myDataRecord.GetOrdinal("fl_status"));
            functionallocation.mAddressName = myDataRecord.GetString(myDataRecord.GetOrdinal("address_name"));
            functionallocation.mStreet = myDataRecord.GetString(myDataRecord.GetOrdinal("street"));
            functionallocation.mCity = myDataRecord.GetString(myDataRecord.GetOrdinal("city"));
            functionallocation.mProvince = myDataRecord.GetString(myDataRecord.GetOrdinal("province"));
            functionallocation.mCountry = myDataRecord.GetString(myDataRecord.GetOrdinal("country"));
            functionallocation.mZipCode = myDataRecord.GetString(myDataRecord.GetOrdinal("zip_code"));
            functionallocation.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            return functionallocation;
        }
    }
}
