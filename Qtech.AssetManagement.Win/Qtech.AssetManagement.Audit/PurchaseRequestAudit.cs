using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PurchaseRequestAudit
    {

        public static AuditCollection Audit(PurchaseRequest purchaserequest, PurchaseRequest purchaserequestOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            
            if (purchaserequest.mRequestedById != purchaserequestOld.mRequestedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequest);
                audit.mField = "Requested By ";
                audit.mOldValue = purchaserequestOld.mRequestedByName.ToString();
                audit.mNewValue = purchaserequest.mRequestedByName.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequest.mDateRequired != purchaserequestOld.mDateRequired)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequest);
                audit.mField = "Date Required";
                audit.mOldValue = purchaserequestOld.mDateRequired.ToString();
                audit.mNewValue = purchaserequest.mDateRequired.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequest.mSupplier1Id != purchaserequestOld.mSupplier1Id)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequest);
                audit.mField = "Supplier 1 ";
                audit.mOldValue = purchaserequestOld.mSupplier1Name.ToString();
                audit.mNewValue = purchaserequest.mSupplier1Name.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequest.mSupplier2Id != purchaserequestOld.mSupplier2Id)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequest);
                audit.mField = "Supplier 2 ";
                audit.mOldValue = purchaserequestOld.mSupplier2Name.ToString();
                audit.mNewValue = purchaserequest.mSupplier2Name.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequest.mSupplier3Id != purchaserequestOld.mSupplier3Id)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequest);
                audit.mField = "Supplier 3 ";
                audit.mOldValue = purchaserequestOld.mSupplier3Name.ToString();
                audit.mNewValue = purchaserequest.mSupplier3Name.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequest.mRemarks != purchaserequestOld.mRemarks)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequest);
                audit.mField = "Remarks";
                audit.mOldValue = purchaserequestOld.mRemarks.ToString();
                audit.mNewValue = purchaserequest.mRemarks.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequest.mApprovedById != purchaserequestOld.mApprovedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequest);
                audit.mField = "Approved By ";
                audit.mOldValue = purchaserequestOld.mApprovedByName.ToString();
                audit.mNewValue = purchaserequest.mApprovedByName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, PurchaseRequest purchaserequest)
        {
            audit.mUserId = purchaserequest.mUserId;
            audit.mTableId = (int)(Tables.amQt_PurchaseRequest);
            audit.mRowId = purchaserequest.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}