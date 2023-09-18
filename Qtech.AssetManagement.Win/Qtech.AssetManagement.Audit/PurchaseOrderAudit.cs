using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PurchaseOrderAudit
    {

        public static AuditCollection Audit(PurchaseOrder purchaseorder, PurchaseOrder purchaseorderOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            
            if (purchaseorder.mDateOfDelivery != purchaseorderOld.mDateOfDelivery)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorder);
                audit.mField = "Date Of Delivery";
                audit.mOldValue = purchaseorderOld.mDateOfDelivery.ToString();
                audit.mNewValue = purchaseorder.mDateOfDelivery.ToString();
                audit_collection.Add(audit);
            }

            if (purchaseorder.mTerms != purchaseorderOld.mTerms)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorder);
                audit.mField = "Terms";
                audit.mOldValue = purchaseorderOld.mTerms.ToString();
                audit.mNewValue = purchaseorder.mTerms.ToString();
                audit_collection.Add(audit);
            }

            if (purchaseorder.mPreparedById != purchaseorderOld.mPreparedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorder);
                audit.mField = "Prepared By ";
                audit.mOldValue = purchaseorderOld.mPreparedByName.ToString();
                audit.mNewValue = purchaseorder.mPreparedByName.ToString();
                audit_collection.Add(audit);
            }

            if (purchaseorder.mNotedById != purchaseorderOld.mNotedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorder);
                audit.mField = "Noted By ";
                audit.mOldValue = purchaseorderOld.mNotedByName.ToString();
                audit.mNewValue = purchaseorder.mNotedByName.ToString();
                audit_collection.Add(audit);
            }

            if (purchaseorder.mApprovedById != purchaseorderOld.mApprovedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorder);
                audit.mField = "Approved By ";
                audit.mOldValue = purchaseorderOld.mApprovedByName.ToString();
                audit.mNewValue = purchaseorder.mApprovedByName.ToString();
                audit_collection.Add(audit);
            }

            if (purchaseorder.mRevised != purchaseorderOld.mRevised)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorder);
                audit.mField = "Revised";
                audit.mOldValue = purchaseorderOld.mRevised.ToString();
                audit.mNewValue = purchaseorder.mRevised.ToString();
                audit_collection.Add(audit);
            }

            if (purchaseorder.mCancelled != purchaseorderOld.mCancelled)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorder);
                audit.mField = "Cancelled";
                audit.mOldValue = purchaseorderOld.mCancelled.ToString();
                audit.mNewValue = purchaseorder.mCancelled.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, PurchaseOrder purchaseorder)
        {
            audit.mUserId = purchaseorder.mUserId;
            audit.mTableId = (int)(Tables.amQt_PurchaseOrder);
            audit.mRowId = purchaseorder.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
