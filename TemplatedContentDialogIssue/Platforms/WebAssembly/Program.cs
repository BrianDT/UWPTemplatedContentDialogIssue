// <copyright file="Program.cs" company="Visual Software Systems Ltd.">Copyright (c) 2020 - 2025 All rights reserved</copyright>
namespace TemplatedContentDialogIssue;

using System.Threading.Tasks;
using Uno.UI.Hosting;

/// <summary>
/// The main program
/// </summary>
public class Program
{
    /// <summary>
    /// This is the main entry point of the application.
    /// </summary>
    /// <param name="args">Any optional arguments</param>
    public static async Task Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseWebAssembly()
            .Build();

        await host.RunAsync();
    }
}
