using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class AssetType : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Code")]
        [NotNullOrEmpty(Message = "Please enter valid asset type code.")]
        public String mCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [NotNullOrEmpty(Message = "Please enter valid asset type name/title.")]
        public String mName { get; set; }

        [Display(Name = "Post")]
        public bool mPost { get; set; }

        public Int32 mAssetAccountId { get; set; }
        public String mAssetAccountName { get; set; }
        public Int32 mAccumulatedDepreciationAccountId { get; set; }
        public String mAccumulatedDepreciationAccountName { get; set; }
        public Int32 mProductionDepreciationExpenseAccountId { get; set; }
        public String mProductionDepreciationExpenseAccountName { get; set; }
        public Decimal mProductionDepreciationExpenseAccountValue { get; set; }
        public Int32 mAdminDepreciationExpenseAccountId { get; set; }
        public String mAdminDepreciationExpenseAccountName { get; set; }
        public Decimal mAdminDepreciationExpenseAccountValue { get; set; }
        public Int32 mDepreciationMethodId { get; set; }
        public String mDepreciationMethodName { get; set; }
        public Int32 mAveragingMethodId { get; set; }
        public String mAveragingMethodName { get; set; }

        public decimal mMonths { get; set; }
        public Decimal mUsefulLifeYears { get; set; }
        public Boolean mActive { get; set; }
        public Boolean mDepreciable { get; set; }

        public string mProductionDepreciationExpenseAccountHeaderText { get { return "Depreciation Expense (Production)"; } }
        public string mAdminDepreciationExpenseAccountHeaderText { get { return "Depreciation Expense (Admin)"; } }
        #endregion
    }
}