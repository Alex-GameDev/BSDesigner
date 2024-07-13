using System;
using BSDesigner.Core.Graphs;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// The base node for state machines elements (states and transitions).
    /// </summary>
    public abstract class FsmNode : Node
    {
        public override Type GraphType => typeof(StateMachine);
    }
}