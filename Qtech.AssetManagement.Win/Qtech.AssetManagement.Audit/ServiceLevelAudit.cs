using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ServiceLevelAudit
    {

        public static AuditCollection Audit(ServiceLevel serviceLevel, ServiceLevel serviceLevelOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (serviceLevel.mCode != serviceLevelOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, serviceLevel);
                audit.mField = "code";
                audit.mOldValue = serviceLevelOld.mCode.ToString();
                audit.mNewValue = serviceLevel.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (serviceLevel.mName != serviceLevelOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, serviceLevel);
                audit.mField = "name";
                audit.mOldValue = serviceLevelOld.mName.ToString();
                audit.mNewValue = serviceLevel.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, ServiceLevel serviceLevel)
        {
            audit.mUserId = serviceLevel.mUserId;
            audit.mTableId = (int)(Tables.amQt_ServiceLevel);
            audit.mRowId = serviceLevel.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}