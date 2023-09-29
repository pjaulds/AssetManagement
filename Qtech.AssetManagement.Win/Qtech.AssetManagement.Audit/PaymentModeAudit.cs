using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PaymentModeAudit
    {

        public static AuditCollection Audit(PaymentMode paymentMode, PaymentMode paymentModeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (paymentMode.mCode != paymentModeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, paymentMode);
                audit.mField = "code";
                audit.mOldValue = paymentModeOld.mCode.ToString();
                audit.mNewValue = paymentMode.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (paymentMode.mName != paymentModeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, paymentMode);
                audit.mField = "name";
                audit.mOldValue = paymentModeOld.mName.ToString();
                audit.mNewValue = paymentMode.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, PaymentMode paymentMode)
        {
            audit.mUserId = paymentMode.mUserId;
            audit.mTableId = (int)(Tables.amQt_PaymentMode);
            audit.mRowId = paymentMode.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}