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

        protected override IEnumerable<Type> GetTargetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("BSDesigner"))
                .SelectMany(a => a.GetTypes());
        }   
    }
}
