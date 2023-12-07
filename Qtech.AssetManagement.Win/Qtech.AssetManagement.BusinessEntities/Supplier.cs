using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Supplier : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Code")]
        [NotNullOrEmpty(Message = "Please enter valid supplier code.")]
        public String mCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [NotNullOrEmpty(Message = "Please enter valid supplier name/title.")]
        public String mName { get; set; }

        [Display(Name = "Address")]
        public String mAddress { get; set; }

        [Display(Name = "TIN")]
        public String mTin { get; set; }

        [Display(Name = "Contact No.")]
        public String mContactNo { get; set; }

        [Display(Name = "Email")]
        public String mEmail { get; set; }

        [Display(Name = "Sales Person")]
        public String mSalesPerson { get; set; }

        [Display(Name = "Vat Registered")]
        public Boolean mVatRegistered { get; set; }

        [Display(Name = "Vat Rate")]
        public Decimal mVatRate { get; set; }

        [Display(Name = "Witholding Tax")]
        public Decimal mWitholdingTax { get; set; }

        [Display(Name = "Business Style")]
        public String mBusinessStyle { get; set; }

        #endregion
    }
}