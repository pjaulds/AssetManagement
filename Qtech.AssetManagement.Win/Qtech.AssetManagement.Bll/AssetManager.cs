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
    public static class AssetManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AssetCollection GetList()
        {
            AssetCriteria asset = new AssetCriteria();
            return GetList(asset);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetCollection GetList(AssetCriteria assetCriteria)
        {
            return AssetDB.GetList(assetCriteria);
        }

        public static int SelectCountForGetList(AssetCriteria assetCriteria)
        {
            return AssetDB.SelectCountForGetList(assetCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Asset GetItem(int id)
        {
            Asset asset = AssetDB.GetItem(id);
            return asset;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Asset myAsset)
        {
            if (!myAsset.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid asset. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAsset.mId != 0)
                    AuditUpdate(myAsset);

                int id = AssetDB.Save(myAsset);

                if (myAsset.mId == 0)
                    AuditInsert(myAsset, id);

                myAsset.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Asset myAsset)
        {
            if (AssetDB.Delete(myAsset.mId))
            {
                AuditDelete(myAsset);
                return myAsset.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Asset myAsset, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAsset.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Asset;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Asset myAsset)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAsset.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Asset;
            audit.mRowId = myAsset.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Asset myAsset)
        {
            Asset old_asset = GetItem(myAsset.mId);
            AuditCollection audit_collection = AssetAudit.Audit(myAsset, old_asset);
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