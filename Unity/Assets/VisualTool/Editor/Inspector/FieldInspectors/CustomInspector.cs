using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    public class CustomInspector : VisualElement
    {
        public CustomInspector()
        {
            var path = $"{ToolSettings.instance.EditorToolPath}/Editor/UI/custominspector.uxml";
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            asset.CloneTree(this);

            var imgui = new IMGUIContainer(Render);
            var element = this.Q<ScrollView>("bw-custominspector-content");
            element.Add(imgui);
        }


        private FieldInspector mainFieldRenderer;

        public void Update(object obj)
        {
            mainFieldRenderer = obj != null ? new ClassInstanceInspector(new StaticReferencePointer("Node", obj), false) : null;
            Render();
        }

        private void Render()
        {
            this.mainFieldRenderer?.Render();
        }
    }

}
