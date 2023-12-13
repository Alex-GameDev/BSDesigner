using System;
using System.Collections.Generic;
using BSDesigner.Core;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Utility element that handle a group of <see cref="UtilityElement"/> itself and
    /// returns the maximum utility if its best candidate utility is higher than the threshold.
    /// </summary>
    public abstract class UtilityBucket : UtilityElement
    {
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

        public override Type ChildType => typeof(UtilityElement);
        public override int MaxOutputConnections => -1;

        /// <summary>
        /// Collection of elements from which the candidate is chosen.
        /// </summary>
        protected IEnumerable<UtilityElement> Candidates
        {
            get
            {
                if (_candidates == null)
                {
                    _candidates = new List<UtilityElement>();
                    foreach (var node in Children)
                    {
                        if (node is UtilityElement selectableNode)
                            _candidates.Add(selectableNode);
                    }
                }

                return _candidates;
            }
        }
        private List<UtilityElement>? _candidates;

        /// <summary>
        /// The current selected element of the candidates in this bucket.
        /// </summary>
        public UtilityElement? SelectedElement { get; private set; }

        /// <summary>
        /// The last executed element in this bucket.
        /// </summary>
        public UtilityElement? LastExecutedElement { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// Use the selected element to compute the utility.
        /// </summary>
        /// <returns>The utility of the selected element, or -inf if the bucket threshold is not reached.</returns>
        protected sealed override float GetUtility()
        {
            SelectedElement = ComputeCurrentBestElement();
            return SelectedElement.Utility >= BucketThreshold ? SelectedElement.Utility : float.MinValue;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Starts the last executed element.
        /// </summary>
        protected override void OnElementStarted()
        {
            foreach (var node in Candidates) node.MarkUtilityAsDirty();

            if (SelectedElement != LastExecutedElement)
            {
                LastExecutedElement?.Stop();
                LastExecutedElement = SelectedElement;
                LastExecutedElement?.Start();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the last executed element.
        /// </summary>
        protected override void OnElementStopped()
        {
            LastExecutedElement?.Stop();
            LastExecutedElement = null;
        }

        /// <summary>
        /// <inheritdoc/>
        /// If the selected element is not the last executed element stops the last one and
        /// starts the selected one, setting it as the last.
        /// Updates the execution of the last executed element.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected override Status OnElementUpdated()
        {
            if (SelectedElement != LastExecutedElement)
            {
                LastExecutedElement?.Stop();
                LastExecutedElement = SelectedElement;
                LastExecutedElement?.Start();
            }

            LastExecutedElement?.Update();
            return LastExecutedElement?.Status ?? Status.Running;
        }

        /// <summary>
        /// Pauses the last executed element.
        /// </summary>
        protected override void OnElementPaused()
        {
            LastExecutedElement?.Pause();
        }

        /// <summary>
        /// Select an element of <see cref="Candidates"/> to be executed if the bucket is selected.
        /// </summary>
        /// <returns>The selected element</returns>
        protected abstract UtilityElement ComputeCurrentBestElement();

        /// <summary>
        /// Priority is enabled only if the utility of the selected element is higher than the priority threshold and bucket threshold.
        /// </summary>
        public override bool HasPriority => SelectedElement?.Utility >= PriorityThreshold && SelectedElement.Utility >= BucketThreshold;
    }
}