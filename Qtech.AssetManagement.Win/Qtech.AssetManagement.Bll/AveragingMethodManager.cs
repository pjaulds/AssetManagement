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
    public static class AveragingMethodManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static AveragingMethodCollection GetList()
        {
            AveragingMethodCriteria averagingmethod = new AveragingMethodCriteria();
            return GetList(averagingmethod);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AveragingMethodCollection GetList(AveragingMethodCriteria averagingmethodCriteria)
        {
            return AveragingMethodDB.GetList(averagingmethodCriteria);
        }

        public static int SelectCountForGetList(AveragingMethodCriteria averagingmethodCriteria)
        {
            return AveragingMethodDB.SelectCountForGetList(averagingmethodCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static AveragingMethod GetItem(int id)
        {
            AveragingMethod averagingmethod = AveragingMethodDB.GetItem(id);
            return averagingmethod;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(AveragingMethod myAveragingMethod)
        {
            if (!myAveragingMethod.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid averagingmethod. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myAveragingMethod.mId != 0)
                    AuditUpdate(myAveragingMethod);

                int id = AveragingMethodDB.Save(myAveragingMethod);

                if (myAveragingMethod.mId == 0)
                    AuditInsert(myAveragingMethod, id);

                myAveragingMethod.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(AveragingMethod myAveragingMethod)
        {
            if (AveragingMethodDB.Delete(myAveragingMethod.mId))
            {
                AuditDelete(myAveragingMethod);
                return myAveragingMethod.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(AveragingMethod myAveragingMethod, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAveragingMethod.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AveragingMethod;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(AveragingMethod myAveragingMethod)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myAveragingMethod.mUserId;
            audit.mTableId = (Int16)Tables.amQt_AveragingMethod;
            audit.mRowId = myAveragingMethod.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(AveragingMethod myAveragingMethod)
        {
            AveragingMethod old_averagingmethod = GetItem(myAveragingMethod.mId);
            AuditCollection audit_collection = AveragingMethodAudit.Audit(myAveragingMethod, old_averagingmethod);
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