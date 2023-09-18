using System;
using System.Runtime.Serialization;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The InvalidSaveOperationException is thrown in business applications when an attempt
    /// is made to save an invalid <see cref="ValidationBase" /> instance in the database.
    /// </summary>
    [Serializable]
    public class InvalidSaveOperationException : Exception
    {
        #region InvalidSaveOperationException()
        /// <summary>
        /// Initializes a new instance of the InvalidSaveOperationException class.
        /// </summary>
        public InvalidSaveOperationException() { }
        #endregion

        #region InvalidSaveOperationException(string message)
        /// <summary>
        /// Initializes a new instance of the InvalidSaveOperationException class.
        /// </summary>
        /// <param name="message">The exception message</param>
        public InvalidSaveOperationException(string message) : base(message) { }
        #endregion

        #region InvalidSaveOperationException(string message, Exception innerException)
        /// <summary>
        /// Initializes a new instance of the InvalidSaveOperationException class.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public InvalidSaveOperationException(string message, Exception innerException) : base(message, innerException) { }
        #endregion

        #region InvalidSaveOperationException(SerializationInfo info, StreamingContext context)
        /// <summary>
        /// Initializes a new instance of the InvalidSaveOperationException class.
        /// Serialization constructor.
        /// </summary>
        protected InvalidSaveOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
