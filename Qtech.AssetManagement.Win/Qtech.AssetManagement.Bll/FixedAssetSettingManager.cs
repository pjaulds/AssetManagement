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
    public static class FixedAssetSettingManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static FixedAssetSettingCollection GetList()
        {
            FixedAssetSettingCriteria fixedassetsetting = new FixedAssetSettingCriteria();
            return GetList(fixedassetsetting);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAssetSettingCollection GetList(FixedAssetSettingCriteria fixedassetsettingCriteria)
        {
            return FixedAssetSettingDB.GetList(fixedassetsettingCriteria);
        }

        public static int SelectCountForGetList(FixedAssetSettingCriteria fixedassetsettingCriteria)
        {
            return FixedAssetSettingDB.SelectCountForGetList(fixedassetsettingCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAssetSetting GetItem(int id)
        {
            FixedAssetSetting fixedassetsetting = FixedAssetSettingDB.GetItem(id);
            return fixedassetsetting;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(FixedAssetSetting myFixedAssetSetting)
        {
            if (!myFixedAssetSetting.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid fixedassetsetting. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myFixedAssetSetting.mId != 0)
                    AuditUpdate(myFixedAssetSetting);

                int id = FixedAssetSettingDB.Save(myFixedAssetSetting);

                if (myFixedAssetSetting.mId == 0)
                    AuditInsert(myFixedAssetSetting, id);

                myFixedAssetSetting.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(FixedAssetSetting myFixedAssetSetting)
        {
            if (FixedAssetSettingDB.Delete(myFixedAssetSetting.mId))
            {
                AuditDelete(myFixedAssetSetting);
                return myFixedAssetSetting.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(FixedAssetSetting myFixedAssetSetting, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAssetSetting.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAssetSetting;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(FixedAssetSetting myFixedAssetSetting)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAssetSetting.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAssetSetting;
            audit.mRowId = myFixedAssetSetting.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(FixedAssetSetting myFixedAssetSetting)
        {
            FixedAssetSetting old_fixedassetsetting = GetItem(myFixedAssetSetting.mId);
            AuditCollection audit_collection = FixedAssetSettingAudit.Audit(myFixedAssetSetting, old_fixedassetsetting);
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