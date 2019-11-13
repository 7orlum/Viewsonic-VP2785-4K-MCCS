using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Threading.Tasks;
using System.Linq;


namespace Viewsonic_VP2785_4K_MCCS
{
    class Program
    {
        static void Main(string[] args)
        {
            SetMode(configWork);
            //CompareSettings();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        static void SetMode(string config)
        {
            try
            {
                //var monitor1 = GetMonitorsAndFeatures()[0];

                var settings = ReadConfig(config);
                UpdateSettings(GetMonitors()[0].Handle, settings);

                //var monitor2 = GetMonitorsAndFeatures()[0];

                //foreach (var difference in GetDifference(monitor1.Capabilities, monitor2.Capabilities))
                //    Console.WriteLine($"{difference.Key:X2} old {difference.Value.Item1} new {difference.Value.Item2}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        static void CompareSettings()
        {
            var monitor1 = GetMonitorsAndFeatures()[0];
            var monitor2 = GetMonitorsAndFeatures()[0];

            foreach (var difference in GetDifference(monitor1.Capabilities, monitor2.Capabilities))
                Console.WriteLine($"{difference.Key:X2} old {difference.Value.Item1} new {difference.Value.Item2}");
        }


        static Dictionary<Feature, uint> ReadConfig(string config)
        {
            var result = new Dictionary<Feature, uint>();

            var yaml = new YamlStream();

            using (var stringReader = new StringReader(config))
            {
                yaml.Load(new StringReader(config));
            }

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            foreach (var entry in mapping.Children)
            {
                Feature feature;
                uint value;
                if (Features.TryParse(entry.Key, entry.Value, out feature, out value))
                    result.Add(feature, value);
                else
                    Console.WriteLine($"Неверная команда {entry.Key}");
            }

            return result;
        }


        const string configTest = @"
            #PIPVideoInput: HDMI1, HDMI2, TypeC
            PIPVideoInput: HDMI1
            ";


        const string configWork = @"
            #PresenceSensor: Off, Level1, Level2, Level3
            PresenceSensor: Level3

            #PIPPosition: [X, Y], X must be beetween 0 and 100 where 0 is the left of the screen, 100 is the right of the screen, Y must be beetween 0 and 100 where 0 is the bottom of the screen, 100 is the top of the screen
            #PIPPosition: [95, 15]

            #PIPSize: value beetween 0 and 10 where 0 is the minimal size, 10 is the maximal size
            #PIPSize: 0

            #VideoInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC
            VideoInput: HDMI2

            #MultiPicture: Off, PIP, PBPLeftRight, PBPTopBottom, QuadWindows
            MultiPicture: Off

            #AudioInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC, Auto
            #AudioInput: HDMI1

            #Volume: value beetween 0 and 100 where 0 is the minimal volume, 10 is the maximal volume
            Volume: 25

            #StandardColor: Adobe
            StandardColor: Adobe
            ";


        const string configPIP = @"
            #PresenceSensor: Off, Level1, Level2, Level3
            PresenceSensor: Level3

            #PIPPosition: [X, Y], X must be beetween 0 and 100 where 0 is the left of the screen, 100 is the right of the screen, Y must be beetween 0 and 100 where 0 is the bottom of the screen, 100 is the top of the screen
            PIPPosition: [95, 15]

            #PIPSize: value beetween 0 and 10 where 0 is the minimal size, 10 is the maximal size
            PIPSize: 0

            #VideoInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC
            VideoInput: HDMI2

            #MultiPicture: Off, PIP, PBPLeftRight, PBPTopBottom, QuadWindows
            MultiPicture: PIP

            #AudioInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC, Auto
            AudioInput: HDMI1

            #Volume: value beetween 0 and 100 where 0 is the minimal volume, 10 is the maximal volume
            Volume: 100

            #StandardColor: Adobe
            StandardColor: Adobe

            #Brightness: value beetween 0 and 100 where 0 is the minimal brightness, 10 is the maximal brightness
            Brightness: 33
            ";


        const string configChrome = @"
            #PresenceSensor: Off, Level1, Level2, Level3
            PresenceSensor: Off

            #PIPPosition: [X, Y], X must be beetween 0 and 100 where 0 is the left of the screen, 100 is the right of the screen, Y must be beetween 0 and 100 where 0 is the bottom of the screen, 100 is the top of the screen
            #PIPPosition: [95, 15]

            #PIPSize: value beetween 0 and 10 where 0 is the minimal size, 10 is the maximal size
            #PIPSize: 0

            #VideoInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC
            VideoInput: HDMI1

            #MultiPicture: Off, PIP, PBPLeftRight, PBPTopBottom, QuadWindows
            MultiPicture: Off

            #AudioInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC, Auto
            #AudioInput: HDMI1

            #Volume: value beetween 0 and 100 where 0 is the minimal volume, 10 is the maximal volume
            Volume: 100

            #DisplayApplication : Movie
            DisplayApplication : Movie
            ";


        const string configMovie = @"
            #PresenceSensor: Off, Level1, Level2, Level3
            PresenceSensor: Off

            #PIPPosition: [X, Y], X must be beetween 0 and 100 where 0 is the left of the screen, 100 is the right of the screen, Y must be beetween 0 and 100 where 0 is the bottom of the screen, 100 is the top of the screen
            #PIPPosition: [95, 15]

            #PIPSize: value beetween 0 and 10 where 0 is the minimal size, 10 is the maximal size
            #PIPSize: 0

            #VideoInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC
            VideoInput: HDMI2

            #MultiPicture: Off, PIP, PBPLeftRight, PBPTopBottom, QuadWindows
            MultiPicture: Off

            #AudioInput: HDMI1, HDMI2, DisplayPort, MiniDisplayPort, TypeC, Auto
            #AudioInput: HDMI1

            #Volume: value beetween 0 and 100 where 0 is the minimal volume, 10 is the maximal volume
            Volume: 100

            #StandardColor: Adobe
            StandardColor: Adobe

            #DisplayApplication : Movie
            DisplayApplication : Movie
            ";


        static void UpdateSettings(SafePhysicalMonitorHandle handle, Dictionary<Feature, uint> settings)
        {
            foreach (var setting in settings)
            {
                Feature feature = setting.Key;
                uint currentValue = default;
                uint newValue = setting.Value;

                try
                {
                    if (!NativeMethods.GetVCPFeatureAndVCPFeatureReply(handle, feature.Code, out _, out currentValue, out _))
                        throw new InvalidOperationException($"{nameof(NativeMethods.GetVCPFeatureAndVCPFeatureReply)} returned error {Marshal.GetLastWin32Error()}");

                    if (newValue == currentValue)
                        continue;
                }
                catch
                {
                    Console.WriteLine($"GetVCPFeature fault for {feature.Name}");
                }

                Console.WriteLine($"Update {feature.Name} {feature.ValueName(currentValue)}->{feature.ValueName(newValue)} ({feature.Code:X2} {currentValue}->{newValue})");

                if (!NativeMethods.SetVCPFeature(handle, feature.Code, newValue))
                    throw new InvalidOperationException($"{nameof(NativeMethods.SetVCPFeature)} returned error {Marshal.GetLastWin32Error()}");

                Task.Delay(feature.Delay).Wait();
            }
        }


        static Dictionary<byte, (uint, uint)> GetDifference(Capabilities a, Capabilities b)
        {
            var result = new Dictionary<byte, (uint, uint)>();

            foreach (var vcpCode in a.VCPCodes.Keys)
            {
                if (a.VCPCodes[vcpCode].CurrentValue != b.VCPCodes[vcpCode].CurrentValue)
                    result.Add(vcpCode, (a.VCPCodes[vcpCode].CurrentValue, b.VCPCodes[vcpCode].CurrentValue));
            }

            return result;
        }


        const string capabilityString = @"(prot(monitor)type(LCD)model(VP2785 series)cmds(01 02 03 07 0C E3 F3)vcp(02 04 05 08 0B 0C 10 12 14(01 02 04 05 06 08 0B 0E 0F 10 11 12 13 15 16 17 18) 16 18 1A 1D(F1 15 0F 11 12 17) 21(01 02 03 04 05) 23(01 02 03) 25(01 02 03) 27(01 02) 2B(01 02) 2D(01 02) 2F(01 02) 31(01 02) 33(01 02) 52 59 5A 5B 5C 5D 5E 60(15 0F 11 12 17) 62 66(01 02) 67(00 01 02 03) 68(01 02 03 04) 6C 6E 70 72(00 78 FB 50 64 78 8C A0) 87 8D 96 97 9B 9C 9D 9E 9F A0 AA AC AE B6 C0 C6 C8 C9 CA(01 02 03 04 05) CC(01 02 03 04 05 06 07 09 0A 0B 0C 0D 12 16) D6(01 04 05) DA(00 02) DB(01 02 03 05 06) DC(00 03 04 30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E) DF E1(00 19 32 4B 64) E2 E3(00 01 02) E4(01 02) E5(01 02) E7(01 02) E8(01 02 03 04 05) E9(01 02) EA EB EC ED(01 02) EF(01 02) F3(00 01 02 03))mswhql(1)asset_eep(40)mccs_ver(2.2))";


        public class Monitor
        {
            public string Name;
            public Capabilities Capabilities;
            public SafePhysicalMonitorHandle Handle;
        }


        public class Capabilities
        {
            public string ProtocolClass;
            public string DysplayType;
            public List<byte> Commands;
            public Dictionary<byte, VCPCode> VCPCodes;
            public string DysplayModel;
            public string SupportedMCCSVersion;
        }


        public class VCPCode
        {
            public NativeMethods.MC_VCP_CODE_TYPE Type;
            public string Access;
            public uint CurrentValue;
            public uint MaximumValue;
            public List<uint> PossibleValues;
            public string Description;
        }


        static Capabilities ParseCapabilitiesString(string capabilitiesString)
        {
            var result = new Capabilities();

            var match = new Regex(@"
				\(
					(
						(?<parameter>[^()]+)
						\(
						(?<value>(?>[^()]+|\((?<Depth>)|\)(?<-Depth>))*(?(Depth)(?!)))
		                \)
	                )*
	            \)
	            ",
                RegexOptions.IgnorePatternWhitespace).Match(capabilitiesString);

            if (!match.Success)
                return result;

            for(var i = 0; i < match.Groups["parameter"].Captures.Count; i++)
                switch (match.Groups["parameter"].Captures[i].Value)
                {
                    case "prot": 
                        result.ProtocolClass = match.Groups["value"].Captures[i].Value;
                        break;
                    case "type":
                        result.DysplayType = match.Groups["value"].Captures[i].Value;
                        break;
                    case "model":
                        result.DysplayModel = match.Groups["value"].Captures[i].Value;
                        break;
                    case "mccs_ver":
                        result.SupportedMCCSVersion = match.Groups["value"].Captures[i].Value;
                        break;
                    case "cmds":
                        result.Commands = ParseCommands(match.Groups["value"].Captures[i].Value);
                        break;
                    case "vcp":
                        result.VCPCodes = ParseVCPCodes(match.Groups["value"].Captures[i].Value);
                        break;
                }

            return result;
        }


        static List<byte> ParseCommands(string commandsString)
        {
            var result = new List<byte>();

            var match = new Regex(@"((?<code>[0123456789ABCDEF]{2,2})\s*)*").Match(commandsString);

            if (match.Success)
            for(var i = 0; i < match.Groups["code"].Captures.Count; i++)
                    result.Add(byte.Parse(match.Groups["code"].Captures[i].Value, NumberStyles.HexNumber));

            return result;
        }


        static Dictionary<byte, VCPCode> ParseVCPCodes(string vcpCodesString)
        {
            var result = new Dictionary<byte, VCPCode>();

            var match = new Regex(@"((?<code>[0-9a-fA-F]{2})(?<values>(\(.*?\))?)\s*)*").Match(vcpCodesString);

            if (match.Success)
                for (var i = 0; i < match.Groups["code"].Captures.Count; i++)
                {
                    var vcpCode = new VCPCode();
                    vcpCode.PossibleValues = ParseVCPCodeValues(match.Groups["values"].Captures[i].Value);

                    result.Add(byte.Parse(match.Groups["code"].Captures[i].Value, NumberStyles.HexNumber), vcpCode);
                }

            return result;
        }


        static List<uint> ParseVCPCodeValues(string vcpCodeValuesString)
        {
            var result = new List<uint>();

            var match = new Regex(@"\(((?<code>[0123456789ABCDEF]{2,2})\s*)*\)").Match(vcpCodeValuesString);

            if (match.Success)
                for (var i = 0; i < match.Groups["code"].Captures.Count; i++)
                    result.Add(uint.Parse(match.Groups["code"].Captures[i].Value, NumberStyles.HexNumber));

            return result;
        }


        static void EnumMonitors2()
        {
            var windowHandler = NativeMethods.GetDesktopWindow();
            var monitorHandler = NativeMethods.MonitorFromWindow(windowHandler, NativeMethods.MONITOR_DEFAULT.MONITOR_DEFAULTTOPRIMARY);

            foreach (var physicalMonitor in MonitorConfiguration.EnumeratePhysicalMonitors(monitorHandler, true))
            { 
            }
        }


        static List<Monitor> GetMonitors()
        {
            var result = new List<Monitor>();

            var windowHandler = NativeMethods.GetDesktopWindow();
            var monitorHandler = NativeMethods.MonitorFromWindow(windowHandler, NativeMethods.MONITOR_DEFAULT.MONITOR_DEFAULTTOPRIMARY);

            uint physicalMonitorCount;
            if (!NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(monitorHandler, out physicalMonitorCount))
                throw new InvalidOperationException($"{nameof(NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR)} returned error {Error.GetMessage()}");

            var physicalMonitors = new NativeMethods.PHYSICAL_MONITOR[physicalMonitorCount];
            if (!NativeMethods.GetPhysicalMonitorsFromHMONITOR(monitorHandler, physicalMonitorCount, physicalMonitors))
                throw new InvalidOperationException($"{nameof(NativeMethods.GetPhysicalMonitorsFromHMONITOR)} returned error {Marshal.GetLastWin32Error()}");

            foreach (var physicalMonitor in physicalMonitors)
                result.Add(new Monitor
                {
                    Name = physicalMonitor.szPhysicalMonitorDescription,
                    Handle = new SafePhysicalMonitorHandle(physicalMonitor.hPhysicalMonitor),
                });

            return result;
        }


        static List<Monitor> GetMonitorsAndFeatures()
        {
            var result = new List<Monitor>();

            foreach (var monitor in GetMonitors())
            {
                uint length;
                if (!NativeMethods.GetCapabilitiesStringLength(monitor.Handle, out length))
                    throw new InvalidOperationException($"{nameof(NativeMethods.GetCapabilitiesStringLength)} returned error {Marshal.GetLastWin32Error()}");

                var capabilitiesString = new StringBuilder((int)length);
                if (!NativeMethods.CapabilitiesRequestAndCapabilitiesReply(monitor.Handle, capabilitiesString, length))
                    throw new InvalidOperationException($"{nameof(NativeMethods.CapabilitiesRequestAndCapabilitiesReply)} returned error {Marshal.GetLastWin32Error()}");

                monitor.Capabilities = ParseCapabilitiesString(capabilitiesString.ToString());
                GetVCPFeatures(monitor.Handle, monitor.Capabilities);
                AddHiddenFeatures(monitor);

                result.Add(monitor);
            }

            return result;
        }


        static void AddHiddenFeatures(Monitor monitor)
        {
            var сapabilities = monitor.Capabilities;

            var candidates = Enumerable
                .Range(1, 255)
                .Select(x => (byte)x)
                .Where(x => !сapabilities.Commands.Contains(x) && !сapabilities.VCPCodes.Keys.Contains(x));

            foreach (var candidate in candidates)
            {
                NativeMethods.MC_VCP_CODE_TYPE type;
                uint value;
                uint maxValue;
                if (NativeMethods.GetVCPFeatureAndVCPFeatureReply(monitor.Handle, candidate, out type, out value, out maxValue))
                {
                    //Console.WriteLine($"{candidate:X2} {type} {value} {maxValue}");
                    
                    сapabilities.VCPCodes.Add(candidate, new VCPCode
                    {
                        Type = type,
                        MaximumValue = maxValue,
                        CurrentValue = value,
                    });
                }
            }
        }


        static void GetVCPFeatures(SafePhysicalMonitorHandle handle, Capabilities capabilities)
        {
            foreach (var vcpCode in capabilities.VCPCodes)
            {
                byte code = vcpCode.Key;
                NativeMethods.MC_VCP_CODE_TYPE type;
                uint value;
                uint maxValue;
                if (!NativeMethods.GetVCPFeatureAndVCPFeatureReply(handle, code, out type, out value, out maxValue))
                    throw new InvalidOperationException($"{nameof(NativeMethods.GetVCPFeatureAndVCPFeatureReply)} returned error {Marshal.GetLastWin32Error()}");

                //Console.WriteLine($"{vcpCode.Key:X2} {type} {value} {maxValue}");

                vcpCode.Value.Type = type;
                vcpCode.Value.MaximumValue = maxValue;
                vcpCode.Value.CurrentValue = value;
            }
        }
    }
}
