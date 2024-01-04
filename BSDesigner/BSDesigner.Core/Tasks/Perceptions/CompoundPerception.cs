using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core.Perceptions
{
    /// <summary>
    /// Perception that computes the result using other perceptions
    /// </summary>
    public abstract class CompoundPerception : PerceptionTask
    {
        public List<PerceptionTask> SubPerceptions;

        protected CompoundPerception()
        {
            SubPerceptions = new List<PerceptionTask>();
        }

        protected CompoundPerception(List<PerceptionTask> subPerceptions)
        {
            SubPerceptions = subPerceptions;
        }

        protected CompoundPerception(params PerceptionTask[] subPerceptions)
        {
            SubPerceptions = subPerceptions.ToList();
        }

        /// <summary>
        /// Begin all sub perceptions
        /// </summary>
        protected override void OnBeginTask()
        {
            SubPerceptions.ForEach(p => p.Start());
        }

        /// <summary>
        /// End all sub perceptions
        /// </summary>
        protected override void OnEndTask()
        {
            SubPerceptions.ForEach(p => p.Stop());
        }

        /// <summary>
        /// Pause all sub perceptions
        /// </summary>
        protected override void OnPauseTask()
        {
            SubPerceptions.ForEach(p => p.Pause());
        }

        /// <summary>
        /// Don't do nothing (sub perceptions are resumed from update).
        /// </summary>
        protected override void OnResumeTask()
        {
        }
    }
}