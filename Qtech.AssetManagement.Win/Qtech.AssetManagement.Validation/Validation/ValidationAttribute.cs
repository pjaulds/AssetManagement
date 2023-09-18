// -----------------------------------------------------------------------
// <copyright file="ValidationAttribute.cs" company="Imar.Spaanjaars.Com">
//   Copyright 2008 - 2009 - Imar.Spaanjaars.Com. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The ValidationAttribute class is the base class for all validation attributes that can be applied to BO properties
    /// in order to define business validation rules.
    /// </summary>
    /// <example>
    /// <code lang="cs">
    /// [NotNullOrEmpty()]
    /// public string Street
    /// { ... }
    /// </code>
    /// </example>
    public abstract class ValidationAttribute : Attribute
    {
        #region Private Variables

        private string _key;
        private string _message;

        #endregion

        /// <summary>
        /// Determines whether the value of the underlying property (passed in as the <paramref name="item"/> parameter) 
        /// is valid according to the validation rule.
        /// </summary>
        /// <param name="item">The underlying value of the propery that is being validated.</param>
        /// <returns>
        /// <c>true</c> if the specified item is valid; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsValid(object item);

        /// <summary>
        /// Gets the validation message associated with this validation.
        /// </summary>
        /// <value>The validation message.</value>
        /// <exception cref="ArgumentException">Thrown when Key already has a value when you try to set the Message property.</exception>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    throw new ArgumentException("Can't set Message when Key has already been set.");
                }
                _message = value;
            }
        }

        /// <summary>
        /// Gets the the globalization key associated with this validation.
        /// </summary>
        /// <value>The globalization key.</value>
        /// <exception cref="ArgumentException">Thrown when Message already has a value when you try to set the Key property.</exception>
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (!string.IsNullOrEmpty(_message))
                {
                    throw new ArgumentException("Can't set Key when Message has already been set.");
                }
                _key = value;
            }
        }
    }
}
