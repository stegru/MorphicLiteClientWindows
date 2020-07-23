// Copyright 2020 Raising the Floor - International
//
// Licensed under the New BSD license. You may not use this file except in
// compliance with this License.
//
// You may obtain a copy of the License at
// https://github.com/GPII/universal/blob/master/LICENSE.txt

namespace Morphic.Client.AppBar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Shell;

    public class AppBar
    {
        private readonly Window window;
        private readonly WindowMovement windowMovement;
        private readonly AppBarApi api;

        private Point mouseDownPos;
        private Size floatingSize = Size.Empty;
        public Edge AppBarEdge { get; private set; } = Edge.None;

        /// <summary>A callback that returns a good height from a given width.</summary>
        public Func<double, double>? GetHeightFromWidth { get; set; }
        /// <summary>A callback that returns a good width from a given height.</summary>
        public Func<double, double>? GetWidthFromHeight { get; set; }

        public bool SnapToEdges { get; set; } = true;

        public event EventHandler<EdgeChangedEventArgs>? EdgeChanged;
        
        public AppBar(Window window) : this(window, new WindowMovement(window, true))
        {
        }
        
        public AppBar(Window window, WindowMovement windowMovement)
        {
            this.window = window;
            this.windowMovement = windowMovement;
            this.api = new AppBarApi(this.windowMovement);

            // Make the window draggable.
            this.window.PreviewMouseDown += this.OnPreviewMouseDown;
            this.window.PreviewMouseMove += this.OnPreviewMouseMove;

            this.windowMovement.SizeComplete += OnSizeComplete;
            this.windowMovement.MoveComplete += this.OnMoveComplete;
            
            this.windowMovement.Moving += this.OnMoving;
            this.windowMovement.Sizing += this.OnSizing;
        }

        private void OnSizing(object? sender, WindowMovement.MovementEventArgs e)
        {
            // Adjust the size to match the content.
            bool horiz = (e.SizeEdge & WindowMovement.SizeEdge.Horizontal) == WindowMovement.SizeEdge.Horizontal;
            Size newSize = this.GetGoodSize(e.Rect.Size, horiz ? Orientation.Horizontal : Orientation.Vertical, true);
            if (newSize != e.Rect.Size)
            {
                e.Rect.Size = newSize;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Gets a size which better fits the content.
        /// </summary>
        /// <param name="size">The suggested size.</param>
        /// <param name="priority">The axis which is more important (reluctant to change).</param>
        /// <param name="inPixels">true if the size is in pixels.</param>
        /// <returns>The new size.</returns>
        public Size GetGoodSize(Size size, Orientation priority, bool inPixels = false)
        {
            bool changed = false;
            Size newSize;
            if (this.GetHeightFromWidth != null || this.GetWidthFromHeight != null)
            {
                newSize = inPixels ? this.FromPixels(size) : size;

                void GetHeight()
                {
                    if (this.GetHeightFromWidth != null && this.AppBarEdge != Edge.Left)
                    {
                        double newHeight = this.GetHeightFromWidth(newSize.Width);
                        if (!double.IsNaN(newHeight))
                        {
                            newSize.Height = newHeight;
                            changed = true;
                        }
                    }
                }

                void GetWidth()
                {
                    if (this.GetWidthFromHeight != null)
                    {
                        double newWidth = this.GetWidthFromHeight(newSize.Height);
                        if (!double.IsNaN(newWidth))
                        {
                            newSize.Width = newWidth;
                            changed = true;
                        }
                    }
                }

                if (priority == Orientation.Horizontal)
                {
                    GetHeight();
                    GetWidth();
                }
                else
                {
                    GetWidth();
                    GetHeight();
                }
            }

            return changed
                ? (inPixels ? this.ToPixels(newSize) : newSize)
                : size;
        }

        private void OnSizeComplete(object? sender, EventArgs e)
        {
            // Re-adjust the reserved desktop space.
            this.api.Update();
        }

        /// <summary>
        /// Called when the window has stopped being moved or sized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMoveComplete(object? sender, EventArgs args)
        {
            // Reserve desktop space for the window.
            this.ApplyAppBar(this.AppBarEdge);
        }

        public void ApplyAppBar(Edge edge)
        {
            this.api.Apply(edge);
            this.OnEdgeChanged(edge, false);
        }

        /// <summary>
        /// Adjusts a thickness so the edges that are touching the screen edge are zero.
        /// </summary>
        /// <param name="thickness">The initial thickness.</param>
        /// <param name="invert">true to remove on the non-touching edge.</param>
        /// <param name="none">Value of "zero" thickness.</param>
        public Thickness AdjustThickness(Thickness thickness, bool invert = false, Thickness? none = null)
        {
            if (none == null)
            {
                none = new Thickness(0);
            }

            if (this.AppBarEdge == Edge.None)
            {
                return invert ? none.Value : thickness;
            }

            Edge notTouching = this.AppBarEdge switch
            {
                Edge.Left => Edge.Right,
                Edge.Top => Edge.Bottom,
                Edge.Right => Edge.Left,
                Edge.Bottom => Edge.Top,
                _ => Edge.None
            };

            Dictionary<Edge, Action> actions = new Dictionary<Edge, Action>()
            {
                {Edge.Left, () => thickness.Left = none.Value.Left },
                {Edge.Right, () => thickness.Right = none.Value.Right },
                {Edge.Top, () => thickness.Top = none.Value.Top },
                {Edge.Bottom, () => thickness.Bottom = none.Value.Bottom },
            };

            foreach ((Edge edge, Action action) in actions)
            {
                if ((edge == notTouching) == invert)
                {
                    action.Invoke();
                }
            }

            return thickness;
        }

        /// <summary>
        /// Called when the window is being moved, to re-adjust the window in-flight.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMoving(object? sender, WindowMovement.MovementEventArgs args)
        {
            args.Handled = true;

            if (args.IsFirst)
            {
                if (this.AppBarEdge == Edge.None)
                {
                    this.floatingSize = args.Rect.Size;
                }
                else
                {
                    // Un-dock the window so it can be moved.
                    this.ApplyAppBar(Edge.None);

                    // Revert to the original size
                    args.Rect.Size = this.floatingSize;

                    // If the window is not under the cursor, move it so the cursor is in the centre.
                    if (args.Rect.X > args.Cursor.X || args.Rect.Right < args.Cursor.X)
                    {
                        args.Rect.X = args.Cursor.X - args.Rect.Width / 2;
                    }

                    if (args.Rect.Y > args.Cursor.Y || args.Rect.Bottom < args.Cursor.Y)
                    {
                        args.Rect.Y = args.Cursor.Y - args.Rect.Height / 2;
                    }

                    // Make it look like the window was this size when the move started.
                    args.InitialRect = args.NewInitialRect = args.Rect;
                }
            }

            // Like magnifier, if the mouse pointer is on the edge then make the window an app bar on that edge.
            System.Drawing.Point mouse = System.Windows.Forms.Control.MousePosition;
            Rect workArea = this.windowMovement.GetWorkArea(new Point(mouse.X, mouse.Y));

            // See what edge the mouse is near (or beyond)
            Rect mouseRect = new Rect(
                Math.Clamp(mouse.X, workArea.Left, workArea.Right),
                Math.Clamp(mouse.Y, workArea.Top, workArea.Bottom), 0, 0);

            Edge lastEdge = this.AppBarEdge;
            this.AppBarEdge = this.NearEdges(workArea, mouseRect, 5).First();
            if (lastEdge != this.AppBarEdge)
            {
                this.OnEdgeChanged(this.AppBarEdge, true);
            }

            // Reposition the window to fit the edge.
            switch (this.AppBarEdge)
            {
                case Edge.Left:
                case Edge.Right:
                    args.Rect.Height = workArea.Height;
                    args.Rect.Width = this.GetGoodSize(args.Rect.Size, Orientation.Vertical, true).Width;
                    args.Rect.Y = workArea.Top;
                    if (this.AppBarEdge == Edge.Left)
                    {
                        args.Rect.X = workArea.X;
                    }
                    else
                    {
                        args.Rect.X = workArea.Right - args.Rect.Width;
                    }

                    break;

                case Edge.Top:
                case Edge.Bottom:
                    args.Rect.Width = workArea.Width;
                    args.Rect.Height = this.GetGoodSize(args.Rect.Size, Orientation.Horizontal, true).Height;
                    args.Rect.X = workArea.X;
                    if (this.AppBarEdge == Edge.Top)
                    {
                        args.Rect.Y = workArea.Y;
                    }
                    else
                    {
                        args.Rect.Y = workArea.Bottom - args.Rect.Height;
                    }

                    break;

                case Edge.None:
                    args.Rect = args.SupposedRect;
                    // Snap to an edge 
                    if (this.SnapToEdges)
                    {
                        this.SnapToEdge(this.windowMovement.GetWorkArea(), ref args.Rect, 20);
                    }

                    break;
            }
        }

        /// <summary>
        /// The width and height of the Window when it is docked.
        /// </summary>
        public Size DockedSizes { get; set; } = new Size(100, 100);

        private Size ToPixels(Size size) => (Size)this.ToPixels((Point) size);
        private Size FromPixels(Size size) => (Size)this.FromPixels((Point) size);

        private Point ToPixels(Point input)
        {
            return PresentationSource.FromVisual(this.window).CompositionTarget.TransformToDevice.Transform(input);
        }
        
        private Point FromPixels(Point input)
        {
            return PresentationSource.FromVisual(this.window).CompositionTarget.TransformFromDevice.Transform(input);
        }

        private Rect FromPixels(Rect input)
        {
            return new Rect(this.FromPixels(input.TopLeft), this.FromPixels(input.BottomRight));
        }
        
        private Rect ToPixels(Rect input)
        {
            return new Rect(this.ToPixels(input.TopLeft), this.ToPixels(input.BottomRight));
        }

        public Size DockedSizesPixels { get; set; }

        /// <summary>
        /// Snaps a rectangle to the edges of another, if it's close enough.
        /// </summary>
        /// <param name="outer">The outer rectangle to check against.</param>
        /// <param name="rect">The inner rect to adjust.</param>
        /// <param name="distance">The distance that the edge can be, in order to snap.</param>
        private void SnapToEdge(Rect outer, ref Rect rect, double distance)
        {
            HashSet<Edge> edges = this.NearEdges(outer, rect, distance);
            if (!edges.Contains(Edge.None))
            {
                if (edges.Contains(Edge.Left))
                {
                    rect.X = outer.X;
                }

                if (edges.Contains(Edge.Top))
                {
                    rect.Y = outer.Y;
                }

                if (edges.Contains(Edge.Right))
                {
                    rect.X = outer.Right - rect.Width;
                }

                if (edges.Contains(Edge.Bottom))
                {
                    rect.Y = outer.Bottom - rect.Height;
                }
            }
        }

        /// <summary>
        /// Determines the edges of a rectangle that are close to the edge of an outer rectangle.
        /// </summary>
        /// <param name="outer">The outer rectangle.</param>
        /// <param name="rect">The inner rectangle.</param>
        /// <param name="distance">The distance that the edges need to be in order to be near.</param>
        /// <returns>Set of edges that are close. Will contain only Edge.None if no edges are close.</returns>
        private HashSet<Edge> NearEdges(Rect outer, Rect rect, double distance)
        {
            HashSet<Edge> result = new HashSet<Edge>();

            bool Near(double a, double b) => Math.Abs(a - b) <= distance;

            if (Near(outer.X, rect.X))
            {
                result.Add(Edge.Left);
            }
            else if (Near(rect.Right, outer.Right))
            {
                result.Add(Edge.Right);
            }

            if (Near(outer.Y, rect.Y))
            {
                result.Add(Edge.Top);
            }
            else if (Near(rect.Bottom, outer.Bottom))
            {
                result.Add(Edge.Bottom);
            }

            if (result.Count == 0)
            {
                result.Add(Edge.None);
            }

            return result;
        }


        /// <summary>
        /// Keeps an eye on the mouse movement. If it's a drag action, start the window move.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPreviewMouseMove(object sender, MouseEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                Point point = args.GetPosition(this.window);

                if (Math.Abs(point.X - this.mouseDownPos.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(point.Y - this.mouseDownPos.Y) >= SystemParameters.MinimumVerticalDragDistance)
                {
                    this.windowMovement.DragMove();
                }
            }
        }

        /// <summary>
        /// Stores the point at which the mouse was pressed, in order to determine if a move becomes a drag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                this.mouseDownPos = args.GetPosition(this.window);
            }
        }

        protected virtual void OnEdgeChanged(Edge edge, bool preview)
        {
            this.OnEdgeChanged(new EdgeChangedEventArgs(edge, preview));
        }
        
        protected virtual void OnEdgeChanged(EdgeChangedEventArgs args)
        {
            this.EdgeChanged?.Invoke(this, args);
        }
    }

    public class EdgeChangedEventArgs : EventArgs
    {
        public EdgeChangedEventArgs(Edge edge, bool preview)
        {
            this.Edge = edge;
            this.Preview = preview;
        }

        /// <summary>
        /// The edge of the screen.
        /// </summary>
        public Edge Edge { get; }
        /// <summary>
        /// true if the current change is only a preview, the desktop reservation has not yet been applied.
        /// </summary>
        public bool Preview { get; }
        
        
    }
}