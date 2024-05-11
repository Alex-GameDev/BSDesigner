using BSDesigner.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSDesigner.BehaviourTrees
{
    /// <summary>
    /// A collection of behaviour engines that works together to define a behaviour
    /// </summary>
    public class BehaviourSystem
    {
        public List<BehaviourEngine> Engines;
    }
}
