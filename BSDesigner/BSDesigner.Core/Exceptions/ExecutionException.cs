using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// A type of execution that is managed when the behaviour system is being executed.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class ExecutionException : Exception
    {
        protected ExecutionException()
        {
        }

        protected ExecutionException(string message) : base(message)
        {
        }

        protected ExecutionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}