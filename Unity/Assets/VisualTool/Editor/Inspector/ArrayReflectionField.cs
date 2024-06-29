using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class ArrayReflectionField : FieldInspector
    {
        private readonly List<FieldInspector> subfields;
        private readonly IFieldPointer fieldPointer;

        public ArrayReflectionField(IFieldPointer pointer, bool nullableElements = false)
        {
            this.fieldPointer = pointer;
            this.subfields = new List<FieldInspector>();

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
                this.subfields.Add(FieldInspector.CreateFromArrayElement(arrayValue, i));
            }
        }

        public override void Render()
        {
            using (var h = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(this.fieldPointer.Name);
                if(GUILayout.Button("+"))
                {
                    AddArrayItem();
                }
            }
            for(int i = 0; i < this.subfields.Count; i++)
            {
                var field = this.subfields[i];
                using (var h = new EditorGUILayout.HorizontalScope())
                {
                    using (var v = new EditorGUILayout.VerticalScope())
                    {
                        field.Render();
                    }
                    if (GUILayout.Button("-"))
                    {
                        RemoveArrayItem(i);
                    }
                }
            }
        }

        private void RemoveArrayItem(int index)
        {
            var arrayValue = (Array)this.fieldPointer.GetValue();
            Array newArray = Array.CreateInstance(this.fieldPointer.Type.GetElementType(), arrayValue.Length - 1);
            
            if(index > 0)
            {
                Array.Copy(arrayValue, 0, newArray, 0, index - 1);
            }
            if(index != arrayValue.Length - 1)
            {
                Array.Copy(arrayValue, index + 1, newArray, index, newArray.Length - index);
            }
            this.fieldPointer.SetValue(newArray);
            this.subfields.RemoveAt(index);
        }

        private void AddArrayItem()
        {
            var arrayValue = (Array)this.fieldPointer.GetValue();
            Array newArray = Array.CreateInstance(this.fieldPointer.Type.GetElementType(), arrayValue.Length + 1);
            Array.Copy(arrayValue, newArray, arrayValue.Length);
            this.fieldPointer.SetValue(newArray);
            this.subfields.Add(FieldInspector.CreateFromArrayElement(newArray, newArray.Length - 1));
        }
    }
}
