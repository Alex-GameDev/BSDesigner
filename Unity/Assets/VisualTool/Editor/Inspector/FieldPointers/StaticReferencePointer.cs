using System;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Pointer to an object instance that cannot be reassignated.
    /// </summary>
    internal class StaticReferencePointer : IFieldPointer
    {
        private object value;
        private readonly string name;

        public string Name => name;

        public StaticReferencePointer(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public Type Type => value.GetType();

        public bool IsNullable => false;

        public object GetValue()
        {
            return value;
        }

        public void SetValue(object value)
        {
            throw new NotSupportedException();
        }
    }
}
