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
    public static class PaymentModeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PaymentModeCollection GetList()
        {
            PaymentModeCriteria paymentMode = new PaymentModeCriteria();
            return GetList(paymentMode);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PaymentModeCollection GetList(PaymentModeCriteria paymentModeCriteria)
        {
            return PaymentModeDB.GetList(paymentModeCriteria);
        }

        public static int SelectCountForGetList(PaymentModeCriteria paymentModeCriteria)
        {
            return PaymentModeDB.SelectCountForGetList(paymentModeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PaymentMode GetItem(int id)
        {
            PaymentMode paymentMode = PaymentModeDB.GetItem(id);
            return paymentMode;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(PaymentMode myPaymentMode)
        {
            if (!myPaymentMode.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid paymentMode. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPaymentMode.mId != 0)
                    AuditUpdate(myPaymentMode);

                int id = PaymentModeDB.Save(myPaymentMode);

                if (myPaymentMode.mId == 0)
                    AuditInsert(myPaymentMode, id);

                myPaymentMode.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(PaymentMode myPaymentMode)
        {
            if (PaymentModeDB.Delete(myPaymentMode.mId))
            {
                AuditDelete(myPaymentMode);
                return myPaymentMode.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(PaymentMode myPaymentMode, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPaymentMode.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PaymentMode;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(PaymentMode myPaymentMode)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPaymentMode.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PaymentMode;
            audit.mRowId = myPaymentMode.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(PaymentMode myPaymentMode)
        {
            PaymentMode old_paymentMode = GetItem(myPaymentMode.mId);
            AuditCollection audit_collection = PaymentModeAudit.Audit(myPaymentMode, old_paymentMode);
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