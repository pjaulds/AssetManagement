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
    public static class CapitalizedCostManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static CapitalizedCostCollection GetList()
        {
            CapitalizedCostCriteria capitalizedCost = new CapitalizedCostCriteria();
            return GetList(capitalizedCost);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static CapitalizedCostCollection GetList(CapitalizedCostCriteria capitalizedCostCriteria)
        {
            return CapitalizedCostDB.GetList(capitalizedCostCriteria);
        }

        public static int SelectCountForGetList(CapitalizedCostCriteria capitalizedCostCriteria)
        {
            return CapitalizedCostDB.SelectCountForGetList(capitalizedCostCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static CapitalizedCost GetItem(int id)
        {
            CapitalizedCost capitalizedCost = CapitalizedCostDB.GetItem(id);
            return capitalizedCost;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(CapitalizedCost myCapitalizedCost)
        {
            if (!myCapitalizedCost.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid capitalizedCost. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myCapitalizedCost.mId != 0)
                    AuditUpdate(myCapitalizedCost);

                int id = CapitalizedCostDB.Save(myCapitalizedCost);

                if (myCapitalizedCost.mId == 0)
                    AuditInsert(myCapitalizedCost, id);

                myCapitalizedCost.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(CapitalizedCost myCapitalizedCost)
        {
            if (CapitalizedCostDB.Delete(myCapitalizedCost.mId))
            {
                AuditDelete(myCapitalizedCost);
                return myCapitalizedCost.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(CapitalizedCost myCapitalizedCost, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myCapitalizedCost.mUserId;
            audit.mTableId = (Int16)Tables.amQt_CapitalizedCost;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(CapitalizedCost myCapitalizedCost)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myCapitalizedCost.mUserId;
            audit.mTableId = (Int16)Tables.amQt_CapitalizedCost;
            audit.mRowId = myCapitalizedCost.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(CapitalizedCost myCapitalizedCost)
        {
            CapitalizedCost old_capitalizedCost = GetItem(myCapitalizedCost.mId);
            AuditCollection audit_collection = CapitalizedCostAudit.Audit(myCapitalizedCost, old_capitalizedCost);
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