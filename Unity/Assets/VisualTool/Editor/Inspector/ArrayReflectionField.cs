using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class ArrayReflectionField : ReflectedField
    {
        private readonly List<ReflectedField> subfields;
        private readonly IFieldPointer fieldPointer;

        public ArrayReflectionField(IFieldPointer pointer)
        {
            this.fieldPointer = pointer;
            this.subfields = new List<ReflectedField>();

            this.GenerateElementFields();
        }

        private void GenerateElementFields()
        {
           this.subfields.Clear();
            var value = this.fieldPointer.GetValue();
            if(value == null)
            {
                this.fieldPointer.SetValue(Array.CreateInstance(this.fieldPointer.Type.GetElementType(), 0));
            }
            var arrayValue = (Array)this.fieldPointer.GetValue();

            for(int i = 0; i < arrayValue.Length; i++)
            {
                this.subfields.Add(ReflectedField.CreateFromArrayElement(arrayValue, i));
            }
        }

        public override void Render()
        {
            using (var h = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(this.fieldPointer.Name);
                if(GUILayout.Button("+"))
                {

                }
            }
            foreach (var field in this.subfields)
            {
                using (var h = new EditorGUILayout.HorizontalScope())
                {
                    using (var v = new EditorGUILayout.VerticalScope())
                    {
                        field.Render();
                    }
                    if (GUILayout.Button("-"))
                    {

                    }
                }
            }
        }
    }
}
