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
    public static class PurchaseOrderDetailManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PurchaseOrderDetailCollection GetList()
        {
            PurchaseOrderDetailCriteria purchaseorderdetail = new PurchaseOrderDetailCriteria();
            return GetList(purchaseorderdetail);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseOrderDetailCollection GetList(PurchaseOrderDetailCriteria purchaseorderdetailCriteria)
        {
            return PurchaseOrderDetailDB.GetList(purchaseorderdetailCriteria);
        }

        public static int SelectCountForGetList(PurchaseOrderDetailCriteria purchaseorderdetailCriteria)
        {
            return PurchaseOrderDetailDB.SelectCountForGetList(purchaseorderdetailCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseOrderDetail GetItem(int id)
        {
            PurchaseOrderDetail purchaseorderdetail = PurchaseOrderDetailDB.GetItem(id);
            return purchaseorderdetail;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(PurchaseOrderDetail myPurchaseOrderDetail)
        {
            if (!myPurchaseOrderDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid purchaseorderdetail. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPurchaseOrderDetail.mId != 0)
                    AuditUpdate(myPurchaseOrderDetail);

                int id = PurchaseOrderDetailDB.Save(myPurchaseOrderDetail);

                if (myPurchaseOrderDetail.mId == 0)
                    AuditInsert(myPurchaseOrderDetail, id);

                myPurchaseOrderDetail.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(PurchaseOrderDetail myPurchaseOrderDetail)
        {
            if (PurchaseOrderDetailDB.Delete(myPurchaseOrderDetail.mId))
            {
                AuditDelete(myPurchaseOrderDetail);
                return myPurchaseOrderDetail.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(PurchaseOrderDetail myPurchaseOrderDetail, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseOrderDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseOrderDetail;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(PurchaseOrderDetail myPurchaseOrderDetail)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseOrderDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseOrderDetail;
            audit.mRowId = myPurchaseOrderDetail.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(PurchaseOrderDetail myPurchaseOrderDetail)
        {
            PurchaseOrderDetail old_purchaseorderdetail = GetItem(myPurchaseOrderDetail.mId);
            AuditCollection audit_collection = PurchaseOrderDetailAudit.Audit(myPurchaseOrderDetail, old_purchaseorderdetail);
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