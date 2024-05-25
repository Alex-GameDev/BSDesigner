using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor
{
    public class BSEngineCardView : VisualElement
    {
        public Type AssignedType { get; }

        public event EventHandler<Type> Selected;

        public BSEngineCardView(Type type)
        {
            this.AssignedType = type;

            VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{ToolSettings.instance.LayoutPath}/bstypecard.uxml");
            asset.CloneTree(this);

            var titleLabel = this.Q<Label>("bw-card-title");
            titleLabel.text = type.Name;

            var iconImg = this.Q("bw-card-icon");
            iconImg.style.backgroundImage = null; //TODO:

            var descriptionLabel = this.Q<Label>("bw-card-description");
            descriptionLabel.text = "Desc ..."; //TODO:

            this.Q<Button>("bw-card-select-btn").clicked += OnSelect;
        }

        private void OnSelect()
        {
            this.Selected?.Invoke(this, AssignedType);
        }
    }
}
