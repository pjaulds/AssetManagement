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
    public static class FunctionalLocationManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static FunctionalLocationCollection GetList()
        {
            FunctionalLocationCriteria functionallocation = new FunctionalLocationCriteria();
            return GetList(functionallocation);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FunctionalLocationCollection GetList(FunctionalLocationCriteria functionallocationCriteria)
        {
            return FunctionalLocationDB.GetList(functionallocationCriteria);
        }

        public static int SelectCountForGetList(FunctionalLocationCriteria functionallocationCriteria)
        {
            return FunctionalLocationDB.SelectCountForGetList(functionallocationCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FunctionalLocation GetItem(int id)
        {
            FunctionalLocation functionallocation = FunctionalLocationDB.GetItem(id);
            return functionallocation;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(FunctionalLocation myFunctionalLocation)
        {
            if (!myFunctionalLocation.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid functionallocation. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myFunctionalLocation.mId != 0)
                    AuditUpdate(myFunctionalLocation);

                int id = FunctionalLocationDB.Save(myFunctionalLocation);

                if (myFunctionalLocation.mId == 0)
                    AuditInsert(myFunctionalLocation, id);

                myFunctionalLocation.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(FunctionalLocation myFunctionalLocation)
        {
            if (FunctionalLocationDB.Delete(myFunctionalLocation.mId))
            {
                AuditDelete(myFunctionalLocation);
                return myFunctionalLocation.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(FunctionalLocation myFunctionalLocation, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFunctionalLocation.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FunctionalLocation;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(FunctionalLocation myFunctionalLocation)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFunctionalLocation.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FunctionalLocation;
            audit.mRowId = myFunctionalLocation.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(FunctionalLocation myFunctionalLocation)
        {
            FunctionalLocation old_functionallocation = GetItem(myFunctionalLocation.mId);
            AuditCollection audit_collection = FunctionalLocationAudit.Audit(myFunctionalLocation, old_functionallocation);
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