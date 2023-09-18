using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class QuotationDetailAudit
    {

        public static AuditCollection Audit(QuotationDetail quotationdetail, QuotationDetail quotationdetailOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (quotationdetail.mQuotationId != quotationdetailOld.mQuotationId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotationdetail);
                audit.mField = "Quotation ";
                audit.mOldValue = quotationdetailOld.mQuotationName.ToString();
                audit.mNewValue = quotationdetail.mQuotationName.ToString();
                audit_collection.Add(audit);
            }
            
            if (quotationdetail.mCost1 != quotationdetailOld.mCost1)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotationdetail);
                audit.mField = "Cost1";
                audit.mOldValue = quotationdetailOld.mCost1.ToString();
                audit.mNewValue = quotationdetail.mCost1.ToString();
                audit_collection.Add(audit);
            }

            if (quotationdetail.mCost2 != quotationdetailOld.mCost2)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotationdetail);
                audit.mField = "Cost2";
                audit.mOldValue = quotationdetailOld.mCost2.ToString();
                audit.mNewValue = quotationdetail.mCost2.ToString();
                audit_collection.Add(audit);
            }

            if (quotationdetail.mCost3 != quotationdetailOld.mCost3)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, quotationdetail);
                audit.mField = "Cost3";
                audit.mOldValue = quotationdetailOld.mCost3.ToString();
                audit.mNewValue = quotationdetail.mCost3.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, QuotationDetail quotationdetail)
        {
            audit.mUserId = quotationdetail.mUserId;
            audit.mTableId = (int)(Tables.amQt_QuotationDetail);
            audit.mRowId = quotationdetail.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}