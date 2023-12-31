using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace BSDesigner.JsonSerialization
{
    /// <summary>
    /// The default contract resolver to serialize and deserialize behaviour systems
    /// </summary>
    public class BSDContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type type)
        {
            var members = new List<MemberInfo>();
            members.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Instance).ToList());
            return members;
        }
    }
}