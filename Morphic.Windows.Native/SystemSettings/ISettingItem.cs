using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Morphic.Windows.Native.SystemSettings
{
    /* NOTE: this interface was implemented using metadata extracted by OleView 
     * - OleView: C:\Program Files (x86)\Windows Kits\10\bin\10.0.18362.0\x64\oleview.exe (run as Admin) [not required, as OleView .NET can do the basics plus more]
     * - OleView .NET: https://github.com/tyranid/oleviewdotnet
     *
     * These tools only provide the implementation details of classes as the method and parameter names are stripped out by the compiler; 
     * the descriptive method names (kept in the same order, since binding is by method order) and parameter names are ours
     * 
     * Additional inspection was done using the following tools:
     * - dumpbin: C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Tools\MSVC\14.25.28610\bin\Hostx64\x64\dumpbin.exe (using the /exports flag on SettingsHandler_nt.dll and similar files)
     * - AndreyBazhan's SymStore Tools and PowerShell Scripts (to download Windows symbols from Microsoft's cloud service): https://github.com/AndreyBazhan/SymStore
     * - symchk: C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symchk (installed with the "Debugging Tools for Windows" option inside the Windows 10 SDK 10.0.18362.1)
     * - Ghidra (US government open source tool): https://ghidra-sre.org/
     * vvv NOT YET NEEDED vvv
     * - symstore: C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore (installed with the "Debugging Tools for Windows" option inside the Windows 10 SDK 10.0.18362.1)
     * - Debugging Tools for Windows (WinDbg): Windows 10 SDK 10.0.18362.1
     * - WinDbg Preview: Microsoft Store
     */
    // our C# implementation of interface SystemSettings::DataModel::ISettingItem : IInspectable
    [ComImport]
    [Guid("40C037CC-D8BF-489E-8697-D66BAA3221BF")]
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    internal interface ISettingItem
    {
        // Proc6
        //HRESULT Proc6(/* Stack Offset: 8 */ [Out] HSTRING* p0);
        //public Int32 Proc6([MarshalAs(UnmanagedType.HString)] out String p0);

        // Proc7
        //HRESULT Proc7(/* Stack Offset: 8 */ [Out] /* ENUM32 */ int* p0);
        //public Int32 Proc7(out Int32 p0);

        // Proc8
        //HRESULT Proc8(/* Stack Offset: 8 */ [Out] sbyte* p0);
        //public Int32 Proc8(out SByte p0);

        // Proc9
        //HRESULT Proc9(/* Stack Offset: 8 */ [Out] sbyte* p0);
        //public Int32 Proc9(out SByte p0);

        // Proc10
        //HRESULT Proc10(/* Stack Offset: 8 */ [Out] sbyte* p0);
        //public Int32 Proc10(out SByte p0);

        // Proc11
        //HRESULT Proc11(/* Stack Offset: 8 */ [Out] HSTRING* p0);
        //public Int32 Proc11([MarshalAs(UnmanagedType.HString)] out String p0);

        // Proc12
        //HRESULT Proc12(/* Stack Offset: 8 */ [Out] sbyte* p0);
        //public Int32 Proc12(out SByte p0);

        // Proc13
        //HRESULT Proc13(/* Stack Offset: 8 */ [In] HSTRING p0, /* Stack Offset: 16 */ [Out] IInspectable** p1);
        //public Int32 Proc13([MarshalAs(UnmanagedType.HString)] out String p0, [MarshalAs(UnmanagedType.IInspectable)] out Object? p1);

        // Proc14
        //HRESULT Proc14(/* Stack Offset: 8 */ [In] HSTRING p0, /* Stack Offset: 16 */ [In] IInspectable* p1);
        //public Int32 Proc14([MarshalAs(UnmanagedType.HString)] in String p0, [MarshalAs(UnmanagedType.IInspectable)] in Object? p1);

        // Proc15
        //HRESULT Proc15(/* Stack Offset: 8 */ [In] HSTRING p0, /* Stack Offset: 16 */ [Out] IInspectable** p1);
        //public Int32 Proc15([MarshalAs(UnmanagedType.HString)] in String p0, [MarshalAs(UnmanagedType.IInspectable)] out Object? p1);

        // Proc16
        //HRESULT Proc16(/* Stack Offset: 8 */ [In] HSTRING p0, /* Stack Offset: 16 */ [In] IInspectable* p1);
        //public Int32 Proc16([MarshalAs(UnmanagedType.HString)] in String p0, [MarshalAs(UnmanagedType.IInspectable)] in Object? p1);

        // Proc17
        //HRESULT Proc17(/* Stack Offset: 8 */ [In] Windows::UI::Core::ICoreWindow* p0, /* Stack Offset: 16 */ [In] struct Struct_3* p1);
        //public Int32 Proc17(in Object? p0, in object? p1);

        // Proc18
        //HRESULT Proc18(/* Stack Offset: 8 */ [In] ITypedEventHandler<IInspectable, HSTRING>* p0, /* Stack Offset: 16 */ [Out] struct Struct_2* p1);
        //public Int32 Proc18(in Object? p0, out Object? p1);

        // Proc19
        //HRESULT Proc19(/* Stack Offset: 8 */ [In] struct Struct_2 p0);
        //public Int32 Proc19(in Object? p0);
    }
}
