using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class FixedAssetSettingDate : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }
        public DateTime mDate { get; set; }

        #endregion
    }
}