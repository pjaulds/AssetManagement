using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FaultSymptomsAudit
    {

        public static AuditCollection Audit(FaultSymptoms faultSymptoms, FaultSymptoms faultSymptomsOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (faultSymptoms.mCode != faultSymptomsOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, faultSymptoms);
                audit.mField = "code";
                audit.mOldValue = faultSymptomsOld.mCode.ToString();
                audit.mNewValue = faultSymptoms.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (faultSymptoms.mName != faultSymptomsOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, faultSymptoms);
                audit.mField = "name";
                audit.mOldValue = faultSymptomsOld.mName.ToString();
                audit.mNewValue = faultSymptoms.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, FaultSymptoms faultSymptoms)
        {
            audit.mUserId = faultSymptoms.mUserId;
            audit.mTableId = (int)(Tables.amQt_FaultSymptoms);
            audit.mRowId = faultSymptoms.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}