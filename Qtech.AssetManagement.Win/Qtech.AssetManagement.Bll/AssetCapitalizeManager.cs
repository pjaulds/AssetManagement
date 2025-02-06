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
    public static class AssetCapitalizeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AssetCapitalizeCollection GetList()
        {
            AssetCapitalizeCriteria assetcapitalize = new AssetCapitalizeCriteria();
            return GetList(assetcapitalize);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetCapitalizeCollection GetList(AssetCapitalizeCriteria assetcapitalizeCriteria)
        {
            return AssetCapitalizeDB.GetList(assetcapitalizeCriteria);
        }

        public static int SelectCountForGetList(AssetCapitalizeCriteria assetcapitalizeCriteria)
        {
            return AssetCapitalizeDB.SelectCountForGetList(assetcapitalizeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AssetCapitalize GetItem(int id)
        {
            AssetCapitalize assetcapitalize = AssetCapitalizeDB.GetItem(id);
            return assetcapitalize;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AssetCapitalize myAssetCapitalize)
        {
            if (!myAssetCapitalize.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid assetcapitalize. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAssetCapitalize.mId != 0)
                    AuditUpdate(myAssetCapitalize);

                int id = AssetCapitalizeDB.Save(myAssetCapitalize);

                if (myAssetCapitalize.mId == 0)
                    AuditInsert(myAssetCapitalize, id);

                myAssetCapitalize.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AssetCapitalize myAssetCapitalize)
        {
            if (AssetCapitalizeDB.Delete(myAssetCapitalize.mId))
            {
                AuditDelete(myAssetCapitalize);
                return myAssetCapitalize.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AssetCapitalize myAssetCapitalize, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetCapitalize.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetCapitalize;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AssetCapitalize myAssetCapitalize)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAssetCapitalize.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AssetCapitalize;
            audit.mRowId = myAssetCapitalize.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AssetCapitalize myAssetCapitalize)
        {
            AssetCapitalize old_assetcapitalize = GetItem(myAssetCapitalize.mId);
            AuditCollection audit_collection = AssetCapitalizeAudit.Audit(myAssetCapitalize, old_assetcapitalize);
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