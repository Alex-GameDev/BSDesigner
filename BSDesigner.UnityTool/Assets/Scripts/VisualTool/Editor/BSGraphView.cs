using BSDesigner.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class BSGraphView : GraphView
    {
        private static readonly float k_MinZoomScale = 0.5f;
        private static readonly float k_MaxZoomScale = 5f;

        private BehaviourGraph m_CurrentGraph;

        public BSGraphView()
        {
            var bg = new GridBackground { name = "Grid" };
            bg.StretchToParentSize();
            Insert(0, bg);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/VisualTool/Editor/uss/GridBackground.uss");
            this.styleSheets.Add(styleSheet);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            SetupZoom(ContentZoomer.DefaultMinScale * k_MinZoomScale, ContentZoomer.DefaultMaxScale * k_MaxZoomScale);
        }
    }

}