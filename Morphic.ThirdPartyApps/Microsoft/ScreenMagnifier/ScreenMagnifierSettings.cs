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
using System.Collections.Generic;

namespace Morphic.ThirdPartyApps.Microsoft
{
    // TODO: consider "MagnificationMode" of FullScreen/Lens/LeftHalf/etc.
    // TODO: consider membership group of FollowFocus, FollowCaret, and FollowMouse
    // TODO: consider "rounding" magnification; I think this was used to turn fractional values into whole numbers (although it appears that 0.00 - 1.00 may be range)
    // TODO: add function (or class) to autodetect whether Magnifier is installed

    /* CRITICAL NOTE: this class is under development, does not yet have a stable API contract, is not QA'd, and should not yet be used  */

    public class ScreenMagnifierSettings
    {
        private RegistryKey BASE_REGISTRY_KEY = Registry.CurrentUser;
        private const String SCREEN_MAGNIFIER_REGISTRY_KEY_PATH = "Software\\Microsoft\\ScreenMagnifier";

        public ScreenMagnifierSettings()
        {
        }

        // TODO: should this be plural (i.e. "SettingsKeys")?
        private struct SettingsKey
        {
            public const String Invert = "Invert";
            public const String Magnification = "Magnification";
            public const String FollowFocus = "FollowFocus";
            public const String FollowCaret = "FollowCaret";
            public const String FollowMouse = "FollowMouse";
            public const String FollowNarrator = "FollowNarrator";
            public const String MagnificationMode = "MagnificationMode";
            public const String FadeToMagIcon = "FadeToMagIcon";
            public const String ZoomIncrement = "ZoomIncrement";
            public const String UseBitmapSmoothing = "UseBitmapSmoothing";
            public const String LensHeight = "LensHeight";
            // TODO: the following settings are marked as "system settings"; treat them as special cases if necessary
            public const String IsAutoStartEnabled = "IsAutoStartEnabled";
            public const String IsAutoStartOnLogonDesktopEnabled = "IsAutoStartOnLogonDesktopEnabled";
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
                case SettingsKey.Invert:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetInvert(value);
                    }
                    break;
                case SettingsKey.Magnification:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetMagnification(value);
                    }
                    break;
                case SettingsKey.FollowFocus:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowFocus(value);
                    }
                    break;
                case SettingsKey.FollowCaret:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowCaret(value);
                    }
                    break;
                case SettingsKey.FollowMouse:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowMouse(value);
                    }
                    break;
                case SettingsKey.FollowNarrator:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowNarrator(value);
                    }
                    break;
                case SettingsKey.MagnificationMode:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetMagnificationMode((MagnificationModeOption)value);
                    }
                    break;
                case SettingsKey.FadeToMagIcon:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFadeToMagIcon(value);
                    }
                    break;
                case SettingsKey.ZoomIncrement:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetZoomIncrement(value);
                    }
                    break;
                case SettingsKey.UseBitmapSmoothing:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetUseBitmapSmoothing(value);
                    }
                    break;
                case SettingsKey.LensHeight:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetLensHeight(value);
                    }
                    break;
                case SettingsKey.IsAutoStartEnabled:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetIsAutoStartEnabled(value);
                    }
                    break;
                case SettingsKey.IsAutoStartOnLogonDesktopEnabled:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetIsAutoStartOnLogonDesktopEnabled(value);
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
                case SettingsKey.Invert:
                    return this.GetInvert();
                case SettingsKey.Magnification:
                    return this.GetMagnification();
                case SettingsKey.FollowFocus:
                    return this.GetFollowFocus();
                case SettingsKey.FollowCaret:
                    return this.GetFollowCaret();
                case SettingsKey.FollowMouse:
                    return this.GetFollowMouse();
                case SettingsKey.FollowNarrator:
                    return this.GetFollowNarrator();
                case SettingsKey.MagnificationMode:
                    return this.GetMagnificationMode();
                case SettingsKey.FadeToMagIcon:
                    return this.GetFadeToMagIcon();
                case SettingsKey.ZoomIncrement:
                    return this.GetZoomIncrement();
                case SettingsKey.UseBitmapSmoothing:
                    return this.GetUseBitmapSmoothing();
                case SettingsKey.LensHeight:
                    return this.GetLensHeight();
                case SettingsKey.IsAutoStartEnabled:
                    return this.GetIsAutoStartEnabled();
                case SettingsKey.IsAutoStartOnLogonDesktopEnabled:
                    return this.GetIsAutoStartOnLogonDesktopEnabled();
                default:
                    throw new ArgumentOutOfRangeException("Setting with key \"" + key + "\" does not exist.");
            }
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetInvert()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.Invert);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetInvert(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.Invert, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public UInt32? GetMagnification()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.Magnification);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            if (valueAsUInt32 < 100 || valueAsUInt32 > 1600)
            {
                throw new InvalidCastException();
            }

            return valueAsUInt32;
        }

        // NOTE: valid magnification range is 100-1600 (as measured in percent)
        public void SetMagnification(UInt32 value)
        {
            // TODO: validate the range
            if (value < 100 || value > 1600)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.Magnification, value);
        }

        public Boolean? GetFollowFocus()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowFocus);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetFollowFocus(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowFocus, valueAsUInt32);
        }

        public Boolean? GetFollowCaret()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowCaret);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetFollowCaret(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowCaret, valueAsUInt32);
        }

        public Boolean? GetFollowMouse()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowMouse);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetFollowMouse(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowMouse, valueAsUInt32);
        }

        public Boolean? GetFollowNarrator()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowNarrator);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetFollowNarrator(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FollowNarrator, valueAsUInt32);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.MagnificationMode);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            switch (valueAsUInt32)
            {
                case (UInt32)MagnificationModeOption.Docked:
                case (UInt32)MagnificationModeOption.FullScreen:
                case (UInt32)MagnificationModeOption.Lens:
                    break;
                default:
                    throw new InvalidCastException();
            }

            return (MagnificationModeOption)valueAsUInt32;
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

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.MagnificationMode, (UInt32)value);
        }

        public Boolean? GetFadeToMagIcon()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FadeToMagIcon);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetFadeToMagIcon(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.FadeToMagIcon, valueAsUInt32);
        }

        public UInt32? GetZoomIncrement()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.ZoomIncrement);
            if (valueAsNullableUInt32.HasValue == false)
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

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.ZoomIncrement, value);
        }

        public Boolean? GetUseBitmapSmoothing()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.UseBitmapSmoothing);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetUseBitmapSmoothing(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.UseBitmapSmoothing, valueAsUInt32);
        }

        public UInt32? GetLensHeight()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.LensHeight);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            if (valueAsUInt32 < 10 || valueAsUInt32 > 100)
            {
                throw new InvalidCastException();
            }

            return valueAsUInt32;
        }

        public void SetLensHeight(UInt32 value)
        {
            // TODO: validate the range
            if (value < 10 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, SCREEN_MAGNIFIER_REGISTRY_KEY_PATH, SettingsKey.LensHeight, value);
        }

        // TODO: consider special-casing these "system settings"

        public Boolean? GetIsAutoStartEnabled()
        {
            throw new NotImplementedException();
        }

        public void SetIsAutoStartEnabled(Boolean value)
        {
            throw new NotImplementedException();
        }

        public Boolean GetIsAutoStartOnLogonDesktopEnabled()
        {
            throw new NotImplementedException();
        }

        public void SetIsAutoStartOnLogonDesktopEnabled(Boolean value)
        {
            throw new NotImplementedException();
        }
    }
}
