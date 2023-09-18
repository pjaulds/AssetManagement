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
    public static class DepreciationMethodManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DepreciationMethodCollection GetList()
        {
            DepreciationMethodCriteria depreciationmethod = new DepreciationMethodCriteria();
            return GetList(depreciationmethod);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DepreciationMethodCollection GetList(DepreciationMethodCriteria depreciationmethodCriteria)
        {
            return DepreciationMethodDB.GetList(depreciationmethodCriteria);
        }

        public static int SelectCountForGetList(DepreciationMethodCriteria depreciationmethodCriteria)
        {
            return DepreciationMethodDB.SelectCountForGetList(depreciationmethodCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DepreciationMethod GetItem(int id)
        {
            DepreciationMethod depreciationmethod = DepreciationMethodDB.GetItem(id);
            return depreciationmethod;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(DepreciationMethod myDepreciationMethod)
        {
            if (!myDepreciationMethod.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid depreciationmethod. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myDepreciationMethod.mId != 0)
                    AuditUpdate(myDepreciationMethod);

                int id = DepreciationMethodDB.Save(myDepreciationMethod);

                if (myDepreciationMethod.mId == 0)
                    AuditInsert(myDepreciationMethod, id);

                myDepreciationMethod.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(DepreciationMethod myDepreciationMethod)
        {
            if (DepreciationMethodDB.Delete(myDepreciationMethod.mId))
            {
                AuditDelete(myDepreciationMethod);
                return myDepreciationMethod.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(DepreciationMethod myDepreciationMethod, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDepreciationMethod.mUserId;
            audit.mTableId = (Int16)Tables.amQt_DepreciationMethod;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(DepreciationMethod myDepreciationMethod)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDepreciationMethod.mUserId;
            audit.mTableId = (Int16)Tables.amQt_DepreciationMethod;
            audit.mRowId = myDepreciationMethod.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(DepreciationMethod myDepreciationMethod)
        {
            DepreciationMethod old_depreciationmethod = GetItem(myDepreciationMethod.mId);
            AuditCollection audit_collection = DepreciationMethodAudit.Audit(myDepreciationMethod, old_depreciationmethod);
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