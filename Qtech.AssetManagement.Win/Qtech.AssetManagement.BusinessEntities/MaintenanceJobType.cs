﻿using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class MaintenanceJobType : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Code")]
        [NotNullOrEmpty(Message = "Please enter valid maintenance job type code.")]
        public String mCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [NotNullOrEmpty(Message = "Please enter valid maintenance job type name/title.")]
        public String mName { get; set; }

        public MaintenanceJobTypeVariantCollection mMaintenanceJobTypeVariantCollection { get; set; }
        public MaintenanceJobTypeVariantCollection mDeletedMaintenanceJobTypeVariantCollection { get; set; }

        #endregion
    }
}