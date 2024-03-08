using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class Users : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Username")]
        public String mUsername { get; set; }
        public String mHash { get; set; }
        public byte[] mSalt { get; set; }

        [Display(Name = "Photo")]
        public byte[] mPhoto { get; set; }

        public string mReturnUrl { get; set; }

        [Display(Name = "Password")]
        public string mPassword { get; set; }
        public bool mRemember { get; set; }

        public int mPersonnelId { get; set; }

        /// <summary>
        /// mostly this is true for admins only
        /// </summary>
        public bool mAllowNoSchedule { get; set; }

        /// <summary>
        /// property to hold the default branch id during login
        /// </summary>
        public int mBranchId { get; set; }
        public bool mDisable { get; set; }
                
        public UserAccessCollection mUserAccessCollection { get; set; }
        #endregion
    }
}