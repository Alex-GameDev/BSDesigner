using Newtonsoft.Json;

namespace BSDesigner.JsonSerialization.Converters
{
    public abstract class ContextConverter<T> : JsonConverter<T> where T : class
    {
        public readonly JsonConversionContext Context;

        protected ContextConverter(JsonConversionContext context)
        {
            Context = context;
        }
    }
}