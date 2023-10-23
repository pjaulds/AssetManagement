using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class MaintenanceRequest : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDate { get; set; }

        [Display(Name = "M.R. No.")]
        public Int32 mNumber { get; set; }
        public string mMaintenanceRequestNo { get; set; }

        [Display(Name = "Actual Start")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mStartDate { get; set; }

        [Display(Name = "Actual End")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mEndDate { get; set; }

        [Display(Name = "Request Type")]
        [NotEqualTo(Message = "Please select request type", mValue = "0")]
        public Int32 mMaintenanceRequestTypeId { get; set; }
        public String mMaintenanceRequestTypeName { get; set; }

        [Display(Name = "Service Level")]
        [NotEqualTo(Message = "Please select service level", mValue = "0")]
        public Int32 mServiceLevelId { get; set; }
        public String mServiceLevelName { get; set; }

        [Display(Name = "Requested By")]
        [NotEqualTo(Message = "Please select requested by", mValue = "0")]
        public Int32 mRequestedById { get; set; }
        public String mRequestedByName { get; set; }

        [Display(Name = "Functional Location")]
        [NotEqualTo(Message = "Please select functional location", mValue = "0")]
        public Int32 mFunctionalLocationId { get; set; }
        public String mFunctionalLocationName { get; set; }

        [Display(Name = "Asset Name")]
        [NotEqualTo(Message = "Please select fixed asset", mValue = "0")]
        public Int32 mFixedAssetId { get; set; }
        public String mFixedAssetName { get; set; }

        [Display(Name = "Fault Symptoms")]
        [NotEqualTo(Message = "Please select fault symptom", mValue = "0")]
        public Int32 mFaultSymptomsId { get; set; }
        public String mFaultSymptomsName { get; set; }

        [Display(Name = "Fault Area")]
        [NotEqualTo(Message = "Please select fault area", mValue = "0")]
        public Int32 mFaultAreaId { get; set; }
        public String mFaultAreaName { get; set; }

        [Display(Name = "Description")]
        public String mDescription { get; set; }

        [Display(Name = "Status")]
        public String mStatus { get; set; }

        [Display(Name = "Active")]
        public Boolean mActive { get; set; }

        #endregion
    }
}
