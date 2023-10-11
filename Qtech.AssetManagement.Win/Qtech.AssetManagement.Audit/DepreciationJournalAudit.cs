using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class DepreciationJournalAudit
    {

        public static AuditCollection Audit(DepreciationJournal depreciationjournal, DepreciationJournal depreciationjournalOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (depreciationjournal.mYear != depreciationjournalOld.mYear)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Year";
                audit.mOldValue = depreciationjournalOld.mYear.ToString();
                audit.mNewValue = depreciationjournal.mYear.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationjournal.mMonth != depreciationjournalOld.mMonth)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Month";
                audit.mOldValue = depreciationjournalOld.mMonth.ToString();
                audit.mNewValue = depreciationjournal.mMonth.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationjournal.mDepreciationExpenseAccountId != depreciationjournalOld.mDepreciationExpenseAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Depreciation Expense Account ";
                audit.mOldValue = depreciationjournalOld.mDepreciationExpenseAccountName.ToString();
                audit.mNewValue = depreciationjournal.mDepreciationExpenseAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationjournal.mDepreciationExpenseAccountDebitCredit != depreciationjournalOld.mDepreciationExpenseAccountDebitCredit)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Depreciation Expense Account Debit Credit";
                audit.mOldValue = depreciationjournalOld.mDepreciationExpenseAccountDebitCredit.ToString();
                audit.mNewValue = depreciationjournal.mDepreciationExpenseAccountDebitCredit.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationjournal.mAccumulatedDepreciationAccountId != depreciationjournalOld.mAccumulatedDepreciationAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Accumulated Depreciation Account ";
                audit.mOldValue = depreciationjournalOld.mAccumulatedDepreciationAccountName.ToString();
                audit.mNewValue = depreciationjournal.mAccumulatedDepreciationAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationjournal.mAccumulatedDepreciationAccountDebitCredit != depreciationjournalOld.mAccumulatedDepreciationAccountDebitCredit)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Accumulated Depreciation Account Debit Credit";
                audit.mOldValue = depreciationjournalOld.mAccumulatedDepreciationAccountDebitCredit.ToString();
                audit.mNewValue = depreciationjournal.mAccumulatedDepreciationAccountDebitCredit.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationjournal.mAmount != depreciationjournalOld.mAmount)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Amount";
                audit.mOldValue = depreciationjournalOld.mAmount.ToString();
                audit.mNewValue = depreciationjournal.mAmount.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationjournal.mDescription != depreciationjournalOld.mDescription)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationjournal);
                audit.mField = "Description";
                audit.mOldValue = depreciationjournalOld.mDescription.ToString();
                audit.mNewValue = depreciationjournal.mDescription.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, DepreciationJournal depreciationjournal)
        {
            audit.mUserId = depreciationjournal.mUserId;
            audit.mTableId = (int)(Tables.amQt_DepreciationJournal);
            audit.mRowId = depreciationjournal.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
