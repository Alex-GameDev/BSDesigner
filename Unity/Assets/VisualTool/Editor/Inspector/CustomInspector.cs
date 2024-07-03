using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    public class CustomInspector : VisualElement
    {
        private static readonly int BUTTON_WIDTH = 20;

        private FieldInspector mainFieldRenderer;

        private readonly ISearchMenuProvider searchMenuProvider;

        public CustomInspector(ISearchMenuProvider searchMenuProvider)
        {
            var path = $"{ToolSettings.instance.EditorToolPath}/Editor/UI/custominspector.uxml";
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            asset.CloneTree(this);

            var imgui = new IMGUIContainer(Render);
            var element = this.Q<ScrollView>("bw-custominspector-content");
            element.Add(imgui);
            this.searchMenuProvider = searchMenuProvider;
        }

        public void Update(object obj)
        {
            mainFieldRenderer = obj != null ? new ClassInstanceInspector(new StaticReferencePointer("Node", obj)) : null;
            Render();
        }

        private void Render()
        {
            var settings = new RenderInspectorSettings
            {
                SearchMenuProvider = searchMenuProvider,
                ActionButtonWidth = BUTTON_WIDTH,
            };
            this.mainFieldRenderer?.Render(settings);
        }
    }

}
