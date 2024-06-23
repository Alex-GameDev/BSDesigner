using System;
using System.Reflection;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Puntero a un campo de una clase accedido por reflexión
    /// </summary>
    internal class ReflectedFieldPointer : IFieldPointer
    {
        private readonly FieldInfo fieldInfo;
        private readonly object parentObj;
        private object cachedValue;

        public ReflectedFieldPointer(FieldInfo field, object value)
        {
            this.fieldInfo= field;
            this.parentObj = value;
            this.cachedValue = fieldInfo.GetValue(value);
        }

        public string Name => fieldInfo.Name;

        public Type Type => fieldInfo.FieldType;

        public object GetValue()
        {
            return this.cachedValue;
        }

        public void SetValue(object value)
        {
            if((value == null && this.cachedValue != null) || (value != null && !value.Equals(this.cachedValue)))
            {
                this.cachedValue = value;
                this.fieldInfo.SetValue(this.parentObj, value);
            }
        }
    }
}
