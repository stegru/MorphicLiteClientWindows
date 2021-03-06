namespace Morphic.Client.Bar.Data.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Media;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Windows;
    using Windows.Native;
    using global::Windows.Media.SpeechSynthesis;
    using Microsoft.Extensions.Logging;
    using Settings.SettingsHandlers;
    using Settings.SolutionsRegistry;
    using UI;
    using Clipboard = System.Windows.Forms.Clipboard;
    using IDataObject = System.Windows.Forms.IDataObject;

    [HasInternalFunctions]
    // ReSharper disable once UnusedType.Global - accessed via reflection.
    public class Functions
    {
        [InternalFunction("screenshot")]
        public static async Task<bool> Screenshot(FunctionArgs args)
        {
            // Hide all application windows
            Dictionary<Window, double> opacity = new Dictionary<Window, double>();
            HashSet<Window> visible = new HashSet<Window>();
            try
            {
                foreach (Window window in App.Current.Windows)
                {
                    if (window is BarWindow || window is QuickHelpWindow) {
                        if (window.AllowsTransparency)
                        {
                            opacity[window] = window.Opacity;
                            window.Opacity = 0;
                        }
                        else
                        {
                            visible.Add(window);
                            window.Visibility = Visibility.Collapsed;
                        }
                    }
                }

                // Give enough time for the windows to disappear
                await Task.Delay(500);

                // Hold down the windows key while pressing shift + s
                const uint windowsKey = 0x5b; // VK_LWIN
                Morphic.Windows.Native.Keyboard.PressKey(windowsKey, true);
                System.Windows.Forms.SendKeys.SendWait("+s");
                Morphic.Windows.Native.Keyboard.PressKey(windowsKey, false);

            }
            finally
            {
                // Give enough time for snip tool to grab the screen without the morphic UI.
                await Task.Delay(3000);

                // Restore the windows
                foreach ((Window window, double o) in opacity)
                {
                    window.Opacity = o;
                }

                foreach (Window window in visible)
                {
                    window.Visibility = Visibility.Visible;
                }
            }

            return true;
        }

        [InternalFunction("menu", "key=Morphic")]
        public static Task<bool> ShowMenu(FunctionArgs args)
        {
            App.Current.ShowMenu();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Lowers or raises the volume.
        /// </summary>
        /// <param name="args">direction: "up"/"down", amount: number of 1/100 to move</param>
        /// <returns></returns>
        [InternalFunction("volume", "direction", "amount=10")]
        public static Task<bool> Volume(FunctionArgs args)
        {
            IntPtr taskTray = WinApi.FindWindow("Shell_TrayWnd", IntPtr.Zero);
            if (taskTray != IntPtr.Zero)
            {
                int action = args["direction"] == "up"
                    ? WinApi.APPCOMMAND_VOLUME_UP
                    : WinApi.APPCOMMAND_VOLUME_DOWN;

                // Each command moves the volume by 2 notches.
                int times = Math.Clamp(Convert.ToInt32(args["amount"]), 1, 20) / 2;
                for (int n = 0; n < times; n++)
                {
                    WinApi.SendMessage(taskTray, WinApi.WM_APPCOMMAND, IntPtr.Zero,
                        (IntPtr)WinApi.MakeLong(0, (short)action));
                }
            }

            return Task.FromResult(true);
        }

        // Plays the speech sound.
        private static SoundPlayer? speechPlayer;

        /// <summary>
        /// Reads the selected text.
        /// </summary>
        /// <param name="args">action: "play", "pause", or "stop"</param>
        /// <returns></returns>
        [InternalFunction("readAloud", "action")]
        public static async Task<bool> ReadAloud(FunctionArgs args)
        {
            string action = args["action"];
            switch (action)
            {
                case "pause":
                    App.Current.Logger.LogError("ReadAloud: pause not supported");
                    break;

                case "stop":
                case "play":
                    Functions.speechPlayer?.Stop();
                    Functions.speechPlayer?.Dispose();
                    Functions.speechPlayer = null;

                    if (action == "stop")
                    {
                        break;
                    }

                    App.Current.Logger.LogDebug("ReadAloud: Storing clipboard");
                    IDataObject? clipboardData = Clipboard.GetDataObject();
                    Dictionary<string, object?>? dataStored = null;
                    if (clipboardData != null)
                    {
                        dataStored = clipboardData.GetFormats()
                            .ToDictionary(format => format, format => (object?)clipboardData.GetData(format, false));
                    }

                    Clipboard.Clear();

                    // Get the selection
                    App.Current.Logger.LogDebug("ReadAloud: Getting selected text");
                    await SelectionReader.Default.GetSelectedText(System.Windows.Forms.SendKeys.SendWait);
                    string text = Clipboard.GetText();

                    // Restore the clipboard
                    App.Current.Logger.LogDebug("ReadAloud: Restoring clipboard");
                    Clipboard.Clear();
                    dataStored?.Where(kv => kv.Value != null).ToList()
                        .ForEach(kv => Clipboard.SetData(kv.Key, kv.Value));

                    // Talk the talk
                    SpeechSynthesizer synth = new SpeechSynthesizer();
                    SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);
                    speechPlayer = new SoundPlayer(stream.AsStream());
                    speechPlayer.LoadCompleted += (o, args) =>
                    {
                        speechPlayer.Play();
                    };

                    speechPlayer.LoadAsync();

                    break;

            }

            return true;
        }

        /// <summary>
        /// Sends key strokes to the active application.
        /// </summary>
        /// <param name="args">keys: the keys (see MSDN for SendKeys.Send())</param>
        /// <returns></returns>
        [InternalFunction("sendKeys", "keys")]
        public static async Task<bool> SendKeys(FunctionArgs args)
        {
            await SelectionReader.Default.ActivateLastActiveWindow();
            System.Windows.Forms.SendKeys.SendWait(args["keys"]);
            return true;
        }

        [InternalFunction("darkMode")]
        public static async Task<bool> DarkMode(FunctionArgs args)
        {
            bool on = args["state"] == "on";

            Setting appSetting = App.Current.MorphicSession.Solutions.GetSetting(SettingId.LightThemeApps);
            await appSetting.SetValue(!on);
            return true;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Windows API naming")]
        [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Windows API naming")]
        private static class WinApi
        {
            public const int APPCOMMAND_VOLUME_DOWN = 9;
            public const int APPCOMMAND_VOLUME_UP = 10;
            public const int WM_APPCOMMAND = 0x319;

            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string lpClassName, IntPtr lpWindowName);

            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

            public static int MakeLong(short low, short high)
            {
                return (low & 0xffff) | ((high & 0xffff) << 16);
            }
        }
    }
}
