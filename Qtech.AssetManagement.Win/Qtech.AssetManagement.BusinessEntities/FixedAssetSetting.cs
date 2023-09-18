﻿using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class FixedAssetSetting : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Asset Type")]
        [NotNullOrEmpty(Message = "Please enter valid asset type.")]
        public String mAssetType { get; set; }

        [Display(Name = "Asset Account")]
        [NotEqualTo(Message = "Please select asset account", mValue = "0")]
        public Int32 mAssetAccountId { get; set; }
        public String mAssetAccountCode { get; set; }
        public String mAssetAccountName { get; set; }

        [Display(Name = "Accumulated Depreciation Account")]
        [NotEqualTo(Message = "Please select accumulated depreciation account", mValue = "0")]
        public Int32 mAccumulatedDepreciationAccountId { get; set; }
        public String mAccumulatedDepreciationAccountCode { get; set; }
        public String mAccumulatedDepreciationAccountName { get; set; }

        [Display(Name = "Depreciation Expense Account")]
        [NotEqualTo(Message = "Please select depreciation expense account", mValue = "0")]
        public Int32 mDepreciationExpenseAccountId { get; set; }
        public String mDepreciationExpenseAccountCode { get; set; }
        public String mDepreciationExpenseAccountName { get; set; }

        [Display(Name = "Depreciation Method")]
        [NotEqualTo(Message = "Please select depreciation method", mValue = "0")]
        public Int32 mDepreciationMethodId { get; set; }
        public String mDepreciationMethodCode { get; set; }
        public String mDepreciationMethodName { get; set; }

        [Display(Name = "Averaging Method")]
        [NotEqualTo(Message = "Please select averaging method", mValue = "0")]
        public Int32 mAveragingMethodId { get; set; }
        public String mAveragingMethodCode { get; set; }
        public String mAveragingMethodName { get; set; }
        public Decimal mUsefulLifeYears { get; set; }


        public string mAssetAccountHeaderText { get { return "Asset Account"; } }
        public string mAccumulatedDepreciationAccountHeaderText { get { return "Accumulated Depreciation Account"; } }
        public string mDepreciationExpenseAccountHeaderText { get { return "Depreciation Expense Account"; } }
        public string mDepreciationMethodText { get { return "Depreciation Method"; } }
        public string mAveragingMethodText { get { return "Averaging Method"; } }
        #endregion
    }
}