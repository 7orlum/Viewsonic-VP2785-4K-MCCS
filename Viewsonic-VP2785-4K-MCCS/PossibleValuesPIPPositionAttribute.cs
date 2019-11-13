using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PossibleValuesPIPPositionAttribute : PossibleValuesAttribute
    {
        private const uint from = 0;
        private const uint to = 100;


        public override bool TryParseValue(YamlNode node, out uint value)
        {
            if (node.NodeType != YamlNodeType.Sequence)
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            var children = ((YamlSequenceNode)node).Children;

            if (children.Count != 2 || children[0].NodeType != YamlNodeType.Scalar || children[1].NodeType != YamlNodeType.Scalar)
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            uint x;
            uint y;

            if (!uint.TryParse(((YamlScalarNode)children[0]).Value, out x) || !uint.TryParse(((YamlScalarNode)children[1]).Value, out y))
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            if (x < from || x > to || y <from || y > to)
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            value = x * 256 + y;
            return true;
        }
    }
}
