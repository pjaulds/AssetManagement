using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class FunctionalLocation : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Code/ID")]
        [NotNullOrEmpty(Message = "Please enter code/ID.")]
        public String mCode { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Functional Name")]
        [NotNullOrEmpty(Message = "Please enter functional location name.")]
        public String mName { get; set; }
        public Int32 mParentFlId { get; set; }
        
        [Display(Name = "Parent FL")]
        public String mParentFlName { get; set; }

        //[Required]
        [Display(Name = "Status")]
        //[NotNullOrEmpty(Message = "Please select status.")]
        public String mFlStatus { get; set; }

        [Display(Name = "Address Name")]
        public String mAddressName { get; set; }

        [Display(Name = "Street")]
        public String mStreet { get; set; }

        [Display(Name = "City")]
        public String mCity { get; set; }

        [Display(Name = "Province")]
        public String mProvince { get; set; }

        [Display(Name = "Country")]
        public String mCountry { get; set; }

        [Display(Name = "Zip Code")]
        public String mZipCode { get; set; }

        [Display(Name = "Active")]
        public bool mActive { get; set; }

        public string mRemarks { get; set; }
        #endregion
    }
}
