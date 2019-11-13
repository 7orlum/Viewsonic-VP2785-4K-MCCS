using System;
using System.Linq;
using System.Reflection;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public enum Feature : byte
    {
        [PossibleValuesRange(0, 100)]
        Brightness = 0x10,

        [PossibleValuesEnum(typeof(StandardColor))]
        StandardColor = 0x14,
        
        [PossibleValuesEnum(typeof(AudioInput))]
        AudioInput = 0x1D,
        
        [PossibleValuesEnum(typeof(LowInputLag))]
        LowInputLag = 0x23,
        
        [PossibleValuesEnum(typeof(ResponceTime))]
        ResponceTime = 0x25,
        
        [PossibleValuesEnum(typeof(VideoInputAutodetect))]
        [DelaySeconds(2)]
        VideoInputAutodetect = 0x33,
        
        [PossibleValuesEnum(typeof(VideoInput))]
        [DelaySeconds(2)]
        VideoInput = 0x60,
        
        [PossibleValuesRange(0, 100)]
        Volume = 0x62,
        
        [PossibleValuesEnum(typeof(AmbientLightSensor))]
        AmbientLightSensor = 0x66,
        
        [PossibleValuesEnum(typeof(PresenceSensor))]
        PresenceSensor = 0x67,
        
        Gamma = 0x72,
        
        [PossibleValuesEnum(typeof(AudioMute))]
        AudioMute = 0x8D,
        
        [PossibleValuesPIPPosition()]
        PIPPosition = 0x96,
        
        [PossibleValuesRange(0, 10)]
        PIPSize = 0x97,

        [PossibleValuesEnum(typeof(DisplayApplication))]
        DisplayApplication = 0xDC,
        
        [PossibleValuesEnum(typeof(MultiPicture))]
        [DelaySeconds(2)]
        MultiPicture = 0xE8,
        
        [PossibleValuesEnum(typeof(Uniformity))]
        Uniformity = 0xE9,
    }
}
