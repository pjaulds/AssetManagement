using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class SupplierDB
    {
        public static Supplier GetItem(int supplierId)
        {
            Supplier supplier = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spSupplierSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", supplierId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        supplier = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return supplier;
        }

        public static SupplierCollection GetList(SupplierCriteria supplierCriteria)
        {
            SupplierCollection tempList = new SupplierCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spSupplierSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", supplierCriteria.mId);

                if (!string.IsNullOrEmpty(supplierCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", supplierCriteria.mCode);

                if (!string.IsNullOrEmpty(supplierCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", supplierCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new SupplierCollection();
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

        public static int SelectCountForGetList(SupplierCriteria supplierCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spSupplierSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", supplierCriteria.mId);

                if (!string.IsNullOrEmpty(supplierCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", supplierCriteria.mCode);

                if (!string.IsNullOrEmpty(supplierCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", supplierCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Supplier mySupplier)
        {
            if (!mySupplier.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a supplier in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spSupplierInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", mySupplier.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", mySupplier.mName);
                Helpers.CreateParameter(myCommand, DbType.String, "@address", string.IsNullOrEmpty(mySupplier.mAddress) ? "" : mySupplier.mAddress);
                Helpers.CreateParameter(myCommand, DbType.String, "@tin", string.IsNullOrEmpty(mySupplier.mTin) ? "" : mySupplier.mTin);
                Helpers.CreateParameter(myCommand, DbType.String, "@contact_no", string.IsNullOrEmpty(mySupplier.mContactNo) ? "" : mySupplier.mContactNo);
                Helpers.CreateParameter(myCommand, DbType.String, "@email", string.IsNullOrEmpty(mySupplier.mEmail) ? "" : mySupplier.mEmail);
                Helpers.CreateParameter(myCommand, DbType.String, "@sales_person", string.IsNullOrEmpty(mySupplier.mSalesPerson) ? "" : mySupplier.mSalesPerson);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@vat_registered", mySupplier.mVatRegistered);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@vat_rate", mySupplier.mVatRate);
                Helpers.CreateParameter(myCommand, DbType.Decimal, "@witholding_tax", mySupplier.mWitholdingTax);
                Helpers.CreateParameter(myCommand, DbType.String, "@business_style", string.IsNullOrEmpty(mySupplier.mBusinessStyle) ? "" : mySupplier.mBusinessStyle);
                Helpers.CreateParameter(myCommand, DbType.Boolean, "@active", mySupplier.mActive);

                Helpers.SetSaveParameters(myCommand, mySupplier);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update supplier as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spSupplierDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Supplier FillDataRecord(IDataRecord myDataRecord)
        {
            Supplier supplier = new Supplier();

            supplier.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            supplier.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            supplier.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            supplier.mAddress = myDataRecord.GetString(myDataRecord.GetOrdinal("address"));
            supplier.mTin = myDataRecord.GetString(myDataRecord.GetOrdinal("tin"));
            supplier.mContactNo = myDataRecord.GetString(myDataRecord.GetOrdinal("contact_no"));
            supplier.mEmail = myDataRecord.GetString(myDataRecord.GetOrdinal("email"));
            supplier.mSalesPerson = myDataRecord.GetString(myDataRecord.GetOrdinal("sales_person"));
            supplier.mVatRegistered = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("vat_registered"));
            supplier.mVatRate = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("vat_rate"));
            supplier.mWitholdingTax = myDataRecord.GetDecimal(myDataRecord.GetOrdinal("witholding_tax"));
            supplier.mBusinessStyle = myDataRecord.GetString(myDataRecord.GetOrdinal("business_style"));
            supplier.mActive = myDataRecord.GetBoolean(myDataRecord.GetOrdinal("active"));
            return supplier;
        }
    }
}