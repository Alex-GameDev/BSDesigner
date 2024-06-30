using System;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public interface ISearchMenuProvider
    {
        public void Create(Type type, Action<Type> onSelect);
        public void Create(Type type, Vector2 displayPosition, Action<Type, Vector2> onSelect);
    }
}
