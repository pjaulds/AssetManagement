using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The NotNullOrEmptyAttribute class allows you to make sure that a string value is not null or an empty string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MaxStringLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// the value to compare
        /// </summary>
        public Int32 Length { get; set; }

        /// <summary>
        /// Determines whether the value of the underlying property (passed in as the <paramref name="item"/> parameter)
        /// is not exceeding on the number of characters required
        /// </summary>
        /// <param name="item">The underlying value of the propery that is being validated.</param>
        /// <returns>
        /// <c>true</c> if the specified item not exceeding on the number of characters required; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(object item)
        {
            return Convert.ToString(item).Length < Length;
        }
    }
}
