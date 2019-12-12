A utility written in pure C# to control Viewsonic VP2785-4K monitors. It uses DDC/CI channel to send VESA Monitor Control Commands (MCCS) aka VCP codes. Run the program with a single parameter that references the YAML configuration file.

For example, to switch the monitor to HDMI1 video input and 30% brightness, write in config.yaml:

```yaml
VideoInput: HDMI1
Brightness: 30
```

and run the program:

```
Viewsonic-VP2785-4K-MCCS.exe config.yaml
```

The full list of commands supported by the utility is given below:

```yaml
#Brightness: value beetween 0 and 100 where 0 is the minimal brightness, 10 is the maximal brightness
Brightness: 0

#StandardColor: NotAvailable, sRGB, Adobe, EBU, SMPTEC, REC709, DICOMSIM, DCIP3, CAL1, CAL2, CAL3, iPhone, Custom
StandardColor: NotAvailable

#AudioInput: NotAvailable, DisplayPort, HDMI1, HDMI2, MiniDisplayPort, TypeC, Auto
AudioInput: NotAvailable

#LowInputLag: NotAvailable, Off, Advanced, UltraFast
LowInputLag: NotAvailable

#ResponceTime: NotAvailable, Standard, Advanced, UltraFast
ResponceTime: NotAvailable

#VideoInputAutodetect: NotAvailable, Off, On
VideoInputAutodetect: NotAvailable

#VideoInput: NotAvailable, DisplayPort, HDMI1, HDMI2, MiniDisplayPort, TypeC
VideoInput: NotAvailable

#Volume: value beetween 0 and 100 where 0 is the minimal volume, 10 is the maximal volume
Volume: 0

#AmbientLightSensor: NotAvailable, On, Off
AmbientLightSensor: NotAvailable

#PresenceSensor: Off, Level1, Level2, Level3
PresenceSensor: Off

#AudioMute: NotAvailable, Mute, Unmute
AudioMute: NotAvailable

#PIPPosition: [X, Y], X must be beetween 0 and 100 where 0 is the left of the screen, 100 is the right of the screen, Y must be beetween 0 and 100 where 0 is the bottom of the screen, 100 is the top of the screen
PIPPosition: [0, 0]

#PIPSize: value beetween 0 and 10 where 0 is the minimal size, 10 is the maximal size
PIPSize: 0

#DisplayApplication: Off, Movie, FPS1, FPS2, RTS, MODA, Web, Text, MAC, CADCAM, Animation, VideoEdit, Retro, Photo, Landscape, Portrait, Monochrome
DisplayApplication: Off

#MultiPicture: NotAvailable, Off, PIP, PBPLeftRight, PBPTopBottom, QuadWindows
MultiPicture: NotAvailable

#Uniformity: NotAvailable, Off, On
Uniformity: NotAvailable
```