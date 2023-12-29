using BSDesigner.Core;

namespace BSDesigner.Unity.Runtime
{
    public abstract class MovementAction : UnityActionTask
    {

        protected override void OnBeginTask()
        {
            context.Movement.IsMovementEnabled = true;
        }

        protected override void OnEndTask()
        {
            context.Movement.IsMovementEnabled = false;
        }

        protected override void OnPauseTask()
        {
            context.Movement.IsMovementEnabled = false;
        }

        protected override void OnResumeTask()
        {
            context.Movement.IsMovementEnabled = true;
        }
    }
}