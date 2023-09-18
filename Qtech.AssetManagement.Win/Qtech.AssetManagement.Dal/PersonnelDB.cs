using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class PersonnelDB
    {
        public static Personnel GetItem(int personnelId)
        {
            Personnel personnel = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPersonnelSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", personnelId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        personnel = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return personnel;
        }

        public static PersonnelCollection GetList(PersonnelCriteria personnelCriteria)
        {
            PersonnelCollection tempList = new PersonnelCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPersonnelSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", personnelCriteria.mId);

                if (!string.IsNullOrEmpty(personnelCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", personnelCriteria.mCode);

                if (!string.IsNullOrEmpty(personnelCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", personnelCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PersonnelCollection();
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

        public static int SelectCountForGetList(PersonnelCriteria personnelCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPersonnelSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", personnelCriteria.mId);

                if (!string.IsNullOrEmpty(personnelCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", personnelCriteria.mCode);

                if (!string.IsNullOrEmpty(personnelCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", personnelCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Personnel myPersonnel)
        {
            if (!myPersonnel.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a personnel in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spPersonnelInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myPersonnel.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myPersonnel.mName);

                Helpers.SetSaveParameters(myCommand, myPersonnel);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update personnel as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spPersonnelDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Personnel FillDataRecord(IDataRecord myDataRecord)
        {
            Personnel personnel = new Personnel();

            personnel.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            personnel.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            personnel.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return personnel;
        }
    }
}