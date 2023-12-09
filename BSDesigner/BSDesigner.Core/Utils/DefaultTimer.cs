using System.Diagnostics.CodeAnalysis;
using System.Timers;

namespace BSDesigner.Core.Utils
{
    [ExcludeFromCodeCoverage]
    public class DefaultTimer : ITimer
    {
        private Timer? _timer;

        public void Start(float timeInSeconds)
        {
            _timer = new Timer(timeInSeconds);
            _timer.Elapsed += (obj,args) => IsTimeout = true;
            _timer.Interval = timeInSeconds * 1000;
            _timer.Enabled = true;
            _timer.Start();
        }

        public void Stop()
        {
            IsTimeout = false;
            _timer?.Dispose();
            _timer = null;
        }

        public void Pause()
        {
            _timer?.Stop();
        }

        public void Resume()
        {
            _timer?.Start();
        }

        public bool IsTimeout { get; private set; }
    }
}