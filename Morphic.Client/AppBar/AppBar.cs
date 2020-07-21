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
    using System.Windows.Input;
    using System.Windows.Shell;

    public class AppBar
    {
        private readonly Window window;
        private readonly WindowMovement windowMovement;
        private readonly AppBarApi api;

        private Point mouseDownPos;
        private Size floatingSize = Size.Empty;
        private Thickness? initialResizeThickness;
        private Edge appBarEdge = Edge.None;

        public AppBar(Window window)
        {
            this.window = window;
            this.windowMovement = new WindowMovement(this.window, true);
            this.api = new AppBarApi(this.windowMovement);

            // Make the window draggable.
            this.window.PreviewMouseDown += this.OnPreviewMouseDown;
            this.window.PreviewMouseMove += this.OnPreviewMouseMove;

            this.windowMovement.SizeComplete += OnSizeComplete;
            this.windowMovement.MoveComplete += this.OnMoveComplete;
            this.windowMovement.Moving += this.OnMoving;
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
            this.api.Apply(this.appBarEdge);

            // Remove the resizable area on the sides which are against the screen edges.
            WindowChrome chrome = WindowChrome.GetWindowChrome(this.window);

            this.initialResizeThickness ??= chrome.ResizeBorderThickness;

            Thickness thickness = (Thickness) this.initialResizeThickness;

            switch (this.appBarEdge)
            {
                case Edge.None:
                    break;
                case Edge.Top:
                    thickness.Left = thickness.Top = thickness.Right = 0;
                    break;
                case Edge.Bottom:
                    thickness.Left = thickness.Right = thickness.Bottom = 0;
                    break;
                case Edge.Left:
                    thickness.Left = thickness.Top = thickness.Bottom = 0;
                    break;
                case Edge.Right:
                    thickness.Top = thickness.Right = thickness.Bottom = 0;
                    break;
            }

            chrome.ResizeBorderThickness = thickness;
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
                if (this.appBarEdge == Edge.None)
                {
                    this.floatingSize = args.Rect.Size;
                }
                else
                {
                    // Un-dock the window so it can be moved.
                    this.api.Apply(Edge.None);

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
            this.appBarEdge = this.NearEdges(workArea, mouseRect, 5).First();

            // Reposition the window to fit the edge.
            switch (this.appBarEdge)
            {
                case Edge.Left:
                case Edge.Right:
                    args.Rect.Width = this.ToPixels(this.DockedSizes).X;
                    args.Rect.Height = workArea.Height;
                    args.Rect.Y = workArea.Top;
                    if (this.appBarEdge == Edge.Left)
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
                    args.Rect.Height = this.ToPixels(this.DockedSizes).Y;
                    args.Rect.X = workArea.X;
                    if (this.appBarEdge == Edge.Top)
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

        private Point ToPixels(Size size)
        {
            return this.ToPixels((Point) size);
        }

        private Point ToPixels(Point input)
        {
            return PresentationSource.FromVisual(this.window).CompositionTarget.TransformToDevice.Transform(input);
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

        public bool SnapToEdges { get; set; } = true;

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
    }
}