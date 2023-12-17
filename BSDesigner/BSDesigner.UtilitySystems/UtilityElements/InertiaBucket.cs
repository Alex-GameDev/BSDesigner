using System;
using System.Linq;
using BSDesigner.Core;
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
        public Parameter<float> Inertia = DefaultInertia;

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
                var utility = candidate.Utility * (candidate == SelectedElement ? Inertia.Value : 1f);

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
    }
}