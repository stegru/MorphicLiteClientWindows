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
    // TODO: consider membership group of CoupleNarratorCursorKeyboard, FollowInsertion, and InteractionMouse
    // TODO: add function (or class) to autodetect whether Narrator is installed

    /* CRITICAL NOTE: this class is under development, does not yet have a stable API contract, is not QA'd, and should not yet be used  */

    public class NarratorSettings
    {
        private RegistryKey BASE_REGISTRY_KEY = Registry.CurrentUser;
        private const String NARRATOR_REGISTRY_KEY_PATH = "Software\\Microsoft\\Narrator";

        public NarratorSettings()
        {
        }

        // TODO: should this be plural (i.e. "SettingsKeys")?
        private struct SettingsKey
        {
            public const String CoupleNarratorCursorKeyboard = "CoupleNarratorCursorKeyboard";
            public const String CoupleNarratorCursorMouse = "CoupleNarratorCursorMouse";
            public const String EchoChars = "EchoChars";
            public const String EchoWords = "EchoWords";
            public const String ErrorNotificationType = "ErrorNotificationType";
            public const String FastKeyEntryEnabled = "FastKeyEntryEnabled";
            public const String FollowInsertion = "FollowInsertion";
            public const String InteractionMouse = "InteractionMouse";
            public const String IntonationPause = "IntonationPause";
            public const String LockNarratorKeys = "LockNarratorKeys";
            public const String NarratorCursorHighlight = "NarratorCursorHighlight";
            public const String PlayAudioCues = "PlayAudioCues";
            public const String ReadingWithIntent = "ReadingWithIntent";
            public const String ReadHints = "ReadHints";
            public const String SpeechSpeed = "SpeechSpeed";
            public const String SpeechPitch = "SpeechPitch";
            // TODO: the following settings are marked as "no roam"; treat them as special cases if necessary
            public const String SpeechVolume = "SpeechVolume";
            public const String SpeechVoice = "SpeechVoice";
            public const String ShowKeyboardIntroduction = "ShowKeyboardIntroduction";
            public const String ShowBrowserSelection = "ShowBrowserSelection";
            public const String ContextVerbosityLevel = "ContextVerbosityLevel";
            public const String RenderContextBeforeElement = "RenderContextBeforeElement";
            public const String DuckAudio = "DuckAudio";
            public const String WinEnterLaunchEnabled = "WinEnterLaunchEnabled";
            public const String VerbosityLevel = "VerbosityLevel";
            public const String DetailedFeedback = "DetailedFeedback";
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
                case SettingsKey.CoupleNarratorCursorKeyboard:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetCoupleNarratorCursorKeyboard(value);
                    }
                    break;
                case SettingsKey.CoupleNarratorCursorMouse:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetCoupleNarratorCursorMouse(value);
                    }
                    break;
                case SettingsKey.EchoChars:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetEchoChars(value);
                    }
                    break;
                case SettingsKey.EchoWords:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetEchoWords(value);
                    }
                    break;
                case SettingsKey.ErrorNotificationType:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetErrorNotificationType(value);
                    }
                    break;
                case SettingsKey.FastKeyEntryEnabled:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFastKeyEntryEnabled(value);
                    }
                    break;
                case SettingsKey.FollowInsertion:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetFollowInsertion(value);
                    }
                    break;
                case SettingsKey.InteractionMouse:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetInteractionMouse(value);
                    }
                    break;
                case SettingsKey.IntonationPause:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetIntonationPause(value);
                    }
                    break;
                case SettingsKey.LockNarratorKeys:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetLockNarratorKeys(value);
                    }
                    break;
                case SettingsKey.NarratorCursorHighlight:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetNarratorCursorHighlight(value);
                    }
                    break;
                case SettingsKey.PlayAudioCues:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetPlayAudioCues(value);
                    }
                    break;
                case SettingsKey.ReadingWithIntent:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetReadingWithIntent(value);
                    }
                    break;
                case SettingsKey.ReadHints:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetReadHints(value);
                    }
                    break;
                case SettingsKey.SpeechSpeed:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetSpeechSpeed(value);
                    }
                    break;
                case SettingsKey.SpeechPitch:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetSpeechPitch(value);
                    }
                    break;
                case SettingsKey.SpeechVolume:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetSpeechVolume(value);
                    }
                    break;
                case SettingsKey.SpeechVoice:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<String>(setting.value);
                        this.SetSpeechVoice(value);
                    }
                    break;
                case SettingsKey.ShowKeyboardIntroduction:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetShowKeyboardIntroduction(value);
                    }
                    break;
                case SettingsKey.ShowBrowserSelection:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetShowBrowserSelection(value);
                    }
                    break;
                case SettingsKey.ContextVerbosityLevel:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetContextVerbosityLevel((ContextVerbosityLevelOption)value);
                    }
                    break;
                case SettingsKey.RenderContextBeforeElement:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetRenderContextBeforeElement((RenderContextBeforeElementOption)value);
                    }
                    break;
                case SettingsKey.DuckAudio:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetDuckAudio(value);
                    }
                    break;
                case SettingsKey.WinEnterLaunchEnabled:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetWinEnterLaunchEnabled(value);
                    }
                    break;
                case SettingsKey.VerbosityLevel:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<UInt32>(setting.value);
                        this.SetVerbosityLevel((VerbosityLevelOption)value);
                    }
                    break;
                case SettingsKey.DetailedFeedback:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetDetailedFeedback(value);
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
                case SettingsKey.CoupleNarratorCursorKeyboard:
                    return this.GetCoupleNarratorCursorKeyboard();
                case SettingsKey.CoupleNarratorCursorMouse:
                    return this.GetCoupleNarratorCursorMouse();
                case SettingsKey.EchoChars:
                    return this.GetEchoChars();
                case SettingsKey.EchoWords:
                    return this.GetEchoWords();
                case SettingsKey.ErrorNotificationType:
                    return this.GetErrorNotificationType();
                case SettingsKey.FastKeyEntryEnabled:
                    return this.GetFastKeyEntryEnabled();
                case SettingsKey.FollowInsertion:
                    return this.GetFollowInsertion();
                case SettingsKey.InteractionMouse:
                    return this.GetInteractionMouse();
                case SettingsKey.IntonationPause:
                    return this.GetIntonationPause();
                case SettingsKey.LockNarratorKeys:
                    return this.GetLockNarratorKeys();
                case SettingsKey.NarratorCursorHighlight:
                    return this.GetNarratorCursorHighlight();
                case SettingsKey.PlayAudioCues:
                    return this.GetPlayAudioCues();
                case SettingsKey.ReadingWithIntent:
                    return this.GetReadingWithIntent();
                case SettingsKey.ReadHints:
                    return this.GetReadHints();
                case SettingsKey.SpeechSpeed:
                    return this.GetSpeechSpeed();
                case SettingsKey.SpeechPitch:
                    return this.GetSpeechPitch();
                case SettingsKey.SpeechVolume:
                    return this.GetSpeechVolume();
                case SettingsKey.SpeechVoice:
                    return this.GetSpeechVoice();
                case SettingsKey.ShowKeyboardIntroduction:
                    return this.GetShowKeyboardIntroduction();
                case SettingsKey.ShowBrowserSelection:
                    return this.GetShowBrowserSelection();
                case SettingsKey.ContextVerbosityLevel:
                    return this.GetContextVerbosityLevel();
                case SettingsKey.RenderContextBeforeElement:
                    return this.GetRenderContextBeforeElement();
                case SettingsKey.DuckAudio:
                    return this.GetDuckAudio();
                case SettingsKey.WinEnterLaunchEnabled:
                    return this.GetWinEnterLaunchEnabled();
                case SettingsKey.VerbosityLevel:
                    return this.GetVerbosityLevel();
                case SettingsKey.DetailedFeedback:
                    return this.GetDetailedFeedback();
                case "AutoStartEnabled":
                    return this.GetIsAutoStartEnabled();
                case "AutoStartOnLogonDesktopEnabled":
                    return this.GetIsAutoStartOnLogonDesktopEnabled();
                default:
                    throw new ArgumentOutOfRangeException("Setting with key \"" + key + "\" does not exist.");
            }
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetCoupleNarratorCursorKeyboard()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.CoupleNarratorCursorKeyboard);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetCoupleNarratorCursorKeyboard(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.CoupleNarratorCursorKeyboard, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetCoupleNarratorCursorMouse()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.CoupleNarratorCursorMouse);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetCoupleNarratorCursorMouse(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.CoupleNarratorCursorMouse, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetEchoChars()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.EchoChars);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetEchoChars(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.EchoChars, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetEchoWords()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.EchoWords);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetEchoWords(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.EchoWords, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetErrorNotificationType()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ErrorNotificationType);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetErrorNotificationType(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ErrorNotificationType, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetFastKeyEntryEnabled()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.FastKeyEntryEnabled);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetFastKeyEntryEnabled(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.FastKeyEntryEnabled, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetFollowInsertion()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.FollowInsertion);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetFollowInsertion(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.FollowInsertion, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetInteractionMouse()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.InteractionMouse);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetInteractionMouse(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.InteractionMouse, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetIntonationPause()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.IntonationPause);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetIntonationPause(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.IntonationPause, valueAsUInt32);
        }
        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetLockNarratorKeys()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.LockNarratorKeys);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetLockNarratorKeys(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.LockNarratorKeys, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetNarratorCursorHighlight()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.NarratorCursorHighlight);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetNarratorCursorHighlight(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.NarratorCursorHighlight, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetPlayAudioCues()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.PlayAudioCues);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetPlayAudioCues(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.PlayAudioCues, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetReadingWithIntent()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ReadingWithIntent);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetReadingWithIntent(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ReadingWithIntent, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetReadHints()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ReadHints);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetReadHints(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ReadHints, valueAsUInt32);
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        // TODO: translate this from -10 to 10 as appropriate
        public UInt32? GetSpeechSpeed()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechSpeed);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            if (valueAsUInt32 < 0 || valueAsUInt32 > 20)
            {
                throw new InvalidCastException();
            }

            return valueAsUInt32;
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // NOTE: valid speech speed range is 0-20
        // TODO: translate this to -10 to 10 as appropriate
        public void SetSpeechSpeed(UInt32 value)
        {
            // TODO: validate the range
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechSpeed, value);
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public UInt32? GetSpeechPitch()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechPitch);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            if (valueAsUInt32 < 0 || valueAsUInt32 > 20)
            {
                throw new InvalidCastException();
            }

            return valueAsUInt32;
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // NOTE: valid speech pitch range is 0-20
        public void SetSpeechPitch(UInt32 value)
        {
            // TODO: validate the range
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechPitch, value);
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // TODO: this range appears to be 0.00-0.99 (or 0.00-1.00), but is stored as a DWORD?  Dig in and determine how to map this.
        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public UInt32? GetSpeechVolume()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechVolume);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            if (valueAsUInt32 < 0 || valueAsUInt32 > 1)
            {
                throw new InvalidCastException();
            }

            return valueAsUInt32;
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // TODO: this range appears to be 0.00-0.99 (or 0.00-1.00), but is stored as a DWORD?  Dig in and determine how to map this.
        // NOTE: valid speech volume range is ??? // TODO: establish the actual range
        public void SetSpeechVolume(UInt32 value)
        {
            // TODO: validate the range
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechVolume, value);
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        // TODO: translate this to/from an enum if appropriate
        public String? GetSpeechVoice()
        {
            var valueAsNullableString = RegistryHelpers.GetRegistryValueData_ReferenceType_NullDefault<String>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechVoice);
            if (valueAsNullableString == null)
            {
                return null;
            }
            var valueAsString = valueAsNullableString!;

            // TODO: validate the result (and consider making it an enum); see the solution registry for example values (and determine if they are exhaustive)
            //if (valueAsString! != "<text goes here>")
            //{
            //    throw new InvalidCastException();
            //}

            return valueAsString;
        }

        // TODO: evaluate the way that this data is presented in Windows and in the solutions registry and how we are to store it (as there may be a larger mapping)
        // TODO: translate this to/from an enum if appropriate
        public void SetSpeechVoice(String value)
        {
            // TODO: validate the value (and consider making it an enum); see the solution registry for example values (and determine if they are exhaustive)
            //if (value < 0 != "<text goes here>")
            //{
            //    throw new ArgumentOutOfRangeException(nameof(value));
            //}

            RegistryHelpers.SetRegistryValueData<String>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechVoice, value);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetShowKeyboardIntroduction()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ShowKeyboardIntroduction);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetShowKeyboardIntroduction(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ShowKeyboardIntroduction, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetShowBrowserSelection()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ShowBrowserSelection);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetShowBrowserSelection(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ShowBrowserSelection, valueAsUInt32);
        }

        // TODO: validate the options
        // TODO: decide on a final name (ContextVerbosityLevelOption vs. ContextVerbosityLevel etc.)
        public enum ContextVerbosityLevelOption : UInt32
        {
            NoContextRendering = 0,
            SoundsOnly = 1,
            ImmediateContext = 2,
            ImmediateContextNameAndType = 3,
            FullContextOfNewControl = 4,
            FullContextOfBothTheOldControlAndNewControl = 5
        }

        public ContextVerbosityLevelOption? GetContextVerbosityLevel()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ContextVerbosityLevel);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            switch (valueAsUInt32)
            {
                case (UInt32)ContextVerbosityLevelOption.NoContextRendering:
                case (UInt32)ContextVerbosityLevelOption.SoundsOnly:
                case (UInt32)ContextVerbosityLevelOption.ImmediateContext:
                case (UInt32)ContextVerbosityLevelOption.ImmediateContextNameAndType:
                case (UInt32)ContextVerbosityLevelOption.FullContextOfNewControl:
                case (UInt32)ContextVerbosityLevelOption.FullContextOfBothTheOldControlAndNewControl:
                    break;
                default:
                    throw new InvalidCastException();
            }

            return (ContextVerbosityLevelOption)valueAsUInt32;
        }

        public void SetContextVerbosityLevel(ContextVerbosityLevelOption value)
        {
            // TODO: validate the range
            switch ((UInt32)value)
            {
                case (UInt32)ContextVerbosityLevelOption.NoContextRendering:
                case (UInt32)ContextVerbosityLevelOption.SoundsOnly:
                case (UInt32)ContextVerbosityLevelOption.ImmediateContext:
                case (UInt32)ContextVerbosityLevelOption.ImmediateContextNameAndType:
                case (UInt32)ContextVerbosityLevelOption.FullContextOfNewControl:
                case (UInt32)ContextVerbosityLevelOption.FullContextOfBothTheOldControlAndNewControl:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ContextVerbosityLevel, (UInt32)value);
        }

        // TODO: validate the options
        // TODO: decide on a final name (RenderContextBeforeElementOption vs. RenderContextBeforeElement etc.)
        public enum RenderContextBeforeElementOption : UInt32
        {
            Before = 0,
            After = 1
        }

        public RenderContextBeforeElementOption? GetRenderContextBeforeElement()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.RenderContextBeforeElement);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            switch (valueAsUInt32)
            {
                case (UInt32)RenderContextBeforeElementOption.Before:
                case (UInt32)RenderContextBeforeElementOption.After:
                    break;
                default:
                    throw new InvalidCastException();
            }

            return (RenderContextBeforeElementOption)valueAsUInt32;
        }

        public void SetRenderContextBeforeElement(RenderContextBeforeElementOption value)
        {
            // TODO: validate the range
            switch ((UInt32)value)
            {
                case (UInt32)RenderContextBeforeElementOption.Before:
                case (UInt32)RenderContextBeforeElementOption.After:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.RenderContextBeforeElement, (UInt32)value);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetDuckAudio()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.DuckAudio);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetDuckAudio(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.DuckAudio, valueAsUInt32);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetWinEnterLaunchEnabled()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.WinEnterLaunchEnabled);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32!);
        }

        public void SetWinEnterLaunchEnabled(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.WinEnterLaunchEnabled, valueAsUInt32);
        }

        // TODO: validate the options
        // TODO: decide on a final name (ContextVerbosityLevelOption vs. ContextVerbosityLevel etc.)
        public enum VerbosityLevelOption : UInt32
        {
            TextOnly = 0,
            HeadersAndErrors = 1,
            BasicInformation = 2,
            OtherAnnotations = 3,
            ExtendedFormatting = 4,
            LayoutAndAnimationInfo = 5
        }

        public VerbosityLevelOption? GetVerbosityLevel()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.VerbosityLevel);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            // TODO: validate the range
            switch (valueAsUInt32)
            {
                case (UInt32)VerbosityLevelOption.TextOnly:
                case (UInt32)VerbosityLevelOption.HeadersAndErrors:
                case (UInt32)VerbosityLevelOption.BasicInformation:
                case (UInt32)VerbosityLevelOption.OtherAnnotations:
                case (UInt32)VerbosityLevelOption.ExtendedFormatting:
                case (UInt32)VerbosityLevelOption.LayoutAndAnimationInfo:
                    break;
                default:
                    throw new InvalidCastException();
            }

            return (VerbosityLevelOption)valueAsUInt32;
        }

        public void SetVerbosityLevel(VerbosityLevelOption value)
        {
            // TODO: validate the range
            switch ((UInt32)value)
            {
                case (UInt32)VerbosityLevelOption.TextOnly:
                case (UInt32)VerbosityLevelOption.HeadersAndErrors:
                case (UInt32)VerbosityLevelOption.BasicInformation:
                case (UInt32)VerbosityLevelOption.OtherAnnotations:
                case (UInt32)VerbosityLevelOption.ExtendedFormatting:
                case (UInt32)VerbosityLevelOption.LayoutAndAnimationInfo:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.VerbosityLevel, (UInt32)value);
        }

        // returns null if setting does not exist in registry
        // throws InvalidCastException if registry data is invalid
        public Boolean? GetDetailedFeedback()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryValueData_ValueType_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.DetailedFeedback);
            if (valueAsNullableUInt32.HasValue == false)
            {
                return null;
            }
            var valueAsUInt32 = valueAsNullableUInt32!.Value;

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsUInt32);
        }

        public void SetDetailedFeedback(Boolean value)
        {
            var valueAsUInt32 = CastingUtils.ConvertBooleanToRegistryUInt32(value);

            RegistryHelpers.SetRegistryValueData<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.DetailedFeedback, valueAsUInt32);
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
