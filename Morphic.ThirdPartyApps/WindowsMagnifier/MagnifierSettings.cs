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

namespace Morphic.ThirdPartyApps
{
    // TODO: consider "MagnificationMode" of FullScreen/Lens/LeftHalf/etc.
    // TODO: consider membership group of FollowFocus, FollowCaret and FollowMouse
    // TODO: consider "rounding" magnification; I think this was used to turn fractional values into whole numbers (although it appears that 0.00 - 1.00 may be range)
    // TODO: add function (or class) to autodetect whether Magnifier is installed

    /* CRITICAL NOTE: this class is under development, does not yet have a stable API contract, is not QA'd, and should not yet be used  */

    public class MagnifierSettings
    {
        private Microsoft.Win32.RegistryKey BASE_REGISTRY_KEY = Microsoft.Win32.Registry.CurrentUser;
        private const String SCREEN_MAGNIFIER_REGISTRY_KEY_PATH = "Software\\Microsoft\\ScreenMagnifier";

        public MagnifierSettings()
        {
        }

        public void SetSettings(List<MorphicKeyValueSetting> settings)
        {
            foreach (var setting in settings)
            {
                this.SetSetting(setting);
            }
        }

        // TOOD: reconsider the "value" in MorphicKeyValueSettings to have a nullable "Value" (and then possibly block those out here)
        // TODO: consider creating a "ClearSetting" function which deletes registry keys
        public void SetSetting(MorphicKeyValueSetting setting)
        {
            switch (setting.key)
            {
                case "Invert":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetInvert(value);
                    }
                    break;
                case "Magnification":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetMagnification(value);
                    }
                    break;
                case "FollowFocus":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowFocus(value);
                    }
                    break;
                case "FollowCaret":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowCaret(value);
                    }
                    break;
                case "FollowMouse":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowMouse(value);
                    }
                    break;
                case "FollowFollowNarrator":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowNarrator(value);
                    }
                    break;
                case "MagnificationMode":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetMagnificationMode((MagnificationModeOption)value);
                    }
                    break;
                case "FadeToMagIcon":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFadeToMagIcon(value);
                    }
                    break;
                case "ZoomIncrement":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetZoomIncrement(value);
                    }
                    break;
                case "UseBitmapSmoothing":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetUseBitmapSmoothing(value);
                    }
                    break;
                case "LensHeight":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetLensHeight(value);
                    }
                    break;
                case "AutoStartEnabled":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetAutoStartEnabled(value);
                    }
                    break;
                case "AutoStartOnLogonDesktopEnabled":
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetAutoStartOnLogonDesktopEnabled(value);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Setting with key \"" + setting.key + "\" does not exist.");
            }
        }

        // TOOD: reconsider the "value" in MorphicKeyValueSettings to have a nullable "Value"
        public List<MorphicKeyValueSetting> GetSettings(List<String> keys)
        {
            var result = new List<MorphicKeyValueSetting>();

            foreach (var key in keys)
            {
                var value = this.GetSetting(key);
#pragma warning disable CS8604 // Possible null reference argument.
                var morphicKeyValueSetting = new MorphicKeyValueSetting(key, value);
#pragma warning restore CS8604 // Possible null reference argument.
                result.Add(morphicKeyValueSetting);
            }

            return result;
        }

        public Object? GetSetting(String key)
        {
            switch (key)
            {
                case "Invert":
                    return this.GetInvert();
                case "Magnification":
                    return this.GetMagnification();
                case "FollowFocus":
                    return this.GetFollowFocus();
                case "FollowCaret":
                    return this.GetFollowCaret();
                case "FollowMouse":
                    return this.GetFollowMouse();
                case "FollowFollowNarrator":
                    return this.GetFollowNarrator();
                case "MagnificationMode":
                    return this.GetMagnificationMode();
                case "FadeToMagIcon":
                    return this.GetFadeToMagIcon();
                case "ZoomIncrement":
                    return this.GetZoomIncrement();
                case "UseBitmapSmoothing":
                    return this.GetUseBitmapSmoothing();
                case "LensHeight":
                    return this.GetLensHeight();
                case "AutoStartEnabled":
                    return this.GetAutoStartEnabled();
                case "AutoStartOnLogonDesktopEnabled":
                    return this.GetAutoStartOnLogonDesktopEnabled();
                default:
                    throw new ArgumentOutOfRangeException("Setting with key \"" + key + "\" does not exist.");
            }
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetInvert()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "Invert");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
        }

        public void SetInvert(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "Invert", valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public UInt32? GetMagnification()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "Magnification");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            // TODO: validate the range
            if (valueAsNullableUInt32! < 100 || valueAsNullableUInt32! > 1600)
            {
                throw new InvalidCastException();
            }

            return valueAsNullableUInt32;
        }

        // NOTE: valid magnification range is 100-1600 (as measured in percent)
        public void SetMagnification(UInt32 value)
        {
            // TODO: validate the range
            if (value < 100 || value > 1600)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "Magnification", value);
        }

        public Boolean? GetFollowFocus()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowFocus");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
        }

        public void SetFollowFocus(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowFocus", valueAsUInt32);
        }

        public Boolean? GetFollowCaret()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowCaret");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
        }

        public void SetFollowCaret(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowCaret", valueAsUInt32);
        }

        public Boolean? GetFollowMouse()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowMouse");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
        }

        public void SetFollowMouse(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowMouse", valueAsUInt32);
        }

        public Boolean? GetFollowNarrator()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowNarrator");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
        }

        public void SetFollowNarrator(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FollowNarrator", valueAsUInt32);
        }

        // TODO: validate the options
        // TODO: decide on a final name (MagnificationModeOption vs. MagnificationMode etc.)
        public enum MagnificationModeOption: UInt32
        {
            Docked = 1,
            FullScreen = 2,
            Lens = 3
        }

        public MagnificationModeOption? GetMagnificationMode()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "MagnificationMode");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            // TODO: validate the range
            switch (valueAsNullableUInt32.Value)
            {
                case (UInt32)MagnificationModeOption.Docked:
                case (UInt32)MagnificationModeOption.FullScreen:
                case (UInt32)MagnificationModeOption.Lens:
                    break;
                default:
                    throw new InvalidCastException();
            }

            return (MagnificationModeOption)valueAsNullableUInt32.Value;
        }

        public void SetMagnificationMode(MagnificationModeOption value)
        {
            // TODO: validate the range
            switch ((UInt32)value)
            {
                case (UInt32)MagnificationModeOption.Docked:
                case (UInt32)MagnificationModeOption.FullScreen:
                case (UInt32)MagnificationModeOption.Lens:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "MagnificationMode", (UInt32)value);
        }

        public Boolean? GetFadeToMagIcon()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FadeToMagIcon");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
        }

        public void SetFadeToMagIcon(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "FadeToMagIcon", valueAsUInt32);
        }

        public UInt32? GetZoomIncrement()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "ZoomIncrement");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            // TODO: validate the range
            // 25, 50, 100 (default), 150, 200, 400
            // throw new InvalidCastException();

            return valueAsNullableUInt32;
        }

        public void SetZoomIncrement(UInt32 value)
        {
            // TODO: implement a switch which throws an ArgumentOutOfRangeException if the zoom increment is not valid

            // 25, 50, 100 (default), 150, 200, 400

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "ZoomIncrement", value);
        }

        public Boolean? GetUseBitmapSmoothing()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "UseBitmapSmoothing");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
        }

        public void SetUseBitmapSmoothing(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "UseBitmapSmoothing", valueAsUInt32);
        }

        public UInt32? GetLensHeight()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(Microsoft.Win32.Registry.CurrentUser, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "LensHeight");
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            // TODO: validate the range
            if (valueAsNullableUInt32! < 10 || valueAsNullableUInt32! > 100)
            {
                throw new InvalidCastException();
            }

            return valueAsNullableUInt32;
        }

        public void SetLensHeight(UInt32 value)
        {
            // TODO: validate the range
            if (value < 10 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, "LensHeight", value);
        }

        // TODO: consider special-casing these "system settings"

        public Boolean? GetAutoStartEnabled()
        {
            throw new NotImplementedException();
        }

        public void SetAutoStartEnabled(Boolean value)
        {
            throw new NotImplementedException();
        }

        public Boolean GetAutoStartOnLogonDesktopEnabled()
        {
            throw new NotImplementedException();
        }

        public void SetAutoStartOnLogonDesktopEnabled(Boolean value)
        {
            throw new NotImplementedException();
        }
    }
}
