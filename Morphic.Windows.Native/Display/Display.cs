// Copyright 2020 Raising the Floor - International
//
// Licensed under the New BSD license. You may not use this file except in
// compliance with this License.
//
// You may obtain a copy of the License at
// https://github.com/GPII/universal/blob/master/LICENSE.txt
//
// The R&D leading to these results received funding from the:
// * Rehabilitation Services Administration, US Dept. of Education under 
//   grant H421A150006 (APCP)
// * National Institute on Disability, Independent Living, and 
//   Rehabilitation Research (NIDILRR)
// * Administration for Independent Living & Dept. of Education under grants 
//   H133E080022 (RERC-IT) and H133E130028/90RE5003-01-00 (UIITA-RERC)
// * European Union's Seventh Framework Programme (FP7/2007-2013) grant 
//   agreement nos. 289016 (Cloud4all) and 610510 (Prosperity4All)
// * William and Flora Hewlett Foundation
// * Ontario Ministry of Research and Innovation
// * Canadian Foundation for Innovation
// * Adobe Foundation
// * Consumer Electronics Association Foundation

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Morphic.Windows.Native
{
    public class Display
    {
        // NOTE: this enumeration method is for testing only; because it uses a callback and does not specify an upper limit for the number of callback calls...it may be useless (unless we find a safe way to measure and watch for all callbacks)
        //public struct DisplayInfo
        //{
        //    public Rectangle FullBounds;
        //    public Rectangle WorkingBounds;
        //    public Boolean IsPrimaryDisplay;
        //    public String DeviceName;
        //}
        //public static List<DisplayInfo>? GetAllDisplays_CallbackTest()
        //{
        //    var listOfDisplays = new List<DisplayInfo>();
        //    var enumerationInProgress = true;
        //    var enumerationIsSuccess = true;

        //    // create delegate to populate list of displays
        //    WindowsApi.MonitorEnumProcDelegate monitorEnumDelegate = delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref WindowsApi.RECT lprcMonitor, IntPtr dwData) {
        //        WindowsApi.MONITORINFOEXA monitorInfo = new WindowsApi.MONITORINFOEXA();
        //        monitorInfo.Init();

        //        var result = WindowsApi.GetMonitorInfo(hMonitor, ref monitorInfo);
        //        if (result == false)
        //        {
        //            // TODO: how do we alert the user that we have failed?
        //            enumerationIsSuccess = false;
        //            return false;
        //        }

        //        var displayInfo = new DisplayInfo();
        //        //
        //        displayInfo.FullBounds = new Rectangle(monitorInfo.rcMonitor.left, monitorInfo.rcMonitor.top, monitorInfo.rcMonitor.right - monitorInfo.rcMonitor.left, monitorInfo.rcMonitor.bottom - monitorInfo.rcMonitor.top);
        //        displayInfo.WorkingBounds = new Rectangle(monitorInfo.rcWork.left, monitorInfo.rcWork.top, monitorInfo.rcWork.right - monitorInfo.rcWork.left, monitorInfo.rcWork.bottom - monitorInfo.rcWork.top);
        //        //
        //        displayInfo.IsPrimaryDisplay = ((monitorInfo.dwFlags & WindowsApi.MONITORINFOF_PRIMARY) != 0);
        //        //
        //        // convert monitor device name to string
        //        var deviceNameLength = Array.FindIndex(monitorInfo.szDevice, 0, (arg0) => { return arg0 == '\0'; });
        //        if (deviceNameLength < 0)
        //        {
        //            deviceNameLength = monitorInfo.szDevice.Length;
        //        }
        //        displayInfo.DeviceName = new String(monitorInfo.szDevice, 0, deviceNameLength);

        //        listOfDisplays.Add(displayInfo);

        //        // TODO: unfortunately there is no way to know when it's done :(
        //        enumerationInProgress = false;

        //        // continue to the next display
        //        return true;
        //    };

        //    // NOTE: EnumDisplayMonitors enumerates both visible monitors and invisible pseudo-monitors (i.e. mirroring monitors, etc.)
        //    var result = WindowsApi.EnumDisplayMonitors(IntPtr.Zero /* entire virtual screen */, IntPtr.Zero /* no clipping */, monitorEnumDelegate, IntPtr.Zero);
        //    if (result == false)
        //    {
        //        return null;
        //    }

        //    while (enumerationInProgress == true)
        //    {
        //        System.Threading.Thread.Sleep(1);
        //    }

        //    if (enumerationIsSuccess == false)
        //    {
        //        return null;
        //    } 
        //    else
        //    {
        //        return listOfDisplays;
        //    }
        //}

        /*** Functions to get display adapter info ***/

        public static String? GetPrimaryDisplayAdapterName()
        {
            var allDisplayAdapters = GetAllDisplayAdapters();

            foreach(var displayAdapterInfo in allDisplayAdapters) {
                if(displayAdapterInfo.isPrimaryDisplayAdapter == true)
                {
                    return displayAdapterInfo.displayAdapterName;
                }
            }

            // if we could not find a primary display adapter, return null
            return null;
        }

        public static List<String> GetAllDisplayAdapterNames()
        {
            var result = new List<String>();

            var allDisplayAdapters = GetAllDisplayAdapters();

            foreach (var displayAdapterInfo in allDisplayAdapters)
            {
                result.Add(displayAdapterInfo.displayAdapterName);
            }

            return result;
        }

        public struct DisplayAdapterInfo
        {
            public String displayAdapterName;
            public Boolean isPrimaryDisplayAdapter;

            public DisplayAdapterInfo(
                String displayAdapterName,
                Boolean isPrimaryDisplayAdapter
            )
            {
                this.displayAdapterName = displayAdapterName;
                this.isPrimaryDisplayAdapter = isPrimaryDisplayAdapter;
            }
        }
        public static List<DisplayAdapterInfo> GetAllDisplayAdapters()
        {
            var result = new List<DisplayAdapterInfo>();

            // get a list of all display adapters
            UInt32 iDisplayAdapter = 0;
            //
            var success = true;
            while (success == true)
            {
                var cDisplayDevice = new WindowsApi.DISPLAY_DEVICEW();
                cDisplayDevice.Init();

                success = WindowsApi.EnumDisplayDevices_DisplayAdapter(IntPtr.Zero, iDisplayAdapter, ref cDisplayDevice, 0);
                if (success == false)
                {
                    break;
                }

                // verify that the device is attached to the desktop
                if ((UInt32)(cDisplayDevice.StateFlags & WindowsApi.DisplayDeviceStateFlags.DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) == 0)
                {
                    iDisplayAdapter += 1;
                    continue;
                }
                // verify that the device is not a mirroring psuedodevice
                if ((UInt32)(cDisplayDevice.StateFlags & WindowsApi.DisplayDeviceStateFlags.DISPLAY_DEVICE_MIRRORING_DRIVER) != 0)
                {
                    iDisplayAdapter += 1;
                    continue;
                }

                var deviceNameAsCharArray = cDisplayDevice.DeviceName;
                var deviceNameAsString = ConvertCharArrayToString(deviceNameAsCharArray);

                var isPrimaryDisplayAdapter = false;
                if ((UInt32)(cDisplayDevice.StateFlags & WindowsApi.DisplayDeviceStateFlags.DISPLAY_DEVICE_PRIMARY_DEVICE) != 0)
                {
                    isPrimaryDisplayAdapter = true;
                }

                var displayAdapterInfo = new DisplayAdapterInfo(deviceNameAsString, isPrimaryDisplayAdapter);
                result.Add(displayAdapterInfo);

                iDisplayAdapter += 1;
            }

            return result;
        }

        /*** Functions to get display adapters' monitor info ***/

        public struct DisplayAdapterMonitorInfo
        {
            public readonly String deviceName;
            public readonly String deviceString;
            public readonly Boolean isActive;

            internal DisplayAdapterMonitorInfo(
                String deviceName,
                String deviceString,
                Boolean isActive
            )
            {
                this.deviceName = deviceName;
                this.deviceString = deviceString;
                this.isActive = isActive;
            }
        }

        public static List<DisplayAdapterMonitorInfo> GetAllMonitorsForDisplayAdapter(String displayAdapterName)
        {
            var result = new List<DisplayAdapterMonitorInfo>();

            var displayAdapterNameAsCharArray = ConvertStringToNullTerminatedCharArray(displayAdapterName);

            // get the monitors associated with this display adapter
            UInt32 iMonitor = 0;
            //
            var success = true;
            while (success == true)
            {
                var cDisplayDevice = new WindowsApi.DISPLAY_DEVICEW();
                cDisplayDevice.Init();

                success = WindowsApi.EnumDisplayDevices_Monitor(displayAdapterNameAsCharArray, iMonitor, ref cDisplayDevice, 0);
                if (success == false)
                {
                    break;
                }

                var monitorDeviceNameAsString = Display.ConvertCharArrayToString(cDisplayDevice.DeviceName);
                var monitorDeviceStringAsString = Display.ConvertCharArrayToString(cDisplayDevice.DeviceString);
                var monitorStateFlags = (UInt32)cDisplayDevice.StateFlags;

                Boolean isAttached = false;
                if ((monitorStateFlags & (UInt32)WindowsApi.ChildDisplayDeviceStateFlags.DISPLAY_DEVICE_ATTACHED) != 0)
                {
                    isAttached = true;
                }
                // if the monitor is not attached, do not include it in the enumeration; we may modify this logic in the future if/as desired
                if (isAttached == false)
                {
                    iMonitor += 1;
                    continue;
                }

                Boolean isActive = false;
                if ((monitorStateFlags & (UInt32)WindowsApi.ChildDisplayDeviceStateFlags.DISPLAY_DEVICE_ACTIVE) != 0)
                {
                    isActive = true;
                }

                var monitorInfo = new DisplayAdapterMonitorInfo(monitorDeviceNameAsString, monitorDeviceStringAsString, isActive);

                result.Add(monitorInfo);

                iMonitor += 1;
            }

            return result;
        }

        /*** Functions to get display settings (per display adapter) ***/

        public struct DisplaySettings
        {
            public enum DisplayOrientation: UInt32
            {
                Default = 0,
                Clockwise90Degrees = 1,
                Clockwise180Degrees = 2,
                Clockwise270Degrees = 3
            }
            //
            public enum FixedResolutionOutputOption: UInt32
            {
                Default = 0,
                StretchToFillScreen = 1,
                CenterInScreen = 2
            }
            //
            internal enum DisplayFlags: UInt32
            {
                // Grayscale = 1, // no longer valid
                Interlaced = 2,
                // TextMode = 4
            }

            internal readonly WindowsApi.DEVMODEW devmode;
            //
            public readonly UInt32 widthInPixels;
            public readonly UInt32 heightInPixels;
            public readonly UInt32 refreshRateInHertz;
            public readonly UInt32 bitsPerPixel;
            public readonly DisplayOrientation? orientation;
            public readonly FixedResolutionOutputOption? fixedOutputOption;
            internal readonly DisplayFlags? flags;
            public readonly System.Drawing.Point? position;

            private DisplaySettings(
                WindowsApi.DEVMODEW devmode,
                UInt32 widthInPixels,
                UInt32 heightInPixels,
                UInt32 refreshRateInHertz,
                UInt32 bitsPerPixel,
                DisplayOrientation? orientation,
                FixedResolutionOutputOption? fixedOutputOption,
                DisplayFlags? flags,
                System.Drawing.Point? position
            )
            {
                this.devmode = devmode;
                this.widthInPixels = widthInPixels;
                this.heightInPixels = heightInPixels;
                this.refreshRateInHertz = refreshRateInHertz;
                this.bitsPerPixel = bitsPerPixel;
                this.orientation = orientation;
                this.fixedOutputOption = fixedOutputOption;
                this.flags = flags;
                this.position = position;
            }

            internal static DisplaySettings? CreateNew(WindowsApi.DEVMODEW devmode)
            {
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_PELSWIDTH) == 0)
                {
                    return null;
                }
                var widthInPixels = devmode.dmPelsWidth;
                //
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_PELSHEIGHT) == 0)
                {
                    return null;
                }
                var heightInPixels = devmode.dmPelsHeight;
                //
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_DISPLAYFREQUENCY) == 0)
                {
                    return null;
                }
                var refreshRateInHertz = devmode.dmDisplayFrequency;
                //
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_BITSPERPEL) == 0)
                {
                    return null;
                }
                var bitsPerPixel = devmode.dmBitsPerPel;
                //
                DisplayOrientation? orientation = null;
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_DISPLAYORIENTATION) != 0)
                {
                    orientation = (DisplayOrientation)devmode.DUMMYUNIONNAME.DUMMYSTRUCTNAME2.dmDisplayOrientation;
                }
                //
                FixedResolutionOutputOption? fixedOutputOption = null;
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_DISPLAYFIXEDOUTPUT) != 0)
                {
                    fixedOutputOption = (FixedResolutionOutputOption)devmode.DUMMYUNIONNAME.DUMMYSTRUCTNAME2.dmDisplayFixedOutput;
                }
                //
                DisplayFlags? flags = null;
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_DISPLAYFLAGS) != 0)
                {
                    flags = (DisplayFlags)devmode.DUMMYUNIONNAME2.dmDisplayFlags;
                }
                //
                System.Drawing.Point? position = null;
                if ((UInt32)(devmode.dmFields & WindowsApi.DM_FieldSelectionBit.DM_POSITION) != 0)
                {
                    position = new System.Drawing.Point(devmode.DUMMYUNIONNAME.DUMMYSTRUCTNAME2.dmPosition.x, devmode.DUMMYUNIONNAME.DUMMYSTRUCTNAME2.dmPosition.y);
                }

                var result = new DisplaySettings(
                    devmode,
                    widthInPixels,
                    heightInPixels,
                    refreshRateInHertz,
                    bitsPerPixel,
                    orientation,
                    fixedOutputOption,
                    flags,
                    position
                );
                return result;
            }
        }

        public static List<DisplaySettings> GetAllDisplaySettingsForDisplayAdapter(String displayAdapterName)
        {
            var result = new List<DisplaySettings>();

            var displayAdapterNameAsCharArray = ConvertStringToNullTerminatedCharArray(displayAdapterName);

            // get the display settings for each graphics mode of this display adapter
            UInt32 iGraphicsMode = 0;

            var success = true;
            while (success == true)
            {
                var devmode = new WindowsApi.DEVMODEW();
                devmode.Init();

                success = WindowsApi.EnumDisplaySettingsEx(displayAdapterNameAsCharArray, iGraphicsMode, ref devmode, 0);
                if (success == false)
                {
                    break;
                }

                var displaySettings = DisplaySettings.CreateNew(devmode);
                if (displaySettings == null)
                {
                    iGraphicsMode += 1;
                    continue;
                }

                result.Add(displaySettings.Value);

                iGraphicsMode += 1;
            }

            return result;
        }

        public static DisplaySettings? GetCurrentDisplaySettingsForDisplayAdapter(String displayAdapterName)
        {
            var displayAdapterNameAsCharArray = ConvertStringToNullTerminatedCharArray(displayAdapterName);

            var success = true;
            var devmode = new WindowsApi.DEVMODEW();
            devmode.Init();

            success = WindowsApi.EnumDisplaySettingsEx(displayAdapterNameAsCharArray, WindowsApi.ENUM_CURRENT_SETTINGS, ref devmode, 0);
            if (success == false)
            {
                return null;
            }

            var displaySettings = DisplaySettings.CreateNew(devmode);
            if (displaySettings == null)
            {
                return null;
            }

            return displaySettings;
        }

        // NOTE: this function throws an exception if the settings could not be set successfully
        public static void SetCurrentDisplaySettings(String displayAdapterName, DisplaySettings settings)
        {
            /* OPTIONAL FLAGS:
             * CDS_GLOBAL | CDS_UPDATEREGISTRY  // save the updated resolution for all users
             * CDS_RESET                        // update the settings even if they're the same as the current settings
             * CDS_SET_PRIMARY                  // make this display adapter the primary display adapter
             * CDS_TEST                         // tests if the change would be valid [NOTE: this _might_ let us detect if a display change requires a reboot]
             * CDS_UPDATEREGISTRY               // save the display change in the system registry (for the user, unless also using CDS_GLOBAL)
             * CDS_DISABLE_UNSAFE_MODES         // prohibits us from accidentally using unsafe display modes
             */
            var displayAdapterNameAsCharArray = ConvertStringToNullTerminatedCharArray(displayAdapterName);
            var devmode = settings.devmode;
            var result = WindowsApi.ChangeDisplaySettingsEx(displayAdapterNameAsCharArray, ref devmode, IntPtr.Zero, 0, IntPtr.Zero);
            switch (result)
            {
                case WindowsApi.DISP_CHANGE_RESULT.DISP_CHANGE_SUCCESSFUL:
                    return;
                default:
                    throw new Exception("Could not change screen settings; result: " + result);
            }
        }

        // NOTE: this function returns True if the display settings can be set, False otherwise
        // NOTE: this _might_ let us detect if a display change requires a reboot (although we don't yet return a result indicating that reality)
        public static Boolean TestDisplaySettings(String displayAdapterName, DisplaySettings settings)
        {
            var displayAdapterNameAsCharArray = ConvertStringToNullTerminatedCharArray(displayAdapterName);
            var devmode = settings.devmode;
            var result = WindowsApi.ChangeDisplaySettingsEx(displayAdapterNameAsCharArray, ref devmode, IntPtr.Zero, (UInt32)WindowsApi.ChangeDisplaySettingsFlags.CDS_TEST, IntPtr.Zero);
            switch (result)
            {
                case WindowsApi.DISP_CHANGE_RESULT.DISP_CHANGE_SUCCESSFUL:
                    return true;
                //case WindowsApi.DISP_CHANGE_RESULT.DISP_CHANGE_RESTART:
                default:
                    return false;
            }
        }

        public static UInt32? GetDpiForWindow(IntPtr handle)
        {
            var dpiForWindow = WindowsApi.GetDpiForWindow(handle);
            if (dpiForWindow == 0)
            {
                return null;
            } 
            else
            {
                return dpiForWindow;
            }
        }

        public static Int32 GetNumberOfVisibleDisplayMonitors()
        {
            var numberOfVisibleDisplayMonitors = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_CMONITORS);
            return numberOfVisibleDisplayMonitors;
        }

        // NOTE: this function is not DPI aware (unless the app is DPI-aware)
        public static Size? GetMainDisplayMonitorSizeInPixels()
        {
            // implementation option 1:
            // TODO: use alternate GetSystemMetricsForDpi in dpi-aware apps
            // NOTE: GetSystemMetrics returns a scaled DPI value (not the physical number of pixels)
            var width = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_CXSCREEN);
            var height = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_CYSCREEN);
            if (width == 0 || height == 0)
            {
                return null;
            }

            // implementation option 2:
            // TODO: ... width = GetDeviceCaps(hdcPrimaryMonitor, HORZRES);
            // TODO: ... height = GetDeviceCaps(hdcPrimaryMonitor, VERTRES);
            //if (width == 0 || height == 0)
            //{
            //    return null;
            //}

            return new Size(width, height);
        }
        //
        //public static Size? GetMainDisplayMonitorSizeInPixels_DpiAware(UInt32 dpi)
        //{
        //    var width = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_CXSCREEN, dpi);
        //    var height = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_CYSCREEN, dpi);
        //    if (width == 0 || height == 0)
        //    {
        //        return null;
        //    }

        //    return new Size(width, height);
        //}


        // TODO: To get the coordinates of the portion of the screen that is not obscured by the system taskbar or by application desktop toolbars, call the SystemParametersInfo function with the SPI_GETWORKAREA value instead
        // NOTE: this function is not DPI aware (unless the app is DPI-aware)
        public static Size? GetMainDisplayMonitorFullScreenWindowSizeInPixels()
        {
            // TODO: use alternate GetSystemMetricsForDpi in dpi-aware apps
            // NOTE: GetSystemMetrics returns a scaled DPI value (not the physical number of pixels)
            var width = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_CXFULLSCREEN);
            var height = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_CYFULLSCREEN);
            if (width == 0 || height == 0)
            {
                return null;
            }

            return new Size(width, height);
        }
        //
        //public static Size? GetMainDisplayMonitorFullScreenWindowSizeInPixels_DpiAware(UInt32 dpi)
        //{
        //    // NOTE: GetSystemMetrics returns a scaled DPI value (not the physical number of pixels)
        //    var width = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_CXFULLSCREEN, dpi);
        //    var height = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_CYFULLSCREEN, dpi);
        //    if (width == 0 || height == 0)
        //    {
        //        return null;
        //    }

        //    return new Size(width, height);
        //}

        // NOTE: this function is not DPI aware (unless the app is DPI-aware)
        public static Rectangle? GetVirtualScreenBoundsInPixels()
        {
            // NOTE: GetSystemMetrics returns a scaled DPI value (not the physical number of pixels)
            var x = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_XVIRTUALSCREEN);
            var y = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_YVIRTUALSCREEN);
            var width = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_CXVIRTUALSCREEN);
            var height = WindowsApi.GetSystemMetrics(WindowsApi.SystemMetricIndex.SM_CYVIRTUALSCREEN);
            if (width == 0 || height == 0)
            {
                return null;
            }

            return new Rectangle(x, y, width, height);
        }
        //
        //public static Rectangle? GetVirtualScreenBoundsInPixels_DpiAware(UInt32 dpi)
        //{
        //    var x = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_XVIRTUALSCREEN, dpi);
        //    var y = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_YVIRTUALSCREEN, dpi);
        //    var width = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_CXVIRTUALSCREEN, dpi);
        //    var height = WindowsApi.GetSystemMetricsForDpi(WindowsApi.SystemMetricIndex.SM_CYVIRTUALSCREEN, dpi);
        //    if (width == 0 || height == 0)
        //    {
        //        return null;
        //    }

        //    return new Rectangle(x, y, width, height);
        //}


        /*** Helper functions: convert String to and from null-terminated Char array ***/

        private static Char[] ConvertStringToNullTerminatedCharArray(String value, Int32? arrayLength = null)
        {
            if (arrayLength != null && (value.Length + 1 < arrayLength.Value))
            {
                throw new ArgumentException("Argument " + nameof(value) + " is longer than the specified array length (including null terminator)");
            }

            // convert the string to a non-null-terminated character array
            Char[] stringAsCharArray = value.ToCharArray();
            // copy the string characters to our result
            Char[] result = new Char[arrayLength ?? (stringAsCharArray.Length + 1)];
            Array.Copy(stringAsCharArray, 0, result, 0, stringAsCharArray.Length);
            // append the null terminator
            result[stringAsCharArray.Length] = '\0';

            return result;
        }

        private static String ConvertCharArrayToString(Char[] value)
        {
            Int32 nullTerminatorPos = Array.IndexOf(value, '\0');
            if (nullTerminatorPos < 0)
            {
                // no null terminator
                return new String(value);
            }
            else
            {
                return new string(value, 0, nullTerminatorPos);
            }
        }

        #region High contrast settings

        public struct HighContrastFeatureSettings
        {
            public Boolean FeatureAvailable;
            public Boolean FeatureEnabled;
            public Boolean HotkeyAvailable;
            public Boolean HotkeyEnabled;
            public Boolean HotkeyConfirmationEnabled;
            public Boolean HotkeySoundEnabled;
            //public Boolean IndicatorEnabled;

            public String? HighContrastTheme;

            internal HighContrastFeatureSettings(UInt32 dwFlags, String? highContrastTheme)
            {
                this.FeatureAvailable = ((dwFlags & (UInt32)WindowsApi.HighContrastFlags.HCF_AVAILABLE) != 0);
                this.FeatureEnabled = ((dwFlags & (UInt32)WindowsApi.HighContrastFlags.HCF_HIGHCONTRASTON) != 0);
                this.HotkeyAvailable = ((dwFlags & (UInt32)WindowsApi.HighContrastFlags.HCF_HOTKEYAVAILABLE) != 0);
                this.HotkeyEnabled = ((dwFlags & (UInt32)WindowsApi.HighContrastFlags.HCF_HOTKEYACTIVE) != 0);
                this.HotkeyConfirmationEnabled = ((dwFlags & (UInt32)WindowsApi.HighContrastFlags.HCF_CONFIRMHOTKEY) != 0);
                this.HotkeySoundEnabled = ((dwFlags & (UInt32)WindowsApi.HighContrastFlags.HCF_HOTKEYSOUND) != 0);
                //this.IndicatorEnabled = ((dwFlags & (UInt32)WindowsApi.HighContrastFlags.HCF_INDICATOR) != 0);

                this.HighContrastTheme = highContrastTheme;
            }
        }

        public static HighContrastFeatureSettings GetHighContrastFeatureSettings()
        {
            var highContrastStruct = new WindowsApi.HIGHCONTRAST(0, IntPtr.Zero);

            var success = WindowsApi.SystemParametersInfo_HIGHCONTRAST(WindowsApi.UIActionParameter.SPI_GETHIGHCONTRAST, highContrastStruct.cbSize, ref highContrastStruct, 0);
            if (success == false)
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            String? highContrastTheme = null;
            if (highContrastStruct.lpszDefaultScheme != IntPtr.Zero)
            {
                highContrastTheme = Marshal.PtrToStringUni(highContrastStruct.lpszDefaultScheme);
            }

            var result = new HighContrastFeatureSettings(highContrastStruct.dwFlags, highContrastTheme);

            return result;
        }

        public static Boolean GetHighContrastFeatureEnabled()
        {
            var highContrastSettings = Display.GetHighContrastFeatureSettings();

            return highContrastSettings.FeatureEnabled;
        }

        // NOTE: this function operates synchronously (i.e. it blocks the calling thread while changing the high contrast mode's enabled state); however,
        //       Windows may asynchronously complete settings/theme changes after this function has completed
        public static void SetHighContrastFeatureSettings(HighContrastFeatureSettings settings)
        {
            // NOTE: we capture the settings directly as a DWORD so that we can preserve any bits which we do not flip (which is also forwards-compatible, in case new features become available in the future)
            var highContrastStruct = new WindowsApi.HIGHCONTRAST(0, IntPtr.Zero);

            // capture the current high contrast mode settings
            var success = WindowsApi.SystemParametersInfo_HIGHCONTRAST(WindowsApi.UIActionParameter.SPI_GETHIGHCONTRAST, highContrastStruct.cbSize, ref highContrastStruct, 0);
            if (success == false)
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            String? currentHighContrastTheme = null;
            if (highContrastStruct.lpszDefaultScheme != IntPtr.Zero)
            {
                currentHighContrastTheme = Marshal.PtrToStringUni(highContrastStruct.lpszDefaultScheme);
            }

            var currentSettings = new HighContrastFeatureSettings(highContrastStruct.dwFlags, currentHighContrastTheme);

            Boolean hcfEnabledChanged = false;
            Boolean hcfThemeChanged = false;

            // FeatureEnabled
            if (settings.FeatureEnabled != currentSettings.FeatureEnabled)
            {
                if (settings.FeatureEnabled == true)
                {
                    highContrastStruct.dwFlags |= (UInt32)WindowsApi.HighContrastFlags.HCF_HIGHCONTRASTON;
                }
                else
                {
                    highContrastStruct.dwFlags &= ~(UInt32)WindowsApi.HighContrastFlags.HCF_HIGHCONTRASTON;
                }
                hcfEnabledChanged = true;
            }
            // FeatureAvailable
            if (settings.FeatureAvailable != currentSettings.FeatureAvailable)
            {
                if (settings.FeatureAvailable == true)
                {
                    highContrastStruct.dwFlags |= (UInt32)WindowsApi.HighContrastFlags.HCF_AVAILABLE;
                }
                else
                {
                    highContrastStruct.dwFlags &= ~(UInt32)WindowsApi.HighContrastFlags.HCF_AVAILABLE;
                }
            }
            // HotkeyAvailable
            if (settings.HotkeyAvailable != currentSettings.HotkeyAvailable)
            {
                // this setting cannot be set by an application
                throw new ArgumentException("settings.HotkeyAvailable is a read-only property");
            }
            // HotkeyEnabled
            if (settings.HotkeyEnabled != currentSettings.HotkeyEnabled)
            {
                if (settings.HotkeyEnabled == true)
                {
                    highContrastStruct.dwFlags |= (UInt32)WindowsApi.HighContrastFlags.HCF_HOTKEYACTIVE;
                }
                else
                {
                    highContrastStruct.dwFlags &= ~(UInt32)WindowsApi.HighContrastFlags.HCF_HOTKEYACTIVE;
                }
            }
            // HotkeyConfirmationEnabled
            if (settings.HotkeyConfirmationEnabled != currentSettings.HotkeyConfirmationEnabled)
            {
                if (settings.HotkeyConfirmationEnabled == true)
                {
                    highContrastStruct.dwFlags |= (UInt32)WindowsApi.HighContrastFlags.HCF_CONFIRMHOTKEY;
                }
                else
                {
                    highContrastStruct.dwFlags &= ~(UInt32)WindowsApi.HighContrastFlags.HCF_CONFIRMHOTKEY;
                }
            }
            // HotkeySoundEnabled
            if (settings.HotkeySoundEnabled != currentSettings.HotkeySoundEnabled)
            {
                if (settings.HotkeySoundEnabled == true)
                {
                    highContrastStruct.dwFlags |= (UInt32)WindowsApi.HighContrastFlags.HCF_HOTKEYSOUND;
                }
                else
                {
                    highContrastStruct.dwFlags &= ~(UInt32)WindowsApi.HighContrastFlags.HCF_HOTKEYSOUND;
                }
            }
            //// IndicatorEnabled
            //if (settings.IndicatorEnabled != currentSettings.IndicatorEnabled)
            //{
            //    if (settings.IndicatorEnabled == true)
            //    {
            //        highContrastStruct.dwFlags |= (UInt32)WindowsApi.HighContrastFlags.HCF_INDICATOR;
            //    }
            //    else
            //    {
            //        highContrastStruct.dwFlags &= ~(UInt32)WindowsApi.HighContrastFlags.HCF_INDICATOR;
            //    }
            //}
            // HighContrastTheme
            if (settings.HighContrastTheme != currentSettings.HighContrastTheme)
            {
                highContrastStruct.lpszDefaultScheme = Marshal.StringToHGlobalUni(settings.HighContrastTheme);
                hcfThemeChanged = true;
            }

            // if the high contrast feature isn't being turned on/off and the theme isn't changing, then don't re-apply theme changes (because doing so would consume milliseconds or maybe even a second or more synchronously...with no benefit)
            if (hcfEnabledChanged == false && hcfThemeChanged == false)
            {
                highContrastStruct.dwFlags |= (UInt32)WindowsApi.HighContrastFlags.NoFlagNameSupported;
            }

            success = WindowsApi.SystemParametersInfo_HIGHCONTRAST(WindowsApi.UIActionParameter.SPI_SETHIGHCONTRAST, highContrastStruct.cbSize, ref highContrastStruct, WindowsApi.SPIF.SPIF_UPDATEINIFILE | WindowsApi.SPIF.SPIF_SENDWININICHANGE);
            if (success == false)
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        // NOTE: this function operates synchronously (i.e. it blocks the calling thread while changing the high contrast mode's enabled state); however
        //       Windows may asynchronously complete settings/theme changes after this function has completed
        public static void SetHighContrastFeatureEnabled(Boolean value)
        {
            var highContrastFeatureSettings = GetHighContrastFeatureSettings();

            highContrastFeatureSettings.FeatureEnabled = value;

            Display.SetHighContrastFeatureSettings(highContrastFeatureSettings);
        }

        public static List<String> GetThemes(String directoryPath)
        {
            var result = new List<String>();

            var themeFilenames = System.IO.Directory.GetFiles(directoryPath, "*.theme");
            foreach (var themeFilename in themeFilenames)
            {
                // open each file as an INI file and read out its "[Theme] > DisplayName" value
                var iniReaderWriter = new Windows.Native.IniFileReaderWriter(themeFilename);
                String iniDisplayNameValue;
                try
                {
                    iniDisplayNameValue = iniReaderWriter.ReadValue("DisplayName", "Theme");
                }
                catch
                {
                    // graceful degradation: if we cannot get this theme's display name, skip this theme
                    continue;

                    // NOTE: if graceful degradation is not what we want, we could throw an exception instead
                    //throw new Exception(...);
                }

                // if the the retrieved DisplayName entry's value is an indirect string (e.g. a resource string in a system DLL), resolve it now

                String displayName;

                // NOTE: SHLoadIndirectString will pass through literal names (i.e. ones that don't start with @), so checking for a start character of '@' isn't strictly necessary
                if (iniDisplayNameValue.Length >= 1 && iniDisplayNameValue.Substring(0, 1) == "@")
                {
                    // if the diplayName starts with @, then it's a reference to a string resource inside a DLL; retrieve it now

                    // create an output buffer
                    const UInt32 cchOutBuf = 257; // 256 wide characters plus a null terminator; note that this is a somewhat arbitrary length
                    var pszOutBuf = Marshal.AllocHGlobal((Int32)cchOutBuf * 2);
                    try
                    {
                        // resolve the indirect string
                        var shlisResult = WindowsApi.SHLoadIndirectString(iniDisplayNameValue, pszOutBuf, cchOutBuf, IntPtr.Zero);
                        if (shlisResult != WindowsApi.S_OK)
                        {
                            // graceful degradation: if we cannot get the theme display name, skip this theme
                            continue;

                            // NOTE: if graceful degradation is not what we want, we could throw the result as an exception instead
                            //throw Marshal.GetExceptionForHR(shlisResult);
                        }

                        displayName = Marshal.PtrToStringUni(pszOutBuf);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(pszOutBuf);
                    }
                }
                else
                {
                    // if the name doesn't start with an @, use the name directly
                    displayName = iniDisplayNameValue;
                }

                result.Add(displayName);
            }

            return result;
        }

        public static List<String> GetHighContrastThemes()
        {
            var directoryPathToHighContrastThemes = Environment.ExpandEnvironmentVariables("%SystemRoot%\\resources\\Ease of Access Themes");

            return GetThemes(directoryPathToHighContrastThemes);
        }

        #endregion
    }
}
