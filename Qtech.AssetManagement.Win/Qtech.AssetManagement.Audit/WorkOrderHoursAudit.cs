using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class WorkOrderHoursAudit
    {

        public static AuditCollection Audit(WorkOrderHours workorderhours, WorkOrderHours workorderhoursOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (workorderhours.mExpenseCategoryId != workorderhoursOld.mExpenseCategoryId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorderhours);
                audit.mField = "Expense Category ";
                audit.mOldValue = workorderhoursOld.mExpenseCategoryName.ToString();
                audit.mNewValue = workorderhours.mExpenseCategoryName.ToString();
                audit_collection.Add(audit);
            }

            if (workorderhours.mHours != workorderhoursOld.mHours)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorderhours);
                audit.mField = "Hours";
                audit.mOldValue = workorderhoursOld.mHours.ToString();
                audit.mNewValue = workorderhours.mHours.ToString();
                audit_collection.Add(audit);
            }

            if (workorderhours.mRatePerHour != workorderhoursOld.mRatePerHour)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, workorderhours);
                audit.mField = "Rate Per Hour";
                audit.mOldValue = workorderhoursOld.mRatePerHour.ToString();
                audit.mNewValue = workorderhours.mRatePerHour.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, WorkOrderHours workorderhours)
        {
            audit.mUserId = workorderhours.mUserId;
            audit.mTableId = (int)(Tables.amQt_WorkOrderHours);
            audit.mRowId = workorderhours.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}