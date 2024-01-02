using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class QuotationAudit
    {

        public static AuditCollection Audit(Quotation quotation, Quotation quotationOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            
            if (quotation.mPurchaseRequestId != quotationOld.mPurchaseRequestId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotation);
                audit.mField = "Purchase Request ";
                audit.mOldValue = quotationOld.mPurchaseRequestNo.ToString();
                audit.mNewValue = quotation.mPurchaseRequestNo.ToString();
                audit_collection.Add(audit);
            }

            if (quotation.mPreparedById != quotationOld.mPreparedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotation);
                audit.mField = "Prepared By ";
                audit.mOldValue = quotationOld.mPreparedByName.ToString();
                audit.mNewValue = quotation.mPreparedByName.ToString();
                audit_collection.Add(audit);
            }

            if (quotation.mCheckedById != quotationOld.mCheckedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotation);
                audit.mField = "Checked By";
                audit.mOldValue = quotationOld.mCheckedByName.ToString();
                audit.mNewValue = quotation.mCheckedByName.ToString();
                audit_collection.Add(audit);
            }

            if (quotation.mApprovedById != quotationOld.mApprovedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotation);
                audit.mField = "Approved By ";
                audit.mOldValue = quotationOld.mApprovedByName.ToString();
                audit.mNewValue = quotation.mApprovedByName.ToString();
                audit_collection.Add(audit);
            }

            if (quotation.mSupplierNo != quotationOld.mSupplierNo)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotation);
                audit.mField = "Supplier No";
                audit.mOldValue = quotationOld.mSupplierNo.ToString();
                audit.mNewValue = quotation.mSupplierNo.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Quotation quotation)
        {
            audit.mUserId = quotation.mUserId;
            audit.mTableId = (int)(Tables.amQt_Quotation);
            audit.mRowId = quotation.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
