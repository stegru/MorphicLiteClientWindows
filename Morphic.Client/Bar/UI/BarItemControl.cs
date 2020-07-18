namespace Morphic.Client.Bar.UI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using UserControl = System.Windows.Controls.UserControl;

    /// <summary>
    /// A bar item control.
    /// </summary>
    public class BarItemControl : UserControl, INotifyPropertyChanged
    {
        private Theme activeTheme = null!;

        /// <summary>
        /// Create an instance of this class, using the given bar item.
        /// </summary>
        /// <param name="barItem">The bar item that this control displays.</param>
        public BarItemControl(BarItem barItem)
        {
            this.DataContext = this;
            this.BarItem = barItem;
            this.ActiveTheme = barItem.Theme;

            // Some events to monitor the state.
            this.MouseEnter += (sender, args) => this.UpdateTheme();
            this.MouseLeave += (sender, args) => this.UpdateTheme();
            this.IsKeyboardFocusWithinChanged += (sender, args) =>
            {
                this.FocusedByKeyboard = this.IsKeyboardFocusWithin &&
                                         (InputManager.Current.MostRecentInputDevice is KeyboardDevice);
                this.UpdateTheme();
            };
        }

        public BarItemControl() : this(new BarItem())
        {
        }

        /// <summary>
        /// The bar item represented by this control.
        /// </summary>
        public BarItem BarItem { get; }

        /// <summary>Tool tip header - the name of the item, if the tooltip info isn't specified.</summary>
        public string ToolTipHeader
            => string.IsNullOrEmpty(this.BarItem.ToolTipInfo) ? this.BarItem.Text : this.BarItem.ToolTip;
        /// <summary>Tool tip text.</summary>
        public string ToolTipText
            => string.IsNullOrEmpty(this.BarItem.ToolTipInfo) ? this.BarItem.ToolTip : this.BarItem.ToolTipInfo;

        /// <summary>
        /// Current theme to use, depending on the state (normal/hover/focus).
        /// </summary>
        public Theme ActiveTheme
        {
            get => this.activeTheme;
            set
            {
                this.activeTheme = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>true if the last focus was performed by the keyboard.</summary>
        public bool FocusedByKeyboard { get; set; }

        /// <summary>
        /// Creates a control for the given bar item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The control for the item, the type depends on the item.</returns>
        public static BarItemControl From(BarItem item)
        {
            return (Activator.CreateInstance(item.ControlType, item) as BarItemControl)!;
        }

        /// <summary>
        /// Update the theme depending on the current state of the control.
        /// </summary>
        public void UpdateTheme()
        {
            bool keyboardFocus = this.IsKeyboardFocusWithin && this.FocusedByKeyboard;
            if (this.IsMouseOver && keyboardFocus)
            {
                this.ActiveTheme = new Theme()
                    .Apply(this.BarItem.Theme.Hover)
                    .Apply(this.BarItem.Theme.Focus);
            }
            else if (this.IsMouseOver)
            {
                this.ActiveTheme = this.BarItem.Theme.Hover;
            }
            else if (keyboardFocus)
            {
                this.ActiveTheme = this.BarItem.Theme.Focus;
            }
            else
            {
                this.ActiveTheme = this.BarItem.Theme;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = null!;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
