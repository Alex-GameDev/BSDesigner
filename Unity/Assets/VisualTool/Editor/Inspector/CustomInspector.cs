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


        private ReflectedField mainFieldRenderer;

        public void Update(object obj)
        {
            //fields.Clear();

            mainFieldRenderer = obj != null ? new ClassInstanceReflectionField(new StaticReferencePointer("Node", obj), false) : null;

            //if(obj != null)
            //{
            //    var typeFields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).OrderBy(field => field.MetadataToken);
            //    foreach (var field in typeFields)
            //    {
            //        fields.Add(CreateReflectedField(field, obj));
            //    }
            //}

            Render();
        }

        private void Render()
        {
            //foreach (var field in fields)
            //{
            //    field.Render();
            //}
            this.mainFieldRenderer?.Render();
        }

        //private ReflectedField CreateReflectedField(FieldInfo fieldInfo, object parentObj)
        //{
        //    Type type = fieldInfo.FieldType;
        //    return new ReflectedField(fieldInfo, parentObj);
        //}
    }
    


    //public class ReflectedField
    //{
    //    private FieldInfo fieldInfo;
    //    private object parentObj;
    //    private object cachedValue;

    //    private List<ReflectedField> subFields = new List<ReflectedField>();

    //    public ReflectedField(FieldInfo fieldInfo, object parentObj)
    //    {
    //        this.fieldInfo = fieldInfo;
    //        this.parentObj = parentObj;
    //        this.cachedValue = fieldInfo.GetValue(parentObj);
    //        if(cachedValue != null)
    //        {
    //            var typeFields = cachedValue.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).OrderBy(field => field.MetadataToken);
    //            foreach (var field in typeFields)
    //            {
    //                subFields.Add(CreateReflectedField(field, cachedValue));
    //            }
    //        }
    //    }

    //    public void Render()
    //    {
    //        var newValue = cachedValue;

    //        if (fieldInfo.FieldType.IsArray)
    //        {
    //            //Si no tiene valor hay que inicializarlo (listas y arrays + strings)
    //            if(fieldInfo.GetValue(parentObj) == null)
    //            {
    //                fieldInfo.SetValue(this.parentObj, Activator.CreateInstance(fieldInfo.FieldType, 0));
    //            }
    //            EditorGUILayout.BeginHorizontal();
    //            EditorGUILayout.LabelField(this.fieldInfo.Name, this.fieldInfo.GetValue(parentObj).GetType().Name);
    //            if (GUILayout.Button("+"))
    //            {
    //                this.AddSubelementInList();
    //            }
    //            EditorGUILayout.EndHorizontal();
    //            foreach (var item in this.subFields)
    //            {
    //                EditorGUILayout.BeginHorizontal();
    //                EditorGUILayout.BeginVertical();
    //                item.Render();
    //                EditorGUILayout.EndVertical();
    //                if (GUILayout.Button("-"))
    //                {
    //                    Debug.Log("Eliminar elemento");
    //                }
    //                EditorGUILayout.EndHorizontal();
    //            }
    //        }
    //        else if (fieldInfo.FieldType == typeof(int))
    //        {
    //            newValue = EditorGUILayout.IntField(this.fieldInfo.Name, (int)cachedValue);
    //        }
    //        else if (fieldInfo.FieldType == typeof(float))
    //        {
    //            newValue = EditorGUILayout.FloatField(this.fieldInfo.Name, (float)cachedValue);
    //        }
    //        else if (fieldInfo.FieldType == typeof(bool))
    //        {
    //            newValue = EditorGUILayout.Toggle(this.fieldInfo.Name, (bool)cachedValue);
    //        }
    //        else if(fieldInfo.FieldType == typeof(string))
    //        {
    //            newValue = EditorGUILayout.TextField(this.fieldInfo.Name, (string)cachedValue);
    //        }
    //        else if (fieldInfo.FieldType.IsEnum)
    //        {
    //            newValue = EditorGUILayout.EnumPopup(this.fieldInfo.Name, (Enum)cachedValue);
    //        }
    //        else if(!fieldInfo.FieldType.IsValueType)
    //        {               

    //            if (fieldInfo.FieldType.IsAbstract)
    //            {
    //                if(fieldInfo.GetValue(this.parentObj) != null)
    //                {
    //                    EditorGUILayout.BeginHorizontal();
    //                    EditorGUILayout.LabelField(this.fieldInfo.Name, this.fieldInfo.GetValue(parentObj).GetType().Name);
    //                    if (GUILayout.Button("x"))
    //                    {
    //                        this.ResetValue();
    //                        EditorGUILayout.EndHorizontal();
    //                        return;
    //                    }
    //                    EditorGUILayout.EndHorizontal();

    //                    foreach (var item in this.subFields)
    //                    {
    //                        item.Render();
    //                    }
    //                }
    //                else
    //                {
    //                    EditorGUILayout.BeginHorizontal();
    //                    EditorGUILayout.LabelField(this.fieldInfo.Name);
    //                    if (GUILayout.Button("Set"))
    //                    {
    //                        this.OpenTypeSelector();
    //                    }
    //                    EditorGUILayout.EndHorizontal();
    //                }
    //            }
    //        }

    //        if (newValue != cachedValue)
    //        {
    //            this.cachedValue = newValue;
    //            this.UpdateValue();
    //        }
    //    }

    //    private void AddSubelementInList()
    //    {
    //        var t = this.fieldInfo.FieldType.GetElementType();
    //        if(!t.IsAbstract)
    //        {
    //        }
    //    }

    //    private void ResetValue()
    //    {
    //        this.cachedValue = null;
    //        this.UpdateValue();
    //        this.subFields.Clear();
    //    }

    //    private void OpenTypeSelector()
    //    {
    //        SearchWindow.Open(new SearchWindowContext(Vector2.zero), SearchWindowProvider.Create(this.fieldInfo.FieldType, this.OnSetType));
    //    }

    //    private void OnSetType(Type type)
    //    {
    //        Debug.Log("Set value");
    //        var value = Activator.CreateInstance(type);
    //        this.cachedValue = value;
    //        this.UpdateValue();
    //        var typeFields = value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).OrderBy(field => field.MetadataToken);
    //        foreach (var field in typeFields)
    //        {
    //            subFields.Add(CreateReflectedField(field, value));
    //        }
    //    }

    //    private ReflectedField CreateReflectedField(FieldInfo fieldInfo, object parentObj)
    //    {
    //        Type type = fieldInfo.FieldType;
    //        return new ReflectedField(fieldInfo, parentObj);
    //    }

    //    private void UpdateValue()
    //    {
    //        fieldInfo.SetValue(parentObj, cachedValue);
    //    }
    //}
}
