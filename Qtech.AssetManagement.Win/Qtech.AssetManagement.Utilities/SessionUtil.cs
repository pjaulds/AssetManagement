﻿using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.Utilities
{
    public class SessionUtil
    {
        public static BusinessEntities.Users mUser = null;
        public static void UserValidate(ref bool allowSelect, ref bool allowInsert, ref bool allowUpdate, ref bool allowDelete, ref bool allowPrint, short moduleId)
        {
            if(mUser == null)
            {
                //maybe linked to qasa.

                UsersCriteria usersCriteria = new UsersCriteria();
                usersCriteria.mUserIdQasaDatabase = Qasa.Utilities.SessionUtil.user_id;
                UsersCollection users = UsersManager.GetList(usersCriteria);
                if (users.Count > 0) mUser = users.First();
            }

            UserAccessCriteria criteria = new UserAccessCriteria();
            criteria.mUserId = mUser.mId;
            criteria.mModuleId = moduleId;

            if (UserAccessManager.SelectCountForGetList(criteria) == 0) return;

            UserAccess access = UserAccessManager.GetList(criteria).First();

            allowSelect = access.mSelect;
            allowInsert = access.mInsert;
            allowUpdate = access.mUpdate;
            allowDelete = access.mDelete;
            allowPrint = access.mPrint;
        }

        public static bool UserAllowNotedBy(Users user, short moduleId)
        {

            UserAccessCriteria criteria = new UserAccessCriteria();
            criteria.mUserId = user.mId;
            criteria.mModuleId = moduleId;

            if (UserAccessManager.SelectCountForGetList(criteria) == 0) return false;

            UserAccess access = UserAccessManager.GetList(criteria).First();
            return access.mNoted;
        }

        public static bool UserAllowCheckedBy(Users user, short moduleId)
        {

            UserAccessCriteria criteria = new UserAccessCriteria();
            criteria.mUserId = user.mId;
            criteria.mModuleId = moduleId;

            if (UserAccessManager.SelectCountForGetList(criteria) == 0) return false;

            UserAccess access = UserAccessManager.GetList(criteria).First();            
            return access.mChecked;
        }

        public static bool UserAllowApprove(Users user, short moduleId)
        {

            UserAccessCriteria criteria = new UserAccessCriteria();
            criteria.mUserId = user.mId;
            criteria.mModuleId = moduleId;

            if (UserAccessManager.SelectCountForGetList(criteria) == 0) return false;

            UserAccess access = UserAccessManager.GetList(criteria).First();
            return access.mApprove;
        }

        public static bool UserAllowDelete(Users user, short moduleId)
        {

            UserAccessCriteria criteria = new UserAccessCriteria();
            criteria.mUserId = user.mId;
            criteria.mModuleId = moduleId;

            if (UserAccessManager.SelectCountForGetList(criteria) == 0) return false;

            UserAccess access = UserAccessManager.GetList(criteria).First();
            return access.mDelete;
        }
    }

    public static class StringExtensions
    {
        public static bool ContainsAny(this string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                bool result = false;
                foreach (string value in values)
                {
                    //return only the words match within a record.
                    if (str.Contains(value))
                        result = true;
                    else
                        return false;
                }

                return result;
            }

            return false;
        }
    }
}
