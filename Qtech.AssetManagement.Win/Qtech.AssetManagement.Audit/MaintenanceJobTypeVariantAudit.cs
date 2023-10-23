using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class MaintenanceJobTypeVariantAudit
    {

        public static AuditCollection Audit(MaintenanceJobTypeVariant maintenanceJobTypeVariant, MaintenanceJobTypeVariant maintenanceJobTypeVariantOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (maintenanceJobTypeVariant.mMaintenanceJobTypeId != maintenanceJobTypeVariantOld.mMaintenanceJobTypeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenanceJobTypeVariant);
                audit.mField = "Maintenance Job Type";
                audit.mOldValue = maintenanceJobTypeVariantOld.mMaintenanceJobTypeName.ToString();
                audit.mNewValue = maintenanceJobTypeVariant.mMaintenanceJobTypeName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenanceJobTypeVariant.mCode != maintenanceJobTypeVariantOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenanceJobTypeVariant);
                audit.mField = "code";
                audit.mOldValue = maintenanceJobTypeVariantOld.mCode.ToString();
                audit.mNewValue = maintenanceJobTypeVariant.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (maintenanceJobTypeVariant.mName != maintenanceJobTypeVariantOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenanceJobTypeVariant);
                audit.mField = "name";
                audit.mOldValue = maintenanceJobTypeVariantOld.mName.ToString();
                audit.mNewValue = maintenanceJobTypeVariant.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, MaintenanceJobTypeVariant maintenanceJobTypeVariant)
        {
            audit.mUserId = maintenanceJobTypeVariant.mUserId;
            audit.mTableId = (int)(Tables.amQt_MaintenanceJobTypeVariant);
            audit.mRowId = maintenanceJobTypeVariant.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}