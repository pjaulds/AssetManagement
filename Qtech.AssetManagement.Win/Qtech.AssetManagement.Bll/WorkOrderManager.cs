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
    public static class WorkOrderManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static WorkOrderCollection GetList()
        {
            WorkOrderCriteria workorder = new WorkOrderCriteria();
            return GetList(workorder);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static WorkOrderCollection GetList(WorkOrderCriteria workorderCriteria)
        {
            return WorkOrderDB.GetList(workorderCriteria);
        }

        public static int SelectCountForGetList(WorkOrderCriteria workorderCriteria)
        {
            return WorkOrderDB.SelectCountForGetList(workorderCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static WorkOrder GetItem(int id)
        {
            WorkOrder workorder = WorkOrderDB.GetItem(id);
            return workorder;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(WorkOrder myWorkOrder)
        {
            if (!myWorkOrder.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid workorder. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myWorkOrder.mId != 0)
                    AuditUpdate(myWorkOrder);

                int id = WorkOrderDB.Save(myWorkOrder);

                if (myWorkOrder.mWorkOrderHoursCollection != null)
                {
                    foreach (WorkOrderHours item in myWorkOrder.mWorkOrderHoursCollection)
                    {
                        item.mWorkOrderId = id;
                        item.mUserId = myWorkOrder.mUserId;
                        WorkOrderHoursManager.Save(item);
                    }
                }

                if (myWorkOrder.mDeletedWorkOrderHoursCollection != null)
                {
                    foreach (WorkOrderHours item in myWorkOrder.mDeletedWorkOrderHoursCollection)
                    {
                        item.mUserId = myWorkOrder.mUserId;
                        WorkOrderHoursManager.Delete(item);
                    }
                }

                if (myWorkOrder.mId == 0)
                    AuditInsert(myWorkOrder, id);

                myWorkOrder.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(WorkOrder myWorkOrder)
        {
            if (WorkOrderDB.Delete(myWorkOrder.mId))
            {
                AuditDelete(myWorkOrder);
                return myWorkOrder.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(WorkOrder myWorkOrder, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myWorkOrder.mUserId;
            audit.mTableId = (Int16)Tables.amQt_WorkOrder;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(WorkOrder myWorkOrder)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myWorkOrder.mUserId;
            audit.mTableId = (Int16)Tables.amQt_WorkOrder;
            audit.mRowId = myWorkOrder.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(WorkOrder myWorkOrder)
        {
            WorkOrder old_workorder = GetItem(myWorkOrder.mId);
            AuditCollection audit_collection = WorkOrderAudit.Audit(myWorkOrder, old_workorder);
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