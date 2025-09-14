// <copyright file=MainActivity.Android.cs" company="Visual Software Systems Ltd.">Copyright (c) 2020 - 2025 All rights reserved</copyright>
namespace TemplatedContentDialogIssue.Droid;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;

/// <summary>
/// The main Android activity.
/// </summary>
[Activity(
    MainLauncher = true,
    ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
    WindowSoftInputMode = SoftInput.AdjustNothing | SoftInput.StateHidden
)]
public class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        global::AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);

        base.OnCreate(savedInstanceState);
    }
}
