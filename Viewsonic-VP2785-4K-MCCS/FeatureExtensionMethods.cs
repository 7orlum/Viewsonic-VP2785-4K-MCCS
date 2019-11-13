using System;
using System.Linq;
using System.Reflection;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public static class FeatureExtensionMethods
    {
        public static bool TryParseValue(this Feature feature, YamlNode node, out uint value)
        {
            var attributeValue = feature.GetAttribute<PossibleValuesAttribute>();

            if (attributeValue == null)
                throw new ApplicationException($"PossibleValuesAttribute isn't defined for {feature}");

            return attributeValue.TryParseValue(node, out value);
        }


        public static TimeSpan Delay(this Feature command)
        {
            return command.GetAttribute<DelaySecondsAttribute>()?.Delay ?? TimeSpan.Zero;
        }


        private static T GetAttribute<T>(this Enum enumeration) where T : Attribute
        {
            return enumeration
                .GetType()
                .GetMember(enumeration.ToString())
                .Where(member => member.MemberType == MemberTypes.Field)
                .FirstOrDefault()
                .GetCustomAttributes(typeof(T), false)
                .Cast<T>()
                .SingleOrDefault();
        }
    }
}
