using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    public interface IMovement
    {
        /// <summary>
        /// The target position where the agent is moving
        /// </summary>
        Vector3 Target { get; set; }

        /// <summary>
        /// Can the agent move?
        /// </summary>
        bool IsMovementEnabled { get; set; }

        /// <summary>
        /// The current transform of the agent.
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// The target position where the agent is moving
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// The agent is on its target?
        /// </summary>
        bool HasArrivedOnTarget { get; }
    }
}
