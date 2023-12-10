using System.Diagnostics.CodeAnalysis;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Exception thrown when a transition is perform and causes an error in the execution.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InvalidTransitionException : ExecutionException
    {
        /// <summary>
        /// The transition that causes the exception
        /// </summary>
        public readonly Transition Transition;

        public InvalidTransitionException(Transition transition) : base()
        {
            Transition = transition;
        }

        public InvalidTransitionException(Transition transition, string message) : base(message)
        {
            Transition = transition;
        }

        public InvalidTransitionException(Transition transition, string? message, System.Exception? innerException) : base(message, innerException)
        {
            Transition = transition;
        }
    }
}