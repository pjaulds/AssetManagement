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
    public static class DepreciationExpenseAccountManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DepreciationExpenseAccountCollection GetList()
        {
            DepreciationExpenseAccountCriteria depreciationexpenseaccount = new DepreciationExpenseAccountCriteria();
            return GetList(depreciationexpenseaccount);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DepreciationExpenseAccountCollection GetList(DepreciationExpenseAccountCriteria depreciationexpenseaccountCriteria)
        {
            return DepreciationExpenseAccountDB.GetList(depreciationexpenseaccountCriteria);
        }

        public static int SelectCountForGetList(DepreciationExpenseAccountCriteria depreciationexpenseaccountCriteria)
        {
            return DepreciationExpenseAccountDB.SelectCountForGetList(depreciationexpenseaccountCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DepreciationExpenseAccount GetItem(int id)
        {
            DepreciationExpenseAccount depreciationexpenseaccount = DepreciationExpenseAccountDB.GetItem(id);
            return depreciationexpenseaccount;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(DepreciationExpenseAccount myDepreciationExpenseAccount)
        {
            if (!myDepreciationExpenseAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid depreciationexpenseaccount. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myDepreciationExpenseAccount.mId != 0)
                    AuditUpdate(myDepreciationExpenseAccount);

                int id = DepreciationExpenseAccountDB.Save(myDepreciationExpenseAccount);

                if (myDepreciationExpenseAccount.mId == 0)
                    AuditInsert(myDepreciationExpenseAccount, id);

                myDepreciationExpenseAccount.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(DepreciationExpenseAccount myDepreciationExpenseAccount)
        {
            if (DepreciationExpenseAccountDB.Delete(myDepreciationExpenseAccount.mId))
            {
                AuditDelete(myDepreciationExpenseAccount);
                return myDepreciationExpenseAccount.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(DepreciationExpenseAccount myDepreciationExpenseAccount, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDepreciationExpenseAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_DepreciationExpenseAccount;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(DepreciationExpenseAccount myDepreciationExpenseAccount)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDepreciationExpenseAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_DepreciationExpenseAccount;
            audit.mRowId = myDepreciationExpenseAccount.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(DepreciationExpenseAccount myDepreciationExpenseAccount)
        {
            DepreciationExpenseAccount old_depreciationexpenseaccount = GetItem(myDepreciationExpenseAccount.mId);
            AuditCollection audit_collection = DepreciationExpenseAccountAudit.Audit(myDepreciationExpenseAccount, old_depreciationexpenseaccount);
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