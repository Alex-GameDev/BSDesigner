using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a graph has not nodes and cannot be executed.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EmptyGraphException : ExecutionException
    {
        public EmptyGraphException() { }

        public EmptyGraphException(string message) : base(message) { }

        public EmptyGraphException(string message, Exception innerException) : base(message, innerException) { }
    }
}