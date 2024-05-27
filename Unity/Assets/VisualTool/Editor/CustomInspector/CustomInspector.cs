using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class CustomInspector : VisualElement
    {
        private object selectedObj;

        private ScrollView contentView;
        public CustomInspector()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ToolSettings.instance.LayoutPath + "/custominspector.uxml");
            root.CloneTree(this);

            this.contentView = this.Q<ScrollView>("bw-custominspector-content");
        }

        public void Update(object obj)
        {
            if (this.selectedObj == obj) return;

            this.Reset();
            this.selectedObj = obj;

            if (this.selectedObj == null) return;

            this.contentView.Add(new Label(obj?.GetType().Name));
        }

        public void Reset()
        {
            this.contentView.Clear();
        }
    }
}
