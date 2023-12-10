using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Utils;

namespace BSDesigner.StateMachines
{
    /// <summary>
    /// Transition that has multiple target states and allows state machine to transit to any of them depending on their probabilities.
    /// </summary>
    public class ProbabilityTransition : Transition
    {
        public override int MaxOutputConnections => -1;

        /// <summary>
        /// The list of probabilities assigned to each state.
        /// </summary>
        public IDictionary<Node, float> Probabilities = new Dictionary<Node, float>();

        /// <summary>
        /// Value used to mock the probability generations
        /// </summary>
        public IRandom Random = new DefaultRandom();

        /// <summary>
        /// The target states of the transition
        /// </summary>
        protected IReadOnlyList<State> TargetStates
        {
            get
            {
                if (_cachedTargetStates == null)
                {
                    if (Children.Count == 0) throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedTargetStates = Children.Cast<State>().ToList();
                }
                return _cachedTargetStates;
            }
        }

        private List<State>? _cachedTargetStates;

        /// <summary>
        /// <inheritdoc/>
        /// Transition to a random state from the target list depending on the probabilities.
        /// </summary>
        protected override void OnTransitionPerformed()
        {
            var selectedTransitionIndex = GetSelectedTransitionIndex();
            StateMachine.ChangeState(TargetStates[selectedTransitionIndex]);
        }

        private int GetSelectedTransitionIndex()
        {
            var totalProb = Probabilities.Values.Where(v => v > 0).Sum();

            if (totalProb == 0) return Random.NextInt(0, Children.Count);

            var randomValue = Random.NextDouble() * totalProb;
            var probabilitySum = 0f;
            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                probabilitySum += Probabilities[child];
                if (probabilitySum > randomValue)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}