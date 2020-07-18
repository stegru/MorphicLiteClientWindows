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
        [JsonProperty("hover", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Theme Hover { get; set; } = Theme.Undefined();
        
        [JsonProperty("focus", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public Theme Focus { get; set; } = Theme.Undefined();

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
            this.Hover.Apply(theme.Hover).Apply(this);
            this.Focus.Apply(theme.Focus).Apply(this);
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

    [JsonObject(MemberSerialization.OptIn)]
    public class Theme : INotifyPropertyChanged
    {
        [JsonProperty("color")]
        public Color? TextColor { get; set; }
        
        [JsonProperty("background")]
        public Color? Background { get; set; }

        [JsonProperty("borderColor")]
        public Color? BorderColor { get; set; }

        [JsonProperty("borderSize")]
        public int? BorderSize { get; set; }

        public bool IsUndefined { get; private set; }

        public static Theme Default()
        {
            return new Theme()
            {
                Background = Colors.Red,
                TextColor = Colors.White
            };
        }

        public static Theme Undefined()
        {
            return new Theme() {IsUndefined = true};
        }

        /// <summary>
        /// Sets the values of this instance using values of another.
        /// </summary>
        /// <param name="source">The instance to read values from.</param>
        /// <param name="all">false to set only values in this instance that are null, true to set all.</param>
        public Theme Apply(Theme source, bool all = false)
        {
            foreach (PropertyInfo property in typeof(Theme).GetProperties())
            {
                object? origValue = all ? null : property.GetValue(this);
                if (origValue == null)
                {
                    object? newValue = property.GetValue(source);
                    property.SetValue(this, newValue);
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.Name));
                }
            }

            return this;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}