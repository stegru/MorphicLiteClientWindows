namespace Morphic.Client.Bar.UI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.TextFormatting;

    /// <summary>
    /// The control for Button bar items.
    /// </summary>
    public partial class BarButtonControl : BarItemControl
    {
        public new BarButton BarItem => (BarButton) base.BarItem;

        public BarButtonControl() : this(new BarButton())
        {
        }

        public BarButtonControl(BarButton barItem) : base(barItem)
        {
            this.InitializeComponent();
        }
    }
    
}
