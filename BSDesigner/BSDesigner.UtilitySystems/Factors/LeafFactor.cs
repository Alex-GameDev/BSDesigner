namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Factor that does not compute its value from other factors.
    /// </summary>
    public abstract class LeafFactor : Factor
    {
        public override int MaxOutputConnections => 0;
    }
}