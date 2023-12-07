using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// A type of execution that is managed when the behaviour system is being created.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class CreationException : Exception
    {
        protected CreationException()
        {
        }

        protected CreationException(string message) : base(message)
        {
        }

        protected CreationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
