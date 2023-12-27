using BSDesigner.Core;
using UnityEngine;

namespace BSDesigner.UnityTool.Runtime
{
    public class UnityExecutionContext : ExecutionContext
    {
        /// <summary>
        /// The script that executes the graph with this context.
        /// </summary>
        public Component RunnerComponent => _runnerComponent;

        /// <summary>
        /// The game object of the agent.
        /// </summary>
        public GameObject GameObject => _runnerComponent.gameObject;

        /// <summary>
        /// The transform of the agent.
        /// </summary>
        public Transform Transform => GameObject.transform;

        private readonly Component _runnerComponent;

        /// <summary>
        /// Create a new unity execution context with a runner script component. Use this constructor
        /// to access methods in the runner component with custom actions or perceptions.
        /// </summary>
        /// <param name="runnerComponent">The runner component.</param>
        public UnityExecutionContext(Component runnerComponent)
        {
            _runnerComponent = runnerComponent;
        }
    }
}