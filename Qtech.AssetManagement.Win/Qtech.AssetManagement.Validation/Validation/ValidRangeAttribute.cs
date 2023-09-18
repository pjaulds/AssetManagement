 // -----------------------------------------------------------------------
// <copyright file="ValidRangeAttribute.cs" company="Imar.Spaanjaars.Com">
//   Copyright 2008 - 2009 - Imar.Spaanjaars.Com. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The ValidRangeAttribute class allows you to make sure that a numeric value falls between a Min and a Max value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValidRangeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets a value that determines the minimum value (inclusive) for the underlying value to be valid.
        /// </summary>
        /// <value>The minimum value.</value>
        public double Min { get; set; }

        /// <summary>
        /// Gets or sets a value that determines the maximum value (inclusive) for the underlying value to be valid.
        /// </summary>
        /// <value>The maximum value.</value>
        public double Max { get; set; }

        /// <summary>
        /// Determines whether the value of the underlying property falls between the Min and Max values.
        /// </summary>
        /// <param name="item">The underlying value of the propery that is being validated.</param>
        /// <returns>
        /// 	<c>true</c> if the specified item falls between Min and Max (inclusive); otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(object item)
        {
            double tempValue = Convert.ToDouble(item);
            return tempValue >= Min && tempValue <= Max;
        }
    }
}
