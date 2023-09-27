using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class UnitAudit
    {

        public static AuditCollection Audit(Unit unit, Unit unitOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (unit.mCode != unitOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, unit);
                audit.mField = "code";
                audit.mOldValue = unitOld.mCode.ToString();
                audit.mNewValue = unit.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (unit.mName != unitOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, unit);
                audit.mField = "name";
                audit.mOldValue = unitOld.mName.ToString();
                audit.mNewValue = unit.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Unit unit)
        {
            audit.mUserId = unit.mUserId;
            audit.mTableId = (int)(Tables.amQt_Unit);
            audit.mRowId = unit.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}