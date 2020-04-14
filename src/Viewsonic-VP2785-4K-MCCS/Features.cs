using System.Linq;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    class Features
    {
        private static List<Feature> features = new List<Feature> 
        {
            new FeatureRange("Brightness", 0x10, 0, 100, description: "Brightness: value beetween 0 and 100 where 0 is the minimal brightness, 100 is the maximal brightness"),
            new FeatureEnumeration<StandardColor>("StandardColor", 0x14),
            new FeatureEnumeration<AudioInput>("AudioInput", 0x1D, delaySeconds: 4),
            new FeatureEnumeration<LowInputLag>("LowInputLag", 0x23),
            new FeatureEnumeration<ResponceTime>("ResponceTime", 0x25),
            new FeatureEnumeration<VideoInputAutodetect>("VideoInputAutodetect", 0x33, delaySeconds: 4),
            new FeatureEnumeration<VideoInput>("VideoInput", 0x60, delaySeconds: 4),
            new FeatureRange("Volume", 0x62, 0, 100, description: "Volume: value beetween 0 and 100 where 0 is the minimal volume, 100 is the maximal volume"),
            new FeatureEnumeration<AmbientLightSensor>("AmbientLightSensor", 0x66),
            new FeatureEnumeration<PresenceSensor>("PresenceSensor", 0x67),
            new FeatureEnumeration<AudioMute>("AudioMute", 0x8D),
            new FeaturePIPPosition("PIPPosition", 0x96),
            new FeatureRange("PIPSize", 0x97, 0, 10, description: "PIPSize: value beetween 0 and 10 where 0 is the minimal size, 10 is the maximal size"),
            new FeatureEnumeration<DisplayApplication>("DisplayApplication", 0xDC),
            new FeatureEnumeration<MultiPicture>("MultiPicture", 0xE8, delaySeconds: 4),
            new FeatureEnumeration<Uniformity>("Uniformity", 0xE9),
        };


        public static bool TryParse(YamlNode keyNode, YamlNode valueNode, out Feature feature, out uint value)
        {
            if (keyNode.NodeType == YamlNodeType.Scalar)
                if ((feature = features.Find(_feature => _feature.Name == ((YamlScalarNode)keyNode).Value)) != null)
                    if (feature.TryParseValue(valueNode, out value))
                        return true;

            feature = default;
            value = default;
            return false;
        }


        public static string YamlConfigTemplate()
        {
            return string.Join("\r\n\r\n", features.Select(feature => feature.YamlConfigTemplate()));
        }
    }
}
