using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a <see cref="IStatusHandler"/> tries to change 
    /// its <see cref="Status"/> in a wrong way.
    /// </summary>
#pragma warning disable RCS1194
    public class ExecutionStatusException : Exception
#pragma warning restore RCS1194
    {
        /// <summary>
        /// The <see cref="IStatusHandler"/> that throws the exception.
        /// </summary>
        public IStatusHandler StatusHandler;

        /// <summary>
        /// Create a new <see cref="ExecutionStatusException"/> with the specified status handler.
        /// </summary>
        /// <param name="statusHandler">The <see cref="IStatusHandler"/> that throws the exception.</param>
        [ExcludeFromCodeCoverage]
        public ExecutionStatusException(IStatusHandler statusHandler)
        {
            StatusHandler = statusHandler;
        }

        /// <summary>
        /// Create a new <see cref="ExecutionStatusException"/> with the specified status handler and a message.
        /// </summary>
        /// <param name="statusHandler">The <see cref="IStatusHandler"/> that throws the exception.</param>
        /// <param name="message">The exception message</param>
        public ExecutionStatusException(IStatusHandler statusHandler, string message) : base(message)
        {
            StatusHandler = statusHandler;
        }

        /// <summary>
        /// Create a new <see cref="ExecutionStatusException"/> with the specified status handler and a message.
        /// </summary>
        /// <param name="statusHandler">The <see cref="IStatusHandler"/> that throws the exception.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        [ExcludeFromCodeCoverage]
        public ExecutionStatusException(IStatusHandler statusHandler, string message, Exception innerException) : base(message, innerException)
        {
            StatusHandler = statusHandler;
        }
    }
}