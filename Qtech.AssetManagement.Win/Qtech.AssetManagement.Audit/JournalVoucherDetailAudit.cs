using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class JournalVoucherDetailAudit
    {

        public static AuditCollection Audit(JournalVoucherDetail journalvoucherdetail, JournalVoucherDetail journalvoucherdetailOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            

            if (journalvoucherdetail.mChartOfAccountId != journalvoucherdetailOld.mChartOfAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucherdetail);
                audit.mField = "Chart Of Account ";
                audit.mOldValue = journalvoucherdetailOld.mChartOfAccountName.ToString();
                audit.mNewValue = journalvoucherdetail.mChartOfAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (journalvoucherdetail.mDebitCredit != journalvoucherdetailOld.mDebitCredit)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucherdetail);
                audit.mField = "Debit Credit";
                audit.mOldValue = journalvoucherdetailOld.mDebitCredit.ToString();
                audit.mNewValue = journalvoucherdetail.mDebitCredit.ToString();
                audit_collection.Add(audit);
            }

            if (journalvoucherdetail.mAmount != journalvoucherdetailOld.mAmount)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucherdetail);
                audit.mField = "Amount";
                audit.mOldValue = journalvoucherdetailOld.mAmount.ToString();
                audit.mNewValue = journalvoucherdetail.mAmount.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, JournalVoucherDetail journalvoucherdetail)
        {
            audit.mUserId = journalvoucherdetail.mUserId;
            audit.mTableId = (int)(Tables.amQt_JournalVoucherDetail);
            audit.mRowId = journalvoucherdetail.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
