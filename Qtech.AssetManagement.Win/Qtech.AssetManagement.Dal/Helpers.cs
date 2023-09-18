using System.Data.Common;
using Qtech.AssetManagement.BusinessEntities;
using System.Data;
using System;
using System.Collections.Generic;

namespace Qtech.AssetManagement.Dal
{
    internal class Helpers
    {
        const string idParamName = "@id";

        internal static void SetSaveParameters(DbCommand command, BusinessBase businessBase)
        {
            DbParameter idParam = command.CreateParameter();
            idParam.DbType = DbType.Int32;
            idParam.Direction = ParameterDirection.InputOutput;
            idParam.ParameterName = idParamName;
            if (businessBase.mId == 0)
            {
                idParam.Value = DBNull.Value;
            }
            else
            {
                idParam.Value = businessBase.mId;
            }
            command.Parameters.Add(idParam);
        }
        

        internal static int GetBusinessBaseId(DbCommand command)
        {
            string x = command.Parameters[idParamName].Value.ToString();
            return (int)command.Parameters[idParamName].Value;
        }

        internal static void CreateParameter(DbCommand command, DbType type, string paramName, object value)
        {
            //parameter for subdivision name
            DbParameter name_param = command.CreateParameter();
            name_param.DbType = type;
            name_param.ParameterName = paramName;
            name_param.Value = value;
            command.Parameters.Add(name_param);
        }

        internal static DbDataReader ExecuteReader(DbCommand myCommand)
        {
            return myCommand.ExecuteReader();
        }

        internal static int ExecuteNonQuery(DbCommand myCommand)
        {
            return myCommand.ExecuteNonQuery();
        }

        public static string FilterString(string value)
        {
            if (value.IndexOf("%", 0) == -1)
            {
                if (value == string.Empty)
                    value = string.Format("%{0}%", value);
                else
                    value = string.Format("{0}%", value);
            }
            return value;
        }
    }

    public static class FieldNames
    {
        public static Dictionary<string, int> GetAllNames(this IDataRecord record)
        {
            var result = new Dictionary<string, int>();
            for (int i = 0; i < record.FieldCount; i++)
            {
                result.Add(record.GetName(i), i);
            }
            return result;
        }
    }
}
