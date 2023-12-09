using BSDesigner.Core;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core.Utils;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// Branch selection node that chooses one of its branches randomly.
    /// </summary>
    public class RandomBranchSelectionNode : BranchSelectionNode
    {
        /// <summary>
        /// The list of probabilities assigned to each branch.
        /// </summary>
        public IDictionary<Node,float> Probabilities = new Dictionary<Node, float>();

        /// <summary>
        /// Value used to mock the probability generations
        /// </summary>
        public IRandom Random = new DefaultRandom();
        
        /// <summary>
        /// <inheritdoc/>
        /// Gets a random child node.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected override int SelectBranchIndex()
        {
            var totalProb = Probabilities.Values.Where(v => v > 0).Sum();

            if (totalProb == 0) return Random.NextInt(0, Children.Count);

            var randomValue = Random.NextDouble() * totalProb;
            var probabilitySum = 0f;
            for(var i = 0; i < Children.Count; i++)
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