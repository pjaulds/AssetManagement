using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AssetCapitalizeAudit
    {

        public static AuditCollection Audit(AssetCapitalize assetcapitalize, AssetCapitalize assetcapitalizeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (assetcapitalize.mAssetId != assetcapitalizeOld.mAssetId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Asset ";
                audit.mOldValue = assetcapitalizeOld.mAssetName.ToString();
                audit.mNewValue = assetcapitalize.mAssetName.ToString();
                audit_collection.Add(audit);
            }

            if (assetcapitalize.mDate != assetcapitalizeOld.mDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Date";
                audit.mOldValue = assetcapitalizeOld.mDate.ToString();
                audit.mNewValue = assetcapitalize.mDate.ToString();
                audit_collection.Add(audit);
            }

            if (assetcapitalize.mNumber != assetcapitalizeOld.mNumber)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Number";
                audit.mOldValue = assetcapitalizeOld.mNumber.ToString();
                audit.mNewValue = assetcapitalize.mNumber.ToString();
                audit_collection.Add(audit);
            }

            if (assetcapitalize.mCapitalizedCostId != assetcapitalizeOld.mCapitalizedCostId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Capitalized Cost ";
                audit.mOldValue = assetcapitalizeOld.mCapitalizedCostName.ToString();
                audit.mNewValue = assetcapitalize.mCapitalizedCostName.ToString();
                audit_collection.Add(audit);
            }

            if (assetcapitalize.mDescription != assetcapitalizeOld.mDescription)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Description";
                audit.mOldValue = assetcapitalizeOld.mDescription.ToString();
                audit.mNewValue = assetcapitalize.mDescription.ToString();
                audit_collection.Add(audit);
            }

            if (assetcapitalize.mAmount != assetcapitalizeOld.mAmount)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Amount";
                audit.mOldValue = assetcapitalizeOld.mAmount.ToString();
                audit.mNewValue = assetcapitalize.mAmount.ToString();
                audit_collection.Add(audit);
            }

            if (assetcapitalize.mUsefulLife != assetcapitalizeOld.mUsefulLife)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Useful Life";
                audit.mOldValue = assetcapitalizeOld.mUsefulLife.ToString();
                audit.mNewValue = assetcapitalize.mUsefulLife.ToString();
                audit_collection.Add(audit);
            }

            if (assetcapitalize.mJournalized != assetcapitalizeOld.mJournalized)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetcapitalize);
                audit.mField = "Journalized";
                audit.mOldValue = assetcapitalizeOld.mJournalized.ToString();
                audit.mNewValue = assetcapitalize.mJournalized.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AssetCapitalize assetcapitalize)
        {
            audit.mUserId = assetcapitalize.mUserId;
            audit.mTableId = (int)(Tables.amQt_AssetCapitalize);
            audit.mRowId = assetcapitalize.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
