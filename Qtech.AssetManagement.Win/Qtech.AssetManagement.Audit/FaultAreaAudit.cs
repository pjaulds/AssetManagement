using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FaultAreaAudit
    {

        public static AuditCollection Audit(FaultArea faultArea, FaultArea faultAreaOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (faultArea.mCode != faultAreaOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, faultArea);
                audit.mField = "code";
                audit.mOldValue = faultAreaOld.mCode.ToString();
                audit.mNewValue = faultArea.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (faultArea.mName != faultAreaOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, faultArea);
                audit.mField = "name";
                audit.mOldValue = faultAreaOld.mName.ToString();
                audit.mNewValue = faultArea.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, FaultArea faultArea)
        {
            audit.mUserId = faultArea.mUserId;
            audit.mTableId = (int)(Tables.amQt_FaultArea);
            audit.mRowId = faultArea.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}