using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class MaintenanceRequestAudit
    {

        public static AuditCollection Audit(MaintenanceRequest maintenancerequest, MaintenanceRequest maintenancerequestOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (maintenancerequest.mStartDate != maintenancerequestOld.mStartDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Start Date";
                audit.mOldValue = maintenancerequestOld.mStartDate.ToString();
                audit.mNewValue = maintenancerequest.mStartDate.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mEndDate != maintenancerequestOld.mEndDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "End Date";
                audit.mOldValue = maintenancerequestOld.mEndDate.ToString();
                audit.mNewValue = maintenancerequest.mEndDate.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mMaintenanceRequestTypeId != maintenancerequestOld.mMaintenanceRequestTypeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Maintenance Request Type ";
                audit.mOldValue = maintenancerequestOld.mMaintenanceRequestTypeName.ToString();
                audit.mNewValue = maintenancerequest.mMaintenanceRequestTypeName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mServiceLevelId != maintenancerequestOld.mServiceLevelId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Service Level ";
                audit.mOldValue = maintenancerequestOld.mServiceLevelName.ToString();
                audit.mNewValue = maintenancerequest.mServiceLevelName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mRequestedById != maintenancerequestOld.mRequestedById)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Requested By ";
                audit.mOldValue = maintenancerequestOld.mRequestedByName.ToString();
                audit.mNewValue = maintenancerequest.mRequestedByName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mFunctionalLocationId != maintenancerequestOld.mFunctionalLocationId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Functional Location ";
                audit.mOldValue = maintenancerequestOld.mFunctionalLocationName.ToString();
                audit.mNewValue = maintenancerequest.mFunctionalLocationName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mFixedAssetId != maintenancerequestOld.mFixedAssetId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Fixed Asset ";
                audit.mOldValue = maintenancerequestOld.mFixedAssetName.ToString();
                audit.mNewValue = maintenancerequest.mFixedAssetName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mFaultSymptomsId != maintenancerequestOld.mFaultSymptomsId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Fault Symptoms ";
                audit.mOldValue = maintenancerequestOld.mFaultSymptomsName.ToString();
                audit.mNewValue = maintenancerequest.mFaultSymptomsName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mFaultAreaId != maintenancerequestOld.mFaultAreaId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Fault Area ";
                audit.mOldValue = maintenancerequestOld.mFaultAreaName.ToString();
                audit.mNewValue = maintenancerequest.mFaultAreaName.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mDescription != maintenancerequestOld.mDescription)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Description";
                audit.mOldValue = maintenancerequestOld.mDescription.ToString();
                audit.mNewValue = maintenancerequest.mDescription.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mStatus != maintenancerequestOld.mStatus)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Status";
                audit.mOldValue = maintenancerequestOld.mStatus.ToString();
                audit.mNewValue = maintenancerequest.mStatus.ToString();
                audit_collection.Add(audit);
            }

            if (maintenancerequest.mActive != maintenancerequestOld.mActive)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, maintenancerequest);
                audit.mField = "Active";
                audit.mOldValue = maintenancerequestOld.mActive.ToString();
                audit.mNewValue = maintenancerequest.mActive.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, MaintenanceRequest maintenancerequest)
        {
            audit.mUserId = maintenancerequest.mUserId;
            audit.mTableId = (int)(Tables.amQt_MaintenanceRequest);
            audit.mRowId = maintenancerequest.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
