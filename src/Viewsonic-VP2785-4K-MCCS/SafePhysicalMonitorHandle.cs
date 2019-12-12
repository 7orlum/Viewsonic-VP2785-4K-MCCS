using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;


namespace Viewsonic_VP2785_4K_MCCS
{
    public class SafePhysicalMonitorHandle : SafeHandleMinusOneIsInvalid
    {
        private SafePhysicalMonitorHandle() : base(true) { }


        public SafePhysicalMonitorHandle(IntPtr handle) : this() => SetHandle(handle);


        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle() => NativeMethods.DestroyPhysicalMonitor(handle);
    }
}
