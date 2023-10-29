using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class WorkOrder : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mDate { get; set; }

        [Display(Name = "Expected Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mExpectedStartDate { get; set; }

        [Display(Name = "Expected End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:D}")]
        public DateTime mExpectedEndDate { get; set; }

        [Display(Name = "W.O. No.")]
        public Int32 mNumber { get; set; }
        public string mWorkOrderNo { get; set; }

        [Display(Name = "M.R. No.")]
        [NotEqualTo(Message = "Please select maintenance request", mValue = "0")]
        public Int32 mMaintenanceRequestId { get; set; }
        public String mMaintenanceRequestNo { get; set; }

        [Display(Name = "W.O. Type")]
        [NotEqualTo(Message = "Please select work order type", mValue = "0")]
        public Int32 mWorkOrderTypeId { get; set; }
        public String mWorkOrderTypeName { get; set; }

        [Display(Name = "Maintenance Job Type Variant")]
        [NotEqualTo(Message = "Please select maintenance job type variant", mValue = "0")]
        public Int32 mMaintenanceJobTypeVariantId { get; set; }
        public String mMaintenanceJobTypeVariantName { get; set; }

        [Display(Name = "Trade")]
        [NotEqualTo(Message = "Please select trade", mValue = "0")]
        public Int32 mTradeId { get; set; }
        public String mTradeName { get; set; }

        [Display(Name = "Asset Name")]
        public String mFixedAssetName { get; set; }

        [Display(Name = "Functional Location")]
        public String mFunctionalLocationName { get; set; }

        [Display(Name = "Description")]
        public String mDescription { get; set; }

        [Display(Name = "Service Level")]
        public String mServiceLevelName { get; set; }

        [Display(Name = "Status")]
        public String mStatus { get; set; }

        [Display(Name = "Active")]
        public Boolean mActive { get; set; }

        public WorkOrderHoursCollection mWorkOrderHoursCollection { get; set; }
        public WorkOrderHoursCollection mDeletedWorkOrderHoursCollection { get; set; }
        #endregion
    }
}
