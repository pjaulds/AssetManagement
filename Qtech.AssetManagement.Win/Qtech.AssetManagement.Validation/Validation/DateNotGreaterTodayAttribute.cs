using System;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The DateNotGreaterTodayAttribute class allows you to make sure that a date value is not greater than today.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateNotGreaterTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object item)
        {
            return Convert.ToDateTime(item) < DateTime.Now;
        }
    }
}
