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
    public static class DepreciationJournalManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DepreciationJournalCollection GetList()
        {
            DepreciationJournalCriteria depreciationjournal = new DepreciationJournalCriteria();
            return GetList(depreciationjournal);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DepreciationJournalCollection GetList(DepreciationJournalCriteria depreciationjournalCriteria)
        {
            return DepreciationJournalDB.GetList(depreciationjournalCriteria);
        }

        public static int SelectCountForGetList(DepreciationJournalCriteria depreciationjournalCriteria)
        {
            return DepreciationJournalDB.SelectCountForGetList(depreciationjournalCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DepreciationJournal GetItem(int id)
        {
            DepreciationJournal depreciationjournal = DepreciationJournalDB.GetItem(id);
            return depreciationjournal;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(DepreciationJournal myDepreciationJournal)
        {
            if (!myDepreciationJournal.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid depreciationjournal. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myDepreciationJournal.mId != 0)
                    AuditUpdate(myDepreciationJournal);

                int id = DepreciationJournalDB.Save(myDepreciationJournal);

                if (myDepreciationJournal.mId == 0)
                    AuditInsert(myDepreciationJournal, id);

                myDepreciationJournal.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(DepreciationJournal myDepreciationJournal)
        {
            if (DepreciationJournalDB.Delete(myDepreciationJournal.mId))
            {
                AuditDelete(myDepreciationJournal);
                return myDepreciationJournal.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(DepreciationJournal myDepreciationJournal, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDepreciationJournal.mUserId;
            audit.mTableId = (Int16)Tables.amQt_DepreciationJournal;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(DepreciationJournal myDepreciationJournal)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDepreciationJournal.mUserId;
            audit.mTableId = (Int16)Tables.amQt_DepreciationJournal;
            audit.mRowId = myDepreciationJournal.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(DepreciationJournal myDepreciationJournal)
        {
            DepreciationJournal old_depreciationjournal = GetItem(myDepreciationJournal.mId);
            AuditCollection audit_collection = DepreciationJournalAudit.Audit(myDepreciationJournal, old_depreciationjournal);
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