using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class UnitDB
    {
        public static Unit GetItem(int unitId)
        {
            Unit unit = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUnitSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", unitId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        unit = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return unit;
        }

        public static UnitCollection GetList(UnitCriteria unitCriteria)
        {
            UnitCollection tempList = new UnitCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUnitSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", unitCriteria.mId);

                if (!string.IsNullOrEmpty(unitCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", unitCriteria.mCode);

                if (!string.IsNullOrEmpty(unitCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", unitCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new UnitCollection();
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

        public static int SelectCountForGetList(UnitCriteria unitCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUnitSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", unitCriteria.mId);

                if (!string.IsNullOrEmpty(unitCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", unitCriteria.mCode);

                if (!string.IsNullOrEmpty(unitCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", unitCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Unit myUnit)
        {
            if (!myUnit.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a unit in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spUnitInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myUnit.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myUnit.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", myUnit.mActive);

                Helpers.SetSaveParameters(myCommand, myUnit);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update unit as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spUnitDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Unit FillDataRecord(IDataRecord myDataRecord)
        {
            Unit unit = new Unit();

            unit.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            unit.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            unit.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            unit.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            return unit;
        }
    }
}