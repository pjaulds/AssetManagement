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
    public static class AssetCategoryManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AssetCategoryCollection GetList()
        {
            AssetCategoryCriteria assetCategory = new AssetCategoryCriteria();
            return GetList(assetCategory);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetCategoryCollection GetList(AssetCategoryCriteria assetCategoryCriteria)
        {
            return AssetCategoryDB.GetList(assetCategoryCriteria);
        }

        public static int SelectCountForGetList(AssetCategoryCriteria assetCategoryCriteria)
        {
            return AssetCategoryDB.SelectCountForGetList(assetCategoryCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetCategory GetItem(int id)
        {
            AssetCategory assetCategory = AssetCategoryDB.GetItem(id);
            return assetCategory;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AssetCategory myAssetCategory)
        {
            if (!myAssetCategory.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid assetCategory. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAssetCategory.mId != 0)
                    AuditUpdate(myAssetCategory);

                int id = AssetCategoryDB.Save(myAssetCategory);

                if (myAssetCategory.mId == 0)
                    AuditInsert(myAssetCategory, id);

                myAssetCategory.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AssetCategory myAssetCategory)
        {
            if (AssetCategoryDB.Delete(myAssetCategory.mId))
            {
                AuditDelete(myAssetCategory);
                return myAssetCategory.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AssetCategory myAssetCategory, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetCategory.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetCategory;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AssetCategory myAssetCategory)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetCategory.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetCategory;
            audit.mRowId = myAssetCategory.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AssetCategory myAssetCategory)
        {
            AssetCategory old_assetCategory = GetItem(myAssetCategory.mId);
            AuditCollection audit_collection = AssetCategoryAudit.Audit(myAssetCategory, old_assetCategory);
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