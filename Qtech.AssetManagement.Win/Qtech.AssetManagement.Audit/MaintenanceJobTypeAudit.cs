using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class MaintenanceJobTypeAudit
    {

        public static AuditCollection Audit(MaintenanceJobType maintenanceJobType, MaintenanceJobType maintenanceJobTypeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (maintenanceJobType.mCode != maintenanceJobTypeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenanceJobType);
                audit.mField = "code";
                audit.mOldValue = maintenanceJobTypeOld.mCode.ToString();
                audit.mNewValue = maintenanceJobType.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (maintenanceJobType.mName != maintenanceJobTypeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenanceJobType);
                audit.mField = "name";
                audit.mOldValue = maintenanceJobTypeOld.mName.ToString();
                audit.mNewValue = maintenanceJobType.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, MaintenanceJobType maintenanceJobType)
        {
            audit.mUserId = maintenanceJobType.mUserId;
            audit.mTableId = (int)(Tables.amQt_MaintenanceJobType);
            audit.mRowId = maintenanceJobType.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}