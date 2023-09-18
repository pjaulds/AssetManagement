using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AssetAccountAudit
    {

        public static AuditCollection Audit(AssetAccount assetaccount, AssetAccount assetaccountOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (assetaccount.mCode != assetaccountOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetaccount);
                audit.mField = "code";
                audit.mOldValue = assetaccountOld.mCode.ToString();
                audit.mNewValue = assetaccount.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (assetaccount.mName != assetaccountOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetaccount);
                audit.mField = "name";
                audit.mOldValue = assetaccountOld.mName.ToString();
                audit.mNewValue = assetaccount.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AssetAccount assetaccount)
        {
            audit.mUserId = assetaccount.mUserId;
            audit.mTableId = (int)(Tables.amQt_AssetAccount);
            audit.mRowId = assetaccount.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}