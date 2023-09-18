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
    public static class AssetAccountManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AssetAccountCollection GetList()
        {
            AssetAccountCriteria assetaccount = new AssetAccountCriteria();
            return GetList(assetaccount);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetAccountCollection GetList(AssetAccountCriteria assetaccountCriteria)
        {
            return AssetAccountDB.GetList(assetaccountCriteria);
        }

        public static int SelectCountForGetList(AssetAccountCriteria assetaccountCriteria)
        {
            return AssetAccountDB.SelectCountForGetList(assetaccountCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetAccount GetItem(int id)
        {
            AssetAccount assetaccount = AssetAccountDB.GetItem(id);
            return assetaccount;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AssetAccount myAssetAccount)
        {
            if (!myAssetAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid assetaccount. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAssetAccount.mId != 0)
                    AuditUpdate(myAssetAccount);

                int id = AssetAccountDB.Save(myAssetAccount);

                if (myAssetAccount.mId == 0)
                    AuditInsert(myAssetAccount, id);

                myAssetAccount.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AssetAccount myAssetAccount)
        {
            if (AssetAccountDB.Delete(myAssetAccount.mId))
            {
                AuditDelete(myAssetAccount);
                return myAssetAccount.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AssetAccount myAssetAccount, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetAccount;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AssetAccount myAssetAccount)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetAccount;
            audit.mRowId = myAssetAccount.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AssetAccount myAssetAccount)
        {
            AssetAccount old_assetaccount = GetItem(myAssetAccount.mId);
            AuditCollection audit_collection = AssetAccountAudit.Audit(myAssetAccount, old_assetaccount);
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