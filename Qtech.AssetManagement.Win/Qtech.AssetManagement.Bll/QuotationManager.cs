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
    public static class QuotationManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static QuotationCollection GetList()
        {
            QuotationCriteria quotation = new QuotationCriteria();
            return GetList(quotation);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static QuotationCollection GetList(QuotationCriteria quotationCriteria)
        {
            return QuotationDB.GetList(quotationCriteria);
        }

        public static int SelectCountForGetList(QuotationCriteria quotationCriteria)
        {
            return QuotationDB.SelectCountForGetList(quotationCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Quotation GetItem(int id)
        {
            Quotation quotation = QuotationDB.GetItem(id);
            return quotation;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Quotation myQuotation)
        {
            if (!myQuotation.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid quotation. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myQuotation.mId != 0)
                    AuditUpdate(myQuotation);

                int id = QuotationDB.Save(myQuotation);

                if (myQuotation.mQuotationDetailCollection != null)
                {
                    foreach (QuotationDetail item in myQuotation.mQuotationDetailCollection)
                    {
                        item.mQuotationId = id;
                        item.mUserId = myQuotation.mUserId;
                        QuotationDetailManager.Save(item);
                    }
                }

                if (myQuotation.mDeletedQuotationDetailCollection != null)
                {
                    foreach (QuotationDetail item in myQuotation.mDeletedQuotationDetailCollection)
                    {
                        item.mUserId = myQuotation.mUserId;
                        QuotationDetailManager.Delete(item);
                    }
                }

                if (myQuotation.mId == 0)
                    AuditInsert(myQuotation, id);

                myQuotation.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Quotation myQuotation)
        {
            if (QuotationDB.Delete(myQuotation.mId))
            {
                AuditDelete(myQuotation);
                return myQuotation.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Quotation myQuotation, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myQuotation.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Quotation;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Quotation myQuotation)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myQuotation.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Quotation;
            audit.mRowId = myQuotation.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Quotation myQuotation)
        {
            Quotation old_quotation = GetItem(myQuotation.mId);
            AuditCollection audit_collection = QuotationAudit.Audit(myQuotation, old_quotation);
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
