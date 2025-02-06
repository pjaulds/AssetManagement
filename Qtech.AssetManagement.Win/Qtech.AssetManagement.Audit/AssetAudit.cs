using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AssetAudit
    {

        public static AuditCollection Audit(Asset asset, Asset assetOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (asset.mCode != assetOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Code";
                audit.mOldValue = assetOld.mCode.ToString();
                audit.mNewValue = asset.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mDate != assetOld.mDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Date";
                audit.mOldValue = assetOld.mDate.ToString();
                audit.mNewValue = asset.mDate.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mReceivedDate != assetOld.mReceivedDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Received Date";
                audit.mOldValue = assetOld.mReceivedDate.ToString();
                audit.mNewValue = asset.mReceivedDate.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mName != assetOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Name";
                audit.mOldValue = assetOld.mName.ToString();
                audit.mNewValue = asset.mName.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mAssetTypeId != assetOld.mAssetTypeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Asset Type ";
                audit.mOldValue = assetOld.mAssetTypeName.ToString();
                audit.mNewValue = asset.mAssetTypeName.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mAcquisitionCost != assetOld.mAcquisitionCost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Acquisition Cost";
                audit.mOldValue = assetOld.mAcquisitionCost.ToString();
                audit.mNewValue = asset.mAcquisitionCost.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mWarrantyExpiry != assetOld.mWarrantyExpiry)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Warranty Expiry";
                audit.mOldValue = assetOld.mWarrantyExpiry.ToString();
                audit.mNewValue = asset.mWarrantyExpiry.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mBrand != assetOld.mBrand)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Brand";
                audit.mOldValue = assetOld.mBrand.ToString();
                audit.mNewValue = asset.mBrand.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mModel != assetOld.mModel)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Model";
                audit.mOldValue = assetOld.mModel.ToString();
                audit.mNewValue = asset.mModel.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mSerialNumber != assetOld.mSerialNumber)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Serial Number";
                audit.mOldValue = assetOld.mSerialNumber.ToString();
                audit.mNewValue = asset.mSerialNumber.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mCapacity != assetOld.mCapacity)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Capacity";
                audit.mOldValue = assetOld.mCapacity.ToString();
                audit.mNewValue = asset.mCapacity.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mEngineNumber != assetOld.mEngineNumber)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Engine Number";
                audit.mOldValue = assetOld.mEngineNumber.ToString();
                audit.mNewValue = asset.mEngineNumber.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mChassisNumber != assetOld.mChassisNumber)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Chassis Number";
                audit.mOldValue = assetOld.mChassisNumber.ToString();
                audit.mNewValue = asset.mChassisNumber.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mPlateNumber != assetOld.mPlateNumber)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Plate Number";
                audit.mOldValue = assetOld.mPlateNumber.ToString();
                audit.mNewValue = asset.mPlateNumber.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mFunctionalLocationId != assetOld.mFunctionalLocationId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Functional Location ";
                audit.mOldValue = assetOld.mFunctionalLocationName.ToString();
                audit.mNewValue = asset.mFunctionalLocationName.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mPersonnelId != assetOld.mPersonnelId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Personnel ";
                audit.mOldValue = assetOld.mPersonnelName.ToString();
                audit.mNewValue = asset.mPersonnelName.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mProjectId != assetOld.mProjectId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Project ";
                audit.mOldValue = assetOld.mProjectName.ToString();
                audit.mNewValue = asset.mProjectName.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mRegisteredById != assetOld.mRegisteredById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Registered By ";
                audit.mOldValue = assetOld.mRegisteredByName.ToString();
                audit.mNewValue = asset.mRegisteredByName.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mRemarks != assetOld.mRemarks)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Remarks";
                audit.mOldValue = assetOld.mRemarks.ToString();
                audit.mNewValue = asset.mRemarks.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mActive != assetOld.mActive)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Active";
                audit.mOldValue = assetOld.mActive.ToString();
                audit.mNewValue = asset.mActive.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mResidualValue != assetOld.mResidualValue)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Residual Value";
                audit.mOldValue = assetOld.mResidualValue.ToString();
                audit.mNewValue = asset.mResidualValue.ToString();
                audit_collection.Add(audit);
            }

            if (asset.mUsefulLife != assetOld.mUsefulLife)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, asset);
                audit.mField = "Useful Life";
                audit.mOldValue = assetOld.mUsefulLife.ToString();
                audit.mNewValue = asset.mUsefulLife.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Asset asset)
        {
            audit.mUserId = asset.mUserId;
            audit.mTableId = (int)(Tables.amQt_Asset);
            audit.mRowId = asset.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
