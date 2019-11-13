using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PossibleValuesEnumAttribute : PossibleValuesAttribute
    {
        public Type Type { get; private set; }


        public PossibleValuesEnumAttribute(Type type)
        {
            if (!typeof(Enum).IsAssignableFrom(type))
                throw new ArgumentException("Argument type must be enumeration", nameof(type));
            
            Type = type;
        }


        public override bool TryParseValue(YamlNode node, out uint value)
        {
            if (node.NodeType != YamlNodeType.Scalar)
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            try
            {
                value = (uint)Enum.Parse(Type, ((YamlScalarNode)node).Value, true);
            }
            catch
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            if (!Enum.IsDefined(Type, value))
            {
                Console.WriteLine($"Неверный параметр {node}");
                value = default;
                return false;
            }

            return true;
        }
    }
}
