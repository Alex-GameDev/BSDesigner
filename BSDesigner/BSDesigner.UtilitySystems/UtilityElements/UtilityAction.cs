using System;
using BSDesigner.Core;
using BSDesigner.Core.Exceptions;
using BSDesigner.Core.Tasks;

namespace BSDesigner.UtilitySystems.UtilityElements
{
    public class UtilityAction : UtilityElement
    {
        public override Type ChildType => typeof(Factor);
        public override int MaxOutputConnections => 1;

        /// <summary>
        /// The <see cref="Action"/> that this <see cref="UtilityAction"/> executes when is selected.
        /// </summary>
        public ActionTask? Action;

        protected Factor Factor
        {
            get
            {
                if (_cachedFactor == null)
                {
                    if (Children.Count == 0) throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedFactor = (Factor)Children[0];
                }
                return _cachedFactor;
            }
        }

        private Factor? _cachedFactor;

        public bool FinishSystemOnComplete;

        protected override float GetUtility()
        {
            Factor?.UpdateUtility();
            return Factor?.Utility ?? 0f;
        }

        protected override void OnNodeStarted() => Action?.Start();

        protected override void OnNodeStopped() => Action?.Stop();

        protected override Status OnNodeUpdated()
        {
            var actionResult = Action?.Update() ?? Status.Running;

            if (FinishSystemOnComplete && actionResult != Status.Running)
            {
                Graph.Finish(actionResult);
            }

            return actionResult;
        }

        protected override void OnNodePaused() => Action?.Pause();
    }
}