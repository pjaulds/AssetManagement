using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FixedAssetCapitalizedCostAudit
    {

        public static AuditCollection Audit(FixedAssetCapitalizedCost fixedassetcapitalizedcost, FixedAssetCapitalizedCost fixedassetcapitalizedcostOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (fixedassetcapitalizedcost.mFixedAssetId != fixedassetcapitalizedcostOld.mFixedAssetId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Fixed Asset ";
                audit.mOldValue = fixedassetcapitalizedcostOld.mFixedAssetName.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mFixedAssetName.ToString();
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

            if (fixedassetcapitalizedcost.mAmount != fixedassetcapitalizedcostOld.mAmount)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetcapitalizedcost);
                audit.mField = "Amount";
                audit.mOldValue = fixedassetcapitalizedcostOld.mAmount.ToString();
                audit.mNewValue = fixedassetcapitalizedcost.mAmount.ToString();
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