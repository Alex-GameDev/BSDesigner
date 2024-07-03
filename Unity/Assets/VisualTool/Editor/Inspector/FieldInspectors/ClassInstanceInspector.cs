using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using BSDesigner.Core.Attributes;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class ClassInstanceInspector : FieldInspector
    {
        private readonly List<FieldInspector> subfields;
        private readonly IFieldPointer fieldPointer;

        public override bool IsSingleLine => false;

        public ClassInstanceInspector(IFieldPointer fieldPointer)
        {
            this.fieldPointer = fieldPointer;

            this.subfields = new List<FieldInspector>();
            this.GenerateSubFields();
        }

        public override void Render(RenderInspectorSettings settings)
        {
            var value = this.fieldPointer.GetValue();
            using(var h = new EditorGUILayout.HorizontalScope()) 
            {
                EditorGUILayout.LabelField(this.fieldPointer.Name, value?.GetType().Name);

                if (this.fieldPointer.IsNullable)
                {
                    if (value != null && GUILayout.Button("x", GUILayout.Width(settings.ActionButtonWidth)))
                    {
                        this.fieldPointer.SetValue(default);
                        this.GenerateSubFields();
                        return;
                    }
                    if (value == null && GUILayout.Button("Set"))
                    {
                        settings.SearchMenuProvider.Create(this.fieldPointer.Type, t => this.OnSetType(t, settings));
                    }
                }
            }

            if(subfields.Count == 0)
            {
                return;
            }

            EditorGUI.indentLevel++;
            using (var v = new EditorGUILayout.VerticalScope())
            {
                foreach (var subField in subfields)
                {
                    subField.Render(settings);
                }
            }
            EditorGUI.indentLevel--;
        }

        private void OnSetType(Type type, RenderInspectorSettings settings)
        {
            var value = type.CreateInstance();
            this.fieldPointer.SetValue(value);
            settings.ChangeFlag = true;
            this.GenerateSubFields();
           
        }

        private void GenerateSubFields()
        {
            this.subfields.Clear();

            var value = fieldPointer.GetValue();

            if(value == null)
            {
                return;
            }

            var type = value.GetType();
            foreach (var field in GetInspectorFields(type))
            {
                this.subfields.Add(CreateFromFieldInfo(field, value));
            }
        }

        private List<FieldInfo> GetInspectorFields(Type type)
        {
            var fields = type.GetPublicFields().Where(f => f.GetCustomAttribute<HideInspectorAttribute>() == null);
            return fields.ToList();
        }
    }
}
