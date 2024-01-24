using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class DisposalAudit
    {

        public static AuditCollection Audit(Disposal disposal, Disposal disposalOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (disposal.mFixedAssetId != disposalOld.mFixedAssetId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, disposal);
                audit.mField = "Fixed Asset ";
                audit.mOldValue = disposalOld.mFixedAssetName.ToString();
                audit.mNewValue = disposal.mFixedAssetName.ToString();
                audit_collection.Add(audit);
            }

            if (disposal.mDateDisposed != disposalOld.mDateDisposed)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, disposal);
                audit.mField = "Date Disposed";
                audit.mOldValue = disposalOld.mDateDisposed.ToString();
                audit.mNewValue = disposal.mDateDisposed.ToString();
                audit_collection.Add(audit);
            }

            if (disposal.mSalesProceeds != disposalOld.mSalesProceeds)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, disposal);
                audit.mField = "Sales Proceeds";
                audit.mOldValue = disposalOld.mSalesProceeds.ToString();
                audit.mNewValue = disposal.mSalesProceeds.ToString();
                audit_collection.Add(audit);
            }

            if (disposal.mGainLosses != disposalOld.mGainLosses)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, disposal);
                audit.mField = "Gain Loss";
                audit.mOldValue = disposalOld.mGainLosses.ToString();
                audit.mNewValue = disposal.mGainLosses.ToString();
                audit_collection.Add(audit);
            }

            if (disposal.mCashAccountId != disposalOld.mCashAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, disposal);
                audit.mField = "Cash Account ";
                audit.mOldValue = disposalOld.mCashAccountName.ToString();
                audit.mNewValue = disposal.mCashAccountName.ToString();
                audit_collection.Add(audit);
            }

            if (disposal.mGainLossAccountId != disposalOld.mGainLossAccountId)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, disposal);
                audit.mField = "Gain Loss Account ";
                audit.mOldValue = disposalOld.mGainLossAccountName.ToString();
                audit.mNewValue = disposal.mGainLossAccountName.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, Disposal disposal)
        {
            audit.mUserId = disposal.mUserId;
            audit.mTableId = (int)(Tables.amQt_Disposal);
            audit.mRowId = disposal.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}