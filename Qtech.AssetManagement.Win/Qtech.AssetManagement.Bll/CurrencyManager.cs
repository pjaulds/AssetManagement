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
    public static class CurrencyManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static CurrencyCollection GetList()
        {
            CurrencyCriteria currency = new CurrencyCriteria();
            return GetList(currency);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static CurrencyCollection GetList(CurrencyCriteria currencyCriteria)
        {
            return CurrencyDB.GetList(currencyCriteria);
        }

        public static int SelectCountForGetList(CurrencyCriteria currencyCriteria)
        {
            return CurrencyDB.SelectCountForGetList(currencyCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Currency GetItem(int id)
        {
            Currency currency = CurrencyDB.GetItem(id);
            return currency;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Currency myCurrency)
        {
            if (!myCurrency.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid currency. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myCurrency.mId != 0)
                    AuditUpdate(myCurrency);

                int id = CurrencyDB.Save(myCurrency);

                if (myCurrency.mId == 0)
                    AuditInsert(myCurrency, id);

                myCurrency.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Currency myCurrency)
        {
            if (CurrencyDB.Delete(myCurrency.mId))
            {
                AuditDelete(myCurrency);
                return myCurrency.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Currency myCurrency, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myCurrency.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Currency;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Currency myCurrency)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myCurrency.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Currency;
            audit.mRowId = myCurrency.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Currency myCurrency)
        {
            Currency old_currency = GetItem(myCurrency.mId);
            AuditCollection audit_collection = CurrencyAudit.Audit(myCurrency, old_currency);
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