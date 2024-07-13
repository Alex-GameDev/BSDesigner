using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.BehaviourTrees.Leafs
{
    /// <summary>
    /// Leaf BT node that can handle a nested behaviour engine.
    /// </summary>
    public class SubsystemBtNode : LeafBtNode
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
                    this._nestedEngine = value;
                }
            }
        }

        private BehaviourEngine? _nestedEngine;

        /// <summary>
        /// Pauses the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnNodePaused()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Pause();
        }

        /// <summary>
        /// Starts the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnNodeStarted()
        {
            if (Subsystem == null)
                throw new MissingBehaviourSystemException("Subsystem cannot be null");

            Subsystem.Start();
        }

        /// <summary>
        /// Stops the nested behaviour engine
        /// </summary>
        /// <exception cref="MissingBehaviourSystemException">Thrown if the subsystem is null</exception>
        protected override void OnNodeStopped()
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
        protected override Status UpdateStatus()
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
