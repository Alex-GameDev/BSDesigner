namespace BSDesigner.Core
{
    public struct Subsystem
    {
        public BehaviourEngine? Engine;

        public Subsystem(BehaviourEngine? engine)
        {
            Engine = engine;
        }

        public static implicit operator Subsystem(BehaviourEngine engine) => new Subsystem(engine);
        public static implicit operator BehaviourEngine?(Subsystem subsystem) => subsystem.Engine;
    }
}