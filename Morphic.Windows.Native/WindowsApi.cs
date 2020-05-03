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
using System.Runtime.InteropServices;
using System.Text;

namespace Morphic.Windows.Native
{
    internal static class WindowsApi
    {
        // NOTE: SYSTEM_INFO Is used by GetSystemInfo and GetNativeSystemInfo
        // https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/ns-sysinfoapi-system_info
        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            public SYSTEM_INFO__DUMMYUNIONNAME dummyUnion;
            public UInt32 dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public UInt32 dwNumberOfProcessors;
            public UInt32 dwProcessorType;
            public UInt32 dwAllocationGranularity;
            public UInt16 wProcessorLevel;
            public UInt16 wProcessorRevision;
        }
        //
        [StructLayout(LayoutKind.Explicit)]
        internal struct SYSTEM_INFO__DUMMYUNIONNAME
        {
            [FieldOffset(0)]
            public UInt32 dwOemId;
            //
            [FieldOffset(0)]
            public SYSTEM_INFO__DUMMYUNIONNAME__DUMMYSTRUCTNAME DUMMYSTRUCTNAME;

            [StructLayout(LayoutKind.Sequential)]
            public struct SYSTEM_INFO__DUMMYUNIONNAME__DUMMYSTRUCTNAME
            {
                public UInt16 wProcessorArchitecture;
                public UInt16 wReserved;
            }
        }

        // NOTE: ProcessArchitecture is used by SYSTEM_INFO
        internal enum ProcessorArchitecture : UInt16
        {
            ARM = 5,
            ARM64 = 12,
            // Neutral = 11,
            AMD64 = 9,
            IA32 = 0,
            // X86_ON_ARM64 = 14,
            UNKNOWN = 0xFFFF
        }

        // NOTE: SystemMetricIndex is used by GetSystemMetrics and GetSystemMetricsForDpi
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        internal enum SystemMetricIndex : Int32
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
            //SM_CXVSCROLL = 2,
            //SM_CYHSCROLL = 3,
            //SM_CYCAPTION = 4,
            //SM_CXBORDER = 5,
            //SM_CYBORDER = 6,
            //SM_CXDLGFRAME = 7,
            //SM_CXFIXEDFRAME = 7,
            //SM_CYDLGFRAME = 8,
            //SM_CYFIXEDFRAME = 8,
            //SM_CYVTHUMB = 9,
            //SM_CXHTHUMB = 10,
            //SM_CXICON = 11,
            //SM_CYICON = 12,
            //SM_CXCURSOR = 13,
            //SM_CYCURSOR = 14,
            //SM_CYMENU = 15,
            SM_CXFULLSCREEN = 16,
            SM_CYFULLSCREEN = 17,
            //SM_CYKANJIWINDOW = 18,
            SM_MOUSEPRESENT = 19,
            //SM_CYVSCROLL = 20,
            //SM_CXHSCROLL = 21,
            //SM_DEBUG = 22,
            SM_SWAPBUTTON = 23,
            //SM_CXMIN = 28,
            //SM_CYMIN = 29,
            //SM_CXSIZE = 30,
            //SM_CYSIZE = 31,
            //SM_CXFRAME = 32,
            //SM_CXSIZEFRAME = 32,
            //SM_CYFRAME = 33,
            //SM_CYSIZEFRAME = 33,
            //SM_CXMINTRACK = 34,
            //SM_CYMINTRACK = 35,
            //SM_CXDOUBLECLK = 36,
            //SM_CYDOUBLECLK = 37,
            //SM_CXICONSPACING = 38,
            //SM_CYICONSPACING = 39,
            //SM_MENUDROPALIGNMENT = 40,
            //SM_PENWINDOWS = 41,
            //SM_DBCSENABLED = 42,
            //SM_CMOUSEBUTTONS = 43,
            //SM_SECURE = 44,
            //SM_CXEDGE = 45,
            //SM_CYEDGE = 46,
            //SM_CXMINSPACING = 47,
            //SM_CYMINSPACING = 48,
            //SM_CXSMICON = 49,
            //SM_CYSMICON = 50,
            //SM_CYSMCAPTION = 51,
            //SM_CXSMSIZE = 52,
            //SM_CYSMSIZE = 53,
            //SM_CXMENUSIZE = 54,
            //SM_CYMENUSIZE = 55,
            //SM_ARRANGE = 56,
            //SM_CXMINIMIZED = 57,
            //SM_CYMINIMIZED = 58,
            //SM_CXMAXTRACK = 59,
            //SM_CYMAXTRACK = 60,
            //SM_CXMAXIMIZED = 61,
            //SM_CYMAXIMIZED = 62,
            //SM_NETWORK = 63,
            //SM_CLEANBOOT = 67,
            //SM_CXDRAG = 68,
            //SM_CYDRAG = 69,
            //SM_SHOWSOUNDS = 70,
            //SM_CXMENUCHECK = 71,
            //SM_CYMENUCHECK = 72,
            //SM_SLOWMACHINE = 73,
            //SM_MIDEASTENABLED = 74,
            //SM_MOUSEWHEELPRESENT = 75,
            SM_XVIRTUALSCREEN = 76,
            SM_YVIRTUALSCREEN = 77,
            SM_CXVIRTUALSCREEN = 78,
            SM_CYVIRTUALSCREEN = 79,
            SM_CMONITORS = 80,
            //SM_SAMEDISPLAYFORMAT = 81,
            //SM_IMMENABLED = 82,
            //SM_CXFOCUSBORDER = 83,
            //SM_CYFOCUSBORDER = 84,
            //SM_TABLETPC = 86,
            //SM_MEDIACENTER = 87,
            //SM_STARTER = 88,
            //SM_SERVERR2 = 89,
            //SM_MOUSEHORIZONTALWHEELPRESENT = 91,
            //SM_CXPADDEDBORDER = 92,
            //SM_DIGITIZER = 94,
            //SM_MAXIMUMTOUCHES = 95,
            //SM_REMOTESESSION = 0x1000,
            //SM_SHUTTINGDOWN = 0x2000,
            //SM_REMOTECONTROL = 0x2001,
            //SM_CONVERTIBLESLATEMODE = 0x2003,
            //SM_SYSTEMDOCKED = 0x2004,
        }

        #region SystemParametersInfo enums/structs/consts

        // NOTE: UIActionParameter is used by SystemParametersInfo
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-systemparametersinfow
        internal enum UIActionParameter: UInt32
        {
            // Accessibility parameters
            //SPI_GETACCESSTIMEOUT = 0x003C,
            //SPI_GETAUDIODESCRIPTION = 0x0074,
            //SPI_GETCLIENTAREAANIMATION = 0x1042,
            //SPI_GETDISABLEOVERLAPPEDCONTENT = 0x1040,
            //SPI_GETFILTERKEYS = 0x0032,
            //SPI_GETFOCUSBORDERHEIGHT = 0x2010,
            //SPI_GETFOCUSBORDERWIDTH = 0x200E,
            SPI_GETHIGHCONTRAST = 0x0042,
            //SPI_GETLOGICALDPIOVERRIDE = 0x009E,
            //SPI_GETMESSAGEDURATION = 0x2016,
            //SPI_GETMOUSECLICKLOCK = 0x101E,
            //SPI_GETMOUSECLICKLOCKTIME = 0x2008,
            //SPI_GETMOUSEKEYS = 0x0036,
            //SPI_GETMOUSESONAR = 0x101C,
            //SPI_GETMOUSEVANISH = 0x1020,
            //SPI_GETSCREENREADER = 0x0046,
            //SPI_GETSERIALKEYS = 0x003E,
            //SPI_GETSHOWSOUNDS = 0x0038,
            //SPI_GETSOUNDSENTRY = 0x0040,
            //SPI_GETSTICKYKEYS = 0x003A,
            //SPI_GETTOGGLEKEYS = 0x0034,
            //SPI_SETACCESSTIMEOUT = 0x003D,
            //SPI_SETAUDIODESCRIPTION = 0x0075,
            //SPI_SETCLIENTAREAANIMATION = 0x1043,
            //SPI_SETDISABLEOVERLAPPEDCONTENT = 0x1041,
            //SPI_SETFILTERKEYS = 0x0033,
            //SPI_SETFOCUSBORDERHEIGHT = 0x2011,
            //SPI_SETFOCUSBORDERWIDTH = 0x200F,
            SPI_SETHIGHCONTRAST = 0x0043,
            //SPI_SETLOGICALDPIOVERRIDE = 0x009F,
            //SPI_SETMESSAGEDURATION = 0x2017,
            //SPI_SETMOUSECLICKLOCK = 0x101F,
            //SPI_SETMOUSECLICKLOCKTIME = 0x2009,
            //SPI_SETMOUSEKEYS = 0x0037,
            //SPI_SETMOUSESONAR = 0x101D,
            //SPI_SETMOUSEVANISH = 0x1021,
            //SPI_SETSCREENREADER = 0x0047,
            //SPI_SETSERIALKEYS = 0x003F,
            //SPI_SETSHOWSOUNDS = 0x0039,
            //SPI_SETSOUNDSENTRY = 0x0041,
            //SPI_SETSTICKYKEYS = 0x003B,
            //SPI_SETTOGGLEKEYS = 0x0035,
            //
            // Desktop parameters
            //SPI_GETCLEARTYPE = 0x1048,
            //SPI_GETDESKWALLPAPER = 0x0073,
            //SPI_GETDROPSHADOW = 0x1024,
            //SPI_GETFLATMENU = 0x1022,
            //SPI_GETFONTSMOOTHING = 0x004A,
            //SPI_GETFONTSMOOTHINGCONTRAST = 0x200C,
            //SPI_GETFONTSMOOTHINGORIENTATION = 0x2012,
            //SPI_GETFONTSMOOTHINGTYPE = 0x200A,
            //SPI_GETWORKAREA = 0x0030,
            //SPI_SETCLEARTYPE = 0x1049,
            //SPI_SETCURSORS = 0x0057,
            //SPI_SETDESKPATTERN = 0x0015,
            //SPI_SETDESKWALLPAPER = 0x0014,
            //SPI_SETDROPSHADOW = 0x1025,
            //SPI_SETFLATMENU = 0x1023,
            //SPI_SETFONTSMOOTHING = 0x004B,
            //SPI_SETFONTSMOOTHINGCONTRAST = 0x200D,
            //SPI_SETFONTSMOOTHINGORIENTATION = 0x2013,
            //SPI_SETFONTSMOOTHINGTYPE = 0x200B,
            //SPI_SETWORKAREA = 0x002F,
            //
            // Icon parameters
            //SPI_GETICONMETRICS = 0x002D,
            //SPI_GETICONTITLELOGFONT = 0x001F,
            //SPI_GETICONTITLEWRAP = 0x0019,
            //SPI_ICONHORIZONTALSPACING = 0x000D,
            //SPI_ICONVERTICALSPACING = 0x0018,
            //SPI_SETICONMETRICS = 0x002E,
            //SPI_SETICONS = 0x0058,
            //SPI_SETICONTITLELOGFONT = 0x0022,
            //SPI_SETICONTITLEWRAP = 0x001A,
            //
            // Input parameters
            //SPI_GETBEEP = 0x0001,
            //SPI_GETBLOCKSENDINPUTRESETS = 0x1026,
            //SPI_GETCONTACTVISUALIZATION = 0x2018,
            //SPI_GETDEFAULTINPUTLANG = 0x0059,
            //SPI_GETGESTUREVISUALIZATION = 0x201A,
            //SPI_GETKEYBOARDCUES = 0x100A,
            //SPI_GETKEYBOARDDELAY = 0x0016,
            //SPI_GETKEYBOARDPREF = 0x0044,
            //SPI_GETKEYBOARDSPEED = 0x000A,
            //SPI_GETMOUSE = 0x0003,
            //SPI_GETMOUSEHOVERHEIGHT = 0x0064,
            //SPI_GETMOUSEHOVERTIME = 0x0066,
            //SPI_GETMOUSEHOVERWIDTH = 0x0062,
            //SPI_GETMOUSESPEED = 0x0070,
            //SPI_GETMOUSETRAILS = 0x005E,
            //SPI_GETMOUSEWHEELROUTING = 0x201C,
            //SPI_GETPENVISUALIZATION = 0x201E,
            //SPI_GETSNAPTODEFBUTTON = 0x005F,
            //SPI_GETSYSTEMLANGUAGEBAR = 0x1050,
            //SPI_GETTHREADLOCALINPUTSETTINGS = 0x104E,
            //SPI_GETWHEELSCROLLCHARS = 0x006C,
            //SPI_GETWHEELSCROLLLINES = 0x0068,
            //SPI_SETBEEP = 0x0002,
            //SPI_SETBLOCKSENDINPUTRESETS = 0x1027,
            //SPI_SETCONTACTVISUALIZATION = 0x2019,
            //SPI_SETDEFAULTINPUTLANG = 0x005A,
            //SPI_SETDOUBLECLICKTIME = 0x0020,
            //SPI_SETDOUBLECLKHEIGHT = 0x001E,
            //SPI_SETDOUBLECLKWIDTH = 0x001D,
            //SPI_SETGESTUREVISUALIZATION = 0x201B,
            //SPI_SETKEYBOARDCUES = 0x100B,
            //SPI_SETKEYBOARDDELAY = 0x0017,
            //SPI_SETKEYBOARDPREF = 0x0045,
            //SPI_SETKEYBOARDSPEED = 0x000B,
            //SPI_SETLANGTOGGLE = 0x005B,
            //SPI_SETMOUSE = 0x0004,
            //SPI_SETMOUSEBUTTONSWAP = 0x0021,
            //SPI_SETMOUSEHOVERHEIGHT = 0x0065,
            //SPI_SETMOUSEHOVERTIME = 0x0067,
            //SPI_SETMOUSEHOVERWIDTH = 0x0063,
            //SPI_SETMOUSESPEED = 0x0071,
            //SPI_SETMOUSETRAILS = 0x005D,
            //SPI_SETMOUSEWHEELROUTING = 0x201D,
            //SPI_SETPENVISUALIZATION = 0x201F,
            //SPI_SETSNAPTODEFBUTTON = 0x0060,
            //SPI_SETSYSTEMLANGUAGEBAR = 0x1051,
            //SPI_SETTHREADLOCALINPUTSETTINGS = 0x104F,
            //SPI_SETWHEELSCROLLCHARS = 0x006D,
            //SPI_SETWHEELSCROLLLINES = 0x0069,
            //
            // Menu parameters
            //SPI_GETMENUDROPALIGNMENT = 0x001B,
            //SPI_GETMENUFADE = 0x1012,
            //SPI_GETMENUSHOWDELAY = 0x006A,
            //SPI_SETMENUDROPALIGNMENT = 0x001C,
            //SPI_SETMENUFADE = 0x1013,
            //SPI_SETMENUSHOWDELAY = 0x006B,
            //
            // Power parameters
            //SPI_GETLOWPOWERACTIVE = 0x0053,
            //SPI_GETLOWPOWERTIMEOUT = 0x004F,
            //SPI_GETPOWEROFFACTIVE = 0x0054,
            //SPI_GETPOWEROFFTIMEOUT = 0x0050,
            //SPI_SETLOWPOWERACTIVE = 0x0055,
            //SPI_SETLOWPOWERTIMEOUT = 0x0051,
            //SPI_SETPOWEROFFACTIVE = 0x0056,
            //SPI_SETPOWEROFFTIMEOUT = 0x0052,
            //
            // Screen saver parameters
            //SPI_GETSCREENSAVEACTIVE = 0x0010,
            //SPI_GETSCREENSAVERRUNNING = 0x0072,
            //SPI_GETSCREENSAVESECURE = 0x0076,
            //SPI_GETSCREENSAVETIMEOUT = 0x000E,
            //SPI_SETSCREENSAVEACTIVE = 0x0011,
            //SPI_SETSCREENSAVESECURE = 0x0077,
            //SPI_SETSCREENSAVETIMEOUT = 0x000F,
            //
            // Time-out parameters
            //SPI_GETHUNGAPPTIMEOUT = 0x0078,
            //SPI_GETWAITTOKILLTIMEOUT = 0x007A,
            //SPI_GETWAITTOKILLSERVICETIMEOUT = 0x007C,
            //SPI_SETHUNGAPPTIMEOUT = 0x0079,
            //SPI_SETWAITTOKILLTIMEOUT = 0x007B,
            //SPI_SETWAITTOKILLSERVICETIMEOUT = 0x007D,
            //
            // UI effect parameters
            //SPI_GETCOMBOBOXANIMATION = 0x1004,
            //SPI_GETCURSORSHADOW = 0x101A,
            //SPI_GETGRADIENTCAPTIONS = 0x1008,
            //SPI_GETHOTTRACKING = 0x100E,
            //SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006,
            //SPI_GETMENUANIMATION = 0x1002,
            //SPI_GETMENUUNDERLINES = 0x100A,
            //SPI_GETSELECTIONFADE = 0x1014,
            //SPI_GETTOOLTIPANIMATION = 0x1016,
            //SPI_GETTOOLTIPFADE = 0x1018,
            //SPI_GETUIEFFECTS = 0x103E,
            //SPI_SETCOMBOBOXANIMATION = 0x1005,
            //SPI_SETCURSORSHADOW = 0x101B,
            //SPI_SETGRADIENTCAPTIONS = 0x1009,
            //SPI_SETHOTTRACKING = 0x100F,
            //SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007,
            //SPI_SETMENUANIMATION = 0x1003,
            //SPI_SETMENUUNDERLINES = 0x100B,
            //SPI_SETSELECTIONFADE = 0x1015,
            //SPI_SETTOOLTIPANIMATION = 0x1017,
            //SPI_SETTOOLTIPFADE = 0x1019,
            //SPI_SETUIEFFECTS = 0x103F,
            //
            // Window parameters
            //SPI_GETACTIVEWINDOWTRACKING = 0x1000,
            //SPI_GETACTIVEWNDTRKZORDER = 0x100C,
            //SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002,
            //SPI_GETANIMATION = 0x0048,
            //SPI_GETBORDER = 0x0005,
            //SPI_GETCARETWIDTH = 0x2006,
            //SPI_GETDOCKMOVING = 0x0090,
            //SPI_GETDRAGFROMMAXIMIZE = 0x008C,
            //SPI_GETDRAGFULLWINDOWS = 0x0026,
            //SPI_GETFOREGROUNDFLASHCOUNT = 0x2004,
            //SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000,
            //SPI_GETMINIMIZEDMETRICS = 0x002B,
            //SPI_GETMOUSEDOCKTHRESHOLD = 0x007E,
            //SPI_GETMOUSEDRAGOUTTHRESHOLD = 0x0084,
            //SPI_GETMOUSESIDEMOVETHRESHOLD = 0x0088,
            //SPI_GETNONCLIENTMETRICS = 0x0029,
            //SPI_GETPENDOCKTHRESHOLD = 0x0080,
            //SPI_GETPENDRAGOUTTHRESHOLD = 0x0086,
            //SPI_GETPENSIDEMOVETHRESHOLD = 0x008A,
            //SPI_GETSHOWIMEUI = 0x006E,
            //SPI_GETSNAPSIZING = 0x008E,
            //SPI_GETWINARRANGING = 0x0082,
            //SPI_SETACTIVEWINDOWTRACKING = 0x1001,
            //SPI_SETACTIVEWNDTRKZORDER = 0x100D,
            //SPI_SETACTIVEWNDTRKTIMEOUT = 0x2003,
            //SPI_SETANIMATION = 0x0049,
            //SPI_SETBORDER = 0x0006,
            //SPI_SETCARETWIDTH = 0x2007,
            //SPI_SETDOCKMOVING = 0x0091,
            //SPI_SETDRAGFROMMAXIMIZE = 0x008D,
            //SPI_SETDRAGFULLWINDOWS = 0x0025,
            //SPI_SETDRAGHEIGHT = 0x004D,
            //SPI_SETDRAGWIDTH = 0x004C,
            //SPI_SETFOREGROUNDFLASHCOUNT = 0x2005,
            //SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001,
            //SPI_SETMINIMIZEDMETRICS = 0x002C,
            //SPI_SETMOUSEDOCKTHRESHOLD = 0x007F,
            //SPI_SETMOUSEDRAGOUTTHRESHOLD = 0x0085,
            //SPI_SETMOUSESIDEMOVETHRESHOLD = 0x0089,
            //SPI_SETNONCLIENTMETRICS = 0x002A,
            //SPI_SETPENDOCKTHRESHOLD = 0x0081,
            //SPI_SETPENDRAGOUTTHRESHOLD = 0x0087,
            //SPI_SETPENSIDEMOVETHRESHOLD = 0x008B,
            //SPI_SETSHOWIMEUI = 0x006F,
            //SPI_SETSNAPSIZING = 0x008F,
            //SPI_SETWINARRANGING = 0x0083,
        }

        // NOTE: HIGHCONTRAST is used by SystemParametersInfo
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct HIGHCONTRAST
        {
            public UInt32 cbSize;
            public UInt32 dwFlags;
            // NOTE: to prevent heap corruption, we manually Marshal the lpszDefaultScheme to/from a pointer (instead of using MarshalAs(UnmanagedType.LPTStr))
            public IntPtr lpszDefaultScheme;

            public HIGHCONTRAST(UInt32 dwFlags, IntPtr lpszDefaultScheme)
            {
                this.dwFlags = dwFlags;
                this.lpszDefaultScheme = lpszDefaultScheme;
                //
                this.cbSize = (UInt32)Marshal.SizeOf<HIGHCONTRAST>();
            }
        }

        // NOTE: HighContrastFlags is used by HIGHCONTRAST which is used by SystemParametersInfo
        [Flags]
        internal enum HighContrastFlags : UInt32
        {
            HCF_HIGHCONTRASTON = 0x00000001,
            HCF_AVAILABLE = 0x00000002,
            HCF_HOTKEYACTIVE = 0x00000004,
            HCF_CONFIRMHOTKEY = 0x00000008,
            HCF_HOTKEYSOUND = 0x00000010,
            HCF_INDICATOR = 0x00000020,
            HCF_HOTKEYAVAILABLE = 0x00000040,
            NoFlagNameSupported = 0x00001000,
        }

        // WinUser.h (Windows 10 SDK v10.0.18632)
        [Flags]
        internal enum SPIF: UInt32
        {
            SPIF_UPDATEINIFILE = 0x0001,
            SPIF_SENDWININICHANGE = 0x0002,
            SPIF_SENDCHANGE = SPIF_SENDWININICHANGE,
        }

        #endregion

        private const Int32 CCHDEVICENAME = 32;
        private const Int32 CCHFORMNAME = 32;

        // DEVMODEW is used by EnumDisplaySettingsEx and other functions
        // https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-devmodew
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DEVMODEW
        {
            private const Int32 privateDriverDataLength = 0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CCHDEVICENAME)]
            public Char[] dmDeviceName;
            public UInt16 dmSpecVersion;
            public UInt16 dmDriverVersion;
            public UInt16 dmSize;
            public UInt16 dmDriverExtra;
            public DM_FieldSelectionBit dmFields;
            public DEVMODEW__DUMMYUNIONNAME DUMMYUNIONNAME;
            public Int16 dmColor;
            public Int16 dmDuplex;
            public Int16 dmYResolution;
            public Int16 dmTTOption;
            public Int16 dmCollate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CCHFORMNAME)]
            public Char[] dmFormName;
            public UInt16 dmLogPixels;
            public UInt32 dmBitsPerPel;
            public UInt32 dmPelsWidth;
            public UInt32 dmPelsHeight;
            public DEVMODEW__DUMMYUNIONNAME2 DUMMYUNIONNAME2;
            public UInt32 dmDisplayFrequency;
            public UInt32 dmICMMethod;
            public UInt32 dmICMIntent;
            public UInt32 dmMediaType;
            public UInt32 dmDitherType;
            public UInt32 dmReserved1;
            public UInt32 dmReserved2;
            public UInt32 dmPanningWidth;
            public UInt32 dmPanningHeight;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = privateDriverDataLength)]
            //public Byte[] privateDriverData;

            public void Init()
            {
                this.dmDeviceName = new Char[CCHDEVICENAME];
                this.dmFormName = new Char[CCHFORMNAME];
                //this.privateDriverData = new byte[privateDriverDataLength];
                this.dmDriverExtra = privateDriverDataLength;
                this.dmSize = (UInt16)Marshal.SizeOf(typeof(DEVMODEW));
            }
        }
        //
        [StructLayout(LayoutKind.Explicit)]
        internal struct DEVMODEW__DUMMYUNIONNAME
        {
            [FieldOffset(0)]
            public DEVMODEW__DUMMYUNIONNAME__DUMMYSTRUCTNAME DUMMYSTRUCTNAME;
            //
            [FieldOffset(0)]
            public POINTL dmPosition;
            //
            [FieldOffset(0)]
            public DEVMODEW__DUMMYUNIONNAME__DUMMYSTRUCTNAME2 DUMMYSTRUCTNAME2;

            [StructLayout(LayoutKind.Sequential)]
            public struct DEVMODEW__DUMMYUNIONNAME__DUMMYSTRUCTNAME
            {
                public Int16 dmOrientation;
                public Int16 dmPaperSize;
                public Int16 dmPaperLength;
                public Int16 dmPaperWidth;
                public Int16 dmScale;
                public Int16 dmCopies;
                public Int16 dmDefaultSource;
                public Int16 dmPrintQuality;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DEVMODEW__DUMMYUNIONNAME__DUMMYSTRUCTNAME2
            {
                public POINTL dmPosition;
                public UInt32 dmDisplayOrientation;
                public UInt32 dmDisplayFixedOutput;
            }
        }
        //
        [StructLayout(LayoutKind.Explicit)]
        internal struct DEVMODEW__DUMMYUNIONNAME2
        {
            [FieldOffset(0)]
            public UInt32 dmDisplayFlags;
            //
            [FieldOffset(0)]
            public UInt32 dmNup;
        }

        // https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-pointl
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTL
        {
            public Int32 x;
            public Int32 y;
        }

        // wingdi.h (Windows 10 SDK v10.0.18632)
        internal enum DM_FieldSelectionBit : UInt32
        {
            DM_ORIENTATION = 0x0000_0001,
            DM_PAPERSIZE = 0x0000_0002,
            DM_PAPERLENGTH = 0x0000_0004,
            DM_PAPERWIDTH = 0x0000_0008,
            DM_SCALE = 0x0000_0010,
            DM_POSITION = 0x0000_0020,
            DM_NUP = 0x0000_0040,
            DM_DISPLAYORIENTATION = 0x0000_0080,
            DM_COPIES = 0x0000_0100,
            DM_DEFAULTSOURCE = 0x0000_0200,
            DM_PRINTQUALITY = 0x0000_0400,
            DM_COLOR = 0x0000_0800,
            DM_DUPLEX = 0x0000_1000,
            DM_YRESOLUTION = 0x0000_2000,
            DM_TTOPTION = 0x0000_4000,
            DM_COLLATE = 0x0000_8000,
            DM_FORMNAME = 0x0001_0000,
            DM_LOGPIXELS = 0x0002_0000,
            DM_BITSPERPEL = 0x0004_0000,
            DM_PELSWIDTH = 0x0008_0000,
            DM_PELSHEIGHT = 0x0010_0000,
            DM_DISPLAYFLAGS = 0x0020_0000,
            DM_DISPLAYFREQUENCY = 0x0040_0000,
            DM_ICMMETHOD = 0x0080_0000,
            DM_ICMINTENT = 0x0100_0000,
            DM_MEDIATYPE = 0x0200_0000,
            DM_DITHERTYPE = 0x0400_0000,
            DM_PANNINGWIDTH = 0x0800_0000,
            DM_PANNINGHEIGHT = 0x1000_0000,
            DM_DISPLAYFIXEDOUTPUT = 0x2000_0000,
        }

        // WinUser.h (Windows 10 SDK v10.0.18632)
        internal static readonly UInt32 ENUM_CURRENT_SETTINGS = BitConverter.ToUInt32(BitConverter.GetBytes((Int32)(-1)));
        //internal static readonly UInt32 ENUM_REGISTRY_SETTINGS = BitConverter.ToUInt32(BitConverter.GetBytes((Int32)(-2)));

        // WinUser.h (Windows 10 SDK v10.0.18632)
        internal enum DISP_CHANGE_RESULT: Int32
        {
            DISP_CHANGE_BADDUALVIEW = -6,
            DISP_CHANGE_BADPARAM = -5,
            DISP_CHANGE_BADFLAGS = -4,
            DISP_CHANGE_NOTUPDATED = -3,
            DISP_CHANGE_BADMODE = -2,
            DISP_CHANGE_FAILED = -1,
            DISP_CHANGE_SUCCESSFUL = 0,
            DISP_CHANGE_RESTART = 1
        }

        internal const UInt32 EDD_GET_DEVICE_INTERFACE_NAME = 0x00000001;

        // DISPLAY_DEVICEW is used by EnumDisplayDevices
        // https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-display_devicew
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DISPLAY_DEVICEW
        {
            public UInt32 cb;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public Char[] DeviceName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public Char[] DeviceString;
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public Char[] DeviceID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public Char[] DeviceKey;

            public void Init()
            {
                this.DeviceName = new char[32];
                this.DeviceString = new char[128];
                this.StateFlags = 0;
                this.DeviceID = new char[128];
                this.DeviceKey = new char[128];
                this.cb = (UInt32)Marshal.SizeOf(typeof(DISPLAY_DEVICEW));
            }
        }

        // https://docs.microsoft.com/en-us/windows/win32/etw/systemconfig-video
        internal enum DisplayDeviceStateFlags : UInt32
        {
            DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x1,
            DISPLAY_DEVICE_PRIMARY_DEVICE = 0x4,
            DISPLAY_DEVICE_MIRRORING_DRIVER = 0x8,
            //DISPLAY_DEVICE_VGA_COMPATIBLE = 0x10,
            //DISPLAY_DEVICE_REMOVABLE = 0x20,
            //DISPLAY_DEVICE_MODESPRUNED = 0x8000000
        }
        // windgi.h (Windows 10 SDK v10.0.18632)
        internal enum ChildDisplayDeviceStateFlags: UInt32
        {
            DISPLAY_DEVICE_ACTIVE = 0x1,
            DISPLAY_DEVICE_ATTACHED = 0x2
        }

        internal const UInt32 MONITORINFOF_PRIMARY = 1;

        // NOTE: MONITORINFOEX is used by the GetMonitorInfo function
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-monitorinfoexa
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MONITORINFOEXA
        {
            public UInt32 cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public UInt32 dwFlags;
            // NOTE: szDevice must be marshalled as a ByValArray instead of a ByValTString so that Marshal.SizeOf can calculate a value
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CCHDEVICENAME)]
            public Char[] szDevice;

            public void Init()
            {
                this.rcMonitor = new RECT();
                this.rcWork = new RECT();
                this.dwFlags = 0;
                this.szDevice = new Char[CCHDEVICENAME];
                this.cbSize = (UInt32)Marshal.SizeOf(typeof(MONITORINFOEXA));
            }
        }

        // NOTE: RECT is used by multiple functions including EnumDisplayMonitor's callbacks
        // https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect
        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }

        // NOTE: this delegate is used as a callback by EnumDisplayMonitors
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-monitorenumproc
        internal delegate Boolean MonitorEnumProcDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        /* kernel32 */
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-getprivateprofilestringw
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern UInt32 GetPrivateProfileString(String lpAppName, String lpKeyName, String lpDefault, IntPtr lpReturnedString, UInt32 size, String lpFileName);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-getsysteminfo
        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-writeprivateprofilesectionw
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern Boolean WritePrivateProfileSection(String lpAppName, String lpString, String lpFileName);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-writeprofilestringw
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern Boolean WritePrivateProfileString(String lpAppName, String lpKeyName, String? lpString, String lpFileName);

        internal enum ChangeDisplaySettingsFlags: UInt32
        {
            //CDS_UPDATEREGISTRY = 0x00000001,
            CDS_TEST = 0x00000002,
            //CDS_FULLSCREEN = 0x00000004,
            //CDS_GLOBAL = 0x00000008,
            //CDS_SET_PRIMARY = 0x00000010,
            //CDS_VIDEOPARAMETERS = 0x00000020,
            //CDS_ENABLE_UNSAFE_MODES = 0x00000100,
            //CDS_DISABLE_UNSAFE_MODES = 0x00000200,
            //CDS_RESET = 0x40000000,
            //CDS_RESET_EX = 0x20000000,
            //CDS_NORESET = 0x10000000,
        }

        // Windows error codes
        private const Int32 ERROR_NOT_FOUND = 0x00000490;

        // https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/0c0bcf55-277e-4120-b5dc-f6115fc8dc38
        private static Int32 HRESULT_FROM_WIN32(Int32 x)
        {
            Int32 FACILITY_WIN32 = 7;
            return unchecked((Int32)(x) <= 0 ? ((Int32)(x)) : ((Int32)(((x) & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000)));
        }

        // HRESULT values
        internal const Int32 S_OK = 0x00000000;
        internal const Int32 E_INVALIDARG = unchecked((Int32)0x80070057);
        internal static Int32 E_NOTFOUND = HRESULT_FROM_WIN32(ERROR_NOT_FOUND);
        internal const Int32 E_OUTOFMEMORY = unchecked((Int32)0x8007000E);
        internal const Int32 E_POINTER = unchecked((Int32)0x80004003);

        /* shlwapi.dll */
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-shloadindirectstring
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 SHLoadIndirectString(String pszSource, IntPtr pszOutBuf, UInt32 cchOutBuf, IntPtr ppvReserved);

        /* user32 */
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsexw
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern DISP_CHANGE_RESULT ChangeDisplaySettingsEx(Char[] lpszDeviceName, ref DEVMODEW lpDevMode, IntPtr hwnd, UInt32 dwflags, IntPtr lParam);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaydevicesw
        [DllImport("user32.dll", EntryPoint = "EnumDisplayDevices", CharSet = CharSet.Unicode)]
        internal static extern Boolean EnumDisplayDevices_DisplayAdapter(IntPtr lpDeviceAsZeroPtr, UInt32 iDevNum, ref DISPLAY_DEVICEW lpDisplayDevice, UInt32 dwFlags);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaydevicesw
        [DllImport("user32.dll", EntryPoint = "EnumDisplayDevices", CharSet = CharSet.Unicode)]
        internal static extern Boolean EnumDisplayDevices_Monitor(Char[] lpDevice, UInt32 iDevNum, ref DISPLAY_DEVICEW lpDisplayDevice, UInt32 dwFlags);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaysettingsexw
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern Boolean EnumDisplaySettingsEx(Char[] lpszDeviceName, UInt32 iModeNum, ref DEVMODEW lpDevMode, UInt32 dwFlags);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaymonitors
        [DllImport("user32.dll")]
        internal static extern Boolean EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProcDelegate lpfnEnum, IntPtr dwData);
        // 
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow
        [DllImport("user32.dll")]
        internal static extern UInt32 GetDpiForWindow(IntPtr hwnd);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getmonitorinfoa
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern Boolean GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEXA lpmi);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        [DllImport("user32.dll")] 
        internal static extern Int32 GetSystemMetrics(SystemMetricIndex smIndex);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetricsfordpi
        [DllImport("user32.dll")]
        internal static extern Int32 GetSystemMetricsForDpi(SystemMetricIndex nIndex, UInt32 dpi);
        //
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-systemparametersinfow
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern Boolean SystemParametersInfo(UIActionParameter uiAction, UInt32 uiParam, IntPtr pvParam, SPIF fWinIni);
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern Boolean SystemParametersInfo_HIGHCONTRAST(UIActionParameter uiAction, UInt32 uiParam, ref HIGHCONTRAST pvParam, SPIF fWinIni);

    }
}
