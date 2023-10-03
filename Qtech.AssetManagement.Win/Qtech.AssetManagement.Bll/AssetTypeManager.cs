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
    public static class AssetTypeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AssetTypeCollection GetList()
        {
            AssetTypeCriteria assetType = new AssetTypeCriteria();
            return GetList(assetType);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetTypeCollection GetList(AssetTypeCriteria assetTypeCriteria)
        {
            return AssetTypeDB.GetList(assetTypeCriteria);
        }

        public static int SelectCountForGetList(AssetTypeCriteria assetTypeCriteria)
        {
            return AssetTypeDB.SelectCountForGetList(assetTypeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetType GetItem(int id)
        {
            AssetType assetType = AssetTypeDB.GetItem(id);
            return assetType;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AssetType myAssetType)
        {
            if (!myAssetType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid assetType. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAssetType.mId != 0)
                    AuditUpdate(myAssetType);

                int id = AssetTypeDB.Save(myAssetType);

                if (myAssetType.mId == 0)
                    AuditInsert(myAssetType, id);

                myAssetType.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AssetType myAssetType)
        {
            if (AssetTypeDB.Delete(myAssetType.mId))
            {
                AuditDelete(myAssetType);
                return myAssetType.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AssetType myAssetType, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetType;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AssetType myAssetType)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetType;
            audit.mRowId = myAssetType.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AssetType myAssetType)
        {
            AssetType old_assetType = GetItem(myAssetType.mId);
            AuditCollection audit_collection = AssetTypeAudit.Audit(myAssetType, old_assetType);
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