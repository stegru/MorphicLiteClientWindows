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
            //public const String SpeechVolume = "SpeechVolume";
            //public const String SpeechVoice = "SpeechVoice";
            //public const String ShowKeyboardIntroduction = "ShowKeyboardIntroduction";
            //public const String ShowBrowserSelection = "ShowBrowserSelection";
            //public const String ContextVerbosityLevel = "ContextVerbosityLevel";
            //public const String RenderContextBeforeElement = "RenderContextBeforeElement";
            //public const String DuckAudio = "DuckAudio";
            //public const String WinEnterLaunchEnabled = "WinEnterLaunchEnabled";
            //public const String VerbosityLevel = "VerbosityLevel";
            //public const String DetailedFeedback = "DetailedFeedback";
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
                //case SettingsKey.SpeechVolume:
                //case SettingsKey.SpeechVoice:
                //case SettingsKey.ShowKeyboardIntroduction:
                //case SettingsKey.ShowBrowserSelection:
                //case SettingsKey.ContextVerbosityLevel:
                //case SettingsKey.RenderContextBeforeElement:
                //case SettingsKey.DuckAudio:
                //case SettingsKey.WinEnterLaunchEnabled:
                //case SettingsKey.VerbosityLevel:
                //case SettingsKey.DetailedFeedback:


                case SettingsKey.IsAutoStartEnabled:
                    {
                        var value = CastingUtils.CastLosslesslyOrThrowException<Boolean>(setting.value);
                        this.SetAutoStartEnabled(value);
                    }
                    break;
                case SettingsKey.IsAutoStartOnLogonDesktopEnabled:
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

                //case SettingsKey.SpeechVolume:
                //case SettingsKey.SpeechVoice:
                //case SettingsKey.ShowKeyboardIntroduction:
                //case SettingsKey.ShowBrowserSelection:
                //case SettingsKey.ContextVerbosityLevel:
                //case SettingsKey.RenderContextBeforeElement:
                //case SettingsKey.DuckAudio:
                //case SettingsKey.WinEnterLaunchEnabled:
                //case SettingsKey.VerbosityLevel:
                //case SettingsKey.DetailedFeedback:

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
        public Boolean? GetCoupleNarratorCursorKeyboard()
        {
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.CoupleNarratorCursorKeyboard);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.CoupleNarratorCursorMouse);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.EchoChars);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.EchoWords);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ErrorNotificationType);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.FastKeyEntryEnabled);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.FollowInsertion);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.InteractionMouse);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.IntonationPause);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.LockNarratorKeys);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.NarratorCursorHighlight);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.PlayAudioCues);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ReadingWithIntent);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.ReadHints);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            return CastingUtils.CastRegistryUInt32ValueToBooleanOrThrowException(valueAsNullableUInt32.Value);
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechSpeed);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            // TODO: validate the range
            if (valueAsNullableUInt32! < 0 || valueAsNullableUInt32! > 20)
            {
                throw new InvalidCastException();
            }

            return valueAsNullableUInt32;
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
            var valueAsNullableUInt32 = RegistryHelpers.GetRegistryEntry_NullDefault<UInt32>(BASE_REGISTRY_KEY, NARRATOR_REGISTRY_KEY_PATH, SettingsKey.SpeechPitch);
            if (valueAsNullableUInt32 == null)
            {
                return null;
            }

            // TODO: validate the range
            if (valueAsNullableUInt32! < 0 || valueAsNullableUInt32! > 20)
            {
                throw new InvalidCastException();
            }

            return valueAsNullableUInt32;
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
