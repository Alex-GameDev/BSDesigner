using System;
using System.Linq;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.UtilitySystems
{
    public class InertiaBucket : UtilityBucket
    {
        public static readonly float DefaultInertia = 1.3f;

        /// <summary>
        /// A multiplier applied to the utility of the last selected element in order to
        /// avoid the fluctuations in the selected element computation.
        /// </summary>
        public float Inertia = DefaultInertia;

        /// <summary>
        /// The utility value that any action in this bucket should reach to be selected.
        /// </summary>
        public float BucketThreshold;

        /// <summary>
        /// The utility value that the selected element in this bucket should reach to
        /// enable the bucket priority. If is lower than the bucket threshold, the higher value
        /// will be used.
        /// </summary>
        public float PriorityThreshold;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyGraphException"></exception>
        protected override UtilityElement ComputeCurrentBestElement()
        {
            var currentHigherUtility = float.MinValue;
            UtilityElement? newBestElement = null;
            foreach (var candidate in Candidates)
            {
                candidate.UpdateUtility();
                var utility = candidate.Utility * (candidate == SelectedElement ? Inertia : 1f);

                if (utility > currentHigherUtility)
                {
                    currentHigherUtility = utility;
                    newBestElement = candidate;
                }

                if (candidate.HasPriority && newBestElement != null)
                    return newBestElement;
            }

            if (newBestElement == null)
                throw new MissingConnectionException("Can't find the best candidate, the list is empty.");

            return newBestElement;
        }

        /// <summary>
        /// The utility of the bucket will be the utility of its selected element unless it does not reach the utility threshold.
        /// </summary>
        /// <returns>The selected element value or -1 if is lower than the bucket threshold.</returns>
        public override float GetComputedUtility() => SelectedElement.Utility >= BucketThreshold ? SelectedElement.Utility : float.MinValue;

        /// <summary>
        /// Priority is enabled only if the utility of the selected element is higher than the priority threshold and bucket threshold.
        /// </summary>
        public override bool HasPriority => SelectedElement.Utility >= PriorityThreshold && SelectedElement.Utility >= BucketThreshold;
    }
}