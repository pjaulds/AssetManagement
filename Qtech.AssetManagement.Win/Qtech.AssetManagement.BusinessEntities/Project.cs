using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Project : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }
        public String mCode { get; set; }
        public String mName { get; set; }
        public Boolean mActive { get; set; }

        #endregion
    }
}
