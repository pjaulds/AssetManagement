using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AssetCategoryAudit
    {

        public static AuditCollection Audit(AssetCategory assetCategory, AssetCategory assetCategoryOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (assetCategory.mCode != assetCategoryOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetCategory);
                audit.mField = "code";
                audit.mOldValue = assetCategoryOld.mCode.ToString();
                audit.mNewValue = assetCategory.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (assetCategory.mName != assetCategoryOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetCategory);
                audit.mField = "name";
                audit.mOldValue = assetCategoryOld.mName.ToString();
                audit.mNewValue = assetCategory.mName.ToString();
                audit_collection.Add(audit);
            }

            if (assetCategory.mPost != assetCategoryOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetCategory);
                audit.mField = "post";
                audit.mOldValue = assetCategoryOld.mPost.ToString();
                audit.mNewValue = assetCategory.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AssetCategory assetCategory)
        {
            audit.mUserId = assetCategory.mUserId;
            audit.mTableId = (int)(Tables.amQt_AssetCategory);
            audit.mRowId = assetCategory.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}