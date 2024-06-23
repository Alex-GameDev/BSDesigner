using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class ListReflectionField : ReflectedField
    {
        private readonly List<ReflectedField> subfields;
        private readonly IFieldPointer fieldPointer;

        public ListReflectionField(IFieldPointer pointer)
        {
            this.fieldPointer = pointer;
            this.subfields = new List<ReflectedField>();

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
                this.subfields.Add(ReflectedField.CreateFromListElement(arrayValue, i));
            }
        }

        public override void Render()
        {
            using (var h = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(this.fieldPointer.Name);
                if (GUILayout.Button("+"))
                {

                }
            }
            foreach (var field in this.subfields)
            {
                using (var h = new EditorGUILayout.HorizontalScope())
                {
                    using(var v = new EditorGUILayout.VerticalScope())
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
