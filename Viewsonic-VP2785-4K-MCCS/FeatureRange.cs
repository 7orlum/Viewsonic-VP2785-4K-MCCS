using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class FeatureRange : Feature
    {
        public uint From { get; private set; }
        public uint To { get; private set; }


        public FeatureRange(string name, byte code, uint from, uint to, float delaySeconds = 0, string description = null) : base(name, code, delaySeconds)
        {
            From = from;
            To = to;
            Description = description ?? $"{Name}: value beetween {From} and {To}";
        }


        public override bool TryParseValue(YamlNode node, out uint value)
        {
            if (node.NodeType == YamlNodeType.Scalar )
                if (uint.TryParse(((YamlScalarNode)node).Value, out value))
                    if (value >= From || value <= To)
                        return true;

            value = default;
            return false;
        }


        public override string ValueName(uint value)
        {
            return value.ToString();
        }


        public override string YamlConfigTemplate()
        {
            return $"#{Description}\r\n#{Name}: {From}";
        }
    }
}
