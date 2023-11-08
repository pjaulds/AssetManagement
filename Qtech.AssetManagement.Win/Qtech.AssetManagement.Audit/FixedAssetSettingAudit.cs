using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FixedAssetSettingAudit
    {

        public static AuditCollection Audit(FixedAssetSetting fixedassetsetting, FixedAssetSetting fixedassetsettingOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (fixedassetsetting.mAssetTypeId != fixedassetsettingOld.mAssetTypeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Asset Type";
                audit.mOldValue = fixedassetsettingOld.mAssetTypeName.ToString();
                audit.mNewValue = fixedassetsetting.mAssetTypeName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetsetting.mAssetClassId != fixedassetsettingOld.mAssetClassId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Asset Class ";
                audit.mOldValue = fixedassetsettingOld.mAssetClassName.ToString();
                audit.mNewValue = fixedassetsetting.mAssetClassName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetsetting.mChartOfAccountId != fixedassetsettingOld.mChartOfAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Chart Of Account ";
                audit.mOldValue = fixedassetsettingOld.mChartOfAccountName.ToString();
                audit.mNewValue = fixedassetsetting.mChartOfAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetsetting.mAccumulatedDepreciationAccountId != fixedassetsettingOld.mAccumulatedDepreciationAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Accumulated Depreciation Account ";
                audit.mOldValue = fixedassetsettingOld.mAccumulatedDepreciationAccountName.ToString();
                audit.mNewValue = fixedassetsetting.mAccumulatedDepreciationAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetsetting.mDepreciationExpenseAccountId != fixedassetsettingOld.mDepreciationExpenseAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Depreciation Expense Account ";
                audit.mOldValue = fixedassetsettingOld.mDepreciationExpenseAccountName.ToString();
                audit.mNewValue = fixedassetsetting.mDepreciationExpenseAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetsetting.mDepreciationMethodId != fixedassetsettingOld.mDepreciationMethodId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Depreciation Method ";
                audit.mOldValue = fixedassetsettingOld.mDepreciationMethodName.ToString();
                audit.mNewValue = fixedassetsetting.mDepreciationMethodName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetsetting.mAveragingMethodId != fixedassetsettingOld.mAveragingMethodId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Averaging Method ";
                audit.mOldValue = fixedassetsettingOld.mAveragingMethodName.ToString();
                audit.mNewValue = fixedassetsetting.mAveragingMethodName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedassetsetting.mUsefulLifeYears != fixedassetsettingOld.mUsefulLifeYears)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsetting);
                audit.mField = "Useful Life Years";
                audit.mOldValue = fixedassetsettingOld.mUsefulLifeYears.ToString();
                audit.mNewValue = fixedassetsetting.mUsefulLifeYears.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, FixedAssetSetting fixedassetsetting)
        {
            audit.mUserId = fixedassetsetting.mUserId;
            audit.mTableId = (int)(Tables.amQt_FixedAssetSetting);
            audit.mRowId = fixedassetsetting.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}