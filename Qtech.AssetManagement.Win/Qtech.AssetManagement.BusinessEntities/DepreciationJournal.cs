using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class DepreciationJournal : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Asset")]
        [NotEqualTo(Message = "Please select item", mValue = "0")]
        public Int32 mFixedAssetId { get; set; }

        [Display(Name = "Year")]
        [NotEqualTo(Message = "Please select year", mValue = "0")]
        public Int16 mYear { get; set; }

        [Display(Name = "Month")]
        [NotEqualTo(Message = "Please select month", mValue = "0")]
        public Byte mMonth { get; set; }

        [Display(Name = "Depreciation Expense Account")]
        [NotEqualTo(Message = "Please select depreciation expense account", mValue = "0")]
        public Int32 mDepreciationExpenseAccountId { get; set; }
        public String mDepreciationExpenseAccountName { get; set; }

        [Display(Name = "DB/CR")]
        public Boolean mDepreciationExpenseAccountDebitCredit { get; set; }

        [Display(Name = "Accumulated Depreciation Account")]
        [NotEqualTo(Message = "Please select accumulated depreciation account", mValue = "0")]
        public Int32 mAccumulatedDepreciationAccountId { get; set; }
        public String mAccumulatedDepreciationAccountName { get; set; }
        
        [Display(Name = "DB/CR")]
        public Boolean mAccumulatedDepreciationAccountDebitCredit { get; set; }

        [Display(Name = "Amount")]
        public Decimal mAmount { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Description")]
        [NotNullOrEmpty(Message = "Please enter description.")]
        public String mDescription { get; set; }

        #endregion
    }
}
