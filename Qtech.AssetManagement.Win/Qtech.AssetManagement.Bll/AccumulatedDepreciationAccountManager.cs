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
    public static class AccumulatedDepreciationAccountManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AccumulatedDepreciationAccountCollection GetList()
        {
            AccumulatedDepreciationAccountCriteria accumulateddepreciationaccount = new AccumulatedDepreciationAccountCriteria();
            return GetList(accumulateddepreciationaccount);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccumulatedDepreciationAccountCollection GetList(AccumulatedDepreciationAccountCriteria accumulateddepreciationaccountCriteria)
        {
            return AccumulatedDepreciationAccountDB.GetList(accumulateddepreciationaccountCriteria);
        }

        public static int SelectCountForGetList(AccumulatedDepreciationAccountCriteria accumulateddepreciationaccountCriteria)
        {
            return AccumulatedDepreciationAccountDB.SelectCountForGetList(accumulateddepreciationaccountCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccumulatedDepreciationAccount GetItem(int id)
        {
            AccumulatedDepreciationAccount accumulateddepreciationaccount = AccumulatedDepreciationAccountDB.GetItem(id);
            return accumulateddepreciationaccount;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AccumulatedDepreciationAccount myAccumulatedDepreciationAccount)
        {
            if (!myAccumulatedDepreciationAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid accumulateddepreciationaccount. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAccumulatedDepreciationAccount.mId != 0)
                    AuditUpdate(myAccumulatedDepreciationAccount);

                int id = AccumulatedDepreciationAccountDB.Save(myAccumulatedDepreciationAccount);

                if (myAccumulatedDepreciationAccount.mId == 0)
                    AuditInsert(myAccumulatedDepreciationAccount, id);

                myAccumulatedDepreciationAccount.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AccumulatedDepreciationAccount myAccumulatedDepreciationAccount)
        {
            if (AccumulatedDepreciationAccountDB.Delete(myAccumulatedDepreciationAccount.mId))
            {
                AuditDelete(myAccumulatedDepreciationAccount);
                return myAccumulatedDepreciationAccount.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AccumulatedDepreciationAccount myAccumulatedDepreciationAccount, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccumulatedDepreciationAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccumulatedDepreciationAccount;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AccumulatedDepreciationAccount myAccumulatedDepreciationAccount)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccumulatedDepreciationAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccumulatedDepreciationAccount;
            audit.mRowId = myAccumulatedDepreciationAccount.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AccumulatedDepreciationAccount myAccumulatedDepreciationAccount)
        {
            AccumulatedDepreciationAccount old_accumulateddepreciationaccount = GetItem(myAccumulatedDepreciationAccount.mId);
            AuditCollection audit_collection = AccumulatedDepreciationAccountAudit.Audit(myAccumulatedDepreciationAccount, old_accumulateddepreciationaccount);
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