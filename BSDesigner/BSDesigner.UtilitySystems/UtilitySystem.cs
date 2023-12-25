using System;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Actions;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Behaviour graph that allows you to select an action in a tree based on a utility value.
    /// </summary>
    public class UtilitySystem : BehaviourGraph
    {
        public override Type NodeType => typeof(UtilityNode);

        public override bool CanCreateLoops => false;

        /// <summary>
        /// The elements of the system as utility nodes.
        /// </summary>
        protected List<UtilityNode> UtilityNodes
        {
            get
            {
                if (_utilityNodes == null)
                    _utilityNodes = Nodes.Cast<UtilityNode>().ToList();
                return _utilityNodes;
            }
        }

        private List<UtilityNode>? _utilityNodes;

        /// <summary>
        /// The element that this system will execute.
        /// </summary>
        protected UtilityElement MainElement
        {
            get
            {
                if (_cachedMainElement == null)
                {
                    var firstState = Nodes.OfType<UtilityElement>().FirstOrDefault();
                    _cachedMainElement = firstState ?? throw new EmptyGraphException("Can't find the root element if graph is empty");
                }
                return _cachedMainElement;
            }
        }

        private UtilityElement? _cachedMainElement;

        /// <summary>
        /// Create a new bucket of type <typeparamref name="T"/> that will select between the elements in <paramref name="candidates"/>.
        /// </summary>
        /// <typeparam name="T">The type of the bucket created.</typeparam>
        /// <param name="bucketThreshold">The minimum utility value for a candidate to be selected.</param>
        /// <param name="priorityThreshold">The minimum utility value for a the selected candidate to enable the priority.</param>
        /// <param name="candidates">The list of candidates of the bucket. They are also its children.</param>
        /// <returns>The bucket created.</returns>
        public T CreateBucket<T>(IEnumerable<UtilityElement> candidates, float bucketThreshold = 0f, float priorityThreshold = 0f) where T : UtilityBucket, new()
        {
            var bucket = CreateNode<T>();
            bucket.BucketThreshold = bucketThreshold;
            bucket.PriorityThreshold = priorityThreshold;
            foreach (var candidate in candidates)
            {
                ConnectNodes(bucket, candidate);
            }
            return bucket;
        }

        /// <summary>
        /// Create a new bucket of type <typeparamref name="T"/> that will select between the elements in <paramref name="candidates"/>.
        /// </summary>
        /// <typeparam name="T">The type of the bucket created.</typeparam>
        /// <param name="bucketThreshold">The minimum utility value for a candidate to be selected.</param>
        /// <param name="priorityThreshold">The minimum utility value for a the selected candidate to enable the priority.</param>
        /// <param name="candidates">The list of candidates of the bucket. They are also its children.</param>
        /// <returns>The bucket created.</returns>
        public T CreateBucket<T>(float bucketThreshold, float priorityThreshold, params UtilityElement[] candidates) where T : UtilityBucket, new() => CreateBucket<T>(candidates, bucketThreshold, priorityThreshold);

        /// <summary>
        /// Create a new bucket of type <typeparamref name="T"/> that will select between the elements in <paramref name="candidates"/>.
        /// </summary>
        /// <typeparam name="T">The type of the bucket created.</typeparam>
        /// <param name="candidates">The list of candidates of the bucket. They are also its children.</param>
        /// <returns>The bucket created.</returns>
        public T CreateBucket<T>(params UtilityElement[] candidates) where T : UtilityBucket, new() => CreateBucket<T>(candidates, 0f);

        /// <summary>
        /// Create a new <see cref="UtilityAction"/> that computes its utility using <paramref name="factor"/> and executes the action specified in <paramref name="action"/>.
        /// To prevent the action from being added to the <see cref="UtilitySystem"/> candidate list.
        /// To make the <see cref="UtilitySystem"/> execution ends when the action ends, set <paramref name="finishOnComplete"/> to true (default is false).
        /// </summary>
        /// <param name="factor">The child factor of the action.</param>
        /// <param name="action">The action executed.</param>
        /// <param name="finishOnComplete">true of the execution of the utility system must finish when the action finish.</param>
        /// <returns>The created utility action</returns>
        public UtilityAction CreateAction(UtilityFactor factor, ActionTask? action = null, bool executeOnLoop = false,  bool finishOnComplete = false)
        {
            var utilityAction = CreateNode<UtilityAction>();
            utilityAction.Action = action;
            utilityAction.ExecuteInLoop = executeOnLoop;
            utilityAction.FinishSystemOnComplete = finishOnComplete;
            ConnectNodes(utilityAction, factor);
            return utilityAction;
        }

        /// <summary>
        /// Create a new fusion factor of type <typeparamref name="T"/> that combines the utility of <paramref name="factors"/>.
        /// </summary>
        /// <typeparam name="T">The type of the factor.</typeparam>
        /// <param name="factors">The list of child factors.</param>
        /// <returns>The <typeparamref name="T"/> created.</returns>
        public T CreateFusion<T>(params UtilityFactor[] factors) where T : UtilityFusion, new() => CreateFusion<T>((IEnumerable<UtilityFactor>) factors);

        /// <summary>
        /// Create a new fusion factor of type <typeparamref name="T"/> that combines the utility of <paramref name="factors"/>.
        /// </summary>
        /// <typeparam name="T">The type of the factor.</typeparam>
        /// <param name="factors">The list of child factors.</param>
        /// <returns>The <typeparamref name="T"/> created.</returns>
        public T CreateFusion<T>(IEnumerable<UtilityFactor> factors) where T : UtilityFusion, new()
        {
            var fusionFactor = CreateNode<T>();
            foreach(var factor in factors)
            {
                ConnectNodes(fusionFactor, factor);
            }
            return fusionFactor;
        }

        /// <summary>
        /// Create a new function factor of type <typeparamref name="T"/> that computes its utility value modifying the utility of <paramref name="child"/> factor.
        /// </summary>
        /// <typeparam name="T">The type of the factor.</typeparam>
        /// <param name="child">The child factor.</param>
        /// <returns>The <typeparamref name="T"/> created.</returns>
        public T CreateCurve<T>(UtilityFactor child) where T : UtilityCurve, new()
        {
            var curveFactor = CreateNode<T>();
            ConnectNodes(curveFactor, child);
            return curveFactor;
        }

        /// <summary>
        /// Use this method to instantiate custom types of leaf factors.
        /// </summary>
        /// <typeparam name="T">The type of the leaf factor.</typeparam>
        /// <returns>The factor of type <typeparamref name="T"/> created.</returns>
        public T CreateLeaf<T>() where T : UtilityLeaf, new() => CreateNode<T>();

        /// <summary>
        /// Create a new <see cref="ConstantUtilityLeaf"/> in this <see cref="ConstantUtilityLeaf"/> with a constant utility value.
        /// </summary>
        /// <param name="value">The utility value</param>
        /// <returns>The <see cref="ConstantUtilityLeaf"/> created.</returns>
        public ConstantUtilityLeaf CreateConstantLeaf(float value)
        {
            var constantFactor = CreateLeaf<ConstantUtilityLeaf>();
            constantFactor.Value = value;
            return constantFactor;
        }

        /// <summary>
        /// Create a new <see cref="VariableUtilityLeaf"/> in this <see cref="VariableUtilityLeaf"/> with a constant utility value.
        /// </summary>
        /// <param name="valueFunction">The function delegate that executes this factor.</param>
        /// <param name="min">The minimum expected value of the result of <paramref name="valueFunction"/></param>
        /// <param name="max">The maximum expected value of the result of <paramref name="valueFunction"/></param>
        /// <returns>The <see cref="VariableUtilityLeaf"/> created.</returns>
        public VariableUtilityLeaf CreateVariableLeaf(Func<float> valueFunction, float min, float max)
        {
            var variableFactor = CreateLeaf<VariableUtilityLeaf>();
            variableFactor.ValueFunction = valueFunction;
            variableFactor.Min = min;
            variableFactor.Max = max;
            return variableFactor;
        }

        /// <summary>
        /// Specify a new root node.
        /// </summary>
        /// <param name="node">The new root node of the behaviour tree.</param>
        public void ChangeRootNode(UtilityElement node)
        {
            ReorderNode(node, 0);
            _cachedMainElement = node;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnStarted()
        {
            foreach (var node in UtilityNodes) node.MarkUtilityAsDirty();
            MainElement.UpdateUtility();
            MainElement.Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Recalculates the utilities of the nodes and selects the best candidate to run it.
        /// If the new candidate chosen is different from the one of the previous iteration, it stops its execution and starts the new one.
        /// </summary>
        protected override void OnUpdated()
        {
            foreach (var node in UtilityNodes) node.MarkUtilityAsDirty();
            MainElement.UpdateUtility();
            MainElement.Update();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the current best element execution.
        /// </summary>
        protected override void OnStopped() => MainElement.Stop();

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the current best element execution.
        /// </summary>
        protected override void OnPaused() => MainElement.Pause();
    }
}
