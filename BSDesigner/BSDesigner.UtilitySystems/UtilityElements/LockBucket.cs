using BSDesigner.Core;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Bucket that locks the selected action until ends
    /// </summary>
    public class LockBucket : UtilityBucket
    {
        protected override UtilityElement ComputeCurrentBestElement()
        {
            if (SelectedElement is { Status: Status.Running })
                return SelectedElement;

            var currentHigherUtility = float.MinValue;
            UtilityElement? newBestElement = null;
            foreach (var candidate in Candidates)
            {
                candidate.UpdateUtility();
                var utility = candidate.Utility;

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