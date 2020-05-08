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

using Morphic.Windows.Native.SystemSettings;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/*** THIS SOURCE FILE IS A WORK IN PROGRESS ***/

namespace Morphic.Windows.Native.Utilities
{
    public class SystemSettingIdNotFound: Exception
    {
        public String SettingId;

        public SystemSettingIdNotFound(String settingId)
        {
            this.SettingId = settingId;
        }
    }

    // TODO: should this be renamed to "SystemSettings"?
    public static class SystemSettingsUtility
    {
        // NOTE: parameter 'settingId' is assumed from the function being called; its type is known because the function uses a WindowsGetStringRawBuffer
        // NOTE: parameter 'pointerToSettingItem' is derived from the trace of the GetSetting function (which casts 
        // TODO: do we need to specify the calling convention as Winapi (or will .NET default to Winapi?)
        internal delegate UIntPtr GetSettingDelegate([MarshalAs(UnmanagedType.HString)] string settingId, out IntPtr pointerToSettingItem);
        // NOTE: according to Ghidra disassembly, GetSetting only has two parameters (not a third IntPtr parameter)
        //[UnmanagedFunctionPointer(CallingConvention.Winapi)] // Winapi = "use the default calling convention"
        //internal delegate IntPtr GetSettingDelegate([MarshalAs(UnmanagedType.HString)] string settingId, out ISettingItem settingItem, IntPtr n);

        // NOTE: set AutoUnloadLibraries to "true" to unload libraries after settings are retrieved or set
        public static Boolean AutoUnloadLibraries = false;
        // NOTE: set CacheSystemSettingMetadata to "false" to prevent caching of the DllName, Type, etc. for SystemSettings (retrieved from the registry)
        public static Boolean CacheSystemSettingMetadata = true;

        // throws SystemSettingIdNotFound if the settingId is not present as an option in the system registry
        // throws InvalidCastException if the settingId metadata in the registry is of an invalid type (or if the SystemSetting's Type is of an unknown type)
        public static Object GetSystemSettingValue(SystemSetting systemSetting)
        {
            // retrieve the setting's DllPath and Type from the system registry
            UpdateSystemSettingMetadataCache(ref systemSetting);

            // TODO: retrieve the SystemSetting's value via a call to the undocumented SystemSettings API

            // temporary code: do not use in production
            //
            // load the library
            IntPtr handleToLoadedModule = WindowsApi.LoadLibraryEx(systemSetting.DllPath!, IntPtr.Zero, 0);
            if (handleToLoadedModule == IntPtr.Zero)
            {
                // TODO: throw a better exception
                throw new Exception("Could not load SystemSetting's handler dll");
            }
            //
            // get the procedure address of the "GetSetting" function
            IntPtr pointerToGetSettingFunction = WindowsApi.GetProcAddress(handleToLoadedModule, "GetSetting");
            if (pointerToGetSettingFunction == IntPtr.Zero)
            {
                // TODO: throw a better exception
                throw new Exception("Could not retrieve GetSetting function pointer from SystemSetting's handler dll");
            }
            //
            // Convert the unmanaged function pointer to a managed delegate
            var getSettingDelegate = Marshal.GetDelegateForFunctionPointer<GetSettingDelegate>(pointerToGetSettingFunction);
            //
            //ISettingItem? settingItem;
            IntPtr pointerToSettingItem;
            // TODO: DEBUG INTO the GetSetting function on multiple DLLs (critically both SettingsHandlers_nt.dll and SettingHandlers_Accessibility.dll) to understand the return value and also the second (output) param
            UIntPtr getSettingFunctionPointerResult = getSettingDelegate(systemSetting.SettingId!, out pointerToSettingItem); // , IntPtr.Zero);
            if (getSettingFunctionPointerResult != UIntPtr.Zero)
            {
                // TODO: throw a better exception
                throw new Exception("Could not call the GetSetting function of SystemSetting's handler dll");
            }
            Object obj2 = Marshal.GetObjectForIUnknown(pointerToSettingItem);
            ISettingItem? settingItem = obj2 as ISettingItem;
            //ISettingItem? settingItem = obj2 as ISettingItem_Alt;

            //if (settingItem == null)
            //{
            //    // TODO: throw a better exception
            //    throw new Exception("The GetSetting function of SystemSetting's handler dll did not return a value");
            //}
            //// TODO: think about wiring up a delegate for when the "setting changes" [as stegru's code did]
            //// settingItem.SettingChanged += SettingItem_SettingChanged;

			// TODO: NOTE: this code has omitted unloading of libraries (as well as any flags on LoadLibraryEx)

            throw new NotImplementedException();
        }

        public static List<(SystemSetting systemSetting, Object value, Exception exception)> GetSystemSettingsValues(List<SystemSetting> systemSettings)
        {
            // TODO: make sure we unload all the libraries AFTER getting all the settings (as an optimization)
            throw new NotImplementedException();
        }

        // throws SystemSettingIdNotFound if the settingId is not present as an option in the system registry
        // throws InvalidCastException if the settingId metadata in the registry is of an invalid type (or if the SystemSetting's Type is of an unknown type)
        public static void SetSystemSettingValue(SystemSetting systemSetting, Object value)
        {
            // retrieve the setting's DllPath and Type from the system registry
            UpdateSystemSettingMetadataCache(ref systemSetting);

            // TODO: verify that the passed-in value as the systemSetting's SettingType are compatible

            // TODO: set the SystemSetting's value via a call to the undocumented SystemSettings API

            throw new NotImplementedException();
        }

        public static void SetSystemSettingsValues(List<(SystemSetting systemSetting, Object value)> systemSettingsWithValues)
        {
            throw new NotImplementedException();
        }

        /* helper functions */

        private static SystemSettingType? ConvertStringToSystemSettingType(String settingTypeAsString)
        {
            switch (settingTypeAsString)
            {
                case "Boolean":
                    return SystemSettingType.Boolean;
                case "DisplayString":
                    return SystemSettingType.DisplayString;
                default:
                    return null;
            }
        }

        // throws SystemSettingIdNotFound if the settingId is not present as an option in the system registry
        // throws InvalidCastException if the settingId metadata in the registry is of an invalid type (or if the SystemSetting's Type is of an unknown type)
        // TODO: verify that this data is passed in by-value (or use ref, etc.)
        private static void UpdateSystemSettingMetadataCache(ref SystemSetting systemSetting)
        {
            if (SystemSettingsUtility.CacheSystemSettingMetadata == true)
            {
                if (systemSetting.DllPath != null && systemSetting.SettingType != null)
                {
                    return;
                }
            }

            String registrySubKeyPath = @"SOFTWARE\Microsoft\SystemSettings\SettingId\" + systemSetting.SettingId;

            Object? dllPathAsNullableObject = RegistryUtility.GetRegistryValueData(Microsoft.Win32.Registry.LocalMachine, registrySubKeyPath, "DllPath");
            if (dllPathAsNullableObject == null)
            {
                throw new SystemSettingIdNotFound(systemSetting.SettingId);
            }
            if (dllPathAsNullableObject!.GetType() != typeof(String))
            {
                throw new InvalidCastException("Registry value data is of the incorrect type, should be of type 'String'.");
            }
            systemSetting.DllPath = (String)(dllPathAsNullableObject!);
            
            Object? settingTypeAsNullableObject = RegistryUtility.GetRegistryValueData(Microsoft.Win32.Registry.LocalMachine, registrySubKeyPath, "Type");
            if (settingTypeAsNullableObject == null)
            {
                throw new SystemSettingIdNotFound(systemSetting.SettingId);
            }
            if (settingTypeAsNullableObject!.GetType() != typeof(String))
            {
                throw new InvalidCastException("Registry value data is of the incorrect type, should be of type 'String'.");
            }
            var settingTypeAsNullable = ConvertStringToSystemSettingType((String)(settingTypeAsNullableObject!));
            if (settingTypeAsNullable == null)
            {
                // unknown setting type
                throw new InvalidCastException("SystemSetting is of an unknown Type.");
            }
            systemSetting.SettingType = settingTypeAsNullable!.Value;
        }

    }
}
