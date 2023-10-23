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
    public static class WorkOrderTypeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static WorkOrderTypeCollection GetList()
        {
            WorkOrderTypeCriteria workOrderType = new WorkOrderTypeCriteria();
            return GetList(workOrderType);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static WorkOrderTypeCollection GetList(WorkOrderTypeCriteria workOrderTypeCriteria)
        {
            return WorkOrderTypeDB.GetList(workOrderTypeCriteria);
        }

        public static int SelectCountForGetList(WorkOrderTypeCriteria workOrderTypeCriteria)
        {
            return WorkOrderTypeDB.SelectCountForGetList(workOrderTypeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static WorkOrderType GetItem(int id)
        {
            WorkOrderType workOrderType = WorkOrderTypeDB.GetItem(id);
            return workOrderType;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(WorkOrderType myWorkOrderType)
        {
            if (!myWorkOrderType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid workOrderType. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myWorkOrderType.mId != 0)
                    AuditUpdate(myWorkOrderType);

                int id = WorkOrderTypeDB.Save(myWorkOrderType);

                if (myWorkOrderType.mId == 0)
                    AuditInsert(myWorkOrderType, id);

                myWorkOrderType.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(WorkOrderType myWorkOrderType)
        {
            if (WorkOrderTypeDB.Delete(myWorkOrderType.mId))
            {
                AuditDelete(myWorkOrderType);
                return myWorkOrderType.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(WorkOrderType myWorkOrderType, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myWorkOrderType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_WorkOrderType;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(WorkOrderType myWorkOrderType)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myWorkOrderType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_WorkOrderType;
            audit.mRowId = myWorkOrderType.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(WorkOrderType myWorkOrderType)
        {
            WorkOrderType old_workOrderType = GetItem(myWorkOrderType.mId);
            AuditCollection audit_collection = WorkOrderTypeAudit.Audit(myWorkOrderType, old_workOrderType);
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