using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class ArrayInspector : FieldInspector
    {
        private readonly List<FieldInspector> subfields;
        private readonly IFieldPointer fieldPointer;

        private readonly Type elementType;
        public override bool IsSingleLine => false;

        public ArrayInspector(IFieldPointer pointer, bool nullableElements = false)
        {
            this.fieldPointer = pointer;
            this.subfields = new List<FieldInspector>();
            this.elementType = pointer.Type.GetElementType();
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

        public override void Render(RenderInspectorSettings settings)
        {
            using (var h = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(this.fieldPointer.Name);
                if(GUILayout.Button("+", GUILayout.Width(settings.ActionButtonWidth)))
                {
                    AddArrayItem(settings);
                }
            }
            for(int i = 0; i < this.subfields.Count; i++)
            {
                var field = this.subfields[i];
                if (field.IsSingleLine)
                {
                    using (var h = new EditorGUILayout.HorizontalScope())
                    {
                        field.Render(settings);
                        if (GUILayout.Button("-", GUILayout.Width(settings.ActionButtonWidth)))
                        {
                            this.RemoveArrayItem(i);
                        }
                    }
                }
                else
                {
                    field.Render(settings);
                    if (GUILayout.Button("-"))
                    {
                        this.RemoveArrayItem(i);
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

        private void AddArrayItem(RenderInspectorSettings settings)
        {
            if (this.elementType.IsAbstract) // Is polymorphic
            {
                settings.SearchMenuProvider.Create(this.fieldPointer.Type.GetElementType(), t => this.AddElement(t, settings));
            }
            else
            {
                var type = this.fieldPointer.Type.GetElementType();
                this.AddElement(type, settings);
            }
        }


        private void AddElement(Type type, RenderInspectorSettings settings)
        {
            var value = type.CreateInstance();
            var arrayValue = (Array)this.fieldPointer.GetValue();
            Array newArray = Array.CreateInstance(this.fieldPointer.Type.GetElementType(), arrayValue.Length + 1);
            Array.Copy(arrayValue, newArray, arrayValue.Length);
            newArray.SetValue(value, newArray.Length - 1);
            this.fieldPointer.SetValue(newArray);
            this.subfields.Add(FieldInspector.CreateFromArrayElement(newArray, newArray.Length - 1));
            settings.ChangeFlag = true;
        }
    }
}
