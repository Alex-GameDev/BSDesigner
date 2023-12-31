using System;
using System.Collections.Generic;
using System.Xml.Linq;
using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    /// <summary>
    /// Display the list of engines of a given system
    /// </summary>
    public class BSEngineListView : VisualElement
    {
        #region Configuration fields

        private static readonly float k_EngineItemHeight = 24f;
        private static readonly string k_Title = "ENGINES";
        private static readonly string k_ListViewName = "ee-scroll-view";
        private static readonly string k_AddEngineText = "Add engine";
        private static readonly string k_ClearEnginesText = "Clear engines";

        #endregion



        #region Events

        /// <summary>
        /// Event called when a new engine is added to the list
        /// </summary>
        public Action<BehaviourEngine> EngineAdded;

        /// <summary>
        /// Event called when a engine is removed from the list
        /// </summary>
        public Action<int> EngineRemoved;

        /// <summary>
        /// Event called when a engine is selected
        /// </summary>
        public Action<BehaviourEngine> EngineSelected;

        /// <summary>
        /// Event called when all the engines are removed
        /// </summary>
        public Action SystemClean;

        #endregion

        #region Private fields

        private ListView m_ListView;

        private BSData m_Data;

        #endregion

        public BSEngineListView()
        {
            CreateGUI();
        }

        public void Populate(BSData data)
        {
            m_Data = data;

            if(m_Data.Engines == null) return;

            this.m_ListView.itemsSource = m_Data.Engines;
            this.m_ListView.Rebuild();
        }

        private void CreateGUI()
        {
            var titleTag = new Label(k_Title);
            this.Add(titleTag);

            m_ListView = new ListView();
            m_ListView.fixedItemHeight = k_EngineItemHeight;
            m_ListView.makeItem =  MakeItem;
            m_ListView.bindItem = BindItem;
            m_ListView.name = k_ListViewName;
            m_ListView.selectionType = SelectionType.None;
            this.Add(m_ListView);

            var addBtn = new Button(OpenEngineCreationWindow) { text = k_AddEngineText };
            this.Add(addBtn);

            var clearBtn = new Button(OpenClearEngineWindow) { text = k_ClearEnginesText };
            this.Add(clearBtn);
        }

        private VisualElement MakeItem()
        {
            var element = new VisualElement();
            element.AddToClassList("row");
            element.AddToClassList("list-element");

            var name_label_div = new VisualElement();
            name_label_div.AddToClassList("ee-name-tag-div");
            element.Add(name_label_div);

            var nameLabel = new Label();
            nameLabel.name = "ee-name-lbl";
            name_label_div.Add(nameLabel);

            var delete_btn = new Button() { name = "ee-delete-btn" };

            delete_btn.AddToClassList("ee-delete-btn");
            element.Add(delete_btn);

            return element;
        } 

        private void BindItem(VisualElement visualElement, int index)
        {
            var engine = m_Data.Engines[index];

            var nameTag = visualElement.Q<Label>("ee-name-lbl");

            if (nameTag != null)
            {
                nameTag.text = engine.Name;
            }

            var deleteBtn = visualElement.Q<Button>("ee-delete-btn");
            deleteBtn.clicked += () => OpenDeleteEngineWindow(index);
        }

        private void OpenEngineCreationWindow()
        {
            CreateEngineView.Open(OnSelectEngineTypeEntry);
        }

        private void OpenDeleteEngineWindow(int index)
        {
            EngineRemoved?.Invoke(index);
            RefreshView();
        }

        private void OpenClearEngineWindow()
        {
            SystemClean?.Invoke();
            RefreshView();
        }

        private void OnSelectEngineTypeEntry(object sender, Type e)
        {
            if (!e.IsSubclassOf(typeof(BehaviourEngine))) return;

            var engine = (BehaviourEngine)Activator.CreateInstance(e);

            EngineAdded.Invoke(engine);
            RefreshView();
        }

        private void RefreshView()
        {
            m_ListView.Rebuild();
        }
    }
}
