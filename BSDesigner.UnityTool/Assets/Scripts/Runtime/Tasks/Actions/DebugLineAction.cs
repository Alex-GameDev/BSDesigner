using BSDesigner.Core;
using BSDesigner.Core.Tasks;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Action that print a line in the scene
    /// </summary>
    [TaskCategory("Debug")]
    public class DebugLineAction : UnityActionTask
    {
        public Vector3 StartPos = Vector3.zero, EndPos = Vector3.zero;

        public Color LineColor = Color.white;

        public float Duration = -1;

        public DebugLineAction()
        {
        }

        public DebugLineAction(Vector3 startPos, Vector3 endPos, Color lineColor = default, float duration = -1)
        {
            StartPos = startPos;
            EndPos = endPos;
            LineColor = lineColor;
            Duration = duration;
        }

        public override string GetInfo() => $"Print a line form {StartPos} to {EndPos} of color {LineColor} during {Duration} seconds";

        protected override void OnBeginTask()
        {
            Debug.DrawLine(StartPos, EndPos, LineColor, Duration);
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