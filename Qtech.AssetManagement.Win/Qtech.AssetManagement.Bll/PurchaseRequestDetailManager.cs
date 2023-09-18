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
    public static class PurchaseRequestDetailManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PurchaseRequestDetailCollection GetList()
        {
            PurchaseRequestDetailCriteria purchaserequestdetail = new PurchaseRequestDetailCriteria();
            return GetList(purchaserequestdetail);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseRequestDetailCollection GetList(PurchaseRequestDetailCriteria purchaserequestdetailCriteria)
        {
            return PurchaseRequestDetailDB.GetList(purchaserequestdetailCriteria);
        }

        public static int SelectCountForGetList(PurchaseRequestDetailCriteria purchaserequestdetailCriteria)
        {
            return PurchaseRequestDetailDB.SelectCountForGetList(purchaserequestdetailCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseRequestDetail GetItem(int id)
        {
            PurchaseRequestDetail purchaserequestdetail = PurchaseRequestDetailDB.GetItem(id);
            return purchaserequestdetail;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(PurchaseRequestDetail myPurchaseRequestDetail)
        {
            if (!myPurchaseRequestDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid purchaserequestdetail. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPurchaseRequestDetail.mId != 0)
                    AuditUpdate(myPurchaseRequestDetail);

                int id = PurchaseRequestDetailDB.Save(myPurchaseRequestDetail);

                if (myPurchaseRequestDetail.mId == 0)
                    AuditInsert(myPurchaseRequestDetail, id);

                myPurchaseRequestDetail.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(PurchaseRequestDetail myPurchaseRequestDetail)
        {
            if (PurchaseRequestDetailDB.Delete(myPurchaseRequestDetail.mId))
            {
                AuditDelete(myPurchaseRequestDetail);
                return myPurchaseRequestDetail.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(PurchaseRequestDetail myPurchaseRequestDetail, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseRequestDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseRequestDetail;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(PurchaseRequestDetail myPurchaseRequestDetail)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseRequestDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseRequestDetail;
            audit.mRowId = myPurchaseRequestDetail.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(PurchaseRequestDetail myPurchaseRequestDetail)
        {
            PurchaseRequestDetail old_purchaserequestdetail = GetItem(myPurchaseRequestDetail.mId);
            AuditCollection audit_collection = PurchaseRequestDetailAudit.Audit(myPurchaseRequestDetail, old_purchaserequestdetail);
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