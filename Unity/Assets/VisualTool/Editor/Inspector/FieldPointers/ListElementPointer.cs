using System;
using System.Collections;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Field pointer represented as a list element
    /// </summary>
    internal class ListElementPointer : IFieldPointer
    {
        private readonly IList list;
        private readonly int index;

        public string Name => $"Item {index}";

        public Type Type => list.GetType();

        public ListElementPointer(IList list, int index)
        {
            this.list = list;
            this.index = index;
        }

        public object GetValue()
        {
            return this.list[index];
        }

        public void SetValue(object value)
        {
            this.list[index] = value;
        }
    }
}
