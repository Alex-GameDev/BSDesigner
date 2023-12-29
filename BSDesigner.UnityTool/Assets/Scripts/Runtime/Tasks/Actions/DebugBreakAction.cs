using BSDesigner.Core;
using BSDesigner.Core.Tasks;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Action that stops the execution (editor only)
    /// </summary>
    [TaskCategory("Debug")]
    public class DebugBreakAction : UnityActionTask
    {
        public override string GetInfo() => "Break point";

        protected override void OnBeginTask()
        {
            Debug.Break();
        }

        protected override void OnEndTask()
        {
        }

        protected override void OnPauseTask()
        {
        }

        protected override void OnResumeTask()
        {
        }

        protected override Status OnUpdateTask() => Status.Success;
    }
}
