using BSDesigner.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    public class CustomInspector : VisualElement
    {
        public CustomInspector()
        {
            var path = $"{ToolSettings.instance.EditorToolPath}/Editor/UI/custominspector.uxml";
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            asset.CloneTree(this);

            var imgui = new IMGUIContainer(Render);
            var element = this.Q<ScrollView>("bw-custominspector-content");
            element.Add(imgui);
        }


        private List<ReflectedField> fields = new List<ReflectedField>();

        public void Update(object obj)
        {
            fields.Clear();

            if(obj != null)
            {
                var typeFields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (var field in typeFields)
                {
                    fields.Add(CreateReflectedField(field, obj));
                }
            }

            Render();
        }

        private void Render()
        {
            foreach (var field in fields)
            {
                field.Render();
            }
        }

        private ReflectedField CreateReflectedField(FieldInfo fieldInfo, object parentObj)
        {
            Type type = fieldInfo.FieldType;
            return new ReflectedField(fieldInfo, parentObj);
        }
    }

    public class ReflectedField
    {
        private FieldInfo fieldInfo;
        private object parentObj;
        private object cachedValue;

        public ReflectedField(FieldInfo fieldInfo, object parentObj)
        {
            this.fieldInfo = fieldInfo;
            this.parentObj = parentObj;
            this.cachedValue = fieldInfo.GetValue(parentObj);
        }

        public void Render()
        {
            var newValue = cachedValue;
            if(fieldInfo.FieldType == typeof(int))
            {
                newValue = EditorGUILayout.IntField(this.fieldInfo.Name, (int)cachedValue);
            }
            else if (fieldInfo.FieldType == typeof(float))
            {
                newValue = EditorGUILayout.FloatField(this.fieldInfo.Name, (float)cachedValue);
            }
            else if (fieldInfo.FieldType == typeof(bool))
            {
                newValue = EditorGUILayout.Toggle(this.fieldInfo.Name, (bool)cachedValue);
            }
            else if(fieldInfo.FieldType == typeof(string))
            {
                newValue = EditorGUILayout.TextField(this.fieldInfo.Name, (string)cachedValue);
            }
            else if (fieldInfo.FieldType.IsEnum)
            {
                newValue = EditorGUILayout.EnumPopup(this.fieldInfo.Name, (Enum)cachedValue);
            }

            if (newValue != cachedValue)
            {
                this.cachedValue = newValue;
                this.UpdateValue();
            }
        }

        private void UpdateValue()
        {
            fieldInfo.SetValue(parentObj, cachedValue);
        }
       
    }
}
