using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AssetClassAudit
    {

        public static AuditCollection Audit(AssetClass assetClass, AssetClass assetClassOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (assetClass.mCode != assetClassOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetClass);
                audit.mField = "code";
                audit.mOldValue = assetClassOld.mCode.ToString();
                audit.mNewValue = assetClass.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (assetClass.mName != assetClassOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetClass);
                audit.mField = "name";
                audit.mOldValue = assetClassOld.mName.ToString();
                audit.mNewValue = assetClass.mName.ToString();
                audit_collection.Add(audit);
            }

            if (assetClass.mPost != assetClassOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetClass);
                audit.mField = "post";
                audit.mOldValue = assetClassOld.mPost.ToString();
                audit.mNewValue = assetClass.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AssetClass assetClass)
        {
            audit.mUserId = assetClass.mUserId;
            audit.mTableId = (int)(Tables.amQt_AssetClass);
            audit.mRowId = assetClass.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}