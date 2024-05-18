using BSDesigner.Core;
using System;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public interface IEngineView
    {
        public event Action DataChanged;

        public void Update(BehaviourEngine engine);

        public void Clear();
    }
}
