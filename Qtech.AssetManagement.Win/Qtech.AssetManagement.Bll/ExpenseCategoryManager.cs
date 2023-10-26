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
    public static class ExpenseCategoryManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ExpenseCategoryCollection GetList()
        {
            ExpenseCategoryCriteria expenseCategory = new ExpenseCategoryCriteria();
            return GetList(expenseCategory);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ExpenseCategoryCollection GetList(ExpenseCategoryCriteria expenseCategoryCriteria)
        {
            return ExpenseCategoryDB.GetList(expenseCategoryCriteria);
        }

        public static int SelectCountForGetList(ExpenseCategoryCriteria expenseCategoryCriteria)
        {
            return ExpenseCategoryDB.SelectCountForGetList(expenseCategoryCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ExpenseCategory GetItem(int id)
        {
            ExpenseCategory expenseCategory = ExpenseCategoryDB.GetItem(id);
            return expenseCategory;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(ExpenseCategory myExpenseCategory)
        {
            if (!myExpenseCategory.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid expenseCategory. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myExpenseCategory.mId != 0)
                    AuditUpdate(myExpenseCategory);

                int id = ExpenseCategoryDB.Save(myExpenseCategory);

                if (myExpenseCategory.mId == 0)
                    AuditInsert(myExpenseCategory, id);

                myExpenseCategory.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(ExpenseCategory myExpenseCategory)
        {
            if (ExpenseCategoryDB.Delete(myExpenseCategory.mId))
            {
                AuditDelete(myExpenseCategory);
                return myExpenseCategory.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(ExpenseCategory myExpenseCategory, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myExpenseCategory.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ExpenseCategory;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(ExpenseCategory myExpenseCategory)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myExpenseCategory.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ExpenseCategory;
            audit.mRowId = myExpenseCategory.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(ExpenseCategory myExpenseCategory)
        {
            ExpenseCategory old_expenseCategory = GetItem(myExpenseCategory.mId);
            AuditCollection audit_collection = ExpenseCategoryAudit.Audit(myExpenseCategory, old_expenseCategory);
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