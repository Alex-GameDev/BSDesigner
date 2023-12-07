using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when the execution flow need a reference to a child or parent node that is missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MissingConnectionException : ExecutionException
    {
        public MissingConnectionException() { }

        public MissingConnectionException(string message) : base(message) { }

        public MissingConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }
}