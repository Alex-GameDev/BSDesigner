using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class ListInspector : FieldInspector
    {
        private static readonly int LIST_BUTTON_WIDTH = 20;

        private readonly List<FieldInspector> subfields;
        private readonly IFieldPointer fieldPointer;

        public override bool IsSingleLine => false;

        public ListInspector(IFieldPointer pointer)
        {
            this.fieldPointer = pointer;
            this.subfields = new List<FieldInspector>();
            this.GenerateElementFields();
        }

        private void GenerateElementFields()
        {
            this.subfields.Clear();
            var value = this.fieldPointer.GetValue();
            if (value != null)
            {
                this.fieldPointer.SetValue(Activator.CreateInstance(this.fieldPointer.Type));
            }
            var arrayValue = (IList)this.fieldPointer.GetValue();

            for (int i = 0; i < arrayValue.Count; i++)
            {
                this.subfields.Add(FieldInspector.CreateFromListElement(arrayValue, i));
            }
        }

        public override void Render(RenderInspectorSettings settings)
        {
            using (var h = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(this.fieldPointer.Name);
                if (GUILayout.Button("+", GUILayout.Width(LIST_BUTTON_WIDTH)))
                {
                    this.AddListElement(settings);
                }
            }
            for (int i = 0; i < this.subfields.Count; i++)
            {
                var field = this.subfields[i];
                if (field.IsSingleLine)
                {
                    using(var h = new EditorGUILayout.HorizontalScope())
                    {
                        field.Render(settings);
                        if (GUILayout.Button("-", GUILayout.Width(LIST_BUTTON_WIDTH)))
                        {
                            this.RemoveListElement(i);
                        }
                    }
                }
                else
                {
                    field.Render(settings);
                    if (GUILayout.Button("-"))
                    {
                        this.RemoveListElement(i);
                    }
                }
            }
        }

        private void RemoveListElement(int i)
        {
            var list = (IList)this.fieldPointer.GetValue();
            list.RemoveAt(i);
            this.subfields.RemoveAt(list.Count);
        }

        private void AddListElement(RenderInspectorSettings settings)
        {
            if(true) // Is polymorphic
            {
                settings.SearchMenuProvider.Create(this.fieldPointer.Type.GetGenericArguments().First(), this.OnSetType);
            }
            else
            {
                var element = Activator.CreateInstance(this.fieldPointer.Type.GetGenericArguments()[0]);
                var list = (IList)this.fieldPointer.GetValue();
                list.Add(element);
                this.subfields.Add(FieldInspector.CreateFromListElement(list, list.Count - 1));
            }
        }

        private void OnSetType(Type type)
        {
            var value = Activator.CreateInstance(type);
            var list = (IList)this.fieldPointer.GetValue();
            list.Add(value);
            this.subfields.Add(FieldInspector.CreateFromListElement(list, list.Count - 1));
        }
    }
}
