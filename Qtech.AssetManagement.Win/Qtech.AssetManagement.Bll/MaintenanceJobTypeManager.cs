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
    public static class MaintenanceJobTypeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static MaintenanceJobTypeCollection GetList()
        {
            MaintenanceJobTypeCriteria maintenanceJobType = new MaintenanceJobTypeCriteria();
            return GetList(maintenanceJobType);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceJobTypeCollection GetList(MaintenanceJobTypeCriteria maintenanceJobTypeCriteria)
        {
            return MaintenanceJobTypeDB.GetList(maintenanceJobTypeCriteria);
        }

        public static int SelectCountForGetList(MaintenanceJobTypeCriteria maintenanceJobTypeCriteria)
        {
            return MaintenanceJobTypeDB.SelectCountForGetList(maintenanceJobTypeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceJobType GetItem(int id)
        {
            MaintenanceJobType maintenanceJobType = MaintenanceJobTypeDB.GetItem(id);
            return maintenanceJobType;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(MaintenanceJobType myMaintenanceJobType)
        {
            if (!myMaintenanceJobType.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid maintenanceJobType. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myMaintenanceJobType.mId != 0)
                    AuditUpdate(myMaintenanceJobType);

                int id = MaintenanceJobTypeDB.Save(myMaintenanceJobType);

                if (myMaintenanceJobType.mId == 0)
                    AuditInsert(myMaintenanceJobType, id);

                myMaintenanceJobType.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(MaintenanceJobType myMaintenanceJobType)
        {
            if (MaintenanceJobTypeDB.Delete(myMaintenanceJobType.mId))
            {
                AuditDelete(myMaintenanceJobType);
                return myMaintenanceJobType.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(MaintenanceJobType myMaintenanceJobType, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceJobType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceJobType;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(MaintenanceJobType myMaintenanceJobType)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceJobType.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceJobType;
            audit.mRowId = myMaintenanceJobType.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(MaintenanceJobType myMaintenanceJobType)
        {
            MaintenanceJobType old_maintenanceJobType = GetItem(myMaintenanceJobType.mId);
            AuditCollection audit_collection = MaintenanceJobTypeAudit.Audit(myMaintenanceJobType, old_maintenanceJobType);
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