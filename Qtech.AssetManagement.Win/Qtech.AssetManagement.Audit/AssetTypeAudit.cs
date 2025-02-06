using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AssetTypeAudit
    {

        public static AuditCollection Audit(AssetType assetType, AssetType assetTypeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (assetType.mCode != assetTypeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Code";
                audit.mOldValue = assetTypeOld.mCode.ToString();
                audit.mNewValue = assetType.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mName != assetTypeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Name";
                audit.mOldValue = assetTypeOld.mName.ToString();
                audit.mNewValue = assetType.mName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mPost != assetTypeOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Post";
                audit.mOldValue = assetTypeOld.mPost.ToString();
                audit.mNewValue = assetType.mPost.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mAssetAccountId != assetTypeOld.mAssetAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Asset Account ";
                audit.mOldValue = assetTypeOld.mAssetAccountName.ToString();
                audit.mNewValue = assetType.mAssetAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mAccumulatedDepreciationAccountId != assetTypeOld.mAccumulatedDepreciationAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Accumulated Depreciation Account ";
                audit.mOldValue = assetTypeOld.mAccumulatedDepreciationAccountName.ToString();
                audit.mNewValue = assetType.mAccumulatedDepreciationAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mProductionDepreciationExpenseAccountId != assetTypeOld.mProductionDepreciationExpenseAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Production Depreciation Expense Account ";
                audit.mOldValue = assetTypeOld.mProductionDepreciationExpenseAccountName.ToString();
                audit.mNewValue = assetType.mProductionDepreciationExpenseAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mProductionDepreciationExpenseAccountValue != assetTypeOld.mProductionDepreciationExpenseAccountValue)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Production Depreciation Expense Account Value";
                audit.mOldValue = assetTypeOld.mProductionDepreciationExpenseAccountValue.ToString();
                audit.mNewValue = assetType.mProductionDepreciationExpenseAccountValue.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mAdminDepreciationExpenseAccountId != assetTypeOld.mAdminDepreciationExpenseAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Admin Depreciation Expense Account ";
                audit.mOldValue = assetTypeOld.mAdminDepreciationExpenseAccountName.ToString();
                audit.mNewValue = assetType.mAdminDepreciationExpenseAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mAdminDepreciationExpenseAccountValue != assetTypeOld.mAdminDepreciationExpenseAccountValue)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Admin Depreciation Expense Account Value";
                audit.mOldValue = assetTypeOld.mAdminDepreciationExpenseAccountValue.ToString();
                audit.mNewValue = assetType.mAdminDepreciationExpenseAccountValue.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mDepreciationMethodId != assetTypeOld.mDepreciationMethodId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Depreciation Method ";
                audit.mOldValue = assetTypeOld.mDepreciationMethodName.ToString();
                audit.mNewValue = assetType.mDepreciationMethodName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mAveragingMethodId != assetTypeOld.mAveragingMethodId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Averaging Method ";
                audit.mOldValue = assetTypeOld.mAveragingMethodName.ToString();
                audit.mNewValue = assetType.mAveragingMethodName.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mMonths != assetTypeOld.mMonths)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Months";
                audit.mOldValue = assetTypeOld.mMonths.ToString();
                audit.mNewValue = assetType.mMonths.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mUsefulLifeYears != assetTypeOld.mUsefulLifeYears)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Useful Life Years";
                audit.mOldValue = assetTypeOld.mUsefulLifeYears.ToString();
                audit.mNewValue = assetType.mUsefulLifeYears.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mActive != assetTypeOld.mActive)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Active";
                audit.mOldValue = assetTypeOld.mActive.ToString();
                audit.mNewValue = assetType.mActive.ToString();
                audit_collection.Add(audit);
            }

            if (assetType.mDepreciable != assetTypeOld.mDepreciable)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, assetType);
                audit.mField = "Depreciable";
                audit.mOldValue = assetTypeOld.mDepreciable.ToString();
                audit.mNewValue = assetType.mDepreciable.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AssetType assetType)
        {
            audit.mUserId = assetType.mUserId;
            audit.mTableId = (int)(Tables.amQt_AssetType);
            audit.mRowId = assetType.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}