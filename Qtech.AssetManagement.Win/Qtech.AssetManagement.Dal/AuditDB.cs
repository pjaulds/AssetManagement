using Qtech.AssetManagement.BusinessEntities;
using System.Data;
using System.Data.Common;
using System;
namespace Qtech.AssetManagement.Dal
{
    public class AuditDB
    {
        public static string GetNewNumber(string tableName)
        {
            string value = "";
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT ISNULL(MAX(CAST(number AS INT)), 0) + 1 FROM " + tableName + " WHERE disable = 0";

                myCommand.Connection.Open();
                value = Convert.ToString(myCommand.ExecuteScalar());
                myCommand.Connection.Close();
            }
            return value;
        }

        public static DateTime GetDateToday()
        {
            DateTime value = DateTime.Now.Date;
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT GETDATE()";

                myCommand.Connection.Open();
                value = Convert.ToDateTime(myCommand.ExecuteScalar());
                myCommand.Connection.Close();
            }
            return value;
        }

        public static AuditCollection GetList(AuditCriteria auditCriteria)
        {
            AuditCollection tempList = new AuditCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommandLog())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "lgQt_spAuditSearchList";

                if (auditCriteria.mStartDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@start_date", auditCriteria.mStartDate);

                if (auditCriteria.mEndDate != DateTime.MinValue)
                    Helpers.CreateParameter(myCommand, DbType.DateTime, "@end_date", auditCriteria.mEndDate);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new AuditCollection();
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

       
        public static void Save(BusinessEntities.Audit audit)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommandLog())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spLogInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@user_id", audit.mUserId);
                Helpers.CreateParameter(myCommand, DbType.Int16, "@table_id", audit.mTableId);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@row_id", audit.mRowId);
                Helpers.CreateParameter(myCommand, DbType.Byte, "@action_id", audit.mActionId);
                Helpers.CreateParameter(myCommand, DbType.String, "@field", string.IsNullOrEmpty(audit.mField) ? "" : audit.mField);
                Helpers.CreateParameter(myCommand, DbType.String, "@old_value", string.IsNullOrEmpty(audit.mOldValue) ? "" : audit.mOldValue);
                Helpers.CreateParameter(myCommand, DbType.String, "@new_value", string.IsNullOrEmpty(audit.mNewValue) ? "" : audit.mNewValue);

                myCommand.Connection.Open();

                myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();
            }

        }

        private static BusinessEntities.Audit FillDataRecord(IDataRecord myDataRecord)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            audit.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            audit.mUserFullName = myDataRecord.GetString(myDataRecord.GetOrdinal("user_name"));
            audit.mTableId = myDataRecord.GetInt16(myDataRecord.GetOrdinal("table_id"));
            audit.mRowId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("row_id"));
            audit.mActionId = myDataRecord.GetByte(myDataRecord.GetOrdinal("action_id"));
            audit.mField = myDataRecord.GetString(myDataRecord.GetOrdinal("field"));
            audit.mOldValue = myDataRecord.GetString(myDataRecord.GetOrdinal("old_value"));
            audit.mNewValue = myDataRecord.GetString(myDataRecord.GetOrdinal("new_value"));
            audit.mDate = myDataRecord.GetDateTime(myDataRecord.GetOrdinal("date"));
            
            return audit;
        }

        public static void BackUpDatabase(string script, string path)
        {

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = script;

                Helpers.CreateParameter(myCommand, DbType.String, "@path", path);
                Helpers.CreateParameter(myCommand, DbType.String, "@path2", path.Replace(".bak","log.bak"));

                myCommand.Connection.Open();

                myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();
            }

        }
    }
}
