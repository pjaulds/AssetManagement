using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AveragingMethodAudit
    {

        public static AuditCollection Audit(AveragingMethod averagingmethod, AveragingMethod averagingmethodOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (averagingmethod.mCode != averagingmethodOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, averagingmethod);
                audit.mField = "code";
                audit.mOldValue = averagingmethodOld.mCode.ToString();
                audit.mNewValue = averagingmethod.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (averagingmethod.mName != averagingmethodOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, averagingmethod);
                audit.mField = "name";
                audit.mOldValue = averagingmethodOld.mName.ToString();
                audit.mNewValue = averagingmethod.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AveragingMethod averagingmethod)
        {
            audit.mUserId = averagingmethod.mUserId;
            audit.mTableId = (int)(Tables.amQt_AveragingMethod);
            audit.mRowId = averagingmethod.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}