using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PurchaseVoucherAudit
    {

        public static AuditCollection Audit(PurchaseVoucher purchasevoucher, PurchaseVoucher purchasevoucherOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            
            if (purchasevoucher.mPaymentModeId != purchasevoucherOld.mPaymentModeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchasevoucher);
                audit.mField = "Payment Mode ";
                audit.mOldValue = purchasevoucherOld.mPaymentModeName.ToString();
                audit.mNewValue = purchasevoucher.mPaymentModeName.ToString();
                audit_collection.Add(audit);
            }

            if (purchasevoucher.mPreparedById != purchasevoucherOld.mPreparedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchasevoucher);
                audit.mField = "Prepared By ";
                audit.mOldValue = purchasevoucherOld.mPreparedByName.ToString();
                audit.mNewValue = purchasevoucher.mPreparedByName.ToString();
                audit_collection.Add(audit);
            }

            if (purchasevoucher.mCheckedById != purchasevoucherOld.mCheckedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchasevoucher);
                audit.mField = "Checked By ";
                audit.mOldValue = purchasevoucherOld.mCheckedByName.ToString();
                audit.mNewValue = purchasevoucher.mCheckedByName.ToString();
                audit_collection.Add(audit);
            }

            if (purchasevoucher.mApprovedById != purchasevoucherOld.mApprovedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, purchasevoucher);
                audit.mField = "Approved By ";
                audit.mOldValue = purchasevoucherOld.mApprovedByName.ToString();
                audit.mNewValue = purchasevoucher.mApprovedByName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, PurchaseVoucher purchasevoucher)
        {
            audit.mUserId = purchasevoucher.mUserId;
            audit.mTableId = (int)(Tables.amQt_PurchaseVoucher);
            audit.mRowId = purchasevoucher.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}