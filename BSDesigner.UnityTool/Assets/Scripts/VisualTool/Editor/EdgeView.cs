using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class EdgeView : Edge
    {
        private static readonly float k_CapRadius = 4f;
        private static readonly float k_InterceptWidth = 6f;

        public EdgeControl control;

        protected override EdgeControl CreateEdgeControl()
        {
            control = new Control(this)
            {
                capRadius = k_CapRadius,
                interceptWidth = k_InterceptWidth
            };
            return control;
        }
    }

    public class Control : EdgeControl
    {
        private static readonly float k_MinJointDistance = 30f;

        private readonly EdgeView m_EdgeView;

        public Control(EdgeView edgeView)
        {
            m_EdgeView = edgeView;
        }

        protected override void ComputeControlPoints()
        {
            base.ComputeControlPoints();

            var inputDir = Vector2.zero;
            var outputDir = Vector2.zero;

            if (m_EdgeView.input is PortView inputPortView)
            {
                inputDir = inputPortView.ConnectionDirection;
            }

            if (m_EdgeView.output is PortView outputPortView)
            {
                outputDir = outputPortView.ConnectionDirection;
            }

            var delta = (controlPoints[3] - controlPoints[0]).magnitude * .25f;
            if (delta > k_MinJointDistance) delta = k_MinJointDistance;

            controlPoints[1] = controlPoints[0] + (delta * outputDir);
            controlPoints[2] = controlPoints[3] + (delta * inputDir);
        }
    }
}
