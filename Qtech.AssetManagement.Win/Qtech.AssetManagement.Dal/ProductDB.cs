using System;
using System.Collections.Generic;
using Qtech.AssetManagement.BusinessEntities;
using System.Data.Common;
using Qtech.AssetManagement.Validation;
using System.Data;

namespace Qtech.AssetManagement.Dal
{
    public class ProductDB
    {
        public static Product GetItem(int productId)
        {
            Product product = null;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProductSelectSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", productId);


                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        product = FillDataRecord(myReader);

                    }
                    myReader.Close();
                }
                myCommand.Connection.Close();
            }

            return product;
        }

        public static ProductCollection GetList(ProductCriteria productCriteria)
        {
            ProductCollection tempList = new ProductCollection();

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProductSearchList";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", productCriteria.mId);

                if (!string.IsNullOrEmpty(productCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", productCriteria.mCode);

                if (!string.IsNullOrEmpty(productCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", productCriteria.mName);

                myCommand.Connection.Open();
                using (DbDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new ProductCollection();
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

        public static int SelectCountForGetList(ProductCriteria productCriteria)
        {
            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProductSearchList";
                DbParameter idParam = myCommand.CreateParameter();
                idParam.DbType = DbType.Int32;
                idParam.Direction = ParameterDirection.InputOutput;
                idParam.ParameterName = "@record_count";
                idParam.Value = 0;
                myCommand.Parameters.Add(idParam);

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", productCriteria.mId);

                if (!string.IsNullOrEmpty(productCriteria.mCode))
                    Helpers.CreateParameter(myCommand, DbType.String, "@code", productCriteria.mCode);

                if (!string.IsNullOrEmpty(productCriteria.mName))
                    Helpers.CreateParameter(myCommand, DbType.String, "@name", productCriteria.mName);

                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
                return (int)myCommand.Parameters["@record_count"].Value;
            }
        }
        public static int Save(Product myProduct)
        {
            if (!myProduct.Validate())
            {
                throw new InvalidSaveOperationException("Can't save a product in an Invalid state. Make sure that IsValid() returns true before you call Save().");
            }
            int result = 0;

            using (DbCommand myCommand = AppConfiguration.CreateCommand())
            {
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "amQt_spProductInsertUpdateSingleItem";

                Helpers.CreateParameter(myCommand, DbType.String, "@code", myProduct.mCode);
                Helpers.CreateParameter(myCommand, DbType.String, "@name", myProduct.mName);

                Helpers.SetSaveParameters(myCommand, myProduct);

                myCommand.Connection.Open();

                int numberOfRecordsAffected = myCommand.ExecuteNonQuery();
                if (numberOfRecordsAffected == 0)
                {
                    throw new DBConcurrencyException("Can't update product as it has been updated by someone else");
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
                myCommand.CommandType = CommandType.StoredProcedure; myCommand.CommandText = "amQt_spProductDeleteSingleItem";

                Helpers.CreateParameter(myCommand, DbType.Int32, "@id", id);

                myCommand.Connection.Open();

                result = myCommand.ExecuteNonQuery();

                myCommand.Connection.Close();

            }
            return result > 0;
        }

        private static Product FillDataRecord(IDataRecord myDataRecord)
        {
            Product product = new Product();

            product.mId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("id"));
            product.mCode = myDataRecord.GetString(myDataRecord.GetOrdinal("code"));
            product.mName = myDataRecord.GetString(myDataRecord.GetOrdinal("name"));
            return product;
        }
    }
}