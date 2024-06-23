using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class ClassInstanceReflectionField : ReflectedField
    {
        private readonly List<ReflectedField> subfields;
        private readonly IFieldPointer fieldPointer;
        private readonly bool isNullable;

        public ClassInstanceReflectionField(IFieldPointer fieldPointer, bool nullable)
        {
            this.fieldPointer = fieldPointer;
            this.isNullable = nullable;

            this.subfields = new List<ReflectedField>();
            this.GenerateSubFields();
        }

        public override void Render()
        {
            var value = this.fieldPointer.GetValue();
            using(var h = new EditorGUILayout.HorizontalScope()) 
            {
                EditorGUILayout.LabelField(this.fieldPointer.Name, value?.GetType().Name);

                if (this.isNullable)
                {
                    if (value != null && GUILayout.Button("x"))
                    {
                        this.fieldPointer.SetValue(default);
                        this.GenerateSubFields();
                        return;
                    }
                    if (value == null && GUILayout.Button("Set"))
                    {
                        SearchWindow.Open(new SearchWindowContext(Vector2.zero), SearchWindowProvider.Create(this.fieldPointer.Type, this.OnSetType));
                    }
                }
            }

            foreach (var subField in subfields)
            {
                subField.Render();
            }
        }

        private void OnSetType(Type type)
        {
            var value = Activator.CreateInstance(type);
            this.fieldPointer.SetValue(value);
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
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public).OrderBy(field => field.MetadataToken))
            {
                this.subfields.Add(CreateFromFieldInfo(field, value));
            }
        }
    }
}
