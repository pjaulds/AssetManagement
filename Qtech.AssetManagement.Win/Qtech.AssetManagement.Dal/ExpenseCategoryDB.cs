using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class ExpenseCategoryDB
    {
        public static ExpenseCategory GetItem(int expenseCategoryId)
        {
            ExpenseCategory expenseCategory = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spExpenseCategorySelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", expenseCategoryId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        expenseCategory = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return expenseCategory;
        }

        public static ExpenseCategoryCollection GetList(ExpenseCategoryCriteria expenseCategoryCriteria)
        {
            ExpenseCategoryCollection tempList = new ExpenseCategoryCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spExpenseCategorySearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", expenseCategoryCriteria.mId);

                if (!string.IsNullOrEmpty(expenseCategoryCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", expenseCategoryCriteria.mCode);

                if (!string.IsNullOrEmpty(expenseCategoryCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", expenseCategoryCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new ExpenseCategoryCollection();
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

        public static int SelectCountForGetList(ExpenseCategoryCriteria expenseCategoryCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spExpenseCategorySearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", expenseCategoryCriteria.mId);

                if (!string.IsNullOrEmpty(expenseCategoryCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", expenseCategoryCriteria.mCode);

                if (!string.IsNullOrEmpty(expenseCategoryCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", expenseCategoryCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(ExpenseCategory myExpenseCategory)
        {
            if (!myExpenseCategory.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a expenseCategory in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spExpenseCategoryInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myExpenseCategory.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myExpenseCategory.mName);

                Helpers.SetSaveParameters(myCommand, myExpenseCategory);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update expenseCategory as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spExpenseCategoryDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static ExpenseCategory FillDataRecord(IDataRecord myDataRecord)
        {
            ExpenseCategory expenseCategory = new ExpenseCategory();

            expenseCategory.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            expenseCategory.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            expenseCategory.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return expenseCategory;
        }
    }
}