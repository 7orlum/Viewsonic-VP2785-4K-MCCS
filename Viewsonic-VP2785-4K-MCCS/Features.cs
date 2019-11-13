using System.Collections.Generic;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    class Features
    {
        private static List<Feature> features = new List<Feature> 
        {
            new FeatureValuesRange("Brightness", 0x10, 0, 100),
            new Feature<StandardColor>("StandardColor", 0x14),
            new Feature<AudioInput>("AudioInput", 0x1D),
            new Feature<LowInputLag>("LowInputLag", 0x23),
            new Feature<ResponceTime>("ResponceTime", 0x25),
            new Feature<VideoInputAutodetect>("VideoInputAutodetect", 0x33, delaySeconds: 2),
            new Feature<VideoInput>("VideoInput", 0x60, delaySeconds: 2),
            new FeatureValuesRange("Volume", 0x62, 0, 100),
            new Feature<AmbientLightSensor>("AmbientLightSensor", 0x66),
            new Feature<PresenceSensor>("PresenceSensor", 0x67),
            new Feature<AudioMute>("AudioMute", 0x8D),
            new FeatureValuesPIPPosition("PIPPosition", 0x96),
            new FeatureValuesRange("PIPSize", 0x97, 0, 10),
            new Feature<DisplayApplication>("DisplayApplication", 0xDC),
            new Feature<MultiPicture>("MultiPicture", 0xE8, delaySeconds: 2),
            new Feature<Uniformity>("Uniformity", 0xE9),
        };


        public static bool TryParse(YamlNode keyNode, YamlNode valueNode, out Feature feature, out uint value)
        {
            if (keyNode.NodeType == YamlNodeType.Scalar)
            {
                feature = features.Find(_feature => _feature.Name == ((YamlScalarNode)keyNode).Value);
                if (feature != null)
                    if (feature.TryParseValue(valueNode, out value))
                        return true;
            }

            feature = default;
            value = default;
            return false;
        }
    }
}
