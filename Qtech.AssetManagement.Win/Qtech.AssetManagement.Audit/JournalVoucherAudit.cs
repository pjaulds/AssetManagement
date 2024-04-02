using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class JournalVoucherAudit
    {

        public static AuditCollection Audit(JournalVoucher journalvoucher, JournalVoucher journalvoucherOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (journalvoucher.mDate != journalvoucherOld.mDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucher);
                audit.mField = "Date";
                audit.mOldValue = journalvoucherOld.mDate.ToString();
                audit.mNewValue = journalvoucher.mDate.ToString();
                audit_collection.Add(audit);
            }
            
            if (journalvoucher.mSupplierId != journalvoucherOld.mSupplierId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucher);
                audit.mField = "Supplier ";
                audit.mOldValue = journalvoucherOld.mSupplierName.ToString();
                audit.mNewValue = journalvoucher.mSupplierName.ToString();
                audit_collection.Add(audit);
            }

            if (journalvoucher.mType != journalvoucherOld.mType)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucher);
                audit.mField = "Type";
                audit.mOldValue = journalvoucherOld.mType.ToString();
                audit.mNewValue = journalvoucher.mType.ToString();
                audit_collection.Add(audit);
            }

            if (journalvoucher.mDetails != journalvoucherOld.mDetails)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucher);
                audit.mField = "Details";
                audit.mOldValue = journalvoucherOld.mDetails.ToString();
                audit.mNewValue = journalvoucher.mDetails.ToString();
                audit_collection.Add(audit);
            }

            if (journalvoucher.mPost != journalvoucherOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, journalvoucher);
                audit.mField = "Post";
                audit.mOldValue = journalvoucherOld.mPost.ToString();
                audit.mNewValue = journalvoucher.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, JournalVoucher journalvoucher)
        {
            audit.mUserId = journalvoucher.mUserId;
            audit.mTableId = (int)(Tables.amQt_JournalVoucher);
            audit.mRowId = journalvoucher.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
