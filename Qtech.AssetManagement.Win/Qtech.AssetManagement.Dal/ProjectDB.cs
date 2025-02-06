using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class ProjectDB
    {
        public static Project GetItem(int projectId)
        {
            Project project = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProjectSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", projectId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        project = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return project;
        }

        public static ProjectCollection GetList(ProjectCriteria projectCriteria)
        {
            ProjectCollection tempList = new ProjectCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProjectSearchList";


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new ProjectCollection();
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

        public static int SelectCountForGetList(ProjectCriteria projectCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProjectSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);


                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Project myProject)
        {
            if (!myProject.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a project in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProjectInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myProject.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myProject.mName);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", myProject.mActive);

                Helpers.SetSaveParameters(myCommand, myProject);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update project as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spProjectDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Project FillDataRecord(IDataRecord myDataRecord)
        {
            Project project = new Project();

            project.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            project.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            project.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            project.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));

            return project;
        }
    }
}