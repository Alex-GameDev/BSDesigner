namespace BSDesigner.Core.Utils
{
    public class MockedTimer : ITimer
    {
        public float CurrentTime;

        public float TotalTime;

        public void Start(float timeInSeconds)
        {
           TotalTime = timeInSeconds;
        }

        public void Stop() { }

        public void Pause() { }

        public void Resume() { }

        public bool IsTimeout => CurrentTime >= TotalTime;
    }
}