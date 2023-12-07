using System;
using System.Diagnostics.CodeAnalysis;

namespace BSDesigner.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when the execution flow needs a task and its reference is null.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MissingTaskException : ExecutionException
    {
        public MissingTaskException() { }

        public MissingTaskException(string message) : base(message) { }

        public MissingTaskException(string message, Exception innerException) : base(message, innerException) { }
    }
}