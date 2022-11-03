// <copyright file="DispatchHelper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019 All rights reserved</copyright>

namespace TemplatedContentDialogIssue.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Windows.UI.Core;
#if WINDOWS_UWP
    using Windows.UI.Xaml;
#else
    using Microsoft.UI.Xaml;
#endif

    public static class DispatchHelper
    {
        /// <summary>
        /// Marshals actions onto the UI thread
        /// </summary>
        private static CoreDispatcher dispatcher;

        public static void Initialise()
        {
            DispatchHelper.dispatcher = Window.Current.Dispatcher;
        }

        /// <summary>
        /// Dispatch an action onto the UI thread
        /// </summary>
        /// <param name="action">The action</param>
        public static void InvokeOnUIThread(Action action)
        {
            if (DispatchHelper.dispatcher == null || DispatchHelper.dispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
                // Not awated execution will continue before the action has completed
                DispatchHelper.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
