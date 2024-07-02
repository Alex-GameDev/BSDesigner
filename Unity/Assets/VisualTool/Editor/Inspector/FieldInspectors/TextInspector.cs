using UnityEditor;

namespace BSDesigner.Unity.VisualTool.Editor.Inspector
{
    internal class TextInspector : FieldInspector
    {
        private readonly IFieldPointer fieldPointer;

        public TextInspector(IFieldPointer pointer)
        {
            this.fieldPointer = pointer;
            var value = this.fieldPointer.GetValue();
            if(value == null)
            {
                this.fieldPointer.SetValue(string.Empty);
            }
        }

        public override bool IsSingleLine => true;

        public override void Render(RenderInspectorSettings settings)
        {
            var value = this.fieldPointer.GetValue();

            if (value == null)
                return;

            value = EditorGUILayout.TextField(this.fieldPointer.Name, (string)value);
            this.fieldPointer.SetValue(value);
        }
    }
}
