using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FixedAssetCapitalizedCostAudit
    {

        public static AuditCollection Audit(FixedAssetCapitalizedCost fixedassetcapitalizedcost, FixedAssetCapitalizedCost fixedassetcapitalizedcostOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            
            if (fixedassetcapitalizedcost.mDate != fixedassetcapitalizedcostOld.mDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Date";
                audit.mOldValue = fixedassetcapitalizedcostOld.mDate.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mDate.ToString();
                audit_collection.Add(audit);
            }
            

            if (fixedassetcapitalizedcost.mCapitalizedCostId != fixedassetcapitalizedcostOld.mCapitalizedCostId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Capitalized Cost ";
                audit.mOldValue = fixedassetcapitalizedcostOld.mCapitalizedCostName.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mCapitalizedCostName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetcapitalizedcost.mDescription != fixedassetcapitalizedcostOld.mDescription)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Description";
                audit.mOldValue = fixedassetcapitalizedcostOld.mDescription.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mDescription.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetcapitalizedcost.mAmount != fixedassetcapitalizedcostOld.mAmount)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Amount";
                audit.mOldValue = fixedassetcapitalizedcostOld.mAmount.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mAmount.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetcapitalizedcost.mUsefulLife != fixedassetcapitalizedcostOld.mUsefulLife)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Useful Life";
                audit.mOldValue = fixedassetcapitalizedcostOld.mUsefulLife.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mUsefulLife.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetcapitalizedcost.mIsJournalized != fixedassetcapitalizedcostOld.mIsJournalized)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Is Journalized";
                audit.mOldValue = fixedassetcapitalizedcostOld.mIsJournalized.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mIsJournalized.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetcapitalizedcost.mAssetAccountId != fixedassetcapitalizedcostOld.mAssetAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Asset Account ";
                audit.mOldValue = fixedassetcapitalizedcostOld.mAssetAccountName.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mAssetAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetcapitalizedcost.mCashPayableAccountId != fixedassetcapitalizedcostOld.mCashPayableAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Cash Payable Account ";
                audit.mOldValue = fixedassetcapitalizedcostOld.mCashPayableAccountName.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mCashPayableAccountName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, FixedAssetCapitalizedCost fixedassetcapitalizedcost)
        {
            audit.mUserId = fixedassetcapitalizedcost.mUserId;
            audit.mTableId = (int)(Tables.amQt_FixedAssetCapitalizedCost);
            audit.mRowId = fixedassetcapitalizedcost.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}