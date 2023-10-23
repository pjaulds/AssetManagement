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
    public static class FaultSymptomsManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static FaultSymptomsCollection GetList()
        {
            FaultSymptomsCriteria faultSymptoms = new FaultSymptomsCriteria();
            return GetList(faultSymptoms);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FaultSymptomsCollection GetList(FaultSymptomsCriteria faultSymptomsCriteria)
        {
            return FaultSymptomsDB.GetList(faultSymptomsCriteria);
        }

        public static int SelectCountForGetList(FaultSymptomsCriteria faultSymptomsCriteria)
        {
            return FaultSymptomsDB.SelectCountForGetList(faultSymptomsCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FaultSymptoms GetItem(int id)
        {
            FaultSymptoms faultSymptoms = FaultSymptomsDB.GetItem(id);
            return faultSymptoms;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(FaultSymptoms myFaultSymptoms)
        {
            if (!myFaultSymptoms.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid faultSymptoms. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myFaultSymptoms.mId != 0)
                    AuditUpdate(myFaultSymptoms);

                int id = FaultSymptomsDB.Save(myFaultSymptoms);

                if (myFaultSymptoms.mId == 0)
                    AuditInsert(myFaultSymptoms, id);

                myFaultSymptoms.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(FaultSymptoms myFaultSymptoms)
        {
            if (FaultSymptomsDB.Delete(myFaultSymptoms.mId))
            {
                AuditDelete(myFaultSymptoms);
                return myFaultSymptoms.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(FaultSymptoms myFaultSymptoms, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFaultSymptoms.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FaultSymptoms;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(FaultSymptoms myFaultSymptoms)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFaultSymptoms.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FaultSymptoms;
            audit.mRowId = myFaultSymptoms.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(FaultSymptoms myFaultSymptoms)
        {
            FaultSymptoms old_faultSymptoms = GetItem(myFaultSymptoms.mId);
            AuditCollection audit_collection = FaultSymptomsAudit.Audit(myFaultSymptoms, old_faultSymptoms);
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