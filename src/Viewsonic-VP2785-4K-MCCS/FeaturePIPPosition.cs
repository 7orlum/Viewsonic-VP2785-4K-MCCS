using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class FeaturePIPPosition : Feature
    {
        private const uint from = 0;
        private const uint to = 100;
        private const uint k = 256;


        public FeaturePIPPosition(string name, byte code, float delaySeconds = 0) : base(name, code, delaySeconds)
        {
            Description = "PIPPosition: [X, Y], X must be beetween 0 and 100 where 0 is the left of the screen, 100 is the right of the screen, Y must be beetween 0 and 100 where 0 is the bottom of the screen, 100 is the top of the screen";
        }


        public override bool TryParseValue(YamlNode node, out uint value)
        {
            if (node.NodeType == YamlNodeType.Sequence)
            {
                var children = ((YamlSequenceNode)node).Children;
                
                if (children.Count == 2 && children[0].NodeType == YamlNodeType.Scalar && children[1].NodeType == YamlNodeType.Scalar)
                    if (uint.TryParse(((YamlScalarNode)children[0]).Value, out var x) && uint.TryParse(((YamlScalarNode)children[1]).Value, out var y))
                        if (x >= from && x <= to && y >= from && y <= to)
                        {
                            value = x * k + y;
                            return true;
                        }
            }

            value = default;
            return false;
        }


        public override string ValueName(uint value)
        {
            return $"[{value / k}, {value % k}]";
        }


        public override string YamlConfigTemplate()
        {
            return $"#{Description}\r\n{Name}: [{from}, {from}]";
        }
    }
}

