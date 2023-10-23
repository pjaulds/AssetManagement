using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class WorkOrderTypeAudit
    {

        public static AuditCollection Audit(WorkOrderType workOrderType, WorkOrderType workOrderTypeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (workOrderType.mCode != workOrderTypeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workOrderType);
                audit.mField = "code";
                audit.mOldValue = workOrderTypeOld.mCode.ToString();
                audit.mNewValue = workOrderType.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (workOrderType.mName != workOrderTypeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workOrderType);
                audit.mField = "name";
                audit.mOldValue = workOrderTypeOld.mName.ToString();
                audit.mNewValue = workOrderType.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, WorkOrderType workOrderType)
        {
            audit.mUserId = workOrderType.mUserId;
            audit.mTableId = (int)(Tables.amQt_WorkOrderType);
            audit.mRowId = workOrderType.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}