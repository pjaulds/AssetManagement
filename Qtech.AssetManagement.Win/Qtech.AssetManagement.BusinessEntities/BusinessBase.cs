using Qtech.AssetManagement.Validation;
using System;
using System.Resources;
using Qtech.AssetManagement.BusinessEntities.Localization;
namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The BusinessBase class serves as the base class for all business entities in the Spaanjaars.ContactManager.BusinessEntities namespace.
    /// Since it inheirts from ValidationBase, it provides validation behavior to its child classes. Additionally, it implements
    /// default behavior for concurrency checks.
    /// </summary>
    public abstract class BusinessBase : ValidationBase
    {
        /// <summary>
        /// The ID of the BusinessBase instance in the database.
        /// </summary>
        public abstract int mId { get; set; }              

        /// <summary>
        /// Gets or sets the current user of the system
        /// </summary>        
        public string mUserFullName { get; set; }
          
        
        public Int32 mUserId { get; set; }
        /// <summary>
        /// The ResuurceManager sued by the validation framework.
        /// </summary>
        public static ResourceManager ResourceManager { get; set; }

        /// <summary>
        /// Gets the localized validation message based on the message key.
        /// </summary>
        /// <param name="key">The translation key of the validation message.</param>
        protected override string GetValidationMessage(string key)
        {
            string tempValue;
            if (ResourceManager != null)
            {
                tempValue = ResourceManager.GetString(key);
            }
            else
            {
                tempValue = string.Empty;
            }
            if (string.IsNullOrEmpty(tempValue))
            {
                tempValue = General.ResourceManager.GetString(key);
            }
            return tempValue;
        }
    }
}