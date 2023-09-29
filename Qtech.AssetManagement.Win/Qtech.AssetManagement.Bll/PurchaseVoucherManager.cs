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
    public static class PurchaseVoucherManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PurchaseVoucherCollection GetList()
        {
            PurchaseVoucherCriteria purchasevoucher = new PurchaseVoucherCriteria();
            return GetList(purchasevoucher);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseVoucherCollection GetList(PurchaseVoucherCriteria purchasevoucherCriteria)
        {
            return PurchaseVoucherDB.GetList(purchasevoucherCriteria);
        }

        public static int SelectCountForGetList(PurchaseVoucherCriteria purchasevoucherCriteria)
        {
            return PurchaseVoucherDB.SelectCountForGetList(purchasevoucherCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PurchaseVoucher GetItem(int id)
        {
            PurchaseVoucher purchasevoucher = PurchaseVoucherDB.GetItem(id);
            return purchasevoucher;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(PurchaseVoucher myPurchaseVoucher)
        {
            if (!myPurchaseVoucher.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid purchasevoucher. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPurchaseVoucher.mId != 0)
                    AuditUpdate(myPurchaseVoucher);

                int id = PurchaseVoucherDB.Save(myPurchaseVoucher);

                if (myPurchaseVoucher.mId == 0)
                    AuditInsert(myPurchaseVoucher, id);

                myPurchaseVoucher.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(PurchaseVoucher myPurchaseVoucher)
        {
            if (PurchaseVoucherDB.Delete(myPurchaseVoucher.mId))
            {
                AuditDelete(myPurchaseVoucher);
                return myPurchaseVoucher.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(PurchaseVoucher myPurchaseVoucher, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseVoucher.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseVoucher;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(PurchaseVoucher myPurchaseVoucher)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPurchaseVoucher.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PurchaseVoucher;
            audit.mRowId = myPurchaseVoucher.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(PurchaseVoucher myPurchaseVoucher)
        {
            PurchaseVoucher old_purchasevoucher = GetItem(myPurchaseVoucher.mId);
            AuditCollection audit_collection = PurchaseVoucherAudit.Audit(myPurchaseVoucher, old_purchasevoucher);
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