using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Base class for all the field inspector renderers
    /// </summary>
    internal abstract class ReflectedField
    {
        /// <summary>
        /// Render the field
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Create a new field renderer from a member info.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static ReflectedField CreateFromFieldInfo(FieldInfo field, object value)
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
        internal static ReflectedField CreateFromArrayElement(Array arrayValue, int i)
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
        internal static ReflectedField CreateFromListElement(IList listValue, int i)
        {
            var type = listValue.GetType().GetElementType();
            IFieldPointer pointer = new ListElementPointer(listValue, i);
            return GetFieldFromPointer(pointer, type);
        }

        private static ReflectedField GetFieldFromPointer(IFieldPointer pointer, Type type)
        {
            if (type.IsPrimitive || type.IsEnum || type == typeof(string))
            {
                return new SimpleReflectionField(pointer);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return new ListReflectionField(pointer);
            }
            else if (type.IsArray)
            {
                return new ArrayReflectionField(pointer);
            }
            else
            {
                return new ClassInstanceReflectionField(pointer, type.IsAbstract);
            }
        }
    }
}
