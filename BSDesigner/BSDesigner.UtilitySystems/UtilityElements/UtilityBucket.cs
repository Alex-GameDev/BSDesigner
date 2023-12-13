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
        public UtilityElement SelectedElement { get; private set; } = null!;

        /// <summary>
        /// The last executed element in this bucket.
        /// </summary>
        public UtilityElement? LastExecutedElement { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// Use the selected element to compute the utility.
        /// </summary>
        /// <returns>The utility of the selected element.</returns>
        protected sealed override float GetUtility()
        {
            SelectedElement = ComputeCurrentBestElement();
            return GetComputedUtility();
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
        /// Override this method to specify how the utility is computed for this bucket.
        /// </summary>
        /// <returns>The computed utility of the bucket.</returns>
        public virtual float GetComputedUtility() => SelectedElement.Utility;
    }
}