namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// UtilityFactor that does not compute its value from other factors.
    /// </summary>
    public abstract class UtilityLeaf : UtilityFactor
    {
        public override int MaxOutputConnections => 0;
    }
}