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
    public static class FixedAssetCapitalizedCostManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static FixedAssetCapitalizedCostCollection GetList()
        {
            FixedAssetCapitalizedCostCriteria fixedassetcapitalizedcost = new FixedAssetCapitalizedCostCriteria();
            return GetList(fixedassetcapitalizedcost);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAssetCapitalizedCostCollection GetList(FixedAssetCapitalizedCostCriteria fixedassetcapitalizedcostCriteria)
        {
            return FixedAssetCapitalizedCostDB.GetList(fixedassetcapitalizedcostCriteria);
        }

        public static int SelectCountForGetList(FixedAssetCapitalizedCostCriteria fixedassetcapitalizedcostCriteria)
        {
            return FixedAssetCapitalizedCostDB.SelectCountForGetList(fixedassetcapitalizedcostCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static FixedAssetCapitalizedCost GetItem(int id)
        {
            FixedAssetCapitalizedCost fixedassetcapitalizedcost = FixedAssetCapitalizedCostDB.GetItem(id);
            return fixedassetcapitalizedcost;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(FixedAssetCapitalizedCost myFixedAssetCapitalizedCost)
        {
            if (!myFixedAssetCapitalizedCost.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid fixedassetcapitalizedcost. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myFixedAssetCapitalizedCost.mId != 0)
                    AuditUpdate(myFixedAssetCapitalizedCost);

                int id = FixedAssetCapitalizedCostDB.Save(myFixedAssetCapitalizedCost);

                if (myFixedAssetCapitalizedCost.mId == 0)
                    AuditInsert(myFixedAssetCapitalizedCost, id);

                myFixedAssetCapitalizedCost.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(FixedAssetCapitalizedCost myFixedAssetCapitalizedCost)
        {
            if (FixedAssetCapitalizedCostDB.Delete(myFixedAssetCapitalizedCost.mId))
            {
                AuditDelete(myFixedAssetCapitalizedCost);
                return myFixedAssetCapitalizedCost.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(FixedAssetCapitalizedCost myFixedAssetCapitalizedCost, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAssetCapitalizedCost.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAssetCapitalizedCost;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(FixedAssetCapitalizedCost myFixedAssetCapitalizedCost)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myFixedAssetCapitalizedCost.mUserId;
            audit.mTableId = (Int16)Tables.amQt_FixedAssetCapitalizedCost;
            audit.mRowId = myFixedAssetCapitalizedCost.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(FixedAssetCapitalizedCost myFixedAssetCapitalizedCost)
        {
            FixedAssetCapitalizedCost old_fixedassetcapitalizedcost = GetItem(myFixedAssetCapitalizedCost.mId);
            AuditCollection audit_collection = FixedAssetCapitalizedCostAudit.Audit(myFixedAssetCapitalizedCost, old_fixedassetcapitalizedcost);
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