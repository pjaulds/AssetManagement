using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class WorkOrderAudit
    {

        public static AuditCollection Audit(WorkOrder workorder, WorkOrder workorderOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (workorder.mExpectedStartDate != workorderOld.mExpectedStartDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorder);
                audit.mField = "Expected Start Date";
                audit.mOldValue = workorderOld.mExpectedStartDate.ToString();
                audit.mNewValue = workorder.mExpectedStartDate.ToString();
                audit_collection.Add(audit);
            }

            if (workorder.mExpectedEndDate != workorderOld.mExpectedEndDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorder);
                audit.mField = "Expected End Date";
                audit.mOldValue = workorderOld.mExpectedEndDate.ToString();
                audit.mNewValue = workorder.mExpectedEndDate.ToString();
                audit_collection.Add(audit);
            }
            

            if (workorder.mMaintenanceRequestId != workorderOld.mMaintenanceRequestId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorder);
                audit.mField = "Maintenance Request ";
                audit.mOldValue = workorderOld.mMaintenanceRequestNo.ToString();
                audit.mNewValue = workorder.mMaintenanceRequestNo.ToString();
                audit_collection.Add(audit);
            }

            if (workorder.mWorkOrderTypeId != workorderOld.mWorkOrderTypeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorder);
                audit.mField = "Work Order Type ";
                audit.mOldValue = workorderOld.mWorkOrderTypeName.ToString();
                audit.mNewValue = workorder.mWorkOrderTypeName.ToString();
                audit_collection.Add(audit);
            }

            if (workorder.mMaintenanceJobTypeVariantId != workorderOld.mMaintenanceJobTypeVariantId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorder);
                audit.mField = "Maintenance Job Type Variant ";
                audit.mOldValue = workorderOld.mMaintenanceJobTypeVariantName.ToString();
                audit.mNewValue = workorder.mMaintenanceJobTypeVariantName.ToString();
                audit_collection.Add(audit);
            }

            if (workorder.mTradeId != workorderOld.mTradeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorder);
                audit.mField = "Trade ";
                audit.mOldValue = workorderOld.mTradeName.ToString();
                audit.mNewValue = workorder.mTradeName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, WorkOrder workorder)
        {
            audit.mUserId = workorder.mUserId;
            audit.mTableId = (int)(Tables.amQt_WorkOrder);
            audit.mRowId = workorder.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}