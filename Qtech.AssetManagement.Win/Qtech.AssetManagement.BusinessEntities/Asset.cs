using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Asset : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }
        public String mCode { get; set; }
        public DateTime mDate { get; set; }
        public DateTime mReceivedDate { get; set; }
        public String mName { get; set; }

        [NotEqualTo(Message = "Please select asset type", mValue = "0")]
        public Int32 mAssetTypeId { get; set; }
        public String mAssetTypeName { get; set; }
        public Decimal mAcquisitionCost { get; set; }
        public DateTime mWarrantyExpiry { get; set; }
        public String mBrand { get; set; }
        public String mModel { get; set; }
        public String mSerialNumber { get; set; }
        public String mCapacity { get; set; }
        public String mEngineNumber { get; set; }
        public String mChassisNumber { get; set; }
        public String mPlateNumber { get; set; }

        [NotEqualTo(Message = "Please select functional location", mValue = "0")]
        public Int32 mFunctionalLocationId { get; set; }
        public String mFunctionalLocationName { get; set; }
        public Int32 mPersonnelId { get; set; }
        public String mPersonnelName { get; set; }
        public Int32 mProjectId { get; set; }
        public String mProjectName { get; set; }
        public Int32 mRegisteredById { get; set; }
        public String mRegisteredByName { get; set; }
        public Boolean mActive { get; set; }

        public string mRemarks { get; set; }
        public bool mDisable { get; set; }

        public string mAssetNo { get; set; }
        public Decimal mResidualValue { get; set; }
        public Decimal mUsefulLife { get; set; }

        #endregion
    }
}
