namespace Morphic.Client.Bar
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Media;
    using Newtonsoft.Json;

    /// <summary>
    /// Theme for a bar item.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BarItemTheme : Theme
    {
        /// <summary>The theme for when the mouse is over the item.</summary>
        [JsonProperty("hover", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Theme Hover { get; set; } = Theme.Undefined();
        
        /// <summary>The theme for when the item has keyboard focus.</summary>
        [JsonProperty("focus", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Theme Focus { get; set; } = Theme.Undefined();

        /// <summary>The theme for when the item is being clicked (mouse is down).</summary>
        [JsonProperty("active", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Theme Active { get; set; } = Theme.Undefined();

        public BarItemTheme()
        {
        }

        public BarItemTheme(Theme theme)
        {
            this.Apply(theme);
        }

        public BarItemTheme Inherit(BarItemTheme theme)
        {
            this.Apply(theme);
            this.Hover.Apply(theme.Hover);//.Apply(this);
            this.Focus.Apply(theme.Focus);//.Apply(this);
            this.Active.Apply(theme.Active);//.Apply(this);
            return this;
        }
    }

    /// <summary>
    /// Theme for the bar.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BarTheme : Theme
    {
        
    }

    /// <summary>
    /// A theme.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Theme : INotifyPropertyChanged
    {
        private Color? background;

        /// <summary>Text colour.</summary>
        [JsonProperty("color")]
        public Color? TextColor { get; set; }

        [JsonProperty("background")]
        public Color? Background { get; set; }

        [JsonProperty("borderColor")]
        public Color? BorderColor { get; set; }

        [JsonProperty("borderSize")]
        public double BorderSize { get; set; } = double.NaN;

        public bool IsUndefined { get; private set; }

        public static Theme DefaultBar()
        {
            return new Theme()
            {
                Background = Colors.White,
                TextColor = Colors.Black,
                BorderColor = Colors.Black,
                BorderSize = 1
            };
        }
        
        /// <summary>
        /// Default item theme.
        /// </summary>
        /// <returns></returns>
        public static Theme DefaultItem()
        {
            return new Theme()
            {
                Background = ColorConverter.ConvertFromString("#002957") as Color?,
                TextColor = Colors.White
            };
        }

        public static Theme Undefined()
        {
            return new Theme() {IsUndefined = true};
        }

        /// <summary>
        /// Sets the unset values of this instance using values of another.
        /// </summary>
        /// <param name="source">The instance to read values from.</param>
        /// <param name="all">true to set all values, false to set only values in this instance that are null.</param>
        public Theme Apply(Theme source, bool all = false)
        {
            foreach (PropertyInfo property in typeof(Theme).GetProperties())
            {
                object? origValue = all ? null : property.GetValue(this);
                if (origValue == null || (origValue is double d && double.IsNaN(d)))
                {
                    object? newValue = property.GetValue(source);
                    property.SetValue(this, newValue);
                }
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.Name));
            }

            return this;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}