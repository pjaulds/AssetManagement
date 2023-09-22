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
    public static class ReceivingDetailManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static ReceivingDetailCollection GetList()
        {
            ReceivingDetailCriteria receivingdetail = new ReceivingDetailCriteria();
            return GetList(receivingdetail);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ReceivingDetailCollection GetList(ReceivingDetailCriteria receivingdetailCriteria)
        {
            return ReceivingDetailDB.GetList(receivingdetailCriteria);
        }

        public static int SelectCountForGetList(ReceivingDetailCriteria receivingdetailCriteria)
        {
            return ReceivingDetailDB.SelectCountForGetList(receivingdetailCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ReceivingDetail GetItem(int id)
        {
            ReceivingDetail receivingdetail = ReceivingDetailDB.GetItem(id);
            return receivingdetail;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(ReceivingDetail myReceivingDetail)
        {
            if (!myReceivingDetail.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid receivingdetail. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myReceivingDetail.mId != 0)
                    AuditUpdate(myReceivingDetail);

                int id = ReceivingDetailDB.Save(myReceivingDetail);

                if (myReceivingDetail.mId == 0)
                    AuditInsert(myReceivingDetail, id);

                myReceivingDetail.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(ReceivingDetail myReceivingDetail)
        {
            if (ReceivingDetailDB.Delete(myReceivingDetail.mId))
            {
                AuditDelete(myReceivingDetail);
                return myReceivingDetail.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(ReceivingDetail myReceivingDetail, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myReceivingDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ReceivingDetail;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(ReceivingDetail myReceivingDetail)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myReceivingDetail.mUserId;
            audit.mTableId = (Int16)Tables.amQt_ReceivingDetail;
            audit.mRowId = myReceivingDetail.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(ReceivingDetail myReceivingDetail)
        {
            ReceivingDetail old_receivingdetail = GetItem(myReceivingDetail.mId);
            AuditCollection audit_collection = ReceivingDetailAudit.Audit(myReceivingDetail, old_receivingdetail);
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