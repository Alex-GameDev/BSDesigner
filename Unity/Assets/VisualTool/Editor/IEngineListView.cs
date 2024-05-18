using BSDesigner.Core;
using System;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public interface IEngineListView
    {
        public event Action DataChanged;

        public event Action<BehaviourEngine> EngineSelected;

        public void Update(BehaviourEngine engine);

        public void Clear();
    }
}
