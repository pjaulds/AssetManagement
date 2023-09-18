using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PurchaseRequestDetailAudit
    {

        public static AuditCollection Audit(PurchaseRequestDetail purchaserequestdetail, PurchaseRequestDetail purchaserequestdetailOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (purchaserequestdetail.mPurchaseRequestId != purchaserequestdetailOld.mPurchaseRequestId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequestdetail);
                audit.mField = "Purchase Request ";
                audit.mOldValue = purchaserequestdetailOld.mPurchaseRequestName.ToString();
                audit.mNewValue = purchaserequestdetail.mPurchaseRequestName.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequestdetail.mProductId != purchaserequestdetailOld.mProductId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequestdetail);
                audit.mField = "Product ";
                audit.mOldValue = purchaserequestdetailOld.mProductName.ToString();
                audit.mNewValue = purchaserequestdetail.mProductName.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequestdetail.mQuantity != purchaserequestdetailOld.mQuantity)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequestdetail);
                audit.mField = "Quantity";
                audit.mOldValue = purchaserequestdetailOld.mQuantity.ToString();
                audit.mNewValue = purchaserequestdetail.mQuantity.ToString();
                audit_collection.Add(audit);
            }

            if (purchaserequestdetail.mCost != purchaserequestdetailOld.mCost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaserequestdetail);
                audit.mField = "Cost";
                audit.mOldValue = purchaserequestdetailOld.mCost.ToString();
                audit.mNewValue = purchaserequestdetail.mCost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, PurchaseRequestDetail purchaserequestdetail)
        {
            audit.mUserId = purchaserequestdetail.mUserId;
            audit.mTableId = (int)(Tables.amQt_PurchaseRequestDetail);
            audit.mRowId = purchaserequestdetail.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
