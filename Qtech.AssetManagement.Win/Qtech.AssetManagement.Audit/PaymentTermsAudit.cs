using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class PaymentTermsAudit
    {

        public static AuditCollection Audit(PaymentTerms paymentTerms, PaymentTerms paymentTermsOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            
            if (paymentTerms.mName != paymentTermsOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, paymentTerms);
                audit.mField = "name";
                audit.mOldValue = paymentTermsOld.mName.ToString();
                audit.mNewValue = paymentTerms.mName.ToString();
                audit_collection.Add(audit);
            }

            if (paymentTerms.mRemarks != paymentTermsOld.mRemarks)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, paymentTerms);
                audit.mField = "remarks";
                audit.mOldValue = paymentTermsOld.mRemarks.ToString();
                audit.mNewValue = paymentTerms.mRemarks.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, PaymentTerms paymentTerms)
        {
            audit.mUserId = paymentTerms.mUserId;
            audit.mTableId = (int)(Tables.amQt_PaymentTerms);
            audit.mRowId = paymentTerms.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}