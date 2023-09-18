// -----------------------------------------------------------------------
// <copyright file="ValidEmailAttribute.cs" company="Imar.Spaanjaars.Com">
//   Copyright 2008 - 2009 - Imar.Spaanjaars.Com. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The ValidEmailAttribute class allows you to make sure that a string value contains a valid e-mail address.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValidEmailAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the value of the underlying property (passed in as the <paramref name="item"/> parameter)
        /// contains a valid e-mail address.
        /// </summary>
        /// <param name="item">The underlying value of the propery that is being validated.</param>
        /// <returns>
        /// 	<c>true</c> if the specified item is not null or an empty string; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>Note: when item is null or en empty string, IsValid returns true. This allows you to create options fields. Apply an additional NotNullOrEmpty attribute to enforce a filled in, and valid e-mail address</remarks>
        public override bool IsValid(object item)
        {
            string tempValue = item as string;
            if (string.IsNullOrEmpty(tempValue))
            {
                return true;
            }
            return (tempValue).Contains("@");
        }
    }
}
