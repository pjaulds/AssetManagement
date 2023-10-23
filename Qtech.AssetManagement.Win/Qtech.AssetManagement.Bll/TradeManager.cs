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
    public static class TradeManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static TradeCollection GetList()
        {
            TradeCriteria trade = new TradeCriteria();
            return GetList(trade);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static TradeCollection GetList(TradeCriteria tradeCriteria)
        {
            return TradeDB.GetList(tradeCriteria);
        }

        public static int SelectCountForGetList(TradeCriteria tradeCriteria)
        {
            return TradeDB.SelectCountForGetList(tradeCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Trade GetItem(int id)
        {
            Trade trade = TradeDB.GetItem(id);
            return trade;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Trade myTrade)
        {
            if (!myTrade.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid trade. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myTrade.mId != 0)
                    AuditUpdate(myTrade);

                int id = TradeDB.Save(myTrade);

                if (myTrade.mId == 0)
                    AuditInsert(myTrade, id);

                myTrade.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Trade myTrade)
        {
            if (TradeDB.Delete(myTrade.mId))
            {
                AuditDelete(myTrade);
                return myTrade.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Trade myTrade, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myTrade.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Trade;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Trade myTrade)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myTrade.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Trade;
            audit.mRowId = myTrade.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Trade myTrade)
        {
            Trade old_trade = GetItem(myTrade.mId);
            AuditCollection audit_collection = TradeAudit.Audit(myTrade, old_trade);
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