using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BSDesigner.Core;
using UnityEditor.VersionControl;
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
            switch (logLevel)
            {
                case LogLevel.Info:
                    Debug.Log(message, _objectReference);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarning(message, _objectReference);
                    break;
                default:
                    Debug.LogError(message, _objectReference);
                    break;
            }
        }

        public void LogFormatMessage(string format, LogLevel logLevel = LogLevel.Info, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Info:
                    Debug.LogFormat(_objectReference, format, args);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarningFormat(_objectReference, format, args);
                    break;
                default:
                    Debug.LogErrorFormat(_objectReference, format, args);
                    break;
            }
        }
    }
}
