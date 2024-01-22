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
    public static class UsersManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static UsersCollection GetList()
        {
            UsersCriteria users = new UsersCriteria();
            return GetList(users, string.Empty, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UsersCollection GetList(string sortExpression)
        {
            UsersCriteria users = new UsersCriteria();
            return GetList(users, sortExpression, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UsersCollection GetList(int startRowIndex, int maximumRows)
        {
            UsersCriteria users = new UsersCriteria();
            return GetList(users, string.Empty, startRowIndex, maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UsersCollection GetList(UsersCriteria usersCriteria)
        {
            return GetList(usersCriteria, string.Empty, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static UsersCollection GetList(UsersCriteria usersCriteria, string sortExpression, int startRowIndex, int maximumRows)
        {
            UsersCollection myCollection = UsersDB.GetList(usersCriteria);
            if (!string.IsNullOrEmpty(sortExpression))
            {
                myCollection.Sort(new UsersComparer(sortExpression));
            }
            if (startRowIndex >= 0 && maximumRows > 0)
            {
                return new UsersCollection(myCollection.Skip(startRowIndex).Take(maximumRows).ToList());
            }
            return myCollection;
        }

        public static int SelectCountForGetList(UsersCriteria usersCriteria)
        {
            return UsersDB.SelectCountForGetList(usersCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Users GetItem(int id)
        {
            Users users = UsersDB.GetItem(id);
            return users;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(Users myUsers)
        {
            if (!myUsers.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid users. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myUsers.mId != 0)
                    AuditUpdate(myUsers);

                int id = UsersDB.Save(myUsers);

                if (myUsers.mUserAccessCollection != null)
                {
                    foreach(UserAccess item in myUsers.mUserAccessCollection)
                    {
                        item.mUserId = id;
                        UserAccessManager.Save(item);
                    }
                }
                
                if (myUsers.mId == 0)
                    AuditInsert(myUsers, id);

                myUsers.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(Users myUsers)
        {
            if (UsersDB.Delete(myUsers.mId))
            {
                AuditDelete(myUsers);
                return myUsers.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(Users myUsers, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myUsers.mUserId;
            audit.mTableId = (short)Tables.amQt_Users;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(Users myUsers)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myUsers.mUserId;
            audit.mTableId = (short)Tables.amQt_Users;
            audit.mRowId = myUsers.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(Users myUsers)
        {
            Users old_users = GetItem(myUsers.mId);
            AuditCollection audit_collection = UsersAudit.Audit(myUsers, old_users);
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
        private class UsersComparer : IComparer<Users>
        {
            private string _sortColumn;
            private bool _reverse;

            public UsersComparer(string sortExpression)
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

            public int Compare(Users x, Users y)
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