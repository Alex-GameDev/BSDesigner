using BSDesigner.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    public class DebugBreakAction : UnityActionTask
    {
        public override string GetInfo()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnBeginTask()
        {
            Debug.Break();
        }

        protected override void OnEndTask()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPauseTask()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnResumeTask()
        {
            throw new System.NotImplementedException();
        }

        protected override Status OnUpdateTask()
        {
            throw new System.NotImplementedException();
        }
    }
}
