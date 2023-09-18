

using System;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The EnumNotNone class allows you to make sure that an enumeration item does not have the None (== 0) value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EnumNotNoneAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumNotNoneAttribute"/> class.
        /// </summary>
        public EnumNotNoneAttribute() { }

        /// <summary>
        /// Determines whether the value of the underlying property (passed in as the <paramref name="item"/> parameter)
        /// is not None (an item with the value of 0).
        /// </summary>
        /// <param name="item">The underlying value of the property that is being validated.</param>
        /// <returns>
        /// 	<c>true</c> if the specified item is not 0; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(object item)
        {
            if (item.GetType().BaseType != typeof(System.Enum)) return false;
            return (int)item != 0;
        }
    }
}
