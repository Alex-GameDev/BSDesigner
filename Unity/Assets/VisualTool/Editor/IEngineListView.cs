using BSDesigner.Core;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public interface IEngineListView
    {
        public event Action<BehaviourEngine> EngineAdded;

        public event Action<BehaviourEngine> EngineRemoved;

        public event Action<BehaviourEngine> EngineSelected;

        public void CreateUI(VisualElement parent);

        public void Update(List<BehaviourEngine> engine);

        public void Clear();
    }
}
