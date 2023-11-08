using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class AccountGroupAudit
    {

        public static AuditCollection Audit(AccountGroup accountGroup, AccountGroup accountGroupOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (accountGroup.mCode != accountGroupOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountGroup);
                audit.mField = "code";
                audit.mOldValue = accountGroupOld.mCode.ToString();
                audit.mNewValue = accountGroup.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (accountGroup.mName != accountGroupOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountGroup);
                audit.mField = "name";
                audit.mOldValue = accountGroupOld.mName.ToString();
                audit.mNewValue = accountGroup.mName.ToString();
                audit_collection.Add(audit);
            }

            if (accountGroup.mPost != accountGroupOld.mPost)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, accountGroup);
                audit.mField = "post";
                audit.mOldValue = accountGroupOld.mPost.ToString();
                audit.mNewValue = accountGroup.mPost.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, AccountGroup accountGroup)
        {
            audit.mUserId = accountGroup.mUserId;
            audit.mTableId = (int)(Tables.amQt_AccountGroup);
            audit.mRowId = accountGroup.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}