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

        #endregion
    }
}
