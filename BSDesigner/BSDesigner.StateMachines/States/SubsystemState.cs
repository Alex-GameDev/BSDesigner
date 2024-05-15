using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.StateMachines.States
{
    /// <summary>
    /// FSM State that can handle a nested behaviour engine.
    /// </summary>
    public class SubsystemState : State, ISubsystem
    {
        /// <summary>
        /// The nested behaviour engine.
        /// </summary>
        public BehaviourEngine? Subsystem 
        { 
            get => _nestedEngine;
            set
            {
                if (_nestedEngine != value && !(_nestedEngine?.IsNestedWith(this.Graph) ?? false))
                {
                    this.Subsystem = value;
                }
            }
        }

        private BehaviourEngine? _nestedEngine;

        public override int MaxInputConnections => -1;

        /// <summary>
        /// Starts the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnEntered()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Start();
        }

        /// <summary>
        /// Stops the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnExited()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Stop();
        }

        /// <summary>
        /// Pauses the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnPaused()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Pause();
        }

        /// <summary>
        /// Updates the nested behaviour engine
        /// </summary>
        /// <returns>The current status of the subsystem.</returns>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override Status OnUpdated()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            return Subsystem.Update();
        }

        public override void SetContext(ExecutionContext context)
        {
            Subsystem?.SetContext(context);
        }
    }
}
