using System;

namespace BSDesigner.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HideInspectorAttribute : Attribute
    {
    }
}
