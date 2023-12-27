using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core.Perceptions
{
    /// <summary>
    /// Compound perception that returns true if all its sub perceptions return true.
    /// </summary>
    public class AndPerception : CompoundPerception
    {
        public AndPerception()
        {
        }

        public AndPerception(List<PerceptionTask> subPerceptions) : base(subPerceptions)
        {
        }

        public AndPerception(params PerceptionTask[] subPerceptions) : base(subPerceptions)
        {
        }

        public override string GetInfo() => $"({string.Join(" && ", SubPerceptions.Select(p => p.GetInfo()))})";

        /// <summary>
        /// Perform an AND operation with the results of the sub perception. If there is no sub perception returns false.
        /// </summary>
        /// <returns>True if all its sub perceptions return true, false otherwise.</returns>
        protected override bool OnCheckPerception() => SubPerceptions.Count > 0 && SubPerceptions.All(p => p.Check());
    }
}