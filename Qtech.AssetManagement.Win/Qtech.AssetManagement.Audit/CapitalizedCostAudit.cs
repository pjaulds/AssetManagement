using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class CapitalizedCostAudit
    {

        public static AuditCollection Audit(CapitalizedCost capitalizedCost, CapitalizedCost capitalizedCostOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (capitalizedCost.mCode != capitalizedCostOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, capitalizedCost);
                audit.mField = "code";
                audit.mOldValue = capitalizedCostOld.mCode.ToString();
                audit.mNewValue = capitalizedCost.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (capitalizedCost.mName != capitalizedCostOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, capitalizedCost);
                audit.mField = "name";
                audit.mOldValue = capitalizedCostOld.mName.ToString();
                audit.mNewValue = capitalizedCost.mName.ToString();
                audit_collection.Add(audit);
            }

            if (capitalizedCost.mPost != capitalizedCostOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, capitalizedCost);
                audit.mField = "post";
                audit.mOldValue = capitalizedCostOld.mPost.ToString();
                audit.mNewValue = capitalizedCost.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, CapitalizedCost capitalizedCost)
        {
            audit.mUserId = capitalizedCost.mUserId;
            audit.mTableId = (int)(Tables.amQt_CapitalizedCost);
            audit.mRowId = capitalizedCost.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}