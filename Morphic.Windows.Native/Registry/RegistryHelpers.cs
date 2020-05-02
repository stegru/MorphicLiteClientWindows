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

using Microsoft.Win32;
using System;

namespace Morphic.Windows.Native
{
    public static class RegistryHelpers
    {
        #region Public functions to get/set registry key values

        // returns null if the value does not exist at the specified key path
        // throws RegistryKeyNotFoundException if the registry key was not found
        public static Object? GetRegistryValueData(RegistryKey baseKey, string subKeyPath, String valueName)
        {
            var registryKey = baseKey.OpenSubKey(subKeyPath);
            if (registryKey == null)
            {
                throw new RegistryKeyNotFoundException();
            }

            // get the value (data)
            var valueAsObject = registryKey.GetValue(valueName);
            if (valueAsObject == null)
            {
                // if the key does not exist, return null
                return null;
            }

            // WORKAROUND: RegistryKey returns DWORDs as Int32s (instead of as the unsigned UInt32s that they are); correct this by casting them to UInt32s
            if (valueAsObject is Int32)
            {
                var valueAsInt32 = (Int32)valueAsObject;
                var valueAsBytes = BitConverter.GetBytes(valueAsInt32);
                var valueAsUInt32 = BitConverter.ToUInt32(valueAsBytes);
                valueAsObject = valueAsUInt32;
            }
            // WORKAROUND: RegistryKey returns QWORDs as Int62s (instead of as the unsigned UInt64s that they are); correct this by casting them to UInt64s
            if (valueAsObject is Int64)
            {
                var valueAsInt64 = (Int64)valueAsObject;
                var valueAsBytes = BitConverter.GetBytes(valueAsInt64);
                var valueAsUInt64 = BitConverter.ToUInt64(valueAsBytes);
                valueAsObject = valueAsUInt64;
            }

            return valueAsObject;
        }

        // NOTE: this function attempts to create the registry key designated by subKeyPath if the subKey does not already exist
        // throws ArgumentException if any argument is null or invalid
        // throws InvalidCastException if the valueData is not a supported registry data type
        public static void SetRegistryValueData(RegistryKey baseKey, String subKeyPath, String valueName, Object valueData)
        {
            if (baseKey == null) { throw new ArgumentNullException(nameof(baseKey)); }
            if (subKeyPath == null) { throw new ArgumentNullException(nameof(subKeyPath)); }
            if (valueName == null) { throw new ArgumentNullException(nameof(valueName)); }
            if (valueData == null) { throw new ArgumentNullException(nameof(valueData)); }

            // capture the type of valueData and specify the RegistryValueKind accordingly (as an extra level of safety)
            var typeOfValueData = valueData.GetType();
            RegistryValueKind registryValueKind;
            if (typeOfValueData == typeof(UInt32))
            {
                registryValueKind = RegistryValueKind.DWord;
            }
            else if (typeOfValueData == typeof(Int32))
            {
                registryValueKind = RegistryValueKind.DWord;
            }
            else if (typeOfValueData == typeof(UInt64))
            {
                registryValueKind = RegistryValueKind.QWord;
            }
            else if (typeOfValueData == typeof(Int64))
            {
                registryValueKind = RegistryValueKind.QWord;
            }
            else if (typeOfValueData == typeof(String))
            {
                registryValueKind = RegistryValueKind.String;
            }
            else
            {
                throw new InvalidCastException("Type " + typeOfValueData.Name + " cannot be cast to a supported registry value data type.");
            }

            // open the specified subkey (or create it if it does not exist)
            var registryKey = baseKey.CreateSubKey(subKeyPath, true);

            // write the data
            registryKey.SetValue(valueName, valueData, registryValueKind);
        }

        #endregion

        #region Helper functions to parse registry key paths

        public struct RegistryKeyComponents
        {
            public RegistryKey BaseKey;
            public String SubKeyPath;

            public RegistryKeyComponents(RegistryKey baseKey, String path)
            {
                this.BaseKey = baseKey;
                this.SubKeyPath = path;
            }
        }

        public static RegistryKeyComponents ParseRegistryKeyFullPath(String fullPath)
        {
            if (fullPath == null)
            {
                throw new ArgumentNullException(nameof(fullPath));
            }
            if (fullPath.IndexOf('\\') < 0)
            {
                throw new ArgumentException("Registry key path must contain a base key and a sub key, separated by a backslash.", nameof(fullPath));
            }

            // split the base key name from the sub key path
            var baseKeyAsString = fullPath.Substring(0, fullPath.IndexOf('\\'));
            var subKeyPath = fullPath.Substring(fullPath.IndexOf('\\') + 1);

            // convert the base key string value into a RegistryKey
            RegistryKey baseKey;
            switch (baseKeyAsString.ToUpperInvariant())
            {
                case "HKEY_LOCAL_MACHINE":
                case "HKLM":
                    baseKey = Registry.LocalMachine;
                    break;
                case "HKEY_CURRENT_CONFIG":
                case "HKCC":
                    baseKey = Registry.CurrentConfig;
                    break;
                case "HKEY_CLASSES_ROOT":
                case "HKCR":
                    baseKey = Registry.ClassesRoot;
                    break;
                case "HKEY_USERS":
                case "HKU":
                    baseKey = Registry.Users;
                    break;
                case "HKEY_CURRENT_USER":
                case "HKCU":
                    baseKey = Registry.CurrentUser;
                    break;
                case "HKEY_PERFORMANCE_DATA":
                    baseKey = Registry.PerformanceData;
                    break;
                default:
                    throw new ArgumentException("Registry key path must begin with a valid base key; \"" + baseKeyAsString + "\" is invalid.", nameof(fullPath));
            }

            return new RegistryKeyComponents(baseKey, subKeyPath);
        }

        #endregion
    }
}
