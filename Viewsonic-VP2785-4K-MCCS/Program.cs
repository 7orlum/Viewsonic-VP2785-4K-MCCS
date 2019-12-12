using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Threading.Tasks;


namespace Viewsonic_VP2785_4K_MCCS
{
    class Program
    {
        static readonly string _help = $@"Changes settings of Viewsonic VP2785-4K monitor, be sure that DDC/CI is turned on in the monitor menu

Viewsonic_VP2785_4K_MCCS configPath
    configPath  path to a configuration file in YAML format, possible settings are:

{Features.YamlConfigTemplate()}";


        static void Main(string[] args)
        {
            if (args != null && args.Length == 1)
                DoWork(args[0]);
            else
                Console.WriteLine(_help);
        }


        private static void DoWork(string path)
        {
            using var streamReader = new StreamReader(path);
            var settings = ReadConfig(streamReader);

            foreach (var monitor in Monitor.GetMonitors())
                UpdateSettings(monitor.Handle, settings);
        }


        static Dictionary<Feature, uint> ReadConfig(TextReader textReader)
        {
            var result = new Dictionary<Feature, uint>();

            var yaml = new YamlStream();
            yaml.Load(textReader);

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            foreach (var entry in mapping.Children)
                if (Features.TryParse(entry.Key, entry.Value, out var feature, out var value))
                    result.Add(feature, value);
                else
                    Console.WriteLine($"Wrong command {entry.Key}, possible commands:\r\n{Features.YamlConfigTemplate()}");

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


        static void CompareSettings()
        {
            var monitor1 = Monitor.GetMonitorsAndFeatures()[0];
            Console.ReadKey(true);
            var monitor2 = Monitor.GetMonitorsAndFeatures()[0];

            foreach (var difference in GetDifference(monitor1.Capabilities, monitor2.Capabilities))
                Console.WriteLine($"{difference.Key:X2} old {difference.Value.Item1} new {difference.Value.Item2}");
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
    }
}
