using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class DepreciationMethodAudit
    {

        public static AuditCollection Audit(DepreciationMethod depreciationmethod, DepreciationMethod depreciationmethodOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (depreciationmethod.mCode != depreciationmethodOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationmethod);
                audit.mField = "code";
                audit.mOldValue = depreciationmethodOld.mCode.ToString();
                audit.mNewValue = depreciationmethod.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationmethod.mName != depreciationmethodOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationmethod);
                audit.mField = "name";
                audit.mOldValue = depreciationmethodOld.mName.ToString();
                audit.mNewValue = depreciationmethod.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, DepreciationMethod depreciationmethod)
        {
            audit.mUserId = depreciationmethod.mUserId;
            audit.mTableId = (int)(Tables.amQt_DepreciationMethod);
            audit.mRowId = depreciationmethod.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}