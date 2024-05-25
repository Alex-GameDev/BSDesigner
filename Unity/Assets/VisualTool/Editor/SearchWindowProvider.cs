using BSDesigner.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{
    /// <summary>
    /// Class that displays a hierarchy search tree window that allows user to select a type.
    /// </summary>
    public class SearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private Action<Type> callback;

        private Type rootType;

        public static SearchWindowProvider Create(Type rootType, Action<Type> callback)
        {
            var windowProvider = CreateInstance<SearchWindowProvider>();
            windowProvider.rootType = rootType;
            windowProvider.callback = callback;
            return windowProvider;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>();
            var hierarchyNode = ToolMetadata.Instance.GetTypeNode(rootType);

            if (hierarchyNode == null)
            {
                searchTreeEntries.AddGroup("No elements found", 0);
            }
            else
            {
                searchTreeEntries.AddGroup(hierarchyNode.Name, 0);
                foreach (var subNode in hierarchyNode.SubTypes)
                {
                    GetSubSearchTreeEntry(subNode, searchTreeEntries, 1);
                }
            }
            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            this.callback?.Invoke((Type)SearchTreeEntry.userData);
            return true;
        }

        void GetSubSearchTreeEntry(TypeHierarchyNode node, List<SearchTreeEntry> list, int level)
        {
            if (node.SubTypes.Count() == 0)
            {
                list.AddEntry(node.Name, level, node.Type);
            }
            else
            {
                list.AddGroup(node.Name, level);
                foreach (var subNode in node.SubTypes)
                {
                    GetSubSearchTreeEntry(subNode, list, level + 1);
                }
            }
        }        
    }

    public static class SearchTreeExtensions
    {
        public static void AddGroup(this List<SearchTreeEntry> entries, string name, int level)
        {
            entries.Add(new SearchTreeGroupEntry(new GUIContent(name), level));
        }

        public static void AddEntry(this List<SearchTreeEntry> entries, string name, int level, object data)
        {
            entries.Add(new SearchTreeEntry(new GUIContent("     " + name)) { level = level, userData = data });
        }
    }
}
