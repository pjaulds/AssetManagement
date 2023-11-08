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
    public static class AccountTypeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AccountTypeCollection GetList()
        {
            AccountTypeCriteria accountType = new AccountTypeCriteria();
            return GetList(accountType);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccountTypeCollection GetList(AccountTypeCriteria accountTypeCriteria)
        {
            return AccountTypeDB.GetList(accountTypeCriteria);
        }

        public static int SelectCountForGetList(AccountTypeCriteria accountTypeCriteria)
        {
            return AccountTypeDB.SelectCountForGetList(accountTypeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccountType GetItem(int id)
        {
            AccountType accountType = AccountTypeDB.GetItem(id);
            return accountType;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AccountType myAccountType)
        {
            if (!myAccountType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid accountType. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAccountType.mId != 0)
                    AuditUpdate(myAccountType);

                int id = AccountTypeDB.Save(myAccountType);

                if (myAccountType.mId == 0)
                    AuditInsert(myAccountType, id);

                myAccountType.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AccountType myAccountType)
        {
            if (AccountTypeDB.Delete(myAccountType.mId))
            {
                AuditDelete(myAccountType);
                return myAccountType.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AccountType myAccountType, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccountType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccountType;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AccountType myAccountType)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccountType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccountType;
            audit.mRowId = myAccountType.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AccountType myAccountType)
        {
            AccountType old_accountType = GetItem(myAccountType.mId);
            AuditCollection audit_collection = AccountTypeAudit.Audit(myAccountType, old_accountType);
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