using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class UserAccess : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }
        public new Int32 mUserId { get; set; }
        public Int16 mModuleId { get; set; }
        public String mModuleName { get; set; }
        public Boolean mSelect { get; set; }
        public Boolean mInsert { get; set; }
        public Boolean mUpdate { get; set; }
        public Boolean mDelete { get; set; }
        public Boolean mPrint { get; set; }

        public string mModuleGroup { get; set; }
        #endregion
    }
}