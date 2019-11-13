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
        static string _help = @"Help";


        static void Main(string[] args)
        {
            if (args != null && args.Length == 1)
                DoWork(args[0]);
            else
                Console.WriteLine(_help);
        }


        private static void DoWork(string path)
        {
            Dictionary<Feature, uint> settings;
            
            using (var streamReader = new StreamReader(path))
            {
                settings = ReadConfig(streamReader);
            }

            UpdateSettings(GetMonitors()[0].Handle, settings);
        }


        static Dictionary<Feature, uint> ReadConfig(TextReader textReader)
        {
            var result = new Dictionary<Feature, uint>();

            var yaml = new YamlStream();
            yaml.Load(textReader);

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
