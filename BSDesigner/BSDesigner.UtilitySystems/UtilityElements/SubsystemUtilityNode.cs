using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.UtilitySystems.UtilityElements
{
    /// <summary>
    /// Utility node that executes a nested engine when is selected
    /// </summary>
    public class SubsystemUtilityNode : FactorizedUtilityNode, ISubsystem
    {
        /// <summary>
        /// The nested subystem.
        /// </summary>
        public BehaviourEngine? Subsystem { get; set; }

        /// <summary>
        /// Pauses the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnElementPaused()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Pause();
        }

        /// <summary>
        /// Starts the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnElementStarted()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Start();
        }

        /// <summary>
        /// Stops the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnElementStopped()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Stop();
        }

        /// <summary>
        /// Updates the nested behaviour engine
        /// </summary>
        /// <returns>The current status of the subsystem.</returns>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override Status OnElementUpdated()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            return Subsystem.Update();
        }
    }
}
