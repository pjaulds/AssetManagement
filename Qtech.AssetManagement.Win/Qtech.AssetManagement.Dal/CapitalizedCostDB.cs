using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class CapitalizedCostDB
    {
        public static CapitalizedCost GetItem(int capitalizedCostId)
        {
            CapitalizedCost capitalizedCost = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCapitalizedCostSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", capitalizedCostId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        capitalizedCost = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return capitalizedCost;
        }

        public static CapitalizedCostCollection GetList(CapitalizedCostCriteria capitalizedCostCriteria)
        {
            CapitalizedCostCollection tempList = new CapitalizedCostCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCapitalizedCostSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", capitalizedCostCriteria.mId);

                if (!string.IsNullOrEmpty(capitalizedCostCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", capitalizedCostCriteria.mCode);

                if (!string.IsNullOrEmpty(capitalizedCostCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", capitalizedCostCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new CapitalizedCostCollection();
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

        public static int SelectCountForGetList(CapitalizedCostCriteria capitalizedCostCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCapitalizedCostSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", capitalizedCostCriteria.mId);

                if (!string.IsNullOrEmpty(capitalizedCostCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", capitalizedCostCriteria.mCode);

                if (!string.IsNullOrEmpty(capitalizedCostCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", capitalizedCostCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(CapitalizedCost myCapitalizedCost)
        {
            if (!myCapitalizedCost.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a capitalizedCost in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCapitalizedCostInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myCapitalizedCost.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myCapitalizedCost.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@post", myCapitalizedCost.mPost);

                Helpers.SetSaveParameters(myCommand, myCapitalizedCost);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update capitalizedCost as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spCapitalizedCostDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static CapitalizedCost FillDataRecord(IDataRecord myDataRecord)
        {
            CapitalizedCost capitalizedCost = new CapitalizedCost();

            capitalizedCost.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            capitalizedCost.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            capitalizedCost.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            capitalizedCost.mPost = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("post"));
            return capitalizedCost;
        }
    }
}