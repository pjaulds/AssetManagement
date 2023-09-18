using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ProductAudit
    {

        public static AuditCollection Audit(Product product, Product productOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (product.mCode != productOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, product);
                audit.mField = "code";
                audit.mOldValue = productOld.mCode.ToString();
                audit.mNewValue = product.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (product.mName != productOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, product);
                audit.mField = "name";
                audit.mOldValue = productOld.mName.ToString();
                audit.mNewValue = product.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Product product)
        {
            audit.mUserId = product.mUserId;
            audit.mTableId = (int)(Tables.amQt_Product);
            audit.mRowId = product.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}