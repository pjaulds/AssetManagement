using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The NotEqualToAttribute class allows you to make sure that a numeric value not fall on a given value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotLessEqualToZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object item)
        {
            decimal tempValue = Convert.ToDecimal(item);
            return tempValue > 0;
        }
    }
}
