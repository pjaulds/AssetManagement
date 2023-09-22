using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ReceivingDetailAudit
    {

        public static AuditCollection Audit(ReceivingDetail receivingdetail, ReceivingDetail receivingdetailOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            

            if (receivingdetail.mQuantity != receivingdetailOld.mQuantity)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receivingdetail);
                audit.mField = "Quantity";
                audit.mOldValue = receivingdetailOld.mQuantity.ToString();
                audit.mNewValue = receivingdetail.mQuantity.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, ReceivingDetail receivingdetail)
        {
            audit.mUserId = receivingdetail.mUserId;
            audit.mTableId = (int)(Tables.amQt_ReceivingDetail);
            audit.mRowId = receivingdetail.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}