using System;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// A type of execution that is managed when an element of the context is not defined or is not valid.
    /// </summary>
    public class ExecutionContextException : ExecutionException
    {
        public ExecutionContextException() { }

        public ExecutionContextException(string message) : base(message) { }

        public ExecutionContextException(string message, Exception innerException) : base(message, innerException) { }
    }
}