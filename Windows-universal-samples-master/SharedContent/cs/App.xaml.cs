﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace SDKTemplate
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Construct();

            //项目启动初始化创建数据库
            SQLiteUtil.InitializeDatabase();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                //设置启动页面为Test页面
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
                //rootFrame.Navigate(typeof(Test), e.Arguments);

            }

            // Ensure the current window is active
            Window.Current.Activate();

            // Some samples want access to the LaunchActivatedEventArgs.
            LaunchCompleted(e);
        }

        private Frame CreateRootFrame()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                rootFrame.NavigationFailed += OnNavigationFailed;
                
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        // Add any application contructor code in heretest
        partial void Construct();

        // Add any OnLaunched customization here.
        partial void LaunchCompleted(LaunchActivatedEventArgs e);

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            // Window management
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }
            switch (args.Kind)
            {
                case ActivationKind.VoiceCommand:
                    {
                        break;
                    }
                case ActivationKind.Protocol:
                    {
                        // Code specific to launch for results
                        var command = args as ProtocolActivatedEventArgs;

                        if (command.Uri.ToString().StartsWith("bluetoothzcr:"))
                        {
                            // Open the page that we created to handle activation for results.
                            rootFrame.Navigate(typeof(MainPage), command);

                        }
                        else
                        {
                            //.....
                        }
                        break;
                    }
            }

            // Ensure the current window is active.
            Window.Current.Activate();
        }

        /*protected override void OnActivated(Windows.ApplicationModel.Activation.IActivatedEventArgs e)
       {
           if (e.Kind == ActivationKind.Protocol)
           {
               ProtocolActivatedEventArgs protocolArgs = (ProtocolActivatedEventArgs)e;
               Uri uri = protocolArgs.Uri;
               if (uri.Scheme == "bluetoothzcr:")
               {
                   Frame rootFrame = new Frame();
                   Window.Current.Content = rootFrame;
                   rootFrame.Navigate(typeof(MainPage), uri.Query);
                   Window.Current.Activate();
               }
           }
       }*/

    }
}
