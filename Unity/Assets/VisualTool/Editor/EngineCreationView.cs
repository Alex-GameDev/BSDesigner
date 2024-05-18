using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor
{
    internal class EngineCreationView : VisualElement
    {
        public EngineCreationView()
        {
            VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{ToolSettings.instance.LayoutPath}/bsenginecreator.uxml");
            asset.CloneTree(this);

            this.Q<Button>("bw-enginecreation-ok-btn").clicked += Hide;
        }

        public void Show() => this.style.visibility = Visibility.Visible;

        public void Hide() => this.style.visibility = Visibility.Hidden;
    }
}
