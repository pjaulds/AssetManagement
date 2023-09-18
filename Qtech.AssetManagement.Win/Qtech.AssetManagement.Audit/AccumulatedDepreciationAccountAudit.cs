using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AccumulatedDepreciationAccountAudit
    {

        public static AuditCollection Audit(AccumulatedDepreciationAccount accumulateddepreciationaccount, AccumulatedDepreciationAccount accumulateddepreciationaccountOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (accumulateddepreciationaccount.mCode != accumulateddepreciationaccountOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accumulateddepreciationaccount);
                audit.mField = "code";
                audit.mOldValue = accumulateddepreciationaccountOld.mCode.ToString();
                audit.mNewValue = accumulateddepreciationaccount.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (accumulateddepreciationaccount.mName != accumulateddepreciationaccountOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accumulateddepreciationaccount);
                audit.mField = "name";
                audit.mOldValue = accumulateddepreciationaccountOld.mName.ToString();
                audit.mNewValue = accumulateddepreciationaccount.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AccumulatedDepreciationAccount accumulateddepreciationaccount)
        {
            audit.mUserId = accumulateddepreciationaccount.mUserId;
            audit.mTableId = (int)(Tables.amQt_AccumulatedDepreciationAccount);
            audit.mRowId = accumulateddepreciationaccount.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}