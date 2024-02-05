using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FixedAssetAudit
    {

        public static AuditCollection Audit(FixedAsset fixedasset, FixedAsset fixedassetOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (fixedasset.mAssetNo != fixedassetOld.mAssetNo)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Asset No";
                audit.mOldValue = fixedassetOld.mAssetNo.ToString();
                audit.mNewValue = fixedasset.mAssetNo.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mAssetTypeId != fixedassetOld.mAssetTypeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Asset Type ";
                audit.mOldValue = fixedassetOld.mAssetTypeName.ToString();
                audit.mNewValue = fixedasset.mAssetTypeName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mFunctionalLocationId != fixedassetOld.mFunctionalLocationId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Functional Location ";
                audit.mOldValue = fixedassetOld.mFunctionalLocationName.ToString();
                audit.mNewValue = fixedasset.mFunctionalLocationName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mPersonnelId != fixedassetOld.mPersonnelId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Personnel";
                audit.mOldValue = fixedassetOld.mPersonnelName.ToString();
                audit.mNewValue = fixedasset.mPersonnelName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mDescription != fixedassetOld.mDescription)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Description";
                audit.mOldValue = fixedassetOld.mDescription.ToString();
                audit.mNewValue = fixedasset.mDescription.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mPurchaseDate != fixedassetOld.mPurchaseDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Purchase Date";
                audit.mOldValue = fixedassetOld.mPurchaseDate.ToString();
                audit.mNewValue = fixedasset.mPurchaseDate.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mPurchasePrice != fixedassetOld.mPurchasePrice)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Purchase Price";
                audit.mOldValue = fixedassetOld.mPurchasePrice.ToString();
                audit.mNewValue = fixedasset.mPurchasePrice.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mWarrantyExpiry != fixedassetOld.mWarrantyExpiry)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Warranty Expiry";
                audit.mOldValue = fixedassetOld.mWarrantyExpiry.ToString();
                audit.mNewValue = fixedasset.mWarrantyExpiry.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mSerialNo != fixedassetOld.mSerialNo)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Serial No";
                audit.mOldValue = fixedassetOld.mSerialNo.ToString();
                audit.mNewValue = fixedasset.mSerialNo.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mModel != fixedassetOld.mModel)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Model";
                audit.mOldValue = fixedassetOld.mModel.ToString();
                audit.mNewValue = fixedasset.mModel.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mDepreciationStartDate != fixedassetOld.mDepreciationStartDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Depreciation Start Date";
                audit.mOldValue = fixedassetOld.mDepreciationStartDate.ToString();
                audit.mNewValue = fixedasset.mDepreciationStartDate.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mDepreciationMethodId != fixedassetOld.mDepreciationMethodId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Depreciation Method ";
                audit.mOldValue = fixedassetOld.mDepreciationMethodName.ToString();
                audit.mNewValue = fixedasset.mDepreciationMethodName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mAveragingMethodId != fixedassetOld.mAveragingMethodId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Averaging Method ";
                audit.mOldValue = fixedassetOld.mAveragingMethodName.ToString();
                audit.mNewValue = fixedasset.mAveragingMethodName.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mAccumulatedDepreciation != fixedassetOld.mAccumulatedDepreciation)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Accumulated Depreciation";
                audit.mOldValue = fixedassetOld.mAccumulatedDepreciation.ToString();
                audit.mNewValue = fixedasset.mAccumulatedDepreciation.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mResidualValue != fixedassetOld.mResidualValue)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Residual Value";
                audit.mOldValue = fixedassetOld.mResidualValue.ToString();
                audit.mNewValue = fixedasset.mResidualValue.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mUsefulLifeYears != fixedassetOld.mUsefulLifeYears)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Useful Life Years";
                audit.mOldValue = fixedassetOld.mUsefulLifeYears.ToString();
                audit.mNewValue = fixedasset.mUsefulLifeYears.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mIsDraft != fixedassetOld.mIsDraft)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Is Draft";
                audit.mOldValue = fixedassetOld.mIsDraft.ToString();
                audit.mNewValue = fixedasset.mIsDraft.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mIsRegistered != fixedassetOld.mIsRegistered)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Is Registered";
                audit.mOldValue = fixedassetOld.mIsRegistered.ToString();
                audit.mNewValue = fixedasset.mIsRegistered.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mIsDisposed != fixedassetOld.mIsDisposed)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Is Disposed";
                audit.mOldValue = fixedassetOld.mIsDisposed.ToString();
                audit.mNewValue = fixedasset.mIsDisposed.ToString();
                audit_collection.Add(audit);
            }

            if (fixedasset.mRegisterById != fixedassetOld.mRegisterById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedasset);
                audit.mField = "Register By";
                audit.mOldValue = fixedassetOld.mRegisterById.ToString();
                audit.mNewValue = fixedasset.mRegisterById.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, FixedAsset fixedasset)
        {
            audit.mUserId = fixedasset.mUserId;
            audit.mTableId = (int)(Tables.amQt_FixedAsset);
            audit.mRowId = fixedasset.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
