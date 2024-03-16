using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace BSDesigner.JsonSerialization.Settings
{
    internal class BSDContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Return all the fields to the given type to be serialized
        /// </summary>
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = new List<MemberInfo>();
            members.AddRange(objectType.GetFields(BindingFlags.Public | BindingFlags.Instance));
            return members.ToList();
        }
    }
}