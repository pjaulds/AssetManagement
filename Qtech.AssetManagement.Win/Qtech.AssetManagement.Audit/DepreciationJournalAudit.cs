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
