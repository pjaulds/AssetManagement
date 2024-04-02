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
    public static class JournalVoucherManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static JournalVoucherCollection GetList()
        {
            JournalVoucherCriteria journalvoucher = new JournalVoucherCriteria();
            return GetList(journalvoucher);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static JournalVoucherCollection GetList(JournalVoucherCriteria journalvoucherCriteria)
        {
            return JournalVoucherDB.GetList(journalvoucherCriteria);
        }

        public static int SelectCountForGetList(JournalVoucherCriteria journalvoucherCriteria)
        {
            return JournalVoucherDB.SelectCountForGetList(journalvoucherCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static JournalVoucher GetItem(int id)
        {
            JournalVoucher journalvoucher = JournalVoucherDB.GetItem(id);
            return journalvoucher;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(JournalVoucher myJournalVoucher)
        {
            if (!myJournalVoucher.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid journalvoucher. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myJournalVoucher.mId != 0)
                    AuditUpdate(myJournalVoucher);

                int id = JournalVoucherDB.Save(myJournalVoucher);

                if (myJournalVoucher.mJournalVoucherDetailCollection != null)
                {
                    foreach (JournalVoucherDetail item in myJournalVoucher.mJournalVoucherDetailCollection)
                    {
                        item.mDebitCredit = item.mDebit > 0;

                        item.mJournalVoucherId = id;
                        item.mUserId = myJournalVoucher.mUserId;
                        JournalVoucherDetailManager.Save(item);
                    }
                }

                if (myJournalVoucher.mDeletedJournalVoucherDetailCollection != null)
                {
                    foreach (JournalVoucherDetail item in myJournalVoucher.mDeletedJournalVoucherDetailCollection)
                    {
                        item.mUserId = myJournalVoucher.mUserId;
                        JournalVoucherDetailManager.Delete(item);
                    }
                }

                if (myJournalVoucher.mId == 0)
                    AuditInsert(myJournalVoucher, id);

                myJournalVoucher.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(JournalVoucher myJournalVoucher)
        {
            if (JournalVoucherDB.Delete(myJournalVoucher.mId))
            {
                AuditDelete(myJournalVoucher);

                JournalVoucherDetailCriteria criteria = new JournalVoucherDetailCriteria();
                criteria.mJournalVoucherId = myJournalVoucher.mId;
                foreach (JournalVoucherDetail item in JournalVoucherDetailManager.GetList(criteria))
                {
                    item.mUserId = myJournalVoucher.mUserId;
                    JournalVoucherDetailManager.Delete(item);
                }

                return myJournalVoucher.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(JournalVoucher myJournalVoucher, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myJournalVoucher.mUserId;
            audit.mTableId = (Int16)Tables.amQt_JournalVoucher;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(JournalVoucher myJournalVoucher)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myJournalVoucher.mUserId;
            audit.mTableId = (Int16)Tables.amQt_JournalVoucher;
            audit.mRowId = myJournalVoucher.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(JournalVoucher myJournalVoucher)
        {
            JournalVoucher old_journalvoucher = GetItem(myJournalVoucher.mId);
            AuditCollection audit_collection = JournalVoucherAudit.Audit(myJournalVoucher, old_journalvoucher);
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