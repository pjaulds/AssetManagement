using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AssetTypeAudit
    {

        public static AuditCollection Audit(AssetType assetType, AssetType assetTypeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (assetType.mCode != assetTypeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "code";
                audit.mOldValue = assetTypeOld.mCode.ToString();
                audit.mNewValue = assetType.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mName != assetTypeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "name";
                audit.mOldValue = assetTypeOld.mName.ToString();
                audit.mNewValue = assetType.mName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mPost != assetTypeOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "post";
                audit.mOldValue = assetTypeOld.mPost.ToString();
                audit.mNewValue = assetType.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AssetType assetType)
        {
            audit.mUserId = assetType.mUserId;
            audit.mTableId = (int)(Tables.amQt_AssetType);
            audit.mRowId = assetType.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}