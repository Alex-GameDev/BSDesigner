using BSDesigner.Core;
using System;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public interface IEngineView
    {
        public event Action DataChanged;

        public void CreateUI(VisualElement parent);

        public void Update(BehaviourEngine engine);

        public void Clear();

        public void Hide();

        public void Show();
    }
}
