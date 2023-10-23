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
    public static class FaultAreaManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static FaultAreaCollection GetList()
        {
            FaultAreaCriteria faultArea = new FaultAreaCriteria();
            return GetList(faultArea);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FaultAreaCollection GetList(FaultAreaCriteria faultAreaCriteria)
        {
            return FaultAreaDB.GetList(faultAreaCriteria);
        }

        public static int SelectCountForGetList(FaultAreaCriteria faultAreaCriteria)
        {
            return FaultAreaDB.SelectCountForGetList(faultAreaCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FaultArea GetItem(int id)
        {
            FaultArea faultArea = FaultAreaDB.GetItem(id);
            return faultArea;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(FaultArea myFaultArea)
        {
            if (!myFaultArea.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid faultArea. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myFaultArea.mId != 0)
                    AuditUpdate(myFaultArea);

                int id = FaultAreaDB.Save(myFaultArea);

                if (myFaultArea.mId == 0)
                    AuditInsert(myFaultArea, id);

                myFaultArea.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(FaultArea myFaultArea)
        {
            if (FaultAreaDB.Delete(myFaultArea.mId))
            {
                AuditDelete(myFaultArea);
                return myFaultArea.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(FaultArea myFaultArea, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFaultArea.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FaultArea;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(FaultArea myFaultArea)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFaultArea.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FaultArea;
            audit.mRowId = myFaultArea.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(FaultArea myFaultArea)
        {
            FaultArea old_faultArea = GetItem(myFaultArea.mId);
            AuditCollection audit_collection = FaultAreaAudit.Audit(myFaultArea, old_faultArea);
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