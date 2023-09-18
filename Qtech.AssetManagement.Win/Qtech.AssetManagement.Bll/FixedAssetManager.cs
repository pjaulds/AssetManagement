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
    public static class FixedAssetManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static FixedAssetCollection GetList()
        {
            FixedAssetCriteria fixedasset = new FixedAssetCriteria();
            return GetList(fixedasset);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAssetCollection GetList(FixedAssetCriteria fixedassetCriteria)
        {
            return FixedAssetDB.GetList(fixedassetCriteria);
        }

        public static int SelectCountForGetList(FixedAssetCriteria fixedassetCriteria)
        {
            return FixedAssetDB.SelectCountForGetList(fixedassetCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAsset GetItem(int id)
        {
            FixedAsset fixedasset = FixedAssetDB.GetItem(id);
            return fixedasset;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(FixedAsset myFixedAsset)
        {
            if (!myFixedAsset.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid fixedasset. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myFixedAsset.mId != 0)
                    AuditUpdate(myFixedAsset);

                int id = FixedAssetDB.Save(myFixedAsset);

                if (myFixedAsset.mId == 0)
                    AuditInsert(myFixedAsset, id);

                myFixedAsset.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(FixedAsset myFixedAsset)
        {
            if (FixedAssetDB.Delete(myFixedAsset.mId))
            {
                AuditDelete(myFixedAsset);
                return myFixedAsset.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(FixedAsset myFixedAsset, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAsset.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAsset;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(FixedAsset myFixedAsset)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAsset.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAsset;
            audit.mRowId = myFixedAsset.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(FixedAsset myFixedAsset)
        {
            FixedAsset old_fixedasset = GetItem(myFixedAsset.mId);
            AuditCollection audit_collection = FixedAssetAudit.Audit(myFixedAsset, old_fixedasset);
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