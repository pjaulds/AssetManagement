using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class FaultAreaDB
    {
        public static FaultArea GetItem(int faultAreaId)
        {
            FaultArea faultArea = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultAreaSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", faultAreaId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        faultArea = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return faultArea;
        }

        public static FaultAreaCollection GetList(FaultAreaCriteria faultAreaCriteria)
        {
            FaultAreaCollection tempList = new FaultAreaCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultAreaSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", faultAreaCriteria.mId);

                if (!string.IsNullOrEmpty(faultAreaCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", faultAreaCriteria.mCode);

                if (!string.IsNullOrEmpty(faultAreaCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", faultAreaCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new FaultAreaCollection();
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

        public static int SelectCountForGetList(FaultAreaCriteria faultAreaCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultAreaSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", faultAreaCriteria.mId);

                if (!string.IsNullOrEmpty(faultAreaCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", faultAreaCriteria.mCode);

                if (!string.IsNullOrEmpty(faultAreaCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", faultAreaCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(FaultArea myFaultArea)
        {
            if (!myFaultArea.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a faultArea in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultAreaInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myFaultArea.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myFaultArea.mName);

                Helpers.SetSaveParameters(myCommand, myFaultArea);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update faultArea as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spFaultAreaDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static FaultArea FillDataRecord(IDataRecord myDataRecord)
        {
            FaultArea faultArea = new FaultArea();

            faultArea.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            faultArea.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            faultArea.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return faultArea;
        }
    }
}