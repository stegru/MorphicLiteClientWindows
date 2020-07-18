namespace Morphic.Client.Bar
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Media;
    using Accessibility;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes a bar.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BarData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("itemTheme", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public BarItemTheme DefaultTheme { get; set; } = new BarItemTheme(Theme.Undefined());
        
        [JsonProperty("barTheme", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Theme BarTheme { get; set; } = Theme.Undefined();
        
        /// <summary>
        /// Gets all items.
        /// </summary>
        [JsonProperty("items")]
        public List<BarItem> AllItems { get; set; }
        
        /// <summary>
        /// Gets the items for the main bar.
        /// </summary>
        public IEnumerable<BarItem> BarItems => this.AllItems.Where(item => !item.Hidden && !item.IsExtra);
        
        /// <summary>
        /// Gets the items for the additional buttons.
        /// </summary>
        public IEnumerable<BarItem> ExtraItems => this.AllItems.Where(item => !item.Hidden && item.IsExtra);

        public static BarData FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {

            };
            BarData bar = JsonSerializer.Create(settings)
                .Deserialize<BarData>(new BarJsonTextReader(new StringReader(json), "win"));

            // Make the theme of each item inherit the default theme.
            bar.DefaultTheme.Apply(Theme.Default());
            foreach (BarItem item in bar.AllItems)
            {
                item.Theme.Inherit(bar.DefaultTheme);
            }

            return bar;
        }

        public static BarData FromFile(string jsonFile)
        {
            return BarData.FromJson(File.ReadAllText(jsonFile));
        }

        public static void test()
        {
            BarData bar = FromFile(@"C:\src\gpii\lite\MorphicLiteClientWindows\Morphic.Client\Bar\test.json5");
        }
        
        [DllImport("shlwapi.dll")]
        public static extern uint ColorHLSToRGB(int H, int L, int S);
        [DllImport("shlwapi.dll")]
        static extern void ColorRGBToHLS(uint RGB, ref int H, ref int L, ref int S);

    }
}