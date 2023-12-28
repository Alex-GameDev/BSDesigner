using BSDesigner.Core;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Timer that uses unity time.
    /// </summary>
    public class UnityTimer : ITimer
    {
        private float _totalTime;

        private float _currentTime;

        public void Start(float timeInSeconds)
        {
            _currentTime = 0;
            _totalTime = timeInSeconds;
        }

        public void Stop()
        {
            _currentTime = 0;
            _totalTime = 0;
        }

        public void Pause()
        {
        }

        public void Tick()
        {
            _totalTime += UnityEngine.Time.deltaTime;
        }

        public bool IsTimeout => _currentTime > _totalTime;
    }
}