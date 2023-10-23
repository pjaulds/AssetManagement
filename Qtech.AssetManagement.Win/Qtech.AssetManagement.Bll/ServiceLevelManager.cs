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
    public static class ServiceLevelManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ServiceLevelCollection GetList()
        {
            ServiceLevelCriteria serviceLevel = new ServiceLevelCriteria();
            return GetList(serviceLevel);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ServiceLevelCollection GetList(ServiceLevelCriteria serviceLevelCriteria)
        {
            return ServiceLevelDB.GetList(serviceLevelCriteria);
        }

        public static int SelectCountForGetList(ServiceLevelCriteria serviceLevelCriteria)
        {
            return ServiceLevelDB.SelectCountForGetList(serviceLevelCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ServiceLevel GetItem(int id)
        {
            ServiceLevel serviceLevel = ServiceLevelDB.GetItem(id);
            return serviceLevel;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(ServiceLevel myServiceLevel)
        {
            if (!myServiceLevel.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid serviceLevel. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myServiceLevel.mId != 0)
                    AuditUpdate(myServiceLevel);

                int id = ServiceLevelDB.Save(myServiceLevel);

                if (myServiceLevel.mId == 0)
                    AuditInsert(myServiceLevel, id);

                myServiceLevel.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(ServiceLevel myServiceLevel)
        {
            if (ServiceLevelDB.Delete(myServiceLevel.mId))
            {
                AuditDelete(myServiceLevel);
                return myServiceLevel.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(ServiceLevel myServiceLevel, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myServiceLevel.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ServiceLevel;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(ServiceLevel myServiceLevel)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myServiceLevel.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ServiceLevel;
            audit.mRowId = myServiceLevel.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(ServiceLevel myServiceLevel)
        {
            ServiceLevel old_serviceLevel = GetItem(myServiceLevel.mId);
            AuditCollection audit_collection = ServiceLevelAudit.Audit(myServiceLevel, old_serviceLevel);
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