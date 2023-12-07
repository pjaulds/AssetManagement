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

            if (supplier.mAddress != supplierOld.mAddress)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Address";
                audit.mOldValue = supplierOld.mAddress.ToString();
                audit.mNewValue = supplier.mAddress.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mTin != supplierOld.mTin)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Tin";
                audit.mOldValue = supplierOld.mTin.ToString();
                audit.mNewValue = supplier.mTin.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mContactNo != supplierOld.mContactNo)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Contact No";
                audit.mOldValue = supplierOld.mContactNo.ToString();
                audit.mNewValue = supplier.mContactNo.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mEmail != supplierOld.mEmail)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Email";
                audit.mOldValue = supplierOld.mEmail.ToString();
                audit.mNewValue = supplier.mEmail.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mSalesPerson != supplierOld.mSalesPerson)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Sales Person";
                audit.mOldValue = supplierOld.mSalesPerson.ToString();
                audit.mNewValue = supplier.mSalesPerson.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mVatRegistered != supplierOld.mVatRegistered)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Vat Registered";
                audit.mOldValue = supplierOld.mVatRegistered.ToString();
                audit.mNewValue = supplier.mVatRegistered.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mVatRate != supplierOld.mVatRate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Vat Rate";
                audit.mOldValue = supplierOld.mVatRate.ToString();
                audit.mNewValue = supplier.mVatRate.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mWitholdingTax != supplierOld.mWitholdingTax)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Witholding Tax";
                audit.mOldValue = supplierOld.mWitholdingTax.ToString();
                audit.mNewValue = supplier.mWitholdingTax.ToString();
                audit_collection.Add(audit);
            }

            if (supplier.mBusinessStyle != supplierOld.mBusinessStyle)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, supplier);
                audit.mField = "Business Style";
                audit.mOldValue = supplierOld.mBusinessStyle.ToString();
                audit.mNewValue = supplier.mBusinessStyle.ToString();
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