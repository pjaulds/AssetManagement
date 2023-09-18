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
    public static class QuotationDetailManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static QuotationDetailCollection GetList()
        {
            QuotationDetailCriteria quotationdetail = new QuotationDetailCriteria();
            return GetList(quotationdetail);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static QuotationDetailCollection GetList(QuotationDetailCriteria quotationdetailCriteria)
        {
            return QuotationDetailDB.GetList(quotationdetailCriteria);
        }

        public static int SelectCountForGetList(QuotationDetailCriteria quotationdetailCriteria)
        {
            return QuotationDetailDB.SelectCountForGetList(quotationdetailCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static QuotationDetail GetItem(int id)
        {
            QuotationDetail quotationdetail = QuotationDetailDB.GetItem(id);
            return quotationdetail;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(QuotationDetail myQuotationDetail)
        {
            if (!myQuotationDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid quotationdetail. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myQuotationDetail.mId != 0)
                    AuditUpdate(myQuotationDetail);

                int id = QuotationDetailDB.Save(myQuotationDetail);

                if (myQuotationDetail.mId == 0)
                    AuditInsert(myQuotationDetail, id);

                myQuotationDetail.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(QuotationDetail myQuotationDetail)
        {
            if (QuotationDetailDB.Delete(myQuotationDetail.mId))
            {
                AuditDelete(myQuotationDetail);
                return myQuotationDetail.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(QuotationDetail myQuotationDetail, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myQuotationDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_QuotationDetail;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(QuotationDetail myQuotationDetail)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myQuotationDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_QuotationDetail;
            audit.mRowId = myQuotationDetail.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(QuotationDetail myQuotationDetail)
        {
            QuotationDetail old_quotationdetail = GetItem(myQuotationDetail.mId);
            AuditCollection audit_collection = QuotationDetailAudit.Audit(myQuotationDetail, old_quotationdetail);
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