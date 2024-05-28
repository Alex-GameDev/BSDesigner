using BSDesigner.Core;
using BSDesigner.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class ToolMetadata : APIMetadata
    {
        public static ToolMetadata _instance;

        public static ToolMetadata Instance
        {
            get
            {
                return _instance ??= new ToolMetadata();
            }
        }

        protected override HashSet<Type> GetRequiredRootTypes() =>
            new HashSet<Type>
            {
                typeof(Node),
                typeof(BehaviourEngine),
                typeof(Task)
            };

        protected override IEnumerable<Type> GetTargetAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var validAssemblies = assemblies.Where(a => a.FullName.StartsWith("BSDesigner")).ToList();
            return validAssemblies.SelectMany(a => a.GetTypes());
        }   
    }
}
