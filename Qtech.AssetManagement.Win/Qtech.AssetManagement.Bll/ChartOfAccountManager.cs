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
    public static class ChartOfAccountManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ChartOfAccountCollection GetList()
        {
            ChartOfAccountCriteria chartOfAccount = new ChartOfAccountCriteria();
            return GetList(chartOfAccount);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ChartOfAccountCollection GetList(ChartOfAccountCriteria chartOfAccountCriteria)
        {
            return ChartOfAccountDB.GetList(chartOfAccountCriteria);
        }

        public static int SelectCountForGetList(ChartOfAccountCriteria chartOfAccountCriteria)
        {
            return ChartOfAccountDB.SelectCountForGetList(chartOfAccountCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ChartOfAccount GetItem(int id)
        {
            ChartOfAccount chartOfAccount = ChartOfAccountDB.GetItem(id);
            return chartOfAccount;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(ChartOfAccount myChartOfAccount)
        {
            if (!myChartOfAccount.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid chartOfAccount. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myChartOfAccount.mId != 0)
                    AuditUpdate(myChartOfAccount);

                int id = ChartOfAccountDB.Save(myChartOfAccount);

                if (myChartOfAccount.mId == 0)
                    AuditInsert(myChartOfAccount, id);

                myChartOfAccount.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(ChartOfAccount myChartOfAccount)
        {
            if (ChartOfAccountDB.Delete(myChartOfAccount.mId))
            {
                AuditDelete(myChartOfAccount);
                return myChartOfAccount.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(ChartOfAccount myChartOfAccount, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myChartOfAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ChartOfAccount;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(ChartOfAccount myChartOfAccount)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myChartOfAccount.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ChartOfAccount;
            audit.mRowId = myChartOfAccount.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(ChartOfAccount myChartOfAccount)
        {
            ChartOfAccount old_chartOfAccount = GetItem(myChartOfAccount.mId);
            AuditCollection audit_collection = ChartOfAccountAudit.Audit(myChartOfAccount, old_chartOfAccount);
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