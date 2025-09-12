// <copyright file="DispatchHelper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019, 2025 All rights reserved</copyright>

namespace TemplatedContentDialogIssue;

using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Core;

/// <summary>
/// A cross platform implementation of the UI Dispatcher facade
/// </summary>
public class UIDispatcher : IDispatchOnUIThread
{
    /// <summary>
    /// Marshals actions onto the UI thread
    /// </summary>
    private SynchronizationContext dispatcher;

    /// <summary>
    /// Initialise the dispatcher
    /// </summary>
    public void Initialise()
    {
        this.dispatcher = SynchronizationContext.Current;
    }

    /// <summary>
    /// Dispatch an action onto the UI thread
    /// </summary>
    /// <param name="action">The action</param>
    public void Invoke(Action action)
    {
        if (this.dispatcher == null || SynchronizationContext.Current == this.dispatcher)
        {
            action();
        }
        else
        {
            // Not awated execution will continue before the action has completed
            this.dispatcher.Post((object? state) => action(), null);
        }
    }
}
