// <copyright file="IDispatchOnUIThread.cs" company="Visual Software Systems Ltd.">Copyright (c) 2013, 2024, 2025 All rights reserved</copyright>
namespace TemplatedContentDialogIssue;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A cross platform implementation of the UI Dispatcher facade
/// </summary>
public interface IDispatchOnUIThread
{
    /// <summary>
    /// Initialise the dispatcher
    /// </summary>
    void Initialise();

    /// <summary>
    /// Execute an action via the dispatcher
    /// </summary>
    /// <param name="action">The action</param>
    public void Invoke(Action action);
}
