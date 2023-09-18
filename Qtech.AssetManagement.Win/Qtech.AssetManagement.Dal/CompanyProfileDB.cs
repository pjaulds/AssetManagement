using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class CompanyProfileDB
    {
        public static CompanyProfile GetItem(int companyprofileId)
        {
            CompanyProfile companyprofile = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCompanyProfileSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", companyprofileId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        companyprofile = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return companyprofile;
        }

        public static CompanyProfileCollection GetList(CompanyProfileCriteria companyprofileCriteria)
        {
            CompanyProfileCollection tempList = new CompanyProfileCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCompanyProfileSearchList";


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new CompanyProfileCollection();
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

        public static int SelectCountForGetList(CompanyProfileCriteria companyprofileCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCompanyProfileSearchList";
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
        public static int Save(CompanyProfile myCompanyProfile)
        {
            if (!myCompanyProfile.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a companyprofile in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spCompanyProfileInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@name", myCompanyProfile.mName);
                Helpers.CreateParameter(myCommand, DbType.String, "@address", myCompanyProfile.mAddress);
                Helpers.CreateParameter(myCommand, DbType.Binary, "@report_logo", myCompanyProfile.mReportLogo);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@width", myCompanyProfile.mWidth);
                Helpers.CreateParameter(myCommand, DbType.Int32, "@height", myCompanyProfile.mHeight);

                Helpers.SetSaveParameters(myCommand, myCompanyProfile);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update companyprofile as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spCompanyProfileDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static CompanyProfile FillDataRecord(IDataRecord myDataRecord)
        {
            CompanyProfile companyprofile = new CompanyProfile();

            companyprofile.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            companyprofile.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            companyprofile.mAddress = myDataRecord.GetString(myDataRecord.GetOrdinal("address"));
            companyprofile.mReportLogo = (byte[]) myDataRecord.GetValue(myDataRecord.GetOrdinal("report_logo"));
            companyprofile.mWidth = myDataRecord.GetInt32(myDataRecord.GetOrdinal("width"));
            companyprofile.mHeight = myDataRecord.GetInt32(myDataRecord.GetOrdinal("height"));
            return companyprofile;
        }
    }
}