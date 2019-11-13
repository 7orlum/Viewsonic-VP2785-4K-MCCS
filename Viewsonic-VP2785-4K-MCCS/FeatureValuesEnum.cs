using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class Feature<T>: Feature where T : Enum
    {
        public Feature(string name, byte code, float delaySeconds = 0) : base(name, code, delaySeconds) { }


        public override bool TryParseValue(YamlNode node, out uint value)
        {
            if (node.NodeType == YamlNodeType.Scalar)
            {
                try
                {
                    value = (uint)Enum.Parse(typeof(T), ((YamlScalarNode)node).Value, true);
                }
                catch
                {
                    value = default;
                    return false;
                }

                if (Enum.IsDefined(typeof(T), value))
                    return true;
            }

            value = default;
            return false;
        }


        public override string ValueName(uint value)
        {
            if (Enum.IsDefined(typeof(T), value))
                return ((T)Enum.ToObject(typeof(T), value)).ToString();
            else
                return value.ToString();
        }
    }
}
