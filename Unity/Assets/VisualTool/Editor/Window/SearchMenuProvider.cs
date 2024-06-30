using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Window
{
    internal class SearchMenuProvider : ISearchMenuProvider
    {
        private EditorWindow editorWindow;

        public SearchMenuProvider(EditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;
        }

        public void Create(Type type, Action<Type> onSelect)
        {
            var context = new SearchWindowContext(editorWindow.position.position);
            var provider = SearchWindowProvider.Create(type, onSelect);
            SearchWindow.Open(context, provider);
        }

        public void Create(Type type, Vector2 displayPosition, Action<Type, Vector2> onSelect)
        {
            var context = new SearchWindowContext(displayPosition);
            var provider = SearchWindowProvider.Create(type, (t) => onSelect(t, displayPosition - editorWindow.position.position));
            SearchWindow.Open(context, provider);
        }
    }
}
