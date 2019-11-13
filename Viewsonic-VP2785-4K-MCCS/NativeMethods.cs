using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;


namespace Viewsonic_VP2785_4K_MCCS
{
    [SuppressUnmanagedCodeSecurity()]
    static class NativeMethods
    {
        /// <summary>
        /// Retrieves a handle to the desktop window. The desktop window covers the entire screen. The desktop window is the area on top of which other windows are painted.
        /// </summary>
        /// <returns>The return value is a handle to the desktop window.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();


        /// <summary>
        /// The MonitorFromWindow function retrieves a handle to the display monitor that has the largest area of intersection with the bounding rectangle of a specified window.
        /// </summary>
        /// <param name="hwnd">A handle to the window of interest.</param>
        /// <param name="dwFlags">Determines the function's return value if the window does not intersect any display monitor.</param>
        /// <returns>If the window intersects one or more display monitor rectangles, the return value is an HMONITOR handle to the display monitor that has the largest area of intersection with the window.
        /// If the window does not intersect a display monitor, the return value depends on the value of dwFlags.</returns>
        /// <remarks>If the window is currently minimized, MonitorFromWindow uses the rectangle of the window before it was minimized.</remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(
            IntPtr hwnd,
            MONITOR_DEFAULT dwFlags);

        
        public enum MONITOR_DEFAULT : UInt32
        {
            /// <summary>
            /// Returns NULL.
            /// </summary>
            MONITOR_DEFAULTTONULL,
            /// <summary>
            /// Returns a handle to the primary display monitor.
            /// </summary>
            MONITOR_DEFAULTTOPRIMARY,
            /// <summary>
            /// Returns a handle to the display monitor that is nearest to the window.
            /// </summary>
            MONITOR_DEFAULTTONEAREST
        }



        /// <summary>
        /// The EnumDisplayMonitors function enumerates display monitors (including invisible pseudo-monitors associated with the mirroring drivers) that intersect a region formed by the intersection of a specified clipping rectangle and the visible region of a device context. EnumDisplayMonitors calls an application-defined MonitorEnumProc callback function once for each monitor that is enumerated. Note that GetSystemMetrics (SM_CMONITORS) counts only the display monitors.
        /// </summary>
        /// <param name="hdc">A handle to a display device context that defines the visible region of interest.
        /// If this parameter is NULL, the hdcMonitor parameter passed to the callback function will be NULL, and the visible region of interest is the virtual screen that encompasses all the displays on the desktop.</param>
        /// <param name="lprcClip">A pointer to a RECT structure that specifies a clipping rectangle. The region of interest is the intersection of the clipping rectangle with the visible region specified by hdc.
        /// If hdc is non-NULL, the coordinates of the clipping rectangle are relative to the origin of the hdc.If hdc is NULL, the coordinates are virtual-screen coordinates.
        /// This parameter can be NULL if you don't want to clip the region specified by hdc.</param>
        /// <param name="lpfnEnum">A pointer to a MonitorEnumProc application-defined callback function.</param>
        /// <param name="dwData">Application-defined data that EnumDisplayMonitors passes directly to the MonitorEnumProc function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        /// <remarks>There are two reasons to call the EnumDisplayMonitors function:
        /// You want to draw optimally into a device context that spans several display monitors, and the monitors have different color formats.
        /// You want to obtain a handle and position rectangle for one or more display monitors.
        /// To determine whether all the display monitors in a system share the same color format, call GetSystemMetrics (SM_SAMEDISPLAYFORMAT).
        /// You do not need to use the EnumDisplayMonitors function when a window spans display monitors that have different color formats. You can continue to paint under the assumption that the entire screen has the color properties of the primary monitor.Your windows will look fine.EnumDisplayMonitors just lets you make them look better.
        /// Setting the hdc parameter to NULL lets you use the EnumDisplayMonitors function to obtain a handle and position rectangle for one or more display monitors.The following table shows how the four combinations of NULL and non-NULLhdc and lprcClip values affect the behavior of the EnumDisplayMonitors function.
        /// hdc lprcRect    EnumDisplayMonitors behavior
        /// NULL NULL    Enumerates all display monitors.The callback function receives a NULL HDC.
        /// NULL non-NULL Enumerates all display monitors that intersect the clipping rectangle. Use virtual screen coordinates for the clipping rectangle.The callback function receives a NULL HDC.
        /// non-NULL NULL    Enumerates all display monitors that intersect the visible region of the device context.The callback function receives a handle to a DC for the specific display monitor.
        /// non-NULL non-NULL Enumerates all display monitors that intersect the visible region of the device context and the clipping rectangle. Use device context coordinates for the clipping rectangle.The callback function receives a handle to a DC for the specific display monitor.</remarks>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplayMonitors(
                    IntPtr hdc,
                    ref RECT lprcClip,
                    MonitorEnumProc lpfnEnum,
                    IntPtr dwData);


        /// <summary>
        /// The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// Specifies the x-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public Int32 Left;
            /// <summary>
            /// Specifies the y-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public Int32 Top;
            /// <summary>
            /// Specifies the x-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public Int32 Right;
            /// <summary>
            /// Specifies the y-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public Int32 Bottom;
        }


        /// <summary>
        /// A MonitorEnumProc function is an application-defined callback function that is called by the EnumDisplayMonitors function.
        /// A value of type MONITORENUMPROC is a pointer to a MonitorEnumProc function.
        /// </summary>
        /// <param name="hMonitor">A handle to the display monitor. This value will always be non-NULL.</param>
        /// <param name="hdcMonitor">A handle to a device context.
        /// The device context has color attributes that are appropriate for the display monitor identified by hMonitor. The clipping area of the device context is set to the intersection of the visible region of the device context identified by the hdc parameter of EnumDisplayMonitors, the rectangle pointed to by the lprcClip parameter of EnumDisplayMonitors, and the display monitor rectangle.
        /// This value is NULL if the hdc parameter of EnumDisplayMonitors was NULL.</param>
        /// <param name="lprcMonitor">A pointer to a RECT structure.
        /// If hdcMonitor is non-NULL, this rectangle is the intersection of the clipping area of the device context identified by hdcMonitor and the display monitor rectangle. The rectangle coordinates are device-context coordinates.
        /// If hdcMonitor is NULL, this rectangle is the display monitor rectangle. The rectangle coordinates are virtual-screen coordinates.</param>
        /// <param name="dwData">Application-defined data that EnumDisplayMonitors passes directly to the enumeration function.</param>
        /// <returns>To continue the enumeration, return TRUE.
        /// To stop the enumeration, return FALSE.</returns>
        /// <remarks>You can use the EnumDisplayMonitors function to enumerate the set of display monitors that intersect the visible region of a specified device context and, optionally, a clipping rectangle. To do this, set the hdc parameter to a non-NULL value, and set the lprcClip parameter as needed.
        /// You can also use the EnumDisplayMonitors function to enumerate one or more of the display monitors on the desktop, without supplying a device context.To do this, set the hdc parameter of EnumDisplayMonitors to NULL and set the lprcClip parameter as needed.
        /// In all cases, EnumDisplayMonitors calls a specified MonitorEnumProc function once for each display monitor in the calculated enumeration set.The MonitorEnumProc function always receives a handle to the display monitor.
        /// If the hdc parameter of EnumDisplayMonitors is non-NULL, the MonitorEnumProc function also receives a handle to a device context whose color format is appropriate for the display monitor.You can then paint into the device context in a manner that is optimal for the display monitor.</remarks>
        public delegate bool MonitorEnumProc(SafePhysicalMonitorHandle hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);
        
        
        /// <summary>
        /// Retrieves the number of physical monitors associated with an HMONITOR monitor handle. Call this function before calling GetPhysicalMonitorsFromHMONITOR.
        /// </summary>
        /// <param name="hMonitor">A monitor handle. Monitor handles are returned by several Multiple Display Monitor functions, including EnumDisplayMonitors and MonitorFromWindow, which are part of the graphics device interface (GDI).</param>
        /// <param name="pdwNumberOfPhysicalMonitors">Receives the number of physical monitors associated with the monitor handle.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        [DllImport("dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(
            IntPtr hMonitor,
            [Out]
            out UInt32 pdwNumberOfPhysicalMonitors);


        /// <summary>
        /// Retrieves the physical monitors associated with an HMONITOR monitor handle.
        /// </summary>
        /// <param name="hMonitor">A monitor handle. Monitor handles are returned by several Multiple Display Monitor functions, including EnumDisplayMonitors and MonitorFromWindow, which are part of the graphics device interface (GDI).</param>
        /// <param name="dwPhysicalMonitorArraySize">Number of elements in pPhysicalMonitorArray. To get the required size of the array, call GetNumberOfPhysicalMonitorsFromHMONITOR.</param>
        /// <param name="pPhysicalMonitorArray">Pointer to an array of PHYSICAL_MONITOR structures. The caller must allocate the array.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        /// <remarks>A single HMONITOR handle can be associated with more than one physical monitor. This function returns a handle and a text description for each physical monitor.
        /// When you are done using the monitor handles, close them by passing the pPhysicalMonitorArray array to the DestroyPhysicalMonitors function.</remarks>
        [DllImport("dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(
            IntPtr hMonitor,
            UInt32 dwPhysicalMonitorArraySize,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            PHYSICAL_MONITOR[] pPhysicalMonitorArray);


        /// <summary>
        /// Closes a handle to a physical monitor. Call this function to close a monitor handle obtained from the GetPhysicalMonitorsFromHMONITOR or GetPhysicalMonitorsFromIDirect3DDevice9 function.
        /// </summary>
        /// <param name="hMonitor">Handle to a physical monitor.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        [DllImport("dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitor(
            IntPtr hMonitor);


        /// <summary>
        /// Closes an array of physical monitor handles. Call this function to close an array of monitor handles obtained from the GetPhysicalMonitorsFromHMONITOR or GetPhysicalMonitorsFromIDirect3DDevice9 function.
        /// </summary>
        /// <param name="dwPhysicalMonitorArraySize">Number of elements in the pPhysicalMonitorArray array.</param>
        /// <param name="pPhysicalMonitorArray">Pointer to an array of PHYSICAL_MONITOR structures.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        [DllImport("dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitors(
            UInt32 dwPhysicalMonitorArraySize,
            PHYSICAL_MONITOR[] pPhysicalMonitorArray);


        /// <summary>
        /// Contains a handle and text description corresponding to a physical monitor.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PHYSICAL_MONITOR
        {
            /// <summary>
            /// Handle to the physical monitor.
            /// </summary>
            public IntPtr hPhysicalMonitor;
            /// <summary>
            /// Text description of the physical monitor.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPhysicalMonitorDescription;
        }


        /// <summary>
        /// Sets the value of a Virtual Control Panel (VCP) code for a monitor.
        /// </summary>
        /// <param name="hMonitor">Handle to a physical monitor. To get the monitor handle, call GetPhysicalMonitorsFromHMONITOR or GetPhysicalMonitorsFromIDirect3DDevice9.</param>
        /// <param name="bVCPCode">VCP code to set. The VCP codes are defined in the VESA Monitor Control Command Set (MCCS) standard, version 1.0 and 2.0. This parameter must specify a continuous or non-continuous VCP, or a vendor-specific code. It should not be a table control code.</param>
        /// <param name="dwNewValue">Value of the VCP code.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        [DllImport("dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetVCPFeature(
            SafePhysicalMonitorHandle hMonitor,
            byte bVCPCode,
            UInt32 dwNewValue);


        /// <summary>
        /// Retrieves the current value, maximum value, and code type of a Virtual Control Panel (VCP) code for a monitor.
        /// </summary>
        /// <param name="hMonitor">Handle to a physical monitor. To get the monitor handle, call GetPhysicalMonitorsFromHMONITOR or GetPhysicalMonitorsFromIDirect3DDevice9.</param>
        /// <param name="bVCPCode">VCP code to query. The VCP codes are Include the VESA Monitor Control Command Set (MCCS) standard, versions 1.0 and 2.0. This parameter must specify a continuous or non-continuous VCP, or a vendor-specific code. It should not be a table control code.</param>
        /// <param name="pvct">Receives the VCP code type, as a member of the MC_VCP_CODE_TYPE enumeration. This parameter can be NULL.</param>
        /// <param name="pdwCurrentValue">Receives the current value of the VCP code. This parameter can be NULL.</param>
        /// <param name="pdwMaximumValue">If bVCPCode specifies a continuous VCP code, this parameter receives the maximum value of the VCP code. If bVCPCode specifies a non-continuous VCP code, the value received in this parameter is undefined. This parameter can be NULL.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        /// <remarks>This function corresponds to the "Get VCP Feature & VCP Feature Reply" command from the Display Data Channel Command Interface (DDC/CI) standard. Vendor-specific VCP codes can be used with this function.
        /// This function takes about 40 milliseconds to return.</remarks>
        [DllImport("dxva2.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetVCPFeatureAndVCPFeatureReply(
            SafePhysicalMonitorHandle hMonitor,
            byte bVCPCode,
            [Out]
            out MC_VCP_CODE_TYPE pvct,
            [Out]
            out UInt32 pdwCurrentValue,
            [Out]
            out UInt32 pdwMaximumValue);


        /// <summary>
        /// Describes a Virtual Control Panel (VCP) code type.
        /// </summary>
        public enum MC_VCP_CODE_TYPE
        {
            /// <summary>
            /// Momentary VCP code. Sending a command of this type causes the monitor to initiate a self-timed operation and then revert to its original state. Examples include display tests and degaussing.
            /// </summary>
            MC_MOMENTARY,
            /// <summary>
            /// Set Parameter VCP code. Sending a command of this type changes some aspect of the monitor's operation.
            /// </summary>
            MC_SET_PARAMETER
        }


        /// <summary>
        /// Retrieves the length of a monitor's capabilities string.
        /// </summary>
        /// <param name="hMonitor">Handle to a physical monitor. To get the monitor handle, call GetPhysicalMonitorsFromHMONITOR or GetPhysicalMonitorsFromIDirect3DDevice9.</param>
        /// <param name="pdwCapabilitiesStringLengthInCharacters">Receives the length of the capabilities string, in characters, including the terminating null character.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        /// <remarks>This function usually returns quickly, but sometimes it can take several seconds to complete.</remarks>
        [DllImport("dxva2.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCapabilitiesStringLength(
            SafePhysicalMonitorHandle hMonitor,
            [Out, MarshalAs(UnmanagedType.U4)]
            out UInt32 pdwCapabilitiesStringLengthInCharacters);


        /// <summary>
        /// Retrieves a string describing a monitor's capabilities.
        /// </summary>
        /// <param name="hMonitor">Handle to a physical monitor. To get the monitor handle, call GetPhysicalMonitorsFromHMONITOR or GetPhysicalMonitorsFromIDirect3DDevice9.</param>
        /// <param name="pszASCIICapabilitiesString">Pointer to a buffer that receives the monitor's capabilities string. The caller must allocate this buffer. To get the size of the string, call GetCapabilitiesStringLength. The capabilities string is always an ASCII string. The buffer must include space for the terminating null character.</param>
        /// <param name="dwCapabilitiesStringLengthInCharacters">Size of pszASCIICapabilitiesString in characters, including the terminating null character.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE. To get extended error information, call GetLastError.</returns>
        /// <remarks>This function corresponds to the "Capabilities Request & Capabilities Reply" command from the Display Data Channel Command Interface (DDC/CI) standard. For more information about the capabilities string, refer to the DDC/CI standard.
        /// This function usually returns quickly, but sometimes it can take several seconds to complete.
        /// You can update a monitor's capabilities string by adding an AddReg directive to the monitor's INF file.Add a registry key named "CapabilitiesString" to the monitor's driver key. The value of the registry key is the capabilities string. The registry data type is REG_SZ.
        /// <code>HKR,,"CapabilitiesString",0x00000000,"updated capabilities string"</code>
        /// <b>Warning</b>Do not modify a monitor's INF file unless you are familiar with the layout of INF files and also understand the DDC/CI standard.</remarks>
        [DllImport("dxva2.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CapabilitiesRequestAndCapabilitiesReply(
            SafePhysicalMonitorHandle hMonitor,
            [In, Out, MarshalAs(UnmanagedType.LPStr)] 
            StringBuilder pszASCIICapabilitiesString,
            UInt32 dwCapabilitiesStringLengthInCharacters);
    }
}
