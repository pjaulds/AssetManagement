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
    public static class PersonnelManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static PersonnelCollection GetList()
        {
            PersonnelCriteria personnel = new PersonnelCriteria();
            return GetList(personnel);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PersonnelCollection GetList(PersonnelCriteria personnelCriteria)
        {
            return PersonnelDB.GetList(personnelCriteria);
        }

        public static int SelectCountForGetList(PersonnelCriteria personnelCriteria)
        {
            return PersonnelDB.SelectCountForGetList(personnelCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Personnel GetItem(int id)
        {
            Personnel personnel = PersonnelDB.GetItem(id);
            return personnel;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Personnel myPersonnel)
        {
            if (!myPersonnel.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid personnel. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myPersonnel.mId != 0)
                    AuditUpdate(myPersonnel);

                int id = PersonnelDB.Save(myPersonnel);

                if (myPersonnel.mId == 0)
                    AuditInsert(myPersonnel, id);

                myPersonnel.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Personnel myPersonnel)
        {
            if (PersonnelDB.Delete(myPersonnel.mId))
            {
                AuditDelete(myPersonnel);
                return myPersonnel.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Personnel myPersonnel, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPersonnel.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Personnel;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Personnel myPersonnel)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myPersonnel.mUserId;
            audit.mTableId = (Int16)Tables.amQt_Personnel;
            audit.mRowId = myPersonnel.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Personnel myPersonnel)
        {
            Personnel old_personnel = GetItem(myPersonnel.mId);
            AuditCollection audit_collection = PersonnelAudit.Audit(myPersonnel, old_personnel);
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