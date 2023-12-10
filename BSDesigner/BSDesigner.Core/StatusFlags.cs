using System;

namespace BSDesigner.Core
{
    [Flags]
    public enum StatusFlags
    {
        None = 0,
        Running = 1,
        Success = 2,
        Failure = 4,
        NotSuccess = Running | Failure,
        NotFailure = Running | Success,
        Finished = Success | Failure,
        Active = Running | Success | Failure,
    }
}