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
    public static class FixedAssetSettingDateManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static FixedAssetSettingDateCollection GetList()
        {
            FixedAssetSettingDateCriteria fixedassetsettingdate = new FixedAssetSettingDateCriteria();
            return GetList(fixedassetsettingdate);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAssetSettingDateCollection GetList(FixedAssetSettingDateCriteria fixedassetsettingdateCriteria)
        {
            return FixedAssetSettingDateDB.GetList(fixedassetsettingdateCriteria);
        }

        public static int SelectCountForGetList(FixedAssetSettingDateCriteria fixedassetsettingdateCriteria)
        {
            return FixedAssetSettingDateDB.SelectCountForGetList(fixedassetsettingdateCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAssetSettingDate GetItem(int id)
        {
            FixedAssetSettingDate fixedassetsettingdate = FixedAssetSettingDateDB.GetItem(id);
            return fixedassetsettingdate;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(FixedAssetSettingDate myFixedAssetSettingDate)
        {
            if (!myFixedAssetSettingDate.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid fixedassetsettingdate. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myFixedAssetSettingDate.mId != 0)
                    AuditUpdate(myFixedAssetSettingDate);

                int id = FixedAssetSettingDateDB.Save(myFixedAssetSettingDate);

                if (myFixedAssetSettingDate.mId == 0)
                    AuditInsert(myFixedAssetSettingDate, id);

                myFixedAssetSettingDate.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(FixedAssetSettingDate myFixedAssetSettingDate)
        {
            if (FixedAssetSettingDateDB.Delete(myFixedAssetSettingDate.mId))
            {
                AuditDelete(myFixedAssetSettingDate);
                return myFixedAssetSettingDate.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(FixedAssetSettingDate myFixedAssetSettingDate, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAssetSettingDate.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAssetSettingDate;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(FixedAssetSettingDate myFixedAssetSettingDate)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAssetSettingDate.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAssetSettingDate;
            audit.mRowId = myFixedAssetSettingDate.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(FixedAssetSettingDate myFixedAssetSettingDate)
        {
            FixedAssetSettingDate old_fixedassetsettingdate = GetItem(myFixedAssetSettingDate.mId);
            AuditCollection audit_collection = FixedAssetSettingDateAudit.Audit(myFixedAssetSettingDate, old_fixedassetsettingdate);
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