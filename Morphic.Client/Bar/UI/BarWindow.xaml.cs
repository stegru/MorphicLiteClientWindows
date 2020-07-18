namespace Morphic.Client.Bar.UI
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Core;

    /// <summary>
    /// The window for the main bar.
    /// </summary>
    public partial class BarWindow : Window
    {
        public BarData Bar { get; }

        public BarWindow()
        {
            this.Bar = BarData.FromFile(@"C:\src\gpii\lite\MorphicLiteClientWindows\Morphic.Client\Bar\test.json5");
            this.DataContext = this.Bar;
            this.InitializeComponent();
        }
        
        public static class PreferenceKeys
        {
            public static Preferences.Key Visible = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "visible");
            public static Preferences.Key Position = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "position.win");
            public static Preferences.Key ShowsHelp = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "showsHelp");
        }

        private void BarControl_OnInitialized(object? sender, EventArgs e)
        {
            if (sender is BarControl bar)
            {
                bar.LoadBar(this.Bar);
                bar.Columns = 1;
            }
        }

        private void BarControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is BarControl bar)
            {
                this.SizeChanged += (sender, args) =>
                {
                    // Resize the Window to match the content - this looks crap while using the mouse
                    // to resize the window, capturing WM_SIZING would be better.
                    bar.Columns = (int) (this.Width / bar.ItemWidth);
                    this.SizeToContent = SizeToContent.WidthAndHeight;
                };
            }
        }
    }
    
    /// <summary>
    /// Converter which returns a value depending on whether or not the input value is false/null.
    /// </summary>
    public class Ternary : IValueConverter
    {
        /// <summary>
        /// The value to return if the input value is false, null, or empty string.
        /// </summary>
        public string False { get; set; }

        /// <summary>
        /// The value to return if the input value is not null or false. Omit to return the input value.
        /// </summary>
        public string True { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == null) || (value as bool? == false) || (value as string == string.Empty))
            {
                return parameter ?? this.False;
            }
            else
            {
                return this.True ?? value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}