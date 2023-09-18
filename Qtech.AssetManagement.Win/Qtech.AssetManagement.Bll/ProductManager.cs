using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Dal;
using Qtech.AssetManagement.Validation;
using System.ComponentModel;

using Qtech.AssetManagement.Audit;

namespace Qtech.AssetManagement.Bll
{
    [DataObjectAttribute()]
    public static class ProductManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ProductCollection GetList()
        {
            ProductCriteria product = new ProductCriteria();
            return GetList(product);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ProductCollection GetList(ProductCriteria productCriteria)
        {
            return ProductDB.GetList(productCriteria);
        }

        public static int SelectCountForGetList(ProductCriteria productCriteria)
        {
            return ProductDB.SelectCountForGetList(productCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Product GetItem(int id)
        {
            Product product = ProductDB.GetItem(id);
            return product;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Product myProduct)
        {
            if (!myProduct.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid product. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myProduct.mId != 0)
                    AuditUpdate(myProduct);

                int id = ProductDB.Save(myProduct);

                if (myProduct.mId == 0)
                    AuditInsert(myProduct, id);

                myProduct.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Product myProduct)
        {
            if (ProductDB.Delete(myProduct.mId))
            {
                AuditDelete(myProduct);
                return myProduct.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Product myProduct, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myProduct.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Product;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Product myProduct)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myProduct.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Product;
            audit.mRowId = myProduct.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Product myProduct)
        {
            Product old_product = GetItem(myProduct.mId);
            AuditCollection audit_collection = ProductAudit.Audit(myProduct, old_product);
            if (audit_collection != null)
            {
                foreach (BusinessEntities.Audit audit in audit_collection)
                {
                    AuditManager.Save(audit);
                }
            }
        }
        #endregion
    }
}