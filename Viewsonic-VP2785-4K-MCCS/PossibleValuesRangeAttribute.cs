using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PossibleValuesRangeAttribute : PossibleValuesAttribute
    {
        public uint From { get; private set; }
        public uint To { get; private set; }


        public PossibleValuesRangeAttribute(uint from, uint to)
        {
            From = from;
            To = to;
        }


        public override bool TryParseValue(YamlNode node, out uint value)
        {
            if (node.NodeType != YamlNodeType.Scalar)
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            if (!uint.TryParse(((YamlScalarNode)node).Value, out value))
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            if (value < From || value > To)
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            return true;
        }
    }
}
