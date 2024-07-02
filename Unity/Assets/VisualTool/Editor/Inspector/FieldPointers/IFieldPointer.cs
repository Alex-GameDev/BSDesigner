using System;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Interface that represents a pointer to a serializable variable or memory element
    /// </summary>
    internal interface IFieldPointer
    {
        /// <summary>
        /// Field name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Field type
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Is this field nullable and instanceable
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        /// get the field value
        /// </summary>
        object GetValue();

        /// <summary>
        /// Set the field value
        /// </summary>
        void SetValue(object value);
    }
}
