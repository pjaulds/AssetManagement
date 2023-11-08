using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AccountTypeAudit
    {

        public static AuditCollection Audit(AccountType accountType, AccountType accountTypeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (accountType.mCode != accountTypeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountType);
                audit.mField = "code";
                audit.mOldValue = accountTypeOld.mCode.ToString();
                audit.mNewValue = accountType.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (accountType.mName != accountTypeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountType);
                audit.mField = "name";
                audit.mOldValue = accountTypeOld.mName.ToString();
                audit.mNewValue = accountType.mName.ToString();
                audit_collection.Add(audit);
            }

            if (accountType.mPost != accountTypeOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountType);
                audit.mField = "post";
                audit.mOldValue = accountTypeOld.mPost.ToString();
                audit.mNewValue = accountType.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AccountType accountType)
        {
            audit.mUserId = accountType.mUserId;
            audit.mTableId = (int)(Tables.amQt_AccountType);
            audit.mRowId = accountType.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}