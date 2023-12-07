using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when try to create or delete a node that is a way that is not valid.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class NodeException : CreationException
    {
        public NodeException() { }

        public NodeException(string message) : base(message) { }

        public NodeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
