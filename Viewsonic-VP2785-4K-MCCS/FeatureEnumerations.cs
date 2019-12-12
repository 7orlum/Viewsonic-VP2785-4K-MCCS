namespace Viewsonic_VP2785_4K_MCCS
{
    public enum StandardColor : uint
    {
        NotAvailable = 0,
        sRGB = 1,
        Adobe = 14,
        EBU = 15,
        SMPTEC = 16,
        REC709 = 17,
        DICOMSIM = 18,
        DCIP3 = 19,
        CAL1 = 21,
        CAL2 = 22,
        CAL3 = 23,
        iPhone = 24,
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
        Off = 0,
        Movie = 3,
        FPS1 = 8,
        FPS2 = 49,
        RTS = 50,
        MODA = 51,
        Web = 52,
        Text = 53,
        MAC = 54,
        CADCAM = 55,
        Animation = 56,
        VideoEdit = 57,
        Retro = 58,
        Photo = 59,
        Landscape = 60,
        Portrait = 61,
        Monochrome = 62,
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
