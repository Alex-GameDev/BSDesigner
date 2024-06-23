using System;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    /// <summary>
    /// Interfaz que representa un puntero a un campo de un objeto.
    /// </summary>
    internal interface IFieldPointer
    {
        string Name { get; }
        Type Type { get; }

        object GetValue();

        void SetValue(object value);
    }
}
