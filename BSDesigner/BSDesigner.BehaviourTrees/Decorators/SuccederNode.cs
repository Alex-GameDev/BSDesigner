using System;
using System.Collections.Generic;
using System.Text;
using BSDesigner.Core;

namespace BSDesigner.BehaviourTrees
{
    public class SuccederNode : DirectDecoratorNode
    {
        /// <summary>
        /// <inheritdoc/>
        /// If <paramref name="childStatus"/> is failure returns success.
        /// </summary>
        /// <param name="childStatus"><inheritdoc/></param>
        /// <returns>Success if <paramref name="childStatus"/> is failure, <paramref name="childStatus"/> otherwise.</returns>
        protected override Status ModifyStatus(Status childStatus)
        {
            if (childStatus == Status.Failure) childStatus = Status.Success;
            return childStatus;
        }
    }
}
