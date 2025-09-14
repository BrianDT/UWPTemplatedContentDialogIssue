// <copyright file="AppStateHelper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2022, 2025 All rights reserved</copyright>

namespace TemplatedContentDialogIssue;

using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.Devices.Geolocation;
using WUI = Microsoft.UI.Xaml;

/// <summary>
/// Static holder for App level properties.
/// </summary>
public static class AppStateHelper
{
    private static WUI.Window? window;

    /// <summary>
    /// Set the main window from the App.xaml.cs.
    /// </summary>
    /// <param name="window">The main window</param>
    public static void SetMainWindow(WUI.Window window)
    {
        AppStateHelper.window = window;
    }

    /// <summary>
    /// Gets the main window.
    /// </summary>
    /// <returns>The main window</returns>
    public static WUI.Window? GetMainWindow()
    {
        return AppStateHelper.window;
    }
}
