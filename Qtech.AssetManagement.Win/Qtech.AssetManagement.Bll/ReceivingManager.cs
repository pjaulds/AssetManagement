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
    public static class ReceivingManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ReceivingCollection GetList()
        {
            ReceivingCriteria receiving = new ReceivingCriteria();
            return GetList(receiving);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ReceivingCollection GetList(ReceivingCriteria receivingCriteria)
        {
            return ReceivingDB.GetList(receivingCriteria);
        }

        public static int SelectCountForGetList(ReceivingCriteria receivingCriteria)
        {
            return ReceivingDB.SelectCountForGetList(receivingCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Receiving GetItem(int id)
        {
            Receiving receiving = ReceivingDB.GetItem(id);
            return receiving;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Receiving myReceiving)
        {
            if (!myReceiving.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid receiving. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myReceiving.mId != 0)
                    AuditUpdate(myReceiving);

                if (myReceiving.mReceivingDetailCollection != null)
                    myReceiving.mAmount = myReceiving.mReceivingDetailCollection.Sum(x => x.mCost * x.mQuantity);

                int id = ReceivingDB.Save(myReceiving);

                if (myReceiving.mReceivingDetailCollection != null)
                {
                    foreach (ReceivingDetail item in myReceiving.mReceivingDetailCollection)
                    {
                        item.mReceivingId = id;
                        item.mUserId = myReceiving.mUserId;
                        ReceivingDetailManager.Save(item);
                    }
                }

                if (myReceiving.mDeletedReceivingDetailCollection != null)
                {
                    foreach (ReceivingDetail item in myReceiving.mDeletedReceivingDetailCollection)
                    {
                        item.mUserId = myReceiving.mUserId;
                        ReceivingDetailManager.Delete(item);
                    }
                }

                if (myReceiving.mId == 0)
                    AuditInsert(myReceiving, id);

                myReceiving.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Receiving myReceiving)
        {
            if (ReceivingDB.Delete(myReceiving.mId))
            {
                AuditDelete(myReceiving);
                return myReceiving.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Receiving myReceiving, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myReceiving.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Receiving;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Receiving myReceiving)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myReceiving.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Receiving;
            audit.mRowId = myReceiving.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Receiving myReceiving)
        {
            Receiving old_receiving = GetItem(myReceiving.mId);
            AuditCollection audit_collection = ReceivingAudit.Audit(myReceiving, old_receiving);
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