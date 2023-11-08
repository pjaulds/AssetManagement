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
    public static class AssetClassManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AssetClassCollection GetList()
        {
            AssetClassCriteria assetClass = new AssetClassCriteria();
            return GetList(assetClass);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetClassCollection GetList(AssetClassCriteria assetClassCriteria)
        {
            return AssetClassDB.GetList(assetClassCriteria);
        }

        public static int SelectCountForGetList(AssetClassCriteria assetClassCriteria)
        {
            return AssetClassDB.SelectCountForGetList(assetClassCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetClass GetItem(int id)
        {
            AssetClass assetClass = AssetClassDB.GetItem(id);
            return assetClass;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AssetClass myAssetClass)
        {
            if (!myAssetClass.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid assetClass. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAssetClass.mId != 0)
                    AuditUpdate(myAssetClass);

                int id = AssetClassDB.Save(myAssetClass);

                if (myAssetClass.mId == 0)
                    AuditInsert(myAssetClass, id);

                myAssetClass.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AssetClass myAssetClass)
        {
            if (AssetClassDB.Delete(myAssetClass.mId))
            {
                AuditDelete(myAssetClass);
                return myAssetClass.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AssetClass myAssetClass, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetClass.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetClass;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AssetClass myAssetClass)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetClass.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetClass;
            audit.mRowId = myAssetClass.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AssetClass myAssetClass)
        {
            AssetClass old_assetClass = GetItem(myAssetClass.mId);
            AuditCollection audit_collection = AssetClassAudit.Audit(myAssetClass, old_assetClass);
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