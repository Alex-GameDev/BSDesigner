using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when the execution flow need a reference to a behaviour engine that is missing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MissingBehaviourSystemException : ExecutionException
    {
        public MissingBehaviourSystemException() { }

        public MissingBehaviourSystemException(string message) : base(message) { }

        public MissingBehaviourSystemException(string message, Exception innerException) : base(message, innerException) { }
    }
}
