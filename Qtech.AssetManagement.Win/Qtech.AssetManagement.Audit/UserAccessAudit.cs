using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class UserAccessAudit
    {

        public static AuditCollection Audit(UserAccess useraccess, UserAccess useraccessOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (useraccess.mUserId != useraccessOld.mUserId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, useraccess);
                audit.mField = "user_id";
                audit.mOldValue = useraccessOld.mUserId.ToString();
                audit.mNewValue = useraccess.mUserId.ToString();
                audit_collection.Add(audit);
            }

            if (useraccess.mModuleId != useraccessOld.mModuleId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, useraccess);
                audit.mField = "module_id";
                audit.mOldValue = useraccessOld.mModuleId.ToString();
                audit.mNewValue = useraccess.mModuleId.ToString();
                audit_collection.Add(audit);
            }

            if (useraccess.mSelect != useraccessOld.mSelect)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, useraccess);
                audit.mField = "select";
                audit.mOldValue = useraccessOld.mSelect.ToString();
                audit.mNewValue = useraccess.mSelect.ToString();
                audit_collection.Add(audit);
            }

            if (useraccess.mInsert != useraccessOld.mInsert)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, useraccess);
                audit.mField = "insert";
                audit.mOldValue = useraccessOld.mInsert.ToString();
                audit.mNewValue = useraccess.mInsert.ToString();
                audit_collection.Add(audit);
            }

            if (useraccess.mUpdate != useraccessOld.mUpdate)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, useraccess);
                audit.mField = "update";
                audit.mOldValue = useraccessOld.mUpdate.ToString();
                audit.mNewValue = useraccess.mUpdate.ToString();
                audit_collection.Add(audit);
            }

            if (useraccess.mDelete != useraccessOld.mDelete)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, useraccess);
                audit.mField = "delete";
                audit.mOldValue = useraccessOld.mDelete.ToString();
                audit.mNewValue = useraccess.mDelete.ToString();
                audit_collection.Add(audit);
            }

            if (useraccess.mPrint != useraccessOld.mPrint)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, useraccess);
                audit.mField = "print";
                audit.mOldValue = useraccessOld.mPrint.ToString();
                audit.mNewValue = useraccess.mPrint.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, UserAccess useraccess)
        {
            audit.mUserId = useraccess.mUserId;
            audit.mTableId = (int)(Tables.amQt_UserAccess);
            audit.mRowId = useraccess.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
