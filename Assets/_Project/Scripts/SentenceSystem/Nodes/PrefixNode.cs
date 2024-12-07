public class PrefixNode : Node, IPrefix
{
    public override void SetName()
    {
        base.SetName();
        gameObject.name += "_Prefix";
    }
}
