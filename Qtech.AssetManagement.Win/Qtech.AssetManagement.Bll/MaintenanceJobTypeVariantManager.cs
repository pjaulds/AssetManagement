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
    public static class MaintenanceJobTypeVariantManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static MaintenanceJobTypeVariantCollection GetList()
        {
            MaintenanceJobTypeVariantCriteria maintenanceJobTypeVariant = new MaintenanceJobTypeVariantCriteria();
            return GetList(maintenanceJobTypeVariant);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceJobTypeVariantCollection GetList(MaintenanceJobTypeVariantCriteria maintenanceJobTypeVariantCriteria)
        {
            return MaintenanceJobTypeVariantDB.GetList(maintenanceJobTypeVariantCriteria);
        }

        public static int SelectCountForGetList(MaintenanceJobTypeVariantCriteria maintenanceJobTypeVariantCriteria)
        {
            return MaintenanceJobTypeVariantDB.SelectCountForGetList(maintenanceJobTypeVariantCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MaintenanceJobTypeVariant GetItem(int id)
        {
            MaintenanceJobTypeVariant maintenanceJobTypeVariant = MaintenanceJobTypeVariantDB.GetItem(id);
            return maintenanceJobTypeVariant;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(MaintenanceJobTypeVariant myMaintenanceJobTypeVariant)
        {
            if (!myMaintenanceJobTypeVariant.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid maintenanceJobTypeVariant. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myMaintenanceJobTypeVariant.mId != 0)
                    AuditUpdate(myMaintenanceJobTypeVariant);

                int id = MaintenanceJobTypeVariantDB.Save(myMaintenanceJobTypeVariant);

                if (myMaintenanceJobTypeVariant.mId == 0)
                    AuditInsert(myMaintenanceJobTypeVariant, id);

                myMaintenanceJobTypeVariant.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(MaintenanceJobTypeVariant myMaintenanceJobTypeVariant)
        {
            if (MaintenanceJobTypeVariantDB.Delete(myMaintenanceJobTypeVariant.mId))
            {
                AuditDelete(myMaintenanceJobTypeVariant);
                return myMaintenanceJobTypeVariant.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(MaintenanceJobTypeVariant myMaintenanceJobTypeVariant, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceJobTypeVariant.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceJobTypeVariant;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(MaintenanceJobTypeVariant myMaintenanceJobTypeVariant)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myMaintenanceJobTypeVariant.mUserId;
            audit.mTableId = (Int16)Tables.amQt_MaintenanceJobTypeVariant;
            audit.mRowId = myMaintenanceJobTypeVariant.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(MaintenanceJobTypeVariant myMaintenanceJobTypeVariant)
        {
            MaintenanceJobTypeVariant old_maintenanceJobTypeVariant = GetItem(myMaintenanceJobTypeVariant.mId);
            AuditCollection audit_collection = MaintenanceJobTypeVariantAudit.Audit(myMaintenanceJobTypeVariant, old_maintenanceJobTypeVariant);
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