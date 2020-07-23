namespace Morphic.Client.Bar.UI
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shell;
    using AppBar;
    using Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The window for the main bar.
    /// </summary>
    public partial class BarWindow : Window, INotifyPropertyChanged
    {
        private readonly AppBar appBar;
        private BarData barData;

        private string barFile = string.Empty;
        private Thickness? initialPadding;
        private Thickness? initialResizeBorder;

        public BarWindow()
        {
            this.appBar = new AppBar(this);
            this.barData = new BarData();
            this.DataContext = this;
            this.InitializeComponent();

            // Accept bar files to be dropped.
            this.AllowDrop = true;
            this.Drop += (sender, args) =>
            {
                if (args.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string file = ((string[]) args.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();
                    this.SetBarFile(file);
                }
            };

            // Tell the app bar to ask the bar control for a good size.
            this.appBar.GetHeightFromWidth = (width)
                => this.BarControl.GetHeightFromWidth(width - this.ExtraWidth) + this.ExtraHeight;
            this.appBar.GetWidthFromHeight = (height)
                => this.BarControl.GetWidthFromHeight(height - this.ExtraHeight) + this.ExtraWidth;

            this.Loaded += (sender, args) => this.AdjustSize();
            this.BarControl.BarLoaded += this.OnBarLoaded;
            this.appBar.EdgeChanged += this.AppBarOnEdgeChanged;
        }

        private void AppBarOnEdgeChanged(object? sender, EdgeChangedEventArgs e)
        {
            this.SetBorder();
        }

        public BarData Bar
        {
            get => this.barData;
            private set
            {
                this.barData = value;
                this.OnBarChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnBarLoaded(object? sender, EventArgs e)
        {
            this.SetBorder();
            this.AdjustSize();
            this.appBar.ApplyAppBar(this.appBar.AppBarEdge);
        }

        private void SetBorder()
        {
            // On the edges that touch the screen, replace the border with a padding.
            this.initialPadding ??= this.Padding;
            //using (this.Dispatcher.DisableProcessing())
            {
                Thickness thickness = new Thickness(this.Bar.BarTheme.BorderSize);
                this.BorderThickness = this.appBar.AdjustThickness(thickness);
            }

            // Remove the resizable area, and window borders, on the sides which are against the screen edges.
            WindowChrome chrome = WindowChrome.GetWindowChrome(this);
            this.initialResizeBorder ??= chrome.ResizeBorderThickness;
            
            // Make sure the size is not below the system defined width.
            Thickness resize = this.initialResizeBorder.Value;
            resize.Left = Math.Max(resize.Left, this.BorderThickness.Left + 1);
            resize.Top = Math.Max(resize.Top, this.BorderThickness.Top + 1);
            resize.Right = Math.Max(resize.Right, this.BorderThickness.Right + 1);
            resize.Bottom = Math.Max(resize.Bottom, this.BorderThickness.Bottom + 1);

            chrome.ResizeBorderThickness = this.appBar.AdjustThickness(resize);
        }

        /// <summary>
        /// The bar has changed.
        /// </summary>
        public event EventHandler? BarChanged;

        /// <summary>Additional width added to the window.</summary>
        public double ExtraWidth =>
            this.BorderThickness.Left + this.BorderThickness.Right +
            this.Padding.Left + this.Padding.Right;
        
        /// <summary>Additional height added to the window.</summary>
        public double ExtraHeight =>
            this.BorderThickness.Top + this.BorderThickness.Bottom +
            this.Padding.Top + this.Padding.Bottom;

        /// <summary>
        /// Asks the bar control for a good size, and adjust the window.
        /// This is the equivalent to setting SizeToContent, but is done manually due to the snapping during resize.
        /// </summary>
        public void AdjustSize()
        {
            //Size size = this.BarControl.AdjustSize();
            Size size = this.appBar.GetGoodSize(this.RenderSize, Orientation.Horizontal);
            // Include the padding and border
            this.Height = size.Height;
            this.Width = size.Width;
        }

        /// <summary>
        /// Set the bar json file (for development/testing)
        /// </summary>
        /// <param name="file"></param>
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
        }

        protected virtual void OnBarChanged()
        {
            this.BarChanged?.Invoke(this, EventArgs.Empty);
            this.OnPropertyChanged(nameof(this.Bar));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static class PreferenceKeys
        {
            public static Preferences.Key Visible = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "visible");
            public static Preferences.Key Position = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "position.win");
            public static Preferences.Key ShowsHelp = new Preferences.Key("org.raisingthefloor.morphic.communityBar", "showsHelp");
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