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
    public static class UserAccessManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static UserAccessCollection GetList()
        {
            UserAccessCriteria useraccess = new UserAccessCriteria();
            return GetList(useraccess, string.Empty, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UserAccessCollection GetList(string sortExpression)
        {
            UserAccessCriteria useraccess = new UserAccessCriteria();
            return GetList(useraccess, sortExpression, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UserAccessCollection GetList(int startRowIndex, int maximumRows)
        {
            UserAccessCriteria useraccess = new UserAccessCriteria();
            return GetList(useraccess, string.Empty, startRowIndex, maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UserAccessCollection GetList(UserAccessCriteria useraccessCriteria)
        {
            return GetList(useraccessCriteria, string.Empty, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UserAccessCollection GetList(UserAccessCriteria useraccessCriteria, string sortExpression, int startRowIndex, int maximumRows)
        {
            UserAccessCollection myCollection = UserAccessDB.GetList(useraccessCriteria);
            if (!string.IsNullOrEmpty(sortExpression))
            {
                myCollection.Sort(new UserAccessComparer(sortExpression));
            }
            if (startRowIndex >= 0 && maximumRows > 0)
            {
                return new UserAccessCollection(myCollection.Skip(startRowIndex).Take(maximumRows).ToList());
            }
            return myCollection;
        }

        public static int SelectCountForGetList(UserAccessCriteria useraccessCriteria)
        {
            return UserAccessDB.SelectCountForGetList(useraccessCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UserAccess GetItem(int id)
        {
            UserAccess useraccess = UserAccessDB.GetItem(id);
            return useraccess;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(UserAccess myUserAccess)
        {
            if (!myUserAccess.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid useraccess. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myUserAccess.mId != 0)
                    AuditUpdate(myUserAccess);

                int id = UserAccessDB.Save(myUserAccess);

                if (myUserAccess.mId == 0)
                    AuditInsert(myUserAccess, id);

                myUserAccess.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(UserAccess myUserAccess)
        {
            if (UserAccessDB.Delete(myUserAccess.mId))
            {
                AuditDelete(myUserAccess);
                return myUserAccess.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(UserAccess myUserAccess, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myUserAccess.mUserId;
            audit.mTableId = (Int16)Tables.amQt_UserAccess;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(UserAccess myUserAccess)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myUserAccess.mUserId;
            audit.mTableId = (Int16)Tables.amQt_UserAccess;
            audit.mRowId = myUserAccess.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(UserAccess myUserAccess)
        {
            UserAccess old_useraccess = GetItem(myUserAccess.mId);
            AuditCollection audit_collection = UserAccessAudit.Audit(myUserAccess, old_useraccess);
            if (audit_collection != null)
            {
                foreach (BusinessEntities.Audit audit in audit_collection)
                {
                    AuditManager.Save(audit);
                }
            }
        }
        #endregion

        #region IComparable
        private class UserAccessComparer : IComparer<UserAccess>
        {
            private string _sortColumn;
            private bool _reverse;

            public UserAccessComparer(string sortExpression)
            {
                if (string.IsNullOrEmpty(sortExpression))
                {
                    sortExpression = "field_name desc";
                }
                _reverse = sortExpression.ToUpperInvariant().EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
                if (_reverse)
                {
                    _sortColumn = sortExpression.Substring(0, sortExpression.Length - 5);
                }
                else
                {
                    _sortColumn = sortExpression;
                }
            }

            public int Compare(UserAccess x, UserAccess y)
            {
                int retVal = 0;
                //switch (_sortColumn.ToUpperInvariant())
                //{
                //    case "FIELD":
                //        retVal = string.Compare(x.mRemarks, y.mRemarks, StringComparison.OrdinalIgnoreCase);
                //        break;
                //}

                int _reverseInt = 1;
                if ((_reverse))
                {
                    _reverseInt = -1;
                }
                return (retVal * _reverseInt);
            }
        }
        #endregion
    }
}