using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class MaintenanceRequestTypeAudit
    {

        public static AuditCollection Audit(MaintenanceRequestType maintenanceRequestType, MaintenanceRequestType maintenanceRequestTypeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (maintenanceRequestType.mCode != maintenanceRequestTypeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenanceRequestType);
                audit.mField = "code";
                audit.mOldValue = maintenanceRequestTypeOld.mCode.ToString();
                audit.mNewValue = maintenanceRequestType.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (maintenanceRequestType.mName != maintenanceRequestTypeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenanceRequestType);
                audit.mField = "name";
                audit.mOldValue = maintenanceRequestTypeOld.mName.ToString();
                audit.mNewValue = maintenanceRequestType.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, MaintenanceRequestType maintenanceRequestType)
        {
            audit.mUserId = maintenanceRequestType.mUserId;
            audit.mTableId = (int)(Tables.amQt_MaintenanceRequestType);
            audit.mRowId = maintenanceRequestType.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}