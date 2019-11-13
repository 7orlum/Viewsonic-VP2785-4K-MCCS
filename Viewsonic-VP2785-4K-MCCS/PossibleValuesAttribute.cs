using System;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class PossibleValuesAttribute : Attribute
    {
        public abstract bool TryParseValue(YamlNode node, out uint value);
    }
}
