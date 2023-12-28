using BSDesigner.Core;
using UnityEngine;
using ILogger = BSDesigner.Core.ILogger;


namespace BSDesigner.Unity.Runtime
{
    public class UnityLoggerFactory : ILoggerProvider
    {
        private readonly GameObject _objectReference;

        public UnityLoggerFactory(GameObject objectReference)
        {
            _objectReference = objectReference;
        }

        public ILogger CreateLogger() => new UnityLogger(_objectReference);
    }
}