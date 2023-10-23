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
    public static class MaintenanceRequestManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static MaintenanceRequestCollection GetList()
        {
            MaintenanceRequestCriteria maintenancerequest = new MaintenanceRequestCriteria();
            return GetList(maintenancerequest);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceRequestCollection GetList(MaintenanceRequestCriteria maintenancerequestCriteria)
        {
            return MaintenanceRequestDB.GetList(maintenancerequestCriteria);
        }

        public static int SelectCountForGetList(MaintenanceRequestCriteria maintenancerequestCriteria)
        {
            return MaintenanceRequestDB.SelectCountForGetList(maintenancerequestCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceRequest GetItem(int id)
        {
            MaintenanceRequest maintenancerequest = MaintenanceRequestDB.GetItem(id);
            return maintenancerequest;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(MaintenanceRequest myMaintenanceRequest)
        {
            if (!myMaintenanceRequest.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid maintenancerequest. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myMaintenanceRequest.mId != 0)
                    AuditUpdate(myMaintenanceRequest);

                int id = MaintenanceRequestDB.Save(myMaintenanceRequest);

                if (myMaintenanceRequest.mId == 0)
                    AuditInsert(myMaintenanceRequest, id);

                myMaintenanceRequest.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(MaintenanceRequest myMaintenanceRequest)
        {
            if (MaintenanceRequestDB.Delete(myMaintenanceRequest.mId))
            {
                AuditDelete(myMaintenanceRequest);
                return myMaintenanceRequest.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(MaintenanceRequest myMaintenanceRequest, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceRequest.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceRequest;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(MaintenanceRequest myMaintenanceRequest)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceRequest.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceRequest;
            audit.mRowId = myMaintenanceRequest.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(MaintenanceRequest myMaintenanceRequest)
        {
            MaintenanceRequest old_maintenancerequest = GetItem(myMaintenanceRequest.mId);
            AuditCollection audit_collection = MaintenanceRequestAudit.Audit(myMaintenanceRequest, old_maintenancerequest);
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