using System;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;
using BSDesigner.UtilitySystems.UtilityElements;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Behaviour graph that choose between different nodes by an utility value and executes it.
    /// </summary>
    public class UtilitySystem : BehaviourGraph
    {
        public override Type NodeType => typeof(UtilityNode);

        /// <summary>
        /// The utility multiplier of the selected element and prevents it from fluctuations.
        /// </summary>
        public float Inertia = 1.3f;

        /// <summary>
        /// The nodes in the system that can be selected as best candidate.
        /// </summary>
        protected List<UtilityElement> Candidates
        {
            get
            {
                if (_candidates == null)
                {
                    _candidates = new List<UtilityElement>();
                    foreach (Node node in Nodes)
                    {
                        if (node is UtilityElement { Parents: { Count: 0 } } selectableNode)
                            _candidates.Add(selectableNode);
                    }
                }

                return _candidates;
            }
        }

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

        private List<UtilityElement>? _candidates;
        private List<UtilityNode>? _utilityNodes;

        private UtilityElement? _currentBestElement;

        /// <summary>
        /// Create a new <see cref="UtilityAction"/> that computes its utility using <paramref name="factor"/> and executes the action specified in <paramref name="action"/>.
        /// To prevent the action from being added to the <see cref="UtilitySystem"/> candidate list.
        /// To make the <see cref="UtilitySystem"/> execution ends when the action ends, set <paramref name="finishOnComplete"/> to true (default is false).
        /// </summary>
        /// <param name="factor">The child factor of the action.</param>
        /// <param name="action">The action executed.</param>
        /// <param name="finishOnComplete">true of the execution of the utility system must finish when the action finish.</param>
        /// <returns>The created utility action</returns>
        public UtilityAction CreateAction(Factor factor, ActionTask? action = null, bool finishOnComplete = false)
        {
            var utilityAction = CreateNode<UtilityAction>();
            utilityAction.Action = action;
            utilityAction.FinishSystemOnComplete = finishOnComplete;
            ConnectNodes(utilityAction, factor);
            return utilityAction;
        }

        /// <summary>
        /// Create a new <see cref="ConstantFactor"/> in this <see cref="ConstantFactor"/> with a constant utility value.
        /// </summary>
        /// <param name="value">The utility value</param>
        /// <returns>The <see cref="ConstantFactor"/> created.</returns>
        public ConstantFactor CreateConstantFactor(float value)
        {
            var constantFactor = CreateNode<ConstantFactor>();
            constantFactor.Value = value;
            return constantFactor;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnStarted()
        {
            foreach (var node in UtilityNodes) node.MarkUtilityAsDirty();

            _currentBestElement = ComputeCurrentBestElement();
            _currentBestElement.Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Recalculates the utilities of the nodes and selects the best candidate to run it.
        /// If the new candidate chosen is different from the one of the previous iteration, it stops its execution and starts the new one.
        /// </summary>
        protected override void OnUpdated()
        {
            _currentBestElement?.Update();

            foreach (var node in UtilityNodes) node.MarkUtilityAsDirty();

            var newBestAction = ComputeCurrentBestElement();

            if (newBestAction == _currentBestElement) return;

            _currentBestElement?.Stop();
            _currentBestElement = newBestAction;
            _currentBestElement.Start();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the current best element execution.
        /// </summary>
        protected override void OnStopped()
        {
            _currentBestElement?.Stop();
            _currentBestElement = null;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the current best element execution.
        /// </summary>
        protected override void OnPaused()
        {
            _currentBestElement?.Pause();
        }

        private UtilityElement ComputeCurrentBestElement()
        {
            var currentHigherUtility = float.MinValue;
            UtilityElement? newBestElement = null;
            foreach (var candidate in Candidates)
            {
                candidate.UpdateUtility();
                var utility = candidate.Utility * (candidate == _currentBestElement ? Inertia : 1f);

                if (utility > currentHigherUtility)
                {
                    currentHigherUtility = utility;
                    newBestElement = candidate;
                }
            }

            if (newBestElement == null)
                throw new EmptyGraphException("Can't find the best candidate, the list is empty.");

            return newBestElement;
        }
    }
}
