using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor
{
    public class BSEngineCardView : VisualElement
    {
        private readonly Type assignedType;

        public event Action<Type> CardSelected;

        public BSEngineCardView(Type type)
        {
            this.assignedType = type;

            VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{ToolSettings.instance.LayoutPath}/bsenginecard.uxml");
            asset.CloneTree(this);

            var titleLabel = this.Q<Label>("bw-card-label");
            titleLabel.text = type.Name;

            var iconImg = this.Q("bw-card-icon");
            iconImg.style.backgroundImage = null; //TODO:

            var descriptionLabel = this.Q<Label>("bw-card-description");
            descriptionLabel.text = "Desc ..."; //TODO:
        }

        private void OnSelect()
        {
            this.CardSelected?.Invoke(assignedType);
        }
    }
}
