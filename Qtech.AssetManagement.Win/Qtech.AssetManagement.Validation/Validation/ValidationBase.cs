// -----------------------------------------------------------------------
// <copyright file="ValidationBase.cs" company="Imar.Spaanjaars.Com">
//   Copyright 2008 - 2009 - Imar.Spaanjaars.Com. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The ValidationBase class serves as the base class for all business entities that want to implement
    /// the validation behavior. It provides Validate methods that are able to check the validity of 
    /// this instance's properties by looking at the applied attributes.
    /// </summary>
    public abstract class ValidationBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BrokenRulesCollection _brokenRules = new BrokenRulesCollection();

        /// <summary>
        /// Determines whether the current instance meets all validation rules. It always clears the BrokenRules collection 
        /// first before adding new BrokenRule instances.
        /// </summary>
        /// <overloads>
        /// Determines whether the current instance meets all validation rules.
        /// </overloads>
        /// <returns>Returns <c>true</c> if the instance is valid, <c>false</c> otherwise.</returns>
        /// <remarks>This method automatically clears the internal BrokenRules collection.</remarks>
        public virtual bool Validate()
        {
            return Validate(true);
        }

        /// <summary>
        /// Determines whether the current instance meets all validation rules. You can optionally determine
        /// whether the BrokenRules collection should be cleared or not.
        /// </summary>
        /// <param name="clearBrokenRules">If set to <c>true</c> the BrokenRules collection is cleared first.</param>
        /// <returns>
        /// Returns <c>true</c> if the instance is valid, <c>false</c> otherwise.
        /// </returns>
        public virtual bool Validate(bool clearBrokenRules)
        {
            if (clearBrokenRules)
            {
                this.BrokenRules.Clear();
            }

            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var valProps = from PropertyInfo property in properties
                           where property.GetCustomAttributes(typeof(ValidationAttribute), true).Length > 0
                           select new
                           {
                               Property = property,
                               ValidationAttributes = property.GetCustomAttributes(typeof(ValidationAttribute), true)
                           };

            foreach (var item in valProps)
            {
                foreach (ValidationAttribute attribute in item.ValidationAttributes)
                {
                    if (attribute.IsValid(item.Property.GetValue(this, null)))
                    {
                        continue;
                    }

                    string message = string.Empty;
                    if (!string.IsNullOrEmpty(attribute.Key))
                    {
                        message = GetValidationMessage(attribute.Key);
                    }
                    else
                    {
                        message = attribute.Message;
                    }
                    BrokenRules.Add(new BrokenRule(item.Property.Name, message));
                }
            }
            return (BrokenRules.Count == 0);
        }

        /// <summary>
        /// When overriden in a child class, this method gets the localized validation message based on the message key.
        /// </summary>
        /// <param name="key">The translation key of the validation message.</param>
        /// <returns>By default, this method returns the key "as is" and does not try to localize it. Classes overriding this method may localize the method using a ResourceManager.</returns>
        protected virtual string GetValidationMessage(string key)
        {
            return key;
        }

        /// <summary>
        /// Gets a collection of <see cref="BrokenRules"/> instances associated with this ValidationBase instance.
        /// </summary>
        /// <value>The broken rules associated with this ValidationBase.</value>
        public BrokenRulesCollection BrokenRules
        {
            get { return _brokenRules; }
        }
    }
}
