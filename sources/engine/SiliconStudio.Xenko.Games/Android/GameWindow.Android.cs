// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
#if SILICONSTUDIO_PLATFORM_ANDROID
using System;
using System.Diagnostics;
using System.Drawing;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Views.InputMethods;
using OpenTK;
using SiliconStudio.Core;
using SiliconStudio.Xenko.Games.Android;
using SiliconStudio.Xenko.Graphics;
using Rectangle = SiliconStudio.Core.Mathematics.Rectangle;
using OpenTK.Platform.Android;
using Configuration = Android.Content.Res.Configuration;
using Android.Hardware;
using Android.Runtime;

namespace SiliconStudio.Xenko.Games
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    internal class GameWindowAndroid : GameWindow<AndroidXenkoGameView>
    {
        public AndroidXenkoGameView XenkoGameForm;
        private WindowHandle nativeWindow;

        public override WindowHandle NativeWindow => nativeWindow;

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {

        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        private Activity GetActivity()
        {
            var context = XenkoGameForm.Context;
            while (context is ContextWrapper) {
                var activity = context as Activity;
                if (activity != null) {
                    return activity;
                }
                context = ((ContextWrapper)context).BaseContext;
            }
            return null;
        }

        protected override void Initialize(GameContext<AndroidXenkoGameView> gameContext)
        {
            XenkoGameForm = gameContext.Control;
            nativeWindow = new WindowHandle(AppContextType.Android, XenkoGameForm, XenkoGameForm.Handle);

            XenkoGameForm.Load += gameForm_Resume;
            XenkoGameForm.OnPause += gameForm_OnPause;
            XenkoGameForm.Unload += gameForm_Unload;
            XenkoGameForm.RenderFrame += gameForm_RenderFrame;
            XenkoGameForm.Resize += gameForm_Resize;

            // Setup the initial size of the window
            var width = gameContext.RequestedWidth;
            if (width == 0)
            {
                width = XenkoGameForm.Width;
            }

            var height = gameContext.RequestedHeight;
            if (height == 0)
            {
                height = XenkoGameForm.Height;
            }

            // Transmit requested back buffer and depth stencil formats to OpenTK
            XenkoGameForm.RequestedBackBufferFormat = gameContext.RequestedBackBufferFormat;
            XenkoGameForm.RequestedGraphicsProfile = gameContext.RequestedGraphicsProfile;

            XenkoGameForm.Size = new Size(width, height);
        }

        private SurfaceOrientation currentOrientation;

        private void gameForm_Resize(object sender, EventArgs e)
        {
            var windowManager = XenkoGameForm.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            if (windowManager != null)
            {
                var newOrientation = windowManager.DefaultDisplay.Rotation;

                if (currentOrientation != newOrientation)
                {
                    currentOrientation = newOrientation;
                    OnOrientationChanged(this, EventArgs.Empty);
                }
            }         
        }

        void gameForm_Resume(object sender, EventArgs e)
        {
            // Call InitCallback only first time
            if (InitCallback != null)
            {
                InitCallback();
                InitCallback = null;
            }
            XenkoGameForm.Run();

            OnResume();
        }

        void gameForm_OnPause(object sender, EventArgs e)
        {
            // Hide android soft keyboard (doesn't work anymore if done during Unload)
            var inputMethodManager = (InputMethodManager)PlatformAndroid.Context.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(GameContext.Control.RootView.WindowToken, HideSoftInputFlags.None);
        }

        void gameForm_Unload(object sender, EventArgs e)
        {
            OnPause();
        }
        
        void gameForm_RenderFrame(object sender, OpenTK.FrameEventArgs e)
        {
            RunCallback();
        }

        internal override void Run()
        {
            Debug.Assert(InitCallback != null);
            Debug.Assert(RunCallback != null);

            if (XenkoGameForm.GraphicsContext != null)
            {
                throw new NotImplementedException("Only supports not yet initialized AndroidXenkoGameView.");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameWindow" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public override bool Visible
        {
            get
            {
                return XenkoGameForm.Visible;
            }
            set
            {
                XenkoGameForm.Visible = value;
            }
        }

        protected override void SetTitle(string title)
        {
            XenkoGameForm.Title = title;
        }

        internal override void Resize(int width, int height)
        {
            XenkoGameForm.Size = new Size(width, height);
        }

        public override bool IsBorderLess
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public override bool AllowUserResizing
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public override Rectangle ClientBounds => new Rectangle(0, 0, XenkoGameForm.Size.Width, XenkoGameForm.Size.Height);

        public override DisplayOrientation CurrentOrientation
        {
            get
            {
                switch (currentOrientation)
                {
                    case SurfaceOrientation.Rotation0:
                        return DisplayOrientation.Portrait;
                    case SurfaceOrientation.Rotation180:
                        return DisplayOrientation.Portrait;
                    case SurfaceOrientation.Rotation270:
                        return DisplayOrientation.LandscapeRight;
                    case SurfaceOrientation.Rotation90:
                        return DisplayOrientation.LandscapeLeft;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override bool IsMinimized => XenkoGameForm.WindowState == OpenTK.WindowState.Minimized;

        public override bool IsMouseVisible
        {
            get { return false; }
            set { }
        }

        protected override void Destroy()
        {
            if (XenkoGameForm != null)
            {
                XenkoGameForm.Load -= gameForm_Resume;
                XenkoGameForm.OnPause -= gameForm_OnPause;
                XenkoGameForm.Unload -= gameForm_Unload;
                XenkoGameForm.RenderFrame -= gameForm_RenderFrame;

                if (XenkoGameForm.GraphicsContext != null)
                {
                    XenkoGameForm.GraphicsContext.MakeCurrent(null);
                    XenkoGameForm.GraphicsContext.Dispose();
                }
                ((AndroidWindow)XenkoGameForm.WindowInfo).TerminateDisplay();
                //xenkoGameForm.Close(); // bug in xamarin
                XenkoGameForm.Holder.RemoveCallback(XenkoGameForm);
                XenkoGameForm.Dispose();
                XenkoGameForm = null;
            }

            base.Destroy();
        }
    }
}

#endif
