using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Product : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Asset Type")]
        [NotEqualTo(Message = "Please select asset type", mValue = "0")]
        public Int32 mAssetTypeId { get; set; }
        public String mAssetTypeName { get; set; }

        [Display(Name = "Asset Class")]
        [NotEqualTo(Message = "Please select asset class", mValue = "0")]
        public Int32 mAssetClassId { get; set; }
        public String mAssetClassName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Code")]
        [NotNullOrEmpty(Message = "Please enter valid product code.")]
        public String mCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [NotNullOrEmpty(Message = "Please enter valid product name/title.")]
        public String mName { get; set; }

        [Display(Name = "Unit")]
        public int mUnitId { get; set; }
        public string mUnitName { get; set; }

        [Display(Name = "Post")]
        public bool mPost { get; set; }
        #endregion
    }
}