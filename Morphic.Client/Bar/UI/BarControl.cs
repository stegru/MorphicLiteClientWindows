
namespace Morphic.Client.Bar.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// This is the thing that contains bar items.
    /// </summary>
    public class BarControl : WrapPanel
    {
        
        public double RecommendedWidth { get; set; }
        
        public BarControl()
        {
            this.LayoutUpdated += OnLayoutUpdated;
        }
        
        private void OnLayoutUpdated(object? sender, EventArgs e)
        {
            double maxWidth = 0;
            foreach (UIElement child in this.Children)
            {
                Point t = child.TranslatePoint(new Point(0, 0), this);
                double right = child.RenderSize.Width + t.X;
                if (right > maxWidth)
                {
                    maxWidth = right;
                }
            }

            this.RecommendedWidth = maxWidth;
            this.TallestItem = this.GetTallestItem();
        }

        public double TallestItem { get; set; }

        public BarData Bar { get; private set; }
        
        private int columns;
        public int Columns
        {
            get => this.columns;
            set
            {
                this.columns = value;
                //this.Width = base.ItemWidth * this.columns;
            }
        }
        
        public void LoadBar(BarData bar)
        {
            this.RemoveItems();
            this.Bar = bar;
            this.LoadItems(this.Bar.AllItems);
            this.Columns = 1;
        }

        public void RemoveItems()
        {
            this.Children.Clear();
        }

        /// <summary>
        /// Load some items.
        /// </summary>
        /// <param name="items"></param>
        public void LoadItems(IEnumerable<BarItem> items)
        {
            foreach (BarItem item in items)
            {
                this.AddItem(item);
            }
        }

        /// <summary>
        /// Add a bar item to the control.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The bar item control.</returns>
        public BarItemControl AddItem(BarItem item)
        {
            BarItemControl control = BarItemControl.From(item);
            control.Style = new Style(control.GetType(), this.Resources["BarItemStyle"] as Style);
            this.Children.Add(control);
            return control;
        }

        public double GetTallestItem()
        {
            return this.Children.OfType<FrameworkElement>()
                .Select(child => child.RenderSize.Height)
                .Max();
        }

    }
}
