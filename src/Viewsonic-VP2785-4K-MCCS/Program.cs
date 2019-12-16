using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Threading.Tasks;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class Program
    {
        private static readonly string _help = 
$@"Changes settings of Viewsonic VP2785-4K monitor, be sure that DDC/CI is turned on in the monitor menu

Viewsonic_VP2785_4K_MCCS path
Viewsonic_VP2785_4K_MCCS /trace

    path    sends to the monitor commands from the specified YAML configuration file
    /trace  use it to find the correspondence between monitor settings and control codes

The full list of commands is given below, copy the desired commands to your configuration file:

{Features.YamlConfigTemplate()}";


        private static void Main(string[] args)
        {
            if (args != null && args.Length == 1)
                if (args[0] == "/trace")
                    Trace();
                else
                    Configure(args[0]);
            else
                Console.WriteLine(_help);
        }


        private static void Configure(string path)
        {
            var features = ReadYamlConfig(path);

            foreach (var monitor in Monitor.GetMonitors())
                SetMonitorFeatures(monitor.Handle, features);
        }


        private static void Trace()
        {
            Capabilities monitorConfiguration1;
            Capabilities monitorConfiguration2;

            Console.WriteLine("Wait, the monitor configuration is being read");
            monitorConfiguration2 = Monitor.GetMonitorsAndFeatures()[0].Capabilities;

            while (true)
            {
                monitorConfiguration1 = monitorConfiguration2;

                Console.WriteLine("Change the monitor settings and press any key to trace the changes or press Ctrl+C to exit");
                Console.ReadKey(true);

                Console.WriteLine("Wait, the monitor configuration is being read");
                monitorConfiguration2 = Monitor.GetMonitorsAndFeatures()[0].Capabilities;

                var differences = GetDifferences(monitorConfiguration1, monitorConfiguration2);

                if (differences.Count == 0)
                    Console.WriteLine("There are no changes");
                else
                    foreach (var difference in differences)
                        Console.WriteLine($"{difference.Key:X2} {difference.Value.Item1}->{difference.Value.Item2}");
            }
        }


        private static Dictionary<Feature, uint> ReadYamlConfig(string path)
        {
            using var textReader = new StreamReader(path);

            var yaml = new YamlStream();
            yaml.Load(textReader);

            var result = new Dictionary<Feature, uint>();

            foreach (var entry in ((YamlMappingNode)yaml.Documents[0].RootNode).Children)
                if (Features.TryParse(entry.Key, entry.Value, out var feature, out var value))
                    result.Add(feature, value);
                else
                    Console.WriteLine($"Wrong command {entry.Key}, possible commands:\r\n{Features.YamlConfigTemplate()}");

            return result;
        }


        private static void SetMonitorFeatures(SafePhysicalMonitorHandle handle, Dictionary<Feature, uint> settings)
        {
            foreach (var setting in settings)
            {
                var feature = setting.Key;
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


        private static Dictionary<byte, (uint, uint)> GetDifferences(Capabilities a, Capabilities b)
        {
            var result = new Dictionary<byte, (uint, uint)>();

            foreach (var vcpCode in a.VCPCodes.Keys)
                if (a.VCPCodes[vcpCode].CurrentValue != b.VCPCodes[vcpCode].CurrentValue)
                    result.Add(vcpCode, (a.VCPCodes[vcpCode].CurrentValue, b.VCPCodes[vcpCode].CurrentValue));

            return result;
        }
    }
}
