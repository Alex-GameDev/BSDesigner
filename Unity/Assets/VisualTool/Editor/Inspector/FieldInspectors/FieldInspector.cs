using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Base class for all the field inspector renderers
    /// </summary>
    internal abstract class FieldInspector
    {
        /// <summary>
        /// Render the field
        /// </summary>
        public abstract void Render(RenderInspectorSettings settings);

        /// <summary>
        /// Create a new field renderer from a member info.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static FieldInspector CreateFromFieldInfo(FieldInfo field, object value)
        {
            var type = field.FieldType;
            IFieldPointer pointer = new ReflectedFieldPointer(field, value);
            return GetFieldFromPointer(pointer, type);
        }

        /// <summary>
        /// Create a new field rendere form an array item.
        /// </summary>
        /// <param name="arrayValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static FieldInspector CreateFromArrayElement(Array arrayValue, int i)
        {
            var type = arrayValue.GetType().GetElementType();
            IFieldPointer pointer = new ArrayElementPointer(arrayValue, i);
            return GetFieldFromPointer(pointer, type);
        }

        /// <summary>
        /// Create a new field rendere form a list item.
        /// </summary>
        /// <param name="listValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static FieldInspector CreateFromListElement(IList listValue, int i)
        {
            var type = listValue.GetType().GetGenericArguments().First();
            IFieldPointer pointer = new ListElementPointer(listValue, i);
            return GetFieldFromPointer(pointer, type);
        }

        private static FieldInspector GetFieldFromPointer(IFieldPointer pointer, Type type)
        {
            if (type.IsPrimitive || type.IsEnum)
            {
                return new SimpleFieldInspector(pointer);
            }
            else if(type == typeof(string))
            {
                return new TextInspector(pointer);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return new ListInspector(pointer);
            }
            else if (type.IsArray)
            {
                return new ArrayInspector(pointer);
            }
            else
            {
                return new ClassInstanceInspector(pointer);
            }
        }
    }
}
