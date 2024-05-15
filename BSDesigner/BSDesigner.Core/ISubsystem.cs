namespace BSDesigner.Core
{
    /// <summary>
    /// Defines an element that can store a sub behaviour system
    /// </summary>
    public interface ISubsystem
    {
        /// <summary>
        /// The sub system handled
        /// </summary>
        public BehaviourEngine? Subsystem { get; }
    }
}
