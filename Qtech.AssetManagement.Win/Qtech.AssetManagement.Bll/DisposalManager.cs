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
    public static class DisposalManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DisposalCollection GetList()
        {
            DisposalCriteria disposal = new DisposalCriteria();
            return GetList(disposal);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DisposalCollection GetList(DisposalCriteria disposalCriteria)
        {
            return DisposalDB.GetList(disposalCriteria);
        }

        public static int SelectCountForGetList(DisposalCriteria disposalCriteria)
        {
            return DisposalDB.SelectCountForGetList(disposalCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Disposal GetItem(int id)
        {
            Disposal disposal = DisposalDB.GetItem(id);
            return disposal;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Disposal myDisposal)
        {
            if (!myDisposal.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid disposal. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myDisposal.mId != 0)
                    AuditUpdate(myDisposal);

                int id = DisposalDB.Save(myDisposal);

                if (myDisposal.mId == 0)
                    AuditInsert(myDisposal, id);

                myDisposal.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Disposal myDisposal)
        {
            if (DisposalDB.Delete(myDisposal.mId))
            {
                AuditDelete(myDisposal);
                return myDisposal.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Disposal myDisposal, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDisposal.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Disposal;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Disposal myDisposal)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myDisposal.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Disposal;
            audit.mRowId = myDisposal.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Disposal myDisposal)
        {
            Disposal old_disposal = GetItem(myDisposal.mId);
            AuditCollection audit_collection = DisposalAudit.Audit(myDisposal, old_disposal);
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