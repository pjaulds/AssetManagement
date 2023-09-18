using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ChartOfAccountAudit
    {

        public static AuditCollection Audit(ChartOfAccount chartOfAccount, ChartOfAccount chartOfAccountOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (chartOfAccount.mCode != chartOfAccountOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartOfAccount);
                audit.mField = "code";
                audit.mOldValue = chartOfAccountOld.mCode.ToString();
                audit.mNewValue = chartOfAccount.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (chartOfAccount.mName != chartOfAccountOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartOfAccount);
                audit.mField = "name";
                audit.mOldValue = chartOfAccountOld.mName.ToString();
                audit.mNewValue = chartOfAccount.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, ChartOfAccount chartOfAccount)
        {
            audit.mUserId = chartOfAccount.mUserId;
            audit.mTableId = (int)(Tables.amQt_ChartOfAccount);
            audit.mRowId = chartOfAccount.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}