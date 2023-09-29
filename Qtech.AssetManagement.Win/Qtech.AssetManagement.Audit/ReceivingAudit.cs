using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ReceivingAudit
    {

        public static AuditCollection Audit(Receiving receiving, Receiving receivingOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (receiving.mDate != receivingOld.mDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Date";
                audit.mOldValue = receivingOld.mDate.ToString();
                audit.mNewValue = receiving.mDate.ToString();
                audit_collection.Add(audit);
            }

            if (receiving.mNumber != receivingOld.mNumber)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Number";
                audit.mOldValue = receivingOld.mNumber.ToString();
                audit.mNewValue = receiving.mNumber.ToString();
                audit_collection.Add(audit);
            }
            
            if (receiving.mPreparedById != receivingOld.mPreparedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Prepared By ";
                audit.mOldValue = receivingOld.mPreparedByName.ToString();
                audit.mNewValue = receiving.mPreparedByName.ToString();
                audit_collection.Add(audit);
            }

            if (receiving.mCheckedById != receivingOld.mCheckedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Checked By ";
                audit.mOldValue = receivingOld.mCheckedByName.ToString();
                audit.mNewValue = receiving.mCheckedByName.ToString();
                audit_collection.Add(audit);
            }

            if (receiving.mApprovedById != receivingOld.mApprovedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Approved By ";
                audit.mOldValue = receivingOld.mApprovedByName.ToString();
                audit.mNewValue = receiving.mApprovedByName.ToString();
                audit_collection.Add(audit);
            }

            if (receiving.mInvoiceNo != receivingOld.mInvoiceNo)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Invoice No";
                audit.mOldValue = receivingOld.mInvoiceNo.ToString();
                audit.mNewValue = receiving.mInvoiceNo.ToString();
                audit_collection.Add(audit);
            }

            if (receiving.mDrNo != receivingOld.mDrNo)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Dr No";
                audit.mOldValue = receivingOld.mDrNo.ToString();
                audit.mNewValue = receiving.mDrNo.ToString();
                audit_collection.Add(audit);
            }

            if (receiving.mAmount != receivingOld.mAmount)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Amount";
                audit.mOldValue = receivingOld.mAmount.ToString();
                audit.mNewValue = receiving.mAmount.ToString();
                audit_collection.Add(audit);
            }


            if (receiving.mRemarks != receivingOld.mRemarks)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, receiving);
                audit.mField = "Remarks";
                audit.mOldValue = receivingOld.mRemarks.ToString();
                audit.mNewValue = receiving.mRemarks.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Receiving receiving)
        {
            audit.mUserId = receiving.mUserId;
            audit.mTableId = (int)(Tables.amQt_Receiving);
            audit.mRowId = receiving.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
