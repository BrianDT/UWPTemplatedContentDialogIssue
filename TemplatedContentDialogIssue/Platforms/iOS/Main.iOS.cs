// <copyright file=Main.iOS.cs" company="Visual Software Systems Ltd.">Copyright (c) 2020 - 2025 All rights reserved</copyright>
namespace TemplatedContentDialogIssue.iOS;

using UIKit;
using Uno.UI.Hosting;

/// <summary>
/// The main program
/// </summary>
public class EntryPoint
{
    /// <summary>
    /// This is the main entry point of the application.
    /// </summary>
    /// <param name="args">Any optional arguments</param>
    public static void Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseAppleUIKit()
            .Build();

        host.Run();
    }
}
