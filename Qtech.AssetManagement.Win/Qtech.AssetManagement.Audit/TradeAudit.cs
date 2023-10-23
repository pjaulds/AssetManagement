using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class TradeAudit
    {

        public static AuditCollection Audit(Trade trade, Trade tradeOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (trade.mCode != tradeOld.mCode)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, trade);
                audit.mField = "code";
                audit.mOldValue = tradeOld.mCode.ToString();
                audit.mNewValue = trade.mCode.ToString();
                audit_collection.Add(audit);
            }

            if (trade.mName != tradeOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, trade);
                audit.mField = "name";
                audit.mOldValue = tradeOld.mName.ToString();
                audit.mNewValue = trade.mName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Trade trade)
        {
            audit.mUserId = trade.mUserId;
            audit.mTableId = (int)(Tables.amQt_Trade);
            audit.mRowId = trade.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}