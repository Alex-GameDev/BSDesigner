using BSDesigner.Core;

namespace TestBSD.BehaviourTrees.Mocks
{
    public class MockedTimerProvider : ITimerProvider, ITimer
    {
        public float CurrentTime;

        public float TotalTime;

        public ITimer CreateTimer() => this;
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