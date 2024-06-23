using System;
using UnityEditor;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class SimpleReflectionField : ReflectedField
    {
        private readonly IFieldPointer fieldPointer;

        internal SimpleReflectionField(IFieldPointer fieldPointer)
        {
            this.fieldPointer = fieldPointer;
        }

        public override void Render()
        {
            var value = this.fieldPointer.GetValue();

            if (value == null)
                return;

            var type = value.GetType();
            if (type == typeof(int))
            {
                value = EditorGUILayout.IntField(this.fieldPointer.Name, (int)value);
            }
            else if (type == typeof(float))
            {
                value = EditorGUILayout.FloatField(this.fieldPointer.Name, (float)value);
            }
            else if (type == typeof(bool))
            {
                value = EditorGUILayout.Toggle(this.fieldPointer.Name, (bool)value);
            }
            else if (type == typeof(string))
            {
                value = EditorGUILayout.TextField(this.fieldPointer.Name, (string)value);
            }
            else if (type.IsEnum)
            {
                value = EditorGUILayout.EnumPopup(this.fieldPointer.Name, (Enum)value);
            }

            this.fieldPointer.SetValue(value);
        }
    }
}
