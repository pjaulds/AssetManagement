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
    public static class JournalVoucherDetailManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static JournalVoucherDetailCollection GetList()
        {
            JournalVoucherDetailCriteria journalvoucherdetail = new JournalVoucherDetailCriteria();
            return GetList(journalvoucherdetail);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static JournalVoucherDetailCollection GetList(JournalVoucherDetailCriteria journalvoucherdetailCriteria)
        {
            return JournalVoucherDetailDB.GetList(journalvoucherdetailCriteria);
        }

        public static int SelectCountForGetList(JournalVoucherDetailCriteria journalvoucherdetailCriteria)
        {
            return JournalVoucherDetailDB.SelectCountForGetList(journalvoucherdetailCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static JournalVoucherDetail GetItem(int id)
        {
            JournalVoucherDetail journalvoucherdetail = JournalVoucherDetailDB.GetItem(id);
            return journalvoucherdetail;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(JournalVoucherDetail myJournalVoucherDetail)
        {
            if (!myJournalVoucherDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid journalvoucherdetail. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myJournalVoucherDetail.mId != 0)
                    AuditUpdate(myJournalVoucherDetail);

                int id = JournalVoucherDetailDB.Save(myJournalVoucherDetail);

                if (myJournalVoucherDetail.mId == 0)
                    AuditInsert(myJournalVoucherDetail, id);

                myJournalVoucherDetail.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(JournalVoucherDetail myJournalVoucherDetail)
        {
            if (JournalVoucherDetailDB.Delete(myJournalVoucherDetail.mId))
            {
                AuditDelete(myJournalVoucherDetail);
                return myJournalVoucherDetail.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(JournalVoucherDetail myJournalVoucherDetail, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myJournalVoucherDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_JournalVoucherDetail;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(JournalVoucherDetail myJournalVoucherDetail)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myJournalVoucherDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_JournalVoucherDetail;
            audit.mRowId = myJournalVoucherDetail.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(JournalVoucherDetail myJournalVoucherDetail)
        {
            JournalVoucherDetail old_journalvoucherdetail = GetItem(myJournalVoucherDetail.mId);
            AuditCollection audit_collection = JournalVoucherDetailAudit.Audit(myJournalVoucherDetail, old_journalvoucherdetail);
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