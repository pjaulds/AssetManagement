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
    public static class PurchaseRequestManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PurchaseRequestCollection GetList()
        {
            PurchaseRequestCriteria purchaserequest = new PurchaseRequestCriteria();
            return GetList(purchaserequest);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseRequestCollection GetList(PurchaseRequestCriteria purchaserequestCriteria)
        {
            return PurchaseRequestDB.GetList(purchaserequestCriteria);
        }

        public static int SelectCountForGetList(PurchaseRequestCriteria purchaserequestCriteria)
        {
            return PurchaseRequestDB.SelectCountForGetList(purchaserequestCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseRequest GetItem(int id)
        {
            PurchaseRequest purchaserequest = PurchaseRequestDB.GetItem(id);
            return purchaserequest;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(PurchaseRequest myPurchaseRequest)
        {
            if (!myPurchaseRequest.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid purchaserequest. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPurchaseRequest.mId != 0)
                    AuditUpdate(myPurchaseRequest);
                
                int id = PurchaseRequestDB.Save(myPurchaseRequest);

                if (myPurchaseRequest.mPurchaseRequestDetailCollection != null)
                {
                    foreach (PurchaseRequestDetail item in myPurchaseRequest.mPurchaseRequestDetailCollection)
                    {
                        item.mPurchaseRequestId = id;
                        item.mUserId = myPurchaseRequest.mUserId;
                        PurchaseRequestDetailManager.Save(item);
                    }
                }

                if (myPurchaseRequest.mDeletedPurchaseRequestDetailCollection != null)
                {
                    foreach (PurchaseRequestDetail item in myPurchaseRequest.mDeletedPurchaseRequestDetailCollection)
                    {
                        item.mUserId = myPurchaseRequest.mUserId;
                        PurchaseRequestDetailManager.Delete(item);
                    }
                }

                if (myPurchaseRequest.mId == 0)
                    AuditInsert(myPurchaseRequest, id);

                myPurchaseRequest.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(PurchaseRequest myPurchaseRequest)
        {
            if (PurchaseRequestDB.Delete(myPurchaseRequest.mId))
            {
                AuditDelete(myPurchaseRequest);
                return myPurchaseRequest.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(PurchaseRequest myPurchaseRequest, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseRequest.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseRequest;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(PurchaseRequest myPurchaseRequest)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseRequest.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseRequest;
            audit.mRowId = myPurchaseRequest.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(PurchaseRequest myPurchaseRequest)
        {
            PurchaseRequest old_purchaserequest = GetItem(myPurchaseRequest.mId);
            AuditCollection audit_collection = PurchaseRequestAudit.Audit(myPurchaseRequest, old_purchaserequest);
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