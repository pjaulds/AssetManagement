using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FunctionalLocationAudit
    {

        public static AuditCollection Audit(FunctionalLocation functionallocation, FunctionalLocation functionallocationOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (functionallocation.mCode != functionallocationOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Code";
                audit.mOldValue = functionallocationOld.mCode.ToString();
                audit.mNewValue = functionallocation.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mName != functionallocationOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Name";
                audit.mOldValue = functionallocationOld.mName.ToString();
                audit.mNewValue = functionallocation.mName.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mParentFlId != functionallocationOld.mParentFlId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Parent Fl ";
                audit.mOldValue = functionallocationOld.mParentFlName.ToString();
                audit.mNewValue = functionallocation.mParentFlName.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mFlStatus != functionallocationOld.mFlStatus)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Fl Status";
                audit.mOldValue = functionallocationOld.mFlStatus.ToString();
                audit.mNewValue = functionallocation.mFlStatus.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mAddressName != functionallocationOld.mAddressName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Address Name";
                audit.mOldValue = functionallocationOld.mAddressName.ToString();
                audit.mNewValue = functionallocation.mAddressName.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mStreet != functionallocationOld.mStreet)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Street";
                audit.mOldValue = functionallocationOld.mStreet.ToString();
                audit.mNewValue = functionallocation.mStreet.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mCity != functionallocationOld.mCity)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "City";
                audit.mOldValue = functionallocationOld.mCity.ToString();
                audit.mNewValue = functionallocation.mCity.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mProvince != functionallocationOld.mProvince)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Province";
                audit.mOldValue = functionallocationOld.mProvince.ToString();
                audit.mNewValue = functionallocation.mProvince.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mCountry != functionallocationOld.mCountry)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Country";
                audit.mOldValue = functionallocationOld.mCountry.ToString();
                audit.mNewValue = functionallocation.mCountry.ToString();
                audit_collection.Add(audit);
            }

            if (functionallocation.mZipCode != functionallocationOld.mZipCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, functionallocation);
                audit.mField = "Zip Code";
                audit.mOldValue = functionallocationOld.mZipCode.ToString();
                audit.mNewValue = functionallocation.mZipCode.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, FunctionalLocation functionallocation)
        {
            audit.mUserId = functionallocation.mUserId;
            audit.mTableId = (int)(Tables.amQt_FunctionalLocation);
            audit.mRowId = functionallocation.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}