using System;
using System.Linq;
using System.Reflection;
using YamlDotNet.RepresentationModel;


namespace Viewsonic_VP2785_4K_MCCS
{
    public enum StandardColor : uint
    {
        NotAvailable = 0,
        Adobe = 14,
        Custom = 255,
    }


    public enum AudioInput : uint
    {
        NotAvailable = 0,
        DisplayPort = 15,
        HDMI1 = 17,
        HDMI2 = 18,
        MiniDisplayPort = 21,
        TypeC = 23,
        Auto = 241,
    }


    public enum LowInputLag : uint
    {
        NotAvailable = 0,
        Off = 1,
        Advanced = 2,
        UltraFast = 3,
    }


    public enum ResponceTime : uint
    {
        NotAvailable = 0,
        Standard = 1,
        Advanced = 2,
        UltraFast = 3,
    }


    public enum VideoInputAutodetect : uint
    {
        NotAvailable = 0,
        Off = 1,
        On = 2,
    }


    public enum VideoInput : uint
    {
        NotAvailable = 0,
        HDMI1 = 17,
        HDMI2 = 18,
        DisplayPort = 15,
        MiniDisplayPort = 21,
        TypeC = 23,
    }


    public enum AmbientLightSensor : uint
    {
        NotAvailable = 0,
        On = 1,
        Off = 2,
    }


    public enum PresenceSensor : uint
    {
        Off = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
    }


    public enum AudioMute : uint
    {
        NotAvailable = 0,
        Mute = 1,
        Unmute = 2,
    }


    public enum DisplayApplication : uint
    {
        Default = 0,
        Movie = 3,
    }


    public enum MultiPicture : uint
    {
        NotAvailable = 0,
        Off = 1,
        PIP = 2,
        PBPLeftRight = 3,
        PBPTopBottom = 4,
        QuadWindows = 5,
    }


    public enum Uniformity : uint
    {
        NotAvailable = 0,
        Off = 1,
        On = 2,
    }
}
