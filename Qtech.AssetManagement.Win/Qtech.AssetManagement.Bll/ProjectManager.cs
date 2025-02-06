using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Dal;
using Qtech.AssetManagement.Validation;
using System.ComponentModel;

using Qtech.AssetManagement.Audit;

namespace Qtech.AssetManagement.Bll
{
    [DataObjectAttribute()]
    public static class ProjectManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ProjectCollection GetList()
        {
            ProjectCriteria project = new ProjectCriteria();
            return GetList(project);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ProjectCollection GetList(ProjectCriteria projectCriteria)
        {
            return ProjectDB.GetList(projectCriteria);
        }

        public static int SelectCountForGetList(ProjectCriteria projectCriteria)
        {
            return ProjectDB.SelectCountForGetList(projectCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Project GetItem(int id)
        {
            Project project = ProjectDB.GetItem(id);
            return project;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Project myProject)
        {
            if (!myProject.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid project. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myProject.mId != 0)
                    AuditUpdate(myProject);

                int id = ProjectDB.Save(myProject);

                if (myProject.mId == 0)
                    AuditInsert(myProject, id);

                myProject.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Project myProject)
        {
            if (ProjectDB.Delete(myProject.mId))
            {
                AuditDelete(myProject);
                return myProject.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Project myProject, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myProject.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Project;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Project myProject)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myProject.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Project;
            audit.mRowId = myProject.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Project myProject)
        {
            Project old_project = GetItem(myProject.mId);
            AuditCollection audit_collection = ProjectAudit.Audit(myProject, old_project);
            if (audit_collection != null)
            {
                foreach (BusinessEntities.Audit audit in audit_collection)
                {
                    AuditManager.Save(audit);
                }
            }
        }
        #endregion
    }
}