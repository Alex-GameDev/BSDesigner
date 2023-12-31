using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomNodeDrawerAttribute : Attribute
    {
        public Type NodeType { get; set; }

        public CustomNodeDrawerAttribute(Type nodeType)
        {
            NodeType = nodeType;
        }
    }

    public abstract class NodeDrawer
    {
        #region Private fields

        private NodeView m_View;

        #endregion

        #region Factory method

        public static NodeDrawer CreateDrawer(NodeView nodeView)
        {
            var type = TypeCache.GetTypesDerivedFrom<NodeDrawer>()
                .First(t => t.GetCustomAttribute<CustomNodeDrawerAttribute>() != null 
                    && t.GetCustomAttribute<CustomNodeDrawerAttribute>().NodeType == nodeView.Node.GetType());

            var drawer = (NodeDrawer)Activator.CreateInstance(type);
            drawer.m_View = nodeView;
            return drawer;
        }

        #endregion

        #region Public method

        /// <summary>
        /// Set up the node ports
        /// </summary>
        public abstract void SetUpPorts();

        /// <summary>
        /// Add options to the node contextual menu.
        /// </summary>
        /// <param name="evt">The contextual menu creation event.</param>
        public virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt) { }

        /// <summary>
        /// Draw the specified elements of the node drawn.
        /// </summary>
        public virtual void OnCreated() { }

        /// <summary>
        /// Method called when the node needs to be repainted.
        /// </summary>
        public virtual void OnRepaint() { }

        /// <summary>
        /// Method called when the node position changes
        /// </summary>
        public virtual void OnMoved() { }

        /// <summary>
        /// Method called when the node is selected.
        /// </summary>
        public virtual void OnSelected() { }

        /// <summary>
        /// Method called when the node is unselected.
        /// </summary>
        public virtual void OnUnselected() { }

        /// <summary>
        /// Method called when a new connection is created in the node.
        /// </summary>
        public virtual void OnConnected(EdgeView edgeView) { }

        /// <summary>
        /// Method called when a new connection is deleted in the node.
        /// </summary>
        public virtual void OnDisconnected(EdgeView edgeView) { }

        /// <summary>
        /// Method called when the node is being removed from the graph.
        /// </summary>
        public virtual void OnDeleted() { }

        /// <summary>
        /// Method called when the node properties changed and must be reflected in the view        
        /// </summary>
        public virtual void OnDataChanged() { }

        /// <summary>
        /// Method called when the node is destroyed in runtime mode, when the selected graph changes.
        /// </summary>
        public virtual void OnDestroy() { }

        public virtual Port GetValidPort(NodeView target, Direction direction)
        {
            if (direction == Direction.Input)
            {
                return m_View.InputPorts.First();
            }
            else
            {
                return m_View.OutputPorts.First();
            }
        }

        #endregion


    }
}
