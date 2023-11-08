using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AccountClassificationAudit
    {

        public static AuditCollection Audit(AccountClassification accountClassification, AccountClassification accountClassificationOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (accountClassification.mCode != accountClassificationOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountClassification);
                audit.mField = "code";
                audit.mOldValue = accountClassificationOld.mCode.ToString();
                audit.mNewValue = accountClassification.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (accountClassification.mName != accountClassificationOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountClassification);
                audit.mField = "name";
                audit.mOldValue = accountClassificationOld.mName.ToString();
                audit.mNewValue = accountClassification.mName.ToString();
                audit_collection.Add(audit);
            }

            if (accountClassification.mPost != accountClassificationOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountClassification);
                audit.mField = "post";
                audit.mOldValue = accountClassificationOld.mPost.ToString();
                audit.mNewValue = accountClassification.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AccountClassification accountClassification)
        {
            audit.mUserId = accountClassification.mUserId;
            audit.mTableId = (int)(Tables.amQt_AccountClassification);
            audit.mRowId = accountClassification.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}