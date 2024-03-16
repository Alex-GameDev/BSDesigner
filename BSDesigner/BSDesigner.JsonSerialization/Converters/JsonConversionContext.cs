using System.Collections.Generic;
using BSDesigner.Core;

namespace BSDesigner.JsonSerialization.Converters
{
    public class JsonConversionContext
    {
        /// <summary>
        /// List of all engines in the system.
        /// </summary>
        private readonly List<BehaviourEngine> engines = new List<BehaviourEngine>();

        /// <summary>
        /// Blackboard that can be acces by all engines in the system.
        /// </summary>
        public Blackboard? GlobalBlackboard { get; set; }

        /// <summary>
        /// Blackboard that can be access only by the specified behaviour engine.
        /// </summary>
        public Blackboard? LocalBlackboard { get; set; }


        private Dictionary<Parameter<BehaviourEngine>, int> subsystemMap;

        private Dictionary<Parameter, BlackboardField> blackboardFieldMap;

        public int RegisterEngine(BehaviourEngine? value)
        {
            if (value == null) return -1;

            var id = engines.IndexOf(value);
            if (id != -1)
            {
                return id;
            }
            else
            {
                engines.Add(value);
                return engines.Count - 1;
            }
        }
    }
}