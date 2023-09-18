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
    public static class PurchaseOrderManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PurchaseOrderCollection GetList()
        {
            PurchaseOrderCriteria purchaseorder = new PurchaseOrderCriteria();
            return GetList(purchaseorder);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseOrderCollection GetList(PurchaseOrderCriteria purchaseorderCriteria)
        {
            return PurchaseOrderDB.GetList(purchaseorderCriteria);
        }

        public static int SelectCountForGetList(PurchaseOrderCriteria purchaseorderCriteria)
        {
            return PurchaseOrderDB.SelectCountForGetList(purchaseorderCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseOrder GetItem(int id)
        {
            PurchaseOrder purchaseorder = PurchaseOrderDB.GetItem(id);
            return purchaseorder;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(PurchaseOrder myPurchaseOrder)
        {
            if (!myPurchaseOrder.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid purchaseorder. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPurchaseOrder.mId != 0)
                    AuditUpdate(myPurchaseOrder);

                int id = PurchaseOrderDB.Save(myPurchaseOrder);

                if (myPurchaseOrder.mPurchaseOrderDetailCollection != null)
                {
                    foreach (PurchaseOrderDetail item in myPurchaseOrder.mPurchaseOrderDetailCollection)
                    {
                        item.mPurchaseOrderId = id;
                        item.mUserId = myPurchaseOrder.mUserId;
                        PurchaseOrderDetailManager.Save(item);
                    }
                }

                if (myPurchaseOrder.mDeletedPurchaseOrderDetailCollection != null)
                {
                    foreach (PurchaseOrderDetail item in myPurchaseOrder.mDeletedPurchaseOrderDetailCollection)
                    {
                        item.mUserId = myPurchaseOrder.mUserId;
                        PurchaseOrderDetailManager.Delete(item);
                    }
                }

                if (myPurchaseOrder.mId == 0)
                    AuditInsert(myPurchaseOrder, id);

                myPurchaseOrder.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(PurchaseOrder myPurchaseOrder)
        {
            if (PurchaseOrderDB.Delete(myPurchaseOrder.mId))
            {
                AuditDelete(myPurchaseOrder);
                return myPurchaseOrder.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(PurchaseOrder myPurchaseOrder, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseOrder.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseOrder;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(PurchaseOrder myPurchaseOrder)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseOrder.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseOrder;
            audit.mRowId = myPurchaseOrder.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(PurchaseOrder myPurchaseOrder)
        {
            PurchaseOrder old_purchaseorder = GetItem(myPurchaseOrder.mId);
            AuditCollection audit_collection = PurchaseOrderAudit.Audit(myPurchaseOrder, old_purchaseorder);
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
