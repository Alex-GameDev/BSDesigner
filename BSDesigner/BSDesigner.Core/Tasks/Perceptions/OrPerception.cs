using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core.Perceptions
{
    /// <summary>
    /// Compound perception that returns true if any of its sub perceptions return true.
    /// </summary>
    public class OrPerception : CompoundPerception
    {

        public OrPerception()
        {
        }

        public OrPerception(List<PerceptionTask> subPerceptions) : base(subPerceptions)
        {
        }

        public OrPerception(params PerceptionTask[] subPerceptions) : base(subPerceptions)
        {
        }

        public override string GetInfo() => $"({string.Join(" | ", SubPerceptions.Select(p => p.GetInfo()))})";

        /// <summary>
        /// Perform an OR operation with the results of the sub perception. If there is no sub perception returns false.
        /// </summary>
        /// <returns>True if any of its sub perceptions return true, false otherwise.</returns>
        protected override bool OnCheckPerception() => SubPerceptions.Count > 0 && SubPerceptions.Any(p => p.Check());
    }
}