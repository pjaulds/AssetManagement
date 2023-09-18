using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class FixedAsset : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Code/Number")]
        [NotNullOrEmpty(Message = "Please enter asset code/number.")]
        public String mCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Functional Name")]
        [NotNullOrEmpty(Message = "Please enter asset name.")]
        public String mName { get; set; }

        [Display(Name = "Asset Type")]
        [NotEqualTo(Message = "Please select asset type", mValue = "0")]
        public Int32 mTypeId { get; set; }
        public String mTypeName { get; set; }

        [Display(Name = "Functional Location")]
        [NotEqualTo(Message = "Please select functional location", mValue = "0")]
        public Int32 mFunctionalLocationId { get; set; }
        public String mFunctionalLocationName { get; set; }

        [Display(Name = "Description")]
        public String mDescription { get; set; }

        [Display(Name = "Purchase Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mPurchaseDate { get; set; }

        [Display(Name = "Purchase Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Decimal mPurchasePrice { get; set; }

        [Display(Name = "Warranty Expiry")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mWarrantyExpiry { get; set; }

        [Display(Name = "Serial No")]
        public String mSerialNo { get; set; }

        [Display(Name = "Model")]
        public String mModel { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDepreciationStartDate { get; set; }

        [Display(Name = "Depreciation Method")]
        public Int32 mDepreciationMethodId { get; set; }
        public String mDepreciationMethodName { get; set; }

        [Display(Name = "Averaging Method")]
        public Int32 mAveragingMethodId { get; set; }
        public String mAveragingMethodName { get; set; }

        [Display(Name = "Residual Value")]
        public Decimal mResidualValue { get; set; }

        [Display(Name = "Useful Life (Years)")]
        public Int16 mUsefulLifeYears { get; set; }
        public Boolean mIsDraft { get; set; }
        public Boolean mIsRegistered { get; set; }
        public Boolean mIsDisposed { get; set; }

        #endregion
    }
}