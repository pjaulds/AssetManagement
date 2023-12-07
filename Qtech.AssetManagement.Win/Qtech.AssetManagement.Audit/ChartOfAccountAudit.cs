using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class ChartOfAccountAudit
    {

        public static AuditCollection Audit(ChartOfAccount chartofaccount, ChartOfAccount chartofaccountOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (chartofaccount.mAccountTypeId != chartofaccountOld.mAccountTypeId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Account Type ";
                audit.mOldValue = chartofaccountOld.mAccountTypeName.ToString();
                audit.mNewValue = chartofaccount.mAccountTypeName.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mAccountGroupId != chartofaccountOld.mAccountGroupId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Account Group ";
                audit.mOldValue = chartofaccountOld.mAccountGroupName.ToString();
                audit.mNewValue = chartofaccount.mAccountGroupName.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mAccountClassificationId != chartofaccountOld.mAccountClassificationId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Account Classification ";
                audit.mOldValue = chartofaccountOld.mAccountClassificationName.ToString();
                audit.mNewValue = chartofaccount.mAccountClassificationName.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mCode != chartofaccountOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Code";
                audit.mOldValue = chartofaccountOld.mCode.ToString();
                audit.mNewValue = chartofaccount.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mName != chartofaccountOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Name";
                audit.mOldValue = chartofaccountOld.mName.ToString();
                audit.mNewValue = chartofaccount.mName.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mChartOfAccountMainId != chartofaccountOld.mChartOfAccountMainId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Chart Of Account Main ";
                audit.mOldValue = chartofaccountOld.mChartOfAccountMainName.ToString();
                audit.mNewValue = chartofaccount.mChartOfAccountMainName.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mChartOfAccountCloseId != chartofaccountOld.mChartOfAccountCloseId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Chart Of Account Close ";
                audit.mOldValue = chartofaccountOld.mChartOfAccountCloseName.ToString();
                audit.mNewValue = chartofaccount.mChartOfAccountCloseName.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mPayableSales != chartofaccountOld.mPayableSales)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Payable Sales";
                audit.mOldValue = chartofaccountOld.mPayableSales.ToString();
                audit.mNewValue = chartofaccount.mPayableSales.ToString();
                audit_collection.Add(audit);
            }

            if (chartofaccount.mDebitCredit != chartofaccountOld.mDebitCredit)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, chartofaccount);
                audit.mField = "Debit Credit";
                audit.mOldValue = chartofaccountOld.mDebitCredit.ToString();
                audit.mNewValue = chartofaccount.mDebitCredit.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, ChartOfAccount chartofaccount)
        {
            audit.mUserId = chartofaccount.mUserId;
            audit.mTableId = (int)(Tables.amQt_ChartOfAccount);
            audit.mRowId = chartofaccount.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}