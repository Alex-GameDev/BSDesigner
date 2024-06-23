using System;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Puntero a un campo representado como un elemento de un array
    /// </summary>
    internal class ArrayElementPointer : IFieldPointer
    {
        private readonly Array array;
        private readonly int index;

        public string Name => $"Item {index}";

        public Type Type => array.GetType();

        public ArrayElementPointer(Array array, int index)
        {
            this.array = array;
            this.index = index;
        }

        public object GetValue()
        {
            return this.array.GetValue(index);
        }

        public void SetValue(object value)
        {
            this.array.SetValue(value, index);
        }
    }
}
