namespace BSDesigner.Core.Utils
{
    public interface ITimer
    {
        public void Start(float timeInSeconds);

        public void Stop();

        public void Pause();

        public void Resume();

        public bool IsTimeout { get; }
    }
}