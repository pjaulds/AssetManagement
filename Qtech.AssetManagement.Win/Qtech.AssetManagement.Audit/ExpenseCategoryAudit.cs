using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ExpenseCategoryAudit
    {

        public static AuditCollection Audit(ExpenseCategory expenseCategory, ExpenseCategory expenseCategoryOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (expenseCategory.mCode != expenseCategoryOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, expenseCategory);
                audit.mField = "code";
                audit.mOldValue = expenseCategoryOld.mCode.ToString();
                audit.mNewValue = expenseCategory.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (expenseCategory.mName != expenseCategoryOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, expenseCategory);
                audit.mField = "name";
                audit.mOldValue = expenseCategoryOld.mName.ToString();
                audit.mNewValue = expenseCategory.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, ExpenseCategory expenseCategory)
        {
            audit.mUserId = expenseCategory.mUserId;
            audit.mTableId = (int)(Tables.amQt_ExpenseCategory);
            audit.mRowId = expenseCategory.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}