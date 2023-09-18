using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class UsersAudit
    {

        public static AuditCollection Audit(Users users, Users usersOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (users.mUsername != usersOld.mUsername)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, users);
                audit.mField = "username";
                audit.mOldValue = usersOld.mUsername.ToString();
                audit.mNewValue = users.mUsername.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Users users)
        {
            audit.mUserId = users.mUserId;
            audit.mTableId = (int)(Tables.amQt_Users);
            audit.mRowId = users.mId;
            audit.mActionId = (int)AuditAction.Update;
        }
    }
}