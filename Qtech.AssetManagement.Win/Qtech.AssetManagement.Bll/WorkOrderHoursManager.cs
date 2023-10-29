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
    public static class WorkOrderHoursManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static WorkOrderHoursCollection GetList()
        {
            WorkOrderHoursCriteria workorderhours = new WorkOrderHoursCriteria();
            return GetList(workorderhours);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static WorkOrderHoursCollection GetList(WorkOrderHoursCriteria workorderhoursCriteria)
        {
            return WorkOrderHoursDB.GetList(workorderhoursCriteria);
        }

        public static int SelectCountForGetList(WorkOrderHoursCriteria workorderhoursCriteria)
        {
            return WorkOrderHoursDB.SelectCountForGetList(workorderhoursCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static WorkOrderHours GetItem(int id)
        {
            WorkOrderHours workorderhours = WorkOrderHoursDB.GetItem(id);
            return workorderhours;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(WorkOrderHours myWorkOrderHours)
        {
            if (!myWorkOrderHours.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid workorderhours. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myWorkOrderHours.mId != 0)
                    AuditUpdate(myWorkOrderHours);

                int id = WorkOrderHoursDB.Save(myWorkOrderHours);

                if (myWorkOrderHours.mId == 0)
                    AuditInsert(myWorkOrderHours, id);

                myWorkOrderHours.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(WorkOrderHours myWorkOrderHours)
        {
            if (WorkOrderHoursDB.Delete(myWorkOrderHours.mId))
            {
                AuditDelete(myWorkOrderHours);
                return myWorkOrderHours.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(WorkOrderHours myWorkOrderHours, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myWorkOrderHours.mUserId;
            audit.mTableId = (Int16)Tables.amQt_WorkOrderHours;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(WorkOrderHours myWorkOrderHours)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myWorkOrderHours.mUserId;
            audit.mTableId = (Int16)Tables.amQt_WorkOrderHours;
            audit.mRowId = myWorkOrderHours.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(WorkOrderHours myWorkOrderHours)
        {
            WorkOrderHours old_workorderhours = GetItem(myWorkOrderHours.mId);
            AuditCollection audit_collection = WorkOrderHoursAudit.Audit(myWorkOrderHours, old_workorderhours);
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