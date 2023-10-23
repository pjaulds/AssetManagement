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
    public static class MaintenanceRequestTypeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static MaintenanceRequestTypeCollection GetList()
        {
            MaintenanceRequestTypeCriteria maintenanceRequestType = new MaintenanceRequestTypeCriteria();
            return GetList(maintenanceRequestType);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceRequestTypeCollection GetList(MaintenanceRequestTypeCriteria maintenanceRequestTypeCriteria)
        {
            return MaintenanceRequestTypeDB.GetList(maintenanceRequestTypeCriteria);
        }

        public static int SelectCountForGetList(MaintenanceRequestTypeCriteria maintenanceRequestTypeCriteria)
        {
            return MaintenanceRequestTypeDB.SelectCountForGetList(maintenanceRequestTypeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceRequestType GetItem(int id)
        {
            MaintenanceRequestType maintenanceRequestType = MaintenanceRequestTypeDB.GetItem(id);
            return maintenanceRequestType;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(MaintenanceRequestType myMaintenanceRequestType)
        {
            if (!myMaintenanceRequestType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid maintenanceRequestType. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myMaintenanceRequestType.mId != 0)
                    AuditUpdate(myMaintenanceRequestType);

                int id = MaintenanceRequestTypeDB.Save(myMaintenanceRequestType);

                if (myMaintenanceRequestType.mId == 0)
                    AuditInsert(myMaintenanceRequestType, id);

                myMaintenanceRequestType.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(MaintenanceRequestType myMaintenanceRequestType)
        {
            if (MaintenanceRequestTypeDB.Delete(myMaintenanceRequestType.mId))
            {
                AuditDelete(myMaintenanceRequestType);
                return myMaintenanceRequestType.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(MaintenanceRequestType myMaintenanceRequestType, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceRequestType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceRequestType;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(MaintenanceRequestType myMaintenanceRequestType)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceRequestType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceRequestType;
            audit.mRowId = myMaintenanceRequestType.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(MaintenanceRequestType myMaintenanceRequestType)
        {
            MaintenanceRequestType old_maintenanceRequestType = GetItem(myMaintenanceRequestType.mId);
            AuditCollection audit_collection = MaintenanceRequestTypeAudit.Audit(myMaintenanceRequestType, old_maintenanceRequestType);
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