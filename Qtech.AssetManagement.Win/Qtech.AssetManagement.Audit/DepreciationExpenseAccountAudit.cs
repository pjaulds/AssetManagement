using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class DepreciationExpenseAccountAudit
    {

        public static AuditCollection Audit(DepreciationExpenseAccount depreciationexpenseaccount, DepreciationExpenseAccount depreciationexpenseaccountOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (depreciationexpenseaccount.mCode != depreciationexpenseaccountOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationexpenseaccount);
                audit.mField = "code";
                audit.mOldValue = depreciationexpenseaccountOld.mCode.ToString();
                audit.mNewValue = depreciationexpenseaccount.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (depreciationexpenseaccount.mName != depreciationexpenseaccountOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, depreciationexpenseaccount);
                audit.mField = "name";
                audit.mOldValue = depreciationexpenseaccountOld.mName.ToString();
                audit.mNewValue = depreciationexpenseaccount.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, DepreciationExpenseAccount depreciationexpenseaccount)
        {
            audit.mUserId = depreciationexpenseaccount.mUserId;
            audit.mTableId = (int)(Tables.amQt_DepreciationExpenseAccount);
            audit.mRowId = depreciationexpenseaccount.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}