using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class Monitor
    {
        public string Name { get; set; }
        public Capabilities Capabilities { get; set; }
        public SafePhysicalMonitorHandle Handle { get; set; }


        public static List<Monitor> GetMonitors()
        {
            var result = new List<Monitor>();

            var windowHandler = NativeMethods.GetDesktopWindow();
            var monitorHandler = NativeMethods.MonitorFromWindow(windowHandler, NativeMethods.MONITOR_DEFAULT.MONITOR_DEFAULTTOPRIMARY);

            uint physicalMonitorCount;
            if (!NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(monitorHandler, out physicalMonitorCount))
                throw new InvalidOperationException($"{nameof(NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR)} returned error {Marshal.GetLastWin32Error()}");

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


        public static List<Monitor> GetMonitorsAndFeatures()
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

                monitor.Capabilities = Capabilities.Parse(capabilitiesString.ToString());
                GetVCPFeatures(monitor.Handle, monitor.Capabilities);
                AddHiddenFeatures(monitor);

                result.Add(monitor);
            }

            return result;
        }


        private static void GetVCPFeatures(SafePhysicalMonitorHandle handle, Capabilities capabilities)
        {
            foreach (var vcpCode in capabilities.VCPCodes)
            {
                if (!NativeMethods.GetVCPFeatureAndVCPFeatureReply(handle, vcpCode.Key, out var type, out var value, out var maxValue))
                    throw new InvalidOperationException($"{nameof(NativeMethods.GetVCPFeatureAndVCPFeatureReply)} returned error {Marshal.GetLastWin32Error()}");

                vcpCode.Value.Type = type;
                vcpCode.Value.MaximumValue = maxValue;
                vcpCode.Value.CurrentValue = value;
            }
        }


        private static void AddHiddenFeatures(Monitor monitor)
        {
            var сapabilities = monitor.Capabilities;

            var candidates = Enumerable
                .Range(1, 255)
                .Select(x => (byte)x)
                .Where(x => !сapabilities.Commands.Contains(x) && !сapabilities.VCPCodes.Keys.Contains(x));

            foreach (var candidate in candidates)
                if (NativeMethods.GetVCPFeatureAndVCPFeatureReply(monitor.Handle, candidate, out var type, out var value, out var maxValue))
                    сapabilities.VCPCodes.Add(candidate, new Capabilities.VCPCode
                    {
                        Type = type,
                        MaximumValue = maxValue,
                        CurrentValue = value,
                    });
        }
    }
}
