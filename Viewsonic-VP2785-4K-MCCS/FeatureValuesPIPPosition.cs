using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class FeatureValuesPIPPosition : Feature
    {
        private const uint from = 0;
        private const uint to = 100;
        private const uint k = 256;


        public FeatureValuesPIPPosition(string name, byte code, float delaySeconds = 0) : base(name, code, delaySeconds) { }


        public override bool TryParseValue(YamlNode node, out uint value)
        {
            if (node.NodeType == YamlNodeType.Sequence)
            {
                var children = ((YamlSequenceNode)node).Children;
                
                if (children.Count == 2 && children[0].NodeType == YamlNodeType.Scalar && children[1].NodeType == YamlNodeType.Scalar)
                {
                    uint x;
                    uint y;
                    
                    if (uint.TryParse(((YamlScalarNode)children[0]).Value, out x) && uint.TryParse(((YamlScalarNode)children[1]).Value, out y))
                        if (x >= from && x <= to && y >= from && y <= to)
                        {
                            value = x * k + y;
                            return true;
                        }
                }
            }

            value = default;
            return false;
        }


        public override string ValueName(uint value)
        {
            return $"[{value / k}, {value % k}]";
        }
    }
}

