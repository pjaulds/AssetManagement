using System;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The BrokenRule class provides (localized) information about the broken rules of validators.
    /// </summary>
    [Serializable]
    public class BrokenRule
    {
        #region Private Variables

        private string _propertyName = string.Empty;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the BrokenRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property that caused the rule to be broken.</param>
        /// <param name="message">The error message associated with the broken rule.</param>
        public BrokenRule(string propertyName, string message)
        {
            _propertyName = propertyName;
            Message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the error message associated with the broken rule.
        /// </summary>
        /// <value>The localized error message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that caused the rule to be broken.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }
                _propertyName = value;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", _propertyName, Message);
        }
        #endregion
    }
}
