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
    public static class UnitManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static UnitCollection GetList()
        {
            UnitCriteria unit = new UnitCriteria();
            return GetList(unit);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UnitCollection GetList(UnitCriteria unitCriteria)
        {
            return UnitDB.GetList(unitCriteria);
        }

        public static int SelectCountForGetList(UnitCriteria unitCriteria)
        {
            return UnitDB.SelectCountForGetList(unitCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Unit GetItem(int id)
        {
            Unit unit = UnitDB.GetItem(id);
            return unit;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Unit myUnit)
        {
            if (!myUnit.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid unit. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myUnit.mId != 0)
                    AuditUpdate(myUnit);

                int id = UnitDB.Save(myUnit);

                if (myUnit.mId == 0)
                    AuditInsert(myUnit, id);

                myUnit.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Unit myUnit)
        {
            if (UnitDB.Delete(myUnit.mId))
            {
                AuditDelete(myUnit);
                return myUnit.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Unit myUnit, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myUnit.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Unit;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Unit myUnit)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myUnit.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Unit;
            audit.mRowId = myUnit.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Unit myUnit)
        {
            Unit old_unit = GetItem(myUnit.mId);
            AuditCollection audit_collection = UnitAudit.Audit(myUnit, old_unit);
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