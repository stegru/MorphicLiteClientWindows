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
        /// <summary>
        /// The text displayed on the item.
        /// </summary>
        [JsonProperty("label")]
        public string? Text { get; set; }
        
        /// <summary>
        /// Tooltip main text.
        /// </summary>
        [JsonProperty("popupText")]
        public string? ToolTip { get; set; }

        /// <summary>
        /// Tooltip smaller text.
        /// </summary>
        [JsonProperty("description")]
        public string? ToolTipInfo { get; set; }
        
        /// <summary>
        /// true if the item is to be displayed on the pull-out bar.
        /// </summary>
        [JsonProperty("isExtra")]
        public bool IsExtra { get; set; }
        
        /// <summary>
        /// Don't display this item.
        /// </summary>
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        /// <summary>
        /// Theme for the item.
        /// </summary>
        [JsonProperty("theme")]
        public BarItemTheme Theme { get; set; } = new BarItemTheme();

        /// <summary>
        /// The type of control used. This is specified by using BarControl attribute in a subclass of this.
        /// </summary>
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
        public string? IconValue { get; set; }

        [JsonProperty("value.action")]
        public BarAction? Action { get; set; }
        
        public bool ShowIcon => string.IsNullOrEmpty(this.IconPath);
        
        public string IconPath
        {
            get
            {
                string? result = this.IconValue;
                
                if (string.IsNullOrEmpty(result))
                {
                    // For web links, use the site's favicon.
                    if (this.Action is BarWebAction action)
                    {
                        // TODO: Rather than pass the URL directly to the UI, download and cache and check for 404.
                        result = $"https://icons.duckduckgo.com/ip2/{action.Uri.Host}.ico";
                    }
                }

                return result ?? string.Empty;
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