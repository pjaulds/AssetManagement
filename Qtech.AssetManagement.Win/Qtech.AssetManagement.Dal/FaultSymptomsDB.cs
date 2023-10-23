using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class FaultSymptomsDB
    {
        public static FaultSymptoms GetItem(int faultSymptomsId)
        {
            FaultSymptoms faultSymptoms = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultSymptomsSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", faultSymptomsId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        faultSymptoms = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return faultSymptoms;
        }

        public static FaultSymptomsCollection GetList(FaultSymptomsCriteria faultSymptomsCriteria)
        {
            FaultSymptomsCollection tempList = new FaultSymptomsCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultSymptomsSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", faultSymptomsCriteria.mId);

                if (!string.IsNullOrEmpty(faultSymptomsCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", faultSymptomsCriteria.mCode);

                if (!string.IsNullOrEmpty(faultSymptomsCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", faultSymptomsCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new FaultSymptomsCollection();
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

        public static int SelectCountForGetList(FaultSymptomsCriteria faultSymptomsCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultSymptomsSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", faultSymptomsCriteria.mId);

                if (!string.IsNullOrEmpty(faultSymptomsCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", faultSymptomsCriteria.mCode);

                if (!string.IsNullOrEmpty(faultSymptomsCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", faultSymptomsCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(FaultSymptoms myFaultSymptoms)
        {
            if (!myFaultSymptoms.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a faultSymptoms in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spFaultSymptomsInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myFaultSymptoms.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myFaultSymptoms.mName);

                Helpers.SetSaveParameters(myCommand, myFaultSymptoms);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update faultSymptoms as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spFaultSymptomsDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static FaultSymptoms FillDataRecord(IDataRecord myDataRecord)
        {
            FaultSymptoms faultSymptoms = new FaultSymptoms();

            faultSymptoms.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            faultSymptoms.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            faultSymptoms.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return faultSymptoms;
        }
    }
}