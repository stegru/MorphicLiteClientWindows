namespace Morphic.Client.Bar.UI
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The window for the main bar.
    /// </summary>
    public partial class BarWindow : Window
    {
        private BarData barData;

        public BarData Bar
        {
            get => this.barData;
            private set
            {
                this.barData = value;
                this.OnBarChanged();
            }
        }

        public BarWindow()
        {
            this.Bar = new BarData();
            this.DataContext = this;
            this.InitializeComponent();

            this.AllowDrop = true;
            this.Drop += (sender, args) =>
            {
                if (args.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string file = ((string[]) args.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();
                    this.SetBarFile(file);
                }
            };
        }

        private event EventHandler BarChanged;
        
        public static class PreferenceKeys
        {
            public static Preferences.Key Visible = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "visible");
            public static Preferences.Key Position = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "position.win");
            public static Preferences.Key ShowsHelp = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "showsHelp");
        }

        private string barFile = string.Empty;
        private async void SetBarFile(string file)
        {
            try
            {
                this.Bar = BarData.FromFile(file);
                this.barFile = file;
            }
            catch (Exception e)
            {
                
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.ToString());

                BarControl? control = this.FindName("BarControl") as BarControl;
                if (control != null)
                {
                    control.RemoveItems();
                    control.AddItem(new BarButton()
                    {
                        Theme = new BarItemTheme()
                        {
                            TextColor = Colors.DarkRed,
                            Background = Colors.White,
                            
                        },
                        Text = e.Message,
                        ToolTip = e.Message,
                        ToolTipInfo = e.ToString()
                    });
                }
            }
            
            // Monitor the file for changes (not using FileSystemWatcher because it doesn't work on network mounts)
            FileInfo lastInfo = new FileInfo(file);
            while (this.barFile == file)
            {
                await Task.Delay(500);
                FileInfo info = new FileInfo(file);
                bool changed = info.Length != lastInfo.Length ||
                               info.CreationTime != lastInfo.CreationTime ||
                               info.LastWriteTime != lastInfo.LastWriteTime;
                if (changed)
                {
                    this.SetBarFile(file);
                    break;
                }
            }
        }

        private void BarControl_OnInitialized(object? sender, EventArgs e)
        {
            if (sender is BarControl bar)
            {
                this.BarChanged += (o, args) =>
                {
                    bar.LoadBar(this.Bar);
                };
                
                this.SetBarFile(Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location), "test-bar.json5"));
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

        protected virtual void OnBarChanged()
        {
            this.BarChanged?.Invoke(this, EventArgs.Empty);
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