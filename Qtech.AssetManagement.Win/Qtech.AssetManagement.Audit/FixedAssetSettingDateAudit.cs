using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class FixedAssetSettingDateAudit
    {

        public static AuditCollection Audit(FixedAssetSettingDate fixedassetsettingdate, FixedAssetSettingDate fixedassetsettingdateOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (fixedassetsettingdate.mDate != fixedassetsettingdateOld.mDate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, fixedassetsettingdate);
                audit.mField = "Date";
                audit.mOldValue = fixedassetsettingdateOld.mDate.ToString();
                audit.mNewValue = fixedassetsettingdate.mDate.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, FixedAssetSettingDate fixedassetsettingdate)
        {
            audit.mUserId = fixedassetsettingdate.mUserId;
            audit.mTableId = (int)(Tables.amQt_FixedAssetSettingDate);
            audit.mRowId = fixedassetsettingdate.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}