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
    public sealed class NotEqualToAttribute : ValidationAttribute
    {
        /// <summary>
        /// the value to compare
        /// </summary>
        public string mValue { get; set; }

        public override bool IsValid(object item)
        {
            string tempValue = Convert.ToString(item);
            return tempValue != mValue;
        }
    }
}
