using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ProjectAudit
    {

        public static AuditCollection Audit(Project project, Project projectOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (project.mCode != projectOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, project);
                audit.mField = "Code";
                audit.mOldValue = projectOld.mCode.ToString();
                audit.mNewValue = project.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (project.mName != projectOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, project);
                audit.mField = "Name";
                audit.mOldValue = projectOld.mName.ToString();
                audit.mNewValue = project.mName.ToString();
                audit_collection.Add(audit);
            }

            if (project.mActive != projectOld.mActive)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, project);
                audit.mField = "Active";
                audit.mOldValue = projectOld.mActive.ToString();
                audit.mNewValue = project.mActive.ToString();
                audit_collection.Add(audit);
            }
            

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Project project)
        {
            audit.mUserId = project.mUserId;
            audit.mTableId = (int)(Tables.amQt_Project);
            audit.mRowId = project.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
