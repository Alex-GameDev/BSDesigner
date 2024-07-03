using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal static class TypeUtilities
    {
        private static readonly Type[] UNITY_BUILTIN_TYPES = new Type[]
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Vector2Int),
            typeof(Vector3Int),
            typeof(Matrix4x4),
            typeof(Rect),
            typeof(RectInt),
            typeof(Color),
            typeof(Gradient),
            typeof(AnimationCurve),
        };

        private static readonly HashSet<Type> unityBuildingTypeSet = UNITY_BUILTIN_TYPES.ToHashSet();

        public static bool IsSerializableInInspector(Type type) =>
            type.IsPrimitive || type == typeof(string) ||
            type.IsEnum || unityBuildingTypeSet.Contains(type) ||
            typeof(UnityEngine.Object).IsAssignableFrom(type);

        public static IEnumerable<FieldInfo> GetPublicFields(this Type type)
        {
            List<FieldInfo> fieldInfos = new List<FieldInfo>();

            while (type != null)
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                fieldInfos.InsertRange(0, fields);
                type = type.BaseType;
            }
            return fieldInfos;
        }

    }
}
