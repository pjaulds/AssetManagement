using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class SupplierAudit
    {

        public static AuditCollection Audit(Supplier supplier, Supplier supplierOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (supplier.mCode != supplierOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "code";
                audit.mOldValue = supplierOld.mCode.ToString();
                audit.mNewValue = supplier.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mName != supplierOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "name";
                audit.mOldValue = supplierOld.mName.ToString();
                audit.mNewValue = supplier.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Supplier supplier)
        {
            audit.mUserId = supplier.mUserId;
            audit.mTableId = (int)(Tables.amQt_Supplier);
            audit.mRowId = supplier.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}