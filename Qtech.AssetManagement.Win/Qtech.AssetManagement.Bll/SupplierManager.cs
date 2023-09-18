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
    public static class SupplierManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static SupplierCollection GetList()
        {
            SupplierCriteria supplier = new SupplierCriteria();
            return GetList(supplier);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static SupplierCollection GetList(SupplierCriteria supplierCriteria)
        {
            return SupplierDB.GetList(supplierCriteria);
        }

        public static int SelectCountForGetList(SupplierCriteria supplierCriteria)
        {
            return SupplierDB.SelectCountForGetList(supplierCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Supplier GetItem(int id)
        {
            Supplier supplier = SupplierDB.GetItem(id);
            return supplier;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Supplier mySupplier)
        {
            if (!mySupplier.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid supplier. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (mySupplier.mId != 0)
                    AuditUpdate(mySupplier);

                int id = SupplierDB.Save(mySupplier);

                if (mySupplier.mId == 0)
                    AuditInsert(mySupplier, id);

                mySupplier.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Supplier mySupplier)
        {
            if (SupplierDB.Delete(mySupplier.mId))
            {
                AuditDelete(mySupplier);
                return mySupplier.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Supplier mySupplier, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = mySupplier.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Supplier;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Supplier mySupplier)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = mySupplier.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Supplier;
            audit.mRowId = mySupplier.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Supplier mySupplier)
        {
            Supplier old_supplier = GetItem(mySupplier.mId);
            AuditCollection audit_collection = SupplierAudit.Audit(mySupplier, old_supplier);
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