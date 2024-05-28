using BSDesigner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor
{
    internal class EngineCreationView : VisualElement
    {     

        private Type selectedType;

        private List<BSEngineCardView> viewList = new List<BSEngineCardView> ();

        private TextField nameField;

        private Action<Type, string> SubmitCallback;
        public EngineCreationView(Action<Type, string> submitCallback)
        {
            this.SubmitCallback = submitCallback;

            VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{ToolSettings.instance.LayoutPath}/bsenginecreator.uxml");
            asset.CloneTree(this);

            this.Q<Button>("bw-enginecreation-ok-btn").clicked += BtnOk_Click;
            var listView = this.Q<ScrollView>("bw-enginecreation-list");
            this.nameField = this.Q<TextField>("bw-enginecreation-name-tf");

            var metadataNode = ToolMetadata.Instance.GetTypeNode(typeof(BehaviourGraph));
            
            foreach(var typeNode in metadataNode.GetConcreteSubtypesRecursively())
            {
                var card = new BSEngineCardView(typeNode.Type);
                card.Selected += OnSelectCard;
                viewList.Add(card);
                listView.Add(card);
            }
        }

        private void OnSelectCard(object sender, Type type)
        {
            selectedType = type;
        }

        private void BtnOk_Click()
        {
            SubmitCallback?.Invoke(selectedType, this.nameField.name);
            Hide();
        }

        public void Show() => this.style.visibility = Visibility.Visible;

        public void Hide() => this.style.visibility = Visibility.Hidden;
    }
}
