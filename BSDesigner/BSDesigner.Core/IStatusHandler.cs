using System;

namespace BSDesigner.Core
{
    public interface IStatusHandler
    {
        /// <summary> 
        /// Gets the status of the element. 
        /// </summary>
        /// <value> The execution status. </value>

        Status Status { get; }

        /// <summary> 
        /// Event invoked when Status value changed. 
        /// </summary>
        /// <value> The status changed event. </value>

        event Action<Status> StatusChanged;
    }
}