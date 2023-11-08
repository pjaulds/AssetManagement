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
    public static class AccountClassificationManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AccountClassificationCollection GetList()
        {
            AccountClassificationCriteria accountClassification = new AccountClassificationCriteria();
            return GetList(accountClassification);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccountClassificationCollection GetList(AccountClassificationCriteria accountClassificationCriteria)
        {
            return AccountClassificationDB.GetList(accountClassificationCriteria);
        }

        public static int SelectCountForGetList(AccountClassificationCriteria accountClassificationCriteria)
        {
            return AccountClassificationDB.SelectCountForGetList(accountClassificationCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AccountClassification GetItem(int id)
        {
            AccountClassification accountClassification = AccountClassificationDB.GetItem(id);
            return accountClassification;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AccountClassification myAccountClassification)
        {
            if (!myAccountClassification.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid accountClassification. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAccountClassification.mId != 0)
                    AuditUpdate(myAccountClassification);

                int id = AccountClassificationDB.Save(myAccountClassification);

                if (myAccountClassification.mId == 0)
                    AuditInsert(myAccountClassification, id);

                myAccountClassification.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AccountClassification myAccountClassification)
        {
            if (AccountClassificationDB.Delete(myAccountClassification.mId))
            {
                AuditDelete(myAccountClassification);
                return myAccountClassification.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AccountClassification myAccountClassification, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccountClassification.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccountClassification;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AccountClassification myAccountClassification)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAccountClassification.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AccountClassification;
            audit.mRowId = myAccountClassification.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AccountClassification myAccountClassification)
        {
            AccountClassification old_accountClassification = GetItem(myAccountClassification.mId);
            AuditCollection audit_collection = AccountClassificationAudit.Audit(myAccountClassification, old_accountClassification);
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