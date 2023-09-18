using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class CompanyProfile : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [NotNullOrEmpty(Message = "Please enter valid company name.")]
        public String mName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Address")]
        [NotNullOrEmpty(Message = "Please enter address.")]
        public String mAddress { get; set; }
        public byte[] mReportLogo { get; set; }
        public Int32 mWidth { get; set; }
        public Int32 mHeight { get; set; }

        #endregion
    }
}