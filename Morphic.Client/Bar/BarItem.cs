using System.Reflection;
using Newtonsoft.Json;

namespace Morphic.Client.Bar
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using Newtonsoft.Json;
    using UI;

    /// <summary>
    /// A bar item.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(TypedJsonConverter), "kind")]
    public class BarItem
    {
        [JsonProperty("label")]
        public string Text { get; set; }
        
        [JsonProperty("popupText")]
        public string ToolTip { get; set; }

        [JsonProperty("description")]
        public string ToolTipInfo { get; set; }
        
        [JsonProperty("isExtra")]
        public bool IsExtra { get; set; }
        
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("theme")]
        public BarItemTheme Theme { get; set; } = new BarItemTheme();

        public Type ControlType => this.GetType().GetCustomAttribute<BarControlAttribute>()?.Type!;
    }

    /// <summary>
    /// Button bar item.
    /// </summary>
    [JsonTypeName("button")]
    [BarControl(typeof(BarButtonControl))]
    public class BarButton : BarItem
    {
        [JsonProperty("value.icon")]
        public string IconValue { get; set; }

        [JsonProperty("value.action")]
        public BarAction Action { get; set; }

        public bool ShowIcon => string.IsNullOrEmpty(this.IconPath);
        
        public string IconPath
        {
            get
            {
                string result = this.IconValue;
                
                if (string.IsNullOrEmpty(result))
                {
                    // For web links, use the site's favicon.
                    if (this.Action is BarWebAction action)
                    {
                        // TODO: Rather than pass the URL directly to the UI, download and cache and check for 404.
                        result = $"https://icons.duckduckgo.com/ip2/{action.Uri.Host}.ico";
                    }
                }

                return result;
            }
        }
    }

    /// <summary>
    /// Image bar item.
    /// </summary>
    [JsonTypeName("image")]
    public class BarImage : BarButton
    {
    }
    
    /// <summary>
    /// Used by a BarItem subclass to identify the control used to display the item.
    /// </summary>
    public class BarControlAttribute : Attribute
    {
        public Type Type { get; }

        public BarControlAttribute(Type type)
        {
            this.Type = type;
        }
    }
}