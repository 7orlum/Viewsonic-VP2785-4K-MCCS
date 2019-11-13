using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class FeatureValuesRange : Feature
    {
        public uint From { get; private set; }
        public uint To { get; private set; }


        public FeatureValuesRange(string name, byte code, uint from, uint to, float delaySeconds = 0) : base(name, code, delaySeconds)
        {
            From = from;
            To = to;
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
    }
}
