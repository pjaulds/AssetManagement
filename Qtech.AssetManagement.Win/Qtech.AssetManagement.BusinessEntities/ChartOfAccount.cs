using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class ChartOfAccount : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Account Type")]
        [NotEqualTo(Message = "Please select account type", mValue = "0")]
        public Int32 mAccountTypeId { get; set; }
        public String mAccountTypeName { get; set; }

        [Display(Name = "Account Group")]
        [NotEqualTo(Message = "Please select account group", mValue = "0")]
        public Int32 mAccountGroupId { get; set; }
        public String mAccountGroupName { get; set; }

        [Display(Name = "Account Classification")]
        [NotEqualTo(Message = "Please select account classification", mValue = "0")]
        public Int32 mAccountClassificationId { get; set; }
        public String mAccountClassificationName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Code")]
        [NotNullOrEmpty(Message = "Please enter valid asset account code.")]
        public String mCode { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [NotNullOrEmpty(Message = "Please enter valid asset account name/title.")]
        public String mName { get; set; }

        [Display(Name = "Main Account")]
        public Int32 mChartOfAccountMainId { get; set; }
        public String mChartOfAccountMainName { get; set; }

        [Display(Name = "Closing Account")]
        public Int32 mChartOfAccountCloseId { get; set; }
        public String mChartOfAccountCloseName { get; set; }

        [Display(Name = "Payable/Sales")]
        public Boolean mPayableSales { get; set; }

        [Display(Name = "Normal Balance")]
        public Boolean mDebitCredit { get; set; }

        [Display(Name = "Active")]
        public bool mActive { get; set; }
      
        #endregion
    }
}