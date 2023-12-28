using System.Collections;
using System.Collections.Generic;
using BSDesigner.Core;
using UnityEngine;
using ILogger = BSDesigner.Core.ILogger;

namespace BSDesigner.Unity.Runtime
{
    public class UnityLogger : ILogger
    {
        private readonly GameObject _objectReference;

        public UnityLogger(GameObject objectReference)
        {
            _objectReference = objectReference;
        }

        public void LogMessage(string message, LogLevel logLevel = LogLevel.Info)
        {
            throw new System.NotImplementedException();
        }

        public void LogFormatMessage(string format, LogLevel logLevel = LogLevel.Info, params object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}
