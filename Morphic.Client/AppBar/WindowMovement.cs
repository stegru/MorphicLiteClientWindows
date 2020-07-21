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
    using System.Windows.Interop;

    /// <summary>
    /// Intercepts the low-level Window move/resize messages, so the window rect can be adjusted while it's
    /// being moved or resized.
    /// </summary>
    public class WindowMovement
    {
        private readonly Window window;

        /// <summary>Raised when the window is being moved.</summary>
        public event EventHandler<MovementEventArgs>? Moving;

        /// <summary>Raised when the window is being resized.</summary>
        public event EventHandler<MovementEventArgs>? Sizing;

        public event EventHandler? EnterSizeMove;
        public event EventHandler? MoveComplete;
        public event EventHandler? SizeComplete;

        public event EventHandler? Ready;
        
        /// <summary>true if the user is currently moving the window.</summary>
        public bool IsMoving { get; private set; }

        /// <summary>true if the user is currently resizing the window.</summary>
        public bool IsResizing { get; private set; }

        /// <summary>Prevent the window from being moved.</summary>
        public bool NoMove { get; set; }

        /// <summary>Prevent the window from being resized.</summary>
        public bool NoSize { get; set; }
        
        /// <summary>
        /// Make the window resizable, regardless of the window's ResizeMode property. Setting the window's
        /// ResizeMode property to `NoResize` and this one to `true`, provides a resizable window that doesn't
        /// get affected by the Aero Snap functionality.
        /// </summary>
        public bool AlwaysResizable
        {
            get => this.alwaysResizable;
            set
            {
                this.alwaysResizable = value;
                if (this.WindowHandle != IntPtr.Zero)
                {
                    if (this.window.ResizeMode == ResizeMode.NoResize ||
                        this.window.ResizeMode != ResizeMode.CanMinimize)
                    {
                        // Set the WS_SIZEBOX window style.
                        int style = (int) WinApi.GetWindowLong(this.WindowHandle, WinApi.GWL_STYLE);
                        if (this.alwaysResizable)
                        {
                            style |= WinApi.WS_SIZEBOX;
                        }
                        else
                        {
                            style &= ~WinApi.WS_SIZEBOX;
                        }

                        WinApi.SetWindowLong(this.WindowHandle, WinApi.GWL_STYLE, (IntPtr) style);
                    }
                }
            }
        }

        /// <summary>
        /// The window edge that the user is using to resize the window.
        /// </summary>
        public SizeEdge CurrentResizeEdge { get; private set; }

        public WindowMovement(Window window, bool alwaysResizable = false)
        {
            this.window = window;
            this.AlwaysResizable = alwaysResizable;

            this.window.SourceInitialized += this.WindowOnSourceInitialized;
        }

        private void WindowOnSourceInitialized(object? sender, EventArgs e)
        {
            // Add the hook for the windows messages.
            this.WindowHandle = new WindowInteropHelper(this.window).Handle;
            HwndSource.FromHwnd(this.WindowHandle).AddHook(this.WindowProc);

            if (this.AlwaysResizable)
            {
                this.AlwaysResizable = this.AlwaysResizable;
            }
            
            this.OnReady();
        }

        private bool firstEvent;
        private Rect initialRect = Rect.Empty;
        private Vector mouseOffset;
        private Point mouseStart;
        public IntPtr WindowHandle { get; private set; }
        private bool alwaysResizable;

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            IntPtr result = IntPtr.Zero;

            switch (msg)
            {
                case WinApi.WM_WINDOWPOSCHANGED:
                case WinApi.WM_WINDOWPOSCHANGING:
                    // Prevent the window being moved.
                    if (!this.IgnoreLock && this.NoMove || this.NoSize)
                    {
                        WinApi.WINDOWPOS windowPos = WinApi.WINDOWPOS.FromPointer(lParam);
                        if (this.NoMove)
                        {
                            windowPos.flags |= WinApi.SWP_NOMOVE;
                        }
                        if (this.NoSize)
                        {
                            windowPos.flags |= WinApi.SWP_NOSIZE;
                        }

                        windowPos.CopyToPointer(lParam);
                    }
                    break;
                
                case WinApi.WM_ENTERSIZEMOVE:
                    this.firstEvent = true;
                    this.OnEnterSizeMove();
                    break;

                case WinApi.WM_EXITSIZEMOVE:
                    if (this.IsMoving)
                    {
                        this.OnMoveComplete();
                    }
                    else if (this.IsResizing)
                    {
                        this.OnSizeComplete();
                    }

                    this.IsMoving = this.IsResizing = false;
                    this.CurrentResizeEdge = SizeEdge.None;
                    break;

                case WinApi.WM_SIZING:
                case WinApi.WM_MOVING:
                    MovementEventArgs eventArgs = new MovementEventArgs();

                    eventArgs.Cursor = WinApi.GetCursorPos();
                    eventArgs.Rect = WinApi.RECT.FromPointer(lParam).ToRect();

                    eventArgs.IsFirst = this.firstEvent;
                    if (this.firstEvent)
                    {
                        this.mouseStart = eventArgs.Cursor;
                        this.initialRect = eventArgs.Rect;
                        this.mouseOffset = this.mouseStart - this.initialRect.TopLeft;
                    }

                    eventArgs.InitialRect = this.initialRect;
                    eventArgs.SupposedRect = this.initialRect;

                    if (!this.firstEvent)
                    {
                        eventArgs.SupposedRect.X = eventArgs.Cursor.X - this.mouseOffset.X;
                        eventArgs.SupposedRect.Y = eventArgs.Cursor.Y - this.mouseOffset.Y;
                    }

                    this.IsMoving = msg == WinApi.WM_MOVING;
                    this.IsResizing = msg == WinApi.WM_SIZING;
                    this.CurrentResizeEdge = this.IsResizing ? (SizeEdge) (int) wParam : SizeEdge.None;
                    eventArgs.SizeEdge = this.CurrentResizeEdge;

                    // Call the event handler.
                    if (this.IsMoving)
                    {
                        this.OnMoving(eventArgs);
                    }
                    else
                    {
                        this.OnSizing(eventArgs);
                    }

                    if (!eventArgs.NewInitialRect.IsEmpty)
                    {
                        this.initialRect = eventArgs.NewInitialRect;
                        if (this.firstEvent)
                        {
                            this.mouseOffset = this.mouseStart - this.initialRect.TopLeft;
                        }
                    }

                    this.firstEvent = false;

                    if (eventArgs.Handled)
                    {
                        new WinApi.RECT(eventArgs.Rect).CopyToPointer(lParam);
                        handled = true;
                        result = new IntPtr(1);
                    }

                    break;
            }

            return result;
        }

        /// <summary>Called when the window is being moved.</summary>
        /// <param name="e"></param>
        protected virtual void OnMoving(MovementEventArgs e)
        {
            this.Moving?.Invoke(this, e);
        }

        /// <summary>Called when the window is being resized.</summary>
        /// <param name="e"></param>
        protected virtual void OnSizing(MovementEventArgs e)
        {
            this.Sizing?.Invoke(this, e);
        }

        /// <summary>
        /// Resize edges of a window.
        /// </summary>
        public enum SizeEdge
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8
        }

        public class MovementEventArgs : EventArgs
        {
            /// <summary>The edge of the window that is being resized.</summary>
            public SizeEdge SizeEdge { get; set; }

            /// <summary>
            /// Set in the event handler to true if the event has been handled and the window rect has changed.
            /// </summary>
            public bool Handled { get; set; }

            /// <summary>true if this is the first event of the current resize/move loop.</summary>
            public bool IsFirst { get; set; }

            /// <summary>The window rect. Update in the event handler to change it.</summary>
            public Rect Rect;

            /// <summary>The window rect before the resize/move began.</summary>
            public Rect InitialRect = Rect.Empty;

            /// <summary>The window rect that the window should have, if there were no adjustments to Rect.</summary>
            public Rect SupposedRect = Rect.Empty;

            /// <summary>
            /// Set in the event handler to change the initial rect, which used to calculate the SupposedRect property.
            /// </summary>
            public Rect NewInitialRect = Rect.Empty;

            /// <summary>The mouse cursor position on the screen.</summary>
            public Point Cursor { get; set; }
        }

        protected virtual void OnEnterSizeMove()
        {
            this.EnterSizeMove?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMoveComplete()
        {
            this.MoveComplete?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSizeComplete()
        {
            this.SizeComplete?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnReady()
        {
            this.Ready?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Like Window.DragMove, but doesn't fire the Click event.
        /// </summary>
        /// <param name="respectNoMove">true to leave `NoMove` alone.</param>
        public void DragMove(bool respectNoMove = false)
        {
            if (!this.NoMove || !respectNoMove)
            {
                this.IgnoreLock = true;
                WinApi.DragMove(this.WindowHandle);
                this.IgnoreLock = false;
            }
        }

        /// <summary>
        /// true to ignore NoMove and NoSize properties.
        /// </summary>
        private bool IgnoreLock { get; set; }

        /// <summary>
        /// Gets the work area of the screen the window is (mostly) on.
        /// </summary>
        /// <returns></returns>
        public Rect GetWorkArea()
        {
            return this.MonitorInfo.rcWork.ToRect();
        }
        
        /// <summary>
        /// Gets the work area at the given point.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Rect GetWorkArea(Point pt)
        {
            return WinApi.GetMonitorInfo(pt).rcWork.ToRect();
        }

        /// <summary>
        /// Get the window rectangle.
        /// </summary>
        /// <returns></returns>
        internal WinApi.RECT GetWindowRc()
        {
            WinApi.GetWindowRect(this.WindowHandle, out WinApi.RECT rc);
            return rc;
        }
        
        /// <summary>
        /// Get the window rectangle.
        /// </summary>
        /// <returns></returns>
        public Rect GetWindowRect()
        {
            return this.GetWindowRc().ToRect();
        }

        /// <summary>
        /// Gets the size of the screen, that the window is located on.
        /// </summary>
        /// <returns></returns>
        public Rect GetScreenSize()
        {
            return this.MonitorInfo.rcMonitor.ToRect();
        }
        
        private WinApi.MONITORINFO MonitorInfo => WinApi.GetMonitorInfo(this.WindowHandle);

        internal void SetWindowRect(WinApi.RECT rc)
        {
            bool ignored = this.IgnoreLock;
            this.IgnoreLock = true;
            WinApi.MoveWindow(this.WindowHandle, rc.Left, rc.Top, rc.Right - rc.Left, rc.Bottom - rc.Top, true);
            this.IgnoreLock = ignored;
        }
        
        public void SetWindowRect(Rect rect)
        {
            bool ignored = this.IgnoreLock;
            this.IgnoreLock = true;
            WinApi.MoveWindow(this.WindowHandle, (int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height, true);
            this.IgnoreLock = ignored;
        }
    }
}