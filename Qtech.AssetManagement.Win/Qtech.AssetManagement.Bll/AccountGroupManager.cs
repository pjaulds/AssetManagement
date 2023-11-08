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
    public static class AccountGroupManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AccountGroupCollection GetList()
        {
            AccountGroupCriteria accountGroup = new AccountGroupCriteria();
            return GetList(accountGroup);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccountGroupCollection GetList(AccountGroupCriteria accountGroupCriteria)
        {
            return AccountGroupDB.GetList(accountGroupCriteria);
        }

        public static int SelectCountForGetList(AccountGroupCriteria accountGroupCriteria)
        {
            return AccountGroupDB.SelectCountForGetList(accountGroupCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccountGroup GetItem(int id)
        {
            AccountGroup accountGroup = AccountGroupDB.GetItem(id);
            return accountGroup;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AccountGroup myAccountGroup)
        {
            if (!myAccountGroup.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid accountGroup. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAccountGroup.mId != 0)
                    AuditUpdate(myAccountGroup);

                int id = AccountGroupDB.Save(myAccountGroup);

                if (myAccountGroup.mId == 0)
                    AuditInsert(myAccountGroup, id);

                myAccountGroup.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AccountGroup myAccountGroup)
        {
            if (AccountGroupDB.Delete(myAccountGroup.mId))
            {
                AuditDelete(myAccountGroup);
                return myAccountGroup.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AccountGroup myAccountGroup, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccountGroup.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccountGroup;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AccountGroup myAccountGroup)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccountGroup.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccountGroup;
            audit.mRowId = myAccountGroup.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AccountGroup myAccountGroup)
        {
            AccountGroup old_accountGroup = GetItem(myAccountGroup.mId);
            AuditCollection audit_collection = AccountGroupAudit.Audit(myAccountGroup, old_accountGroup);
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