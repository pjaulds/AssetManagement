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
    public static class CompanyProfileManager
    {
        #region Public Methods
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static CompanyProfileCollection GetList()
        {
            CompanyProfileCriteria companyprofile = new CompanyProfileCriteria();
            return GetList(companyprofile);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static CompanyProfileCollection GetList(CompanyProfileCriteria companyprofileCriteria)
        {
            return CompanyProfileDB.GetList(companyprofileCriteria);
        }

        public static int SelectCountForGetList(CompanyProfileCriteria companyprofileCriteria)
        {
            return CompanyProfileDB.SelectCountForGetList(companyprofileCriteria);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static CompanyProfile GetItem(int id)
        {
            CompanyProfile companyprofile = CompanyProfileDB.GetItem(id);
            return companyprofile;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static int Save(CompanyProfile myCompanyProfile)
        {
            if (!myCompanyProfile.Validate())
            {
                throw new InvalidSaveOperationException("Can't save an invalid companyprofile. Please make sure Validate() returns true before you call Save.");
            }
            using (TransactionScope myTransactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {


                if (myCompanyProfile.mId != 0)
                    AuditUpdate(myCompanyProfile);

                int id = CompanyProfileDB.Save(myCompanyProfile);

                if (myCompanyProfile.mId == 0)
                    AuditInsert(myCompanyProfile, id);

                myCompanyProfile.mId = id;
                myTransactionScope.Complete();
                return id;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static int Delete(CompanyProfile myCompanyProfile)
        {
            if (CompanyProfileDB.Delete(myCompanyProfile.mId))
            {
                AuditDelete(myCompanyProfile);
                return myCompanyProfile.mId;
            }

            else
                return 0;
        }
        #endregion

        #region Audit
        private static void AuditInsert(CompanyProfile myCompanyProfile, int id)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myCompanyProfile.mUserId;
            audit.mTableId = (Int16)Tables.amQt_CompanyProfile;
            audit.mRowId = id;
            audit.mActionId = (byte)AuditAction.Insert;
            AuditDB.Save(audit);
        }

        private static void AuditDelete(CompanyProfile myCompanyProfile)
        {
            BusinessEntities.Audit audit = new BusinessEntities.Audit();
            audit.mUserId = myCompanyProfile.mUserId;
            audit.mTableId = (Int16)Tables.amQt_CompanyProfile;
            audit.mRowId = myCompanyProfile.mId;
            audit.mActionId = (byte)AuditAction.Delete;
            AuditDB.Save(audit);
        }

        private static void AuditUpdate(CompanyProfile myCompanyProfile)
        {
            CompanyProfile old_companyprofile = GetItem(myCompanyProfile.mId);
            AuditCollection audit_collection = CompanyProfileAudit.Audit(myCompanyProfile, old_companyprofile);
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