using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PurchaseOrderDetailAudit
    {

        public static AuditCollection Audit(PurchaseOrderDetail purchaseorderdetail, PurchaseOrderDetail purchaseorderdetailOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            

            if (purchaseorderdetail.mQuantity != purchaseorderdetailOld.mQuantity)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchaseorderdetail);
                audit.mField = "Quantity";
                audit.mOldValue = purchaseorderdetailOld.mQuantity.ToString();
                audit.mNewValue = purchaseorderdetail.mQuantity.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, PurchaseOrderDetail purchaseorderdetail)
        {
            audit.mUserId = purchaseorderdetail.mUserId;
            audit.mTableId = (int)(Tables.amQt_PurchaseOrderDetail);
            audit.mRowId = purchaseorderdetail.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
