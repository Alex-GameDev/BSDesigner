using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when try to create or delete a connection that is a way that is not valid.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConnectionException : CreationException
    {
        public ConnectionException() { }

        public ConnectionException(string message) : base(message) { }

        public ConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
