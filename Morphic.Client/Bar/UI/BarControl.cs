﻿
namespace Morphic.Client.Bar.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// This is the thing that contains bar items.
    /// </summary>
    public class BarControl : WrapPanel, INotifyPropertyChanged
    {
        private double tallestItem;

        public BarControl()
        {
            this.LayoutUpdated += OnLayoutUpdated;
        }

        public BarData Bar { get; private set; }

        public event EventHandler? BarLoaded;

        private void OnLayoutUpdated(object? sender, EventArgs e)
        {
            this.tallestItem = double.IsNaN(this.ItemHeight)
                ? this.Children.OfType<UIElement>()
                    .Select(child => child.RenderSize.Height)
                    .Max()
                : this.ItemHeight;
        }

        /// <summary>Gets a width that fits all items with the given height.</summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public double GetWidthFromHeight(double height)
        {
            double width = Math.Ceiling(this.Children.Count / Math.Floor(height / this.tallestItem)) * this.ItemWidth;
            return Math.Clamp(width, this.ItemWidth, this.ItemWidth * this.Children.Count);
        }

        /// <summary>Gets a heigh that fits all items with the given width.</summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public double GetHeightFromWidth(double width)
        {
            double height = Math.Ceiling(this.Children.Count / Math.Floor(width / this.ItemWidth)) * this.tallestItem;
            return Math.Clamp(height, this.tallestItem, this.tallestItem * this.Children.Count);
        }

        public void LoadBar(BarData bar)
        {
            this.RemoveItems();
            this.Bar = bar;
            this.LoadItems(this.Bar.AllItems);
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
            
            this.OnBarLoaded();
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

        protected virtual void OnBarLoaded()
        {
            this.BarLoaded?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
