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
    public static class PaymentTermsManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PaymentTermsCollection GetList()
        {
            PaymentTermsCriteria paymentTerms = new PaymentTermsCriteria();
            return GetList(paymentTerms);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PaymentTermsCollection GetList(PaymentTermsCriteria paymentTermsCriteria)
        {
            return PaymentTermsDB.GetList(paymentTermsCriteria);
        }

        public static int SelectCountForGetList(PaymentTermsCriteria paymentTermsCriteria)
        {
            return PaymentTermsDB.SelectCountForGetList(paymentTermsCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PaymentTerms GetItem(int id)
        {
            PaymentTerms paymentTerms = PaymentTermsDB.GetItem(id);
            return paymentTerms;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(PaymentTerms myPaymentTerms)
        {
            if (!myPaymentTerms.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid paymentTerms. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPaymentTerms.mId != 0)
                    AuditUpdate(myPaymentTerms);

                int id = PaymentTermsDB.Save(myPaymentTerms);

                if (myPaymentTerms.mId == 0)
                    AuditInsert(myPaymentTerms, id);

                myPaymentTerms.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(PaymentTerms myPaymentTerms)
        {
            if (PaymentTermsDB.Delete(myPaymentTerms.mId))
            {
                AuditDelete(myPaymentTerms);
                return myPaymentTerms.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(PaymentTerms myPaymentTerms, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPaymentTerms.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PaymentTerms;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(PaymentTerms myPaymentTerms)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPaymentTerms.mUserId;
            audit.mTableId = (Int16)Tables.amQt_PaymentTerms;
            audit.mRowId = myPaymentTerms.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(PaymentTerms myPaymentTerms)
        {
            PaymentTerms old_paymentTerms = GetItem(myPaymentTerms.mId);
            AuditCollection audit_collection = PaymentTermsAudit.Audit(myPaymentTerms, old_paymentTerms);
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