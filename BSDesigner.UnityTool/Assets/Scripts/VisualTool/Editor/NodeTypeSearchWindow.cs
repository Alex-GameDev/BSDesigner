using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class NodeTypeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private Type NodeRootType;

        private EventHandler<Type> onEntrySelectedHandler;

        public static NodeTypeSearchWindow Create(Type baseNodeType, EventHandler<Type> onEntrySelectedHandler)
        {
            var provider = CreateInstance<NodeTypeSearchWindow>();
            provider.NodeRootType = baseNodeType;
            provider.onEntrySelectedHandler = onEntrySelectedHandler;
            return provider;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent(NodeRootType.Name)));
            var nodeTypes = TypeCache.GetTypesDerivedFrom(NodeRootType);

            foreach (var nodeType in nodeTypes)
            {
                if (!nodeType.IsAbstract)
                    entries.Add(new SearchTreeEntry(new GUIContent(nodeType.Name)) { level = 1, userData = nodeType });
            }
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var type = (Type)SearchTreeEntry.userData;
            onEntrySelectedHandler.Invoke(this, type);
            return true;
        }
    }
}
