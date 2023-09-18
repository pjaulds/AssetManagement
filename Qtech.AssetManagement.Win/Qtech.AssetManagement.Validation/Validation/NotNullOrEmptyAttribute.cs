// -----------------------------------------------------------------------
// <copyright file="NotNullOrEmptyAttribute.cs" company="Imar.Spaanjaars.Com">
//   Copyright 2008 - 2009 - Imar.Spaanjaars.Com. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The NotNullOrEmptyAttribute class allows you to make sure that a string value is not null or an empty string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotNullOrEmptyAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the value of the underlying property (passed in as the <paramref name="item"/> parameter)
        /// is not null or an empty string.
        /// </summary>
        /// <param name="item">The underlying value of the propery that is being validated.</param>
        /// <returns>
        /// <c>true</c> if the specified item is not null or an empty string; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(object item)
        {
            if (item is string)
            {
                return !string.IsNullOrEmpty(item as String);
            }
            return item != null;
        }
    }
}
