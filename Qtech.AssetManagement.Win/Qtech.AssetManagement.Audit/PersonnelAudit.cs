using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PersonnelAudit
    {

        public static AuditCollection Audit(Personnel personnel, Personnel personnelOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (personnel.mCode != personnelOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, personnel);
                audit.mField = "code";
                audit.mOldValue = personnelOld.mCode.ToString();
                audit.mNewValue = personnel.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (personnel.mName != personnelOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, personnel);
                audit.mField = "name";
                audit.mOldValue = personnelOld.mName.ToString();
                audit.mNewValue = personnel.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Personnel personnel)
        {
            audit.mUserId = personnel.mUserId;
            audit.mTableId = (int)(Tables.amQt_Personnel);
            audit.mRowId = personnel.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}