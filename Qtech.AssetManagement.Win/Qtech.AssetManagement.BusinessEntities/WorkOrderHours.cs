using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class WorkOrderHours : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }

        public Int32 mWorkOrderId { get; set; }

        [Display(Name = "Expense Category")]
        [NotEqualTo(Message = "Please select expense category", mValue = "0")]
        public Int32 mExpenseCategoryId { get; set; }
        public String mExpenseCategoryName { get; set; }

        [Display(Name = "Hours")]
        [NotLessEqualToZeroAttribute(Message = "Please enter valid hours.")]
        public Decimal mHours { get; set; }

        [Display(Name = "Rate Per Hour")]
        [NotLessEqualToZeroAttribute(Message = "Please enter rate per hour.")]
        public Decimal mRatePerHour { get; set; }

        #endregion
    }
}