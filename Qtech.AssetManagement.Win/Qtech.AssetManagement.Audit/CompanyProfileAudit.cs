using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Audit
{
    public class CompanyProfileAudit
    {

        public static AuditCollection Audit(CompanyProfile companyprofile, CompanyProfile companyprofileOld)
        {
            AuditCollection audit_collection = new AuditCollection();
            BusinessEntities.Audit audit = new BusinessEntities.Audit();

            if (companyprofile.mName != companyprofileOld.mName)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, companyprofile);
                audit.mField = "name";
                audit.mOldValue = companyprofileOld.mName.ToString();
                audit.mNewValue = companyprofile.mName.ToString();
                audit_collection.Add(audit);
            }

            if (companyprofile.mAddress != companyprofileOld.mAddress)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, companyprofile);
                audit.mField = "address";
                audit.mOldValue = companyprofileOld.mAddress.ToString();
                audit.mNewValue = companyprofile.mAddress.ToString();
                audit_collection.Add(audit);
            }

            if (companyprofile.mReportLogo != companyprofileOld.mReportLogo)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, companyprofile);
                audit.mField = "report_logo";
                audit.mOldValue = companyprofileOld.mReportLogo.ToString();
                audit.mNewValue = companyprofile.mReportLogo.ToString();
                audit_collection.Add(audit);
            }

            if (companyprofile.mWidth != companyprofileOld.mWidth)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, companyprofile);
                audit.mField = "width";
                audit.mOldValue = companyprofileOld.mWidth.ToString();
                audit.mNewValue = companyprofile.mWidth.ToString();
                audit_collection.Add(audit);
            }

            if (companyprofile.mHeight != companyprofileOld.mHeight)
            {
                audit = new BusinessEntities.Audit();
                LoadCommonData(ref audit, companyprofile);
                audit.mField = "height";
                audit.mOldValue = companyprofileOld.mHeight.ToString();
                audit.mNewValue = companyprofile.mHeight.ToString();
                audit_collection.Add(audit);
            }

            return audit_collection;
        }

        static void LoadCommonData(ref BusinessEntities.Audit audit, CompanyProfile companyprofile)
        {
            audit.mUserId = companyprofile.mUserId;
            audit.mTableId = (int)(Tables.amQt_CompanyProfile);
            audit.mRowId = companyprofile.mId;
            audit.mActionId = (byte)AuditAction.Update;
        }
    }
}
