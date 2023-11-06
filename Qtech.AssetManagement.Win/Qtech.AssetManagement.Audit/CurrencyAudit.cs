using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class CurrencyAudit
    {

        public static AuditCollection Audit(Currency currency, Currency currencyOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (currency.mCode != currencyOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, currency);
                audit.mField = "code";
                audit.mOldValue = currencyOld.mCode.ToString();
                audit.mNewValue = currency.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (currency.mName != currencyOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, currency);
                audit.mField = "name";
                audit.mOldValue = currencyOld.mName.ToString();
                audit.mNewValue = currency.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Currency currency)
        {
            audit.mUserId = currency.mUserId;
            audit.mTableId = (int)(Tables.amQt_Currency);
            audit.mRowId = currency.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}