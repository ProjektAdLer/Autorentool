using System.IO;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronWrapper;

/// <summary>
/// Indirection so we don't have to reference ElectronNET.API directly in AuthoringTool
/// </summary>
public static class StartupExtensions
{
    public static IServiceCollection AddElectronInternals(this IServiceCollection services)
    {
        services.AddElectron();
        
        return services;
    }

    public static IServiceCollection AddElectronWrappers(this IServiceCollection services)
    {
        services.AddSingleton<IWindowManagerWrapper, WindowManagerWrapper>();
        services.AddSingleton<IDialogWrapper, DialogWrapper>();
        services.AddSingleton<IAppWrapper, AppWrapper>();
        return services;
    }

    public static IApplicationBuilder ConfigureElectronWindow(this IApplicationBuilder appBuilder)
    {
        if (HybridSupport.IsElectronActive)
            Task.Run(async () =>
            {
                var options = new BrowserWindowOptions
                {
                    Fullscreenable = true,
                    //Setting closable will only prevent clicking the close button on Windows and macOS, not on Linux.
                    //This is an Electron limitation, and we need to instead warn users about the fact that they should NOT
                    //close the window using the close button and instead use our applications close button.
                    Closable = false,
                    Height = 800,
                    MinHeight = 800,
                    Width = 1200,
                    MinWidth = 1200,
                    Icon = Path.Join("wwwroot", "favicon.ico")
                };
                return await Electron.WindowManager.CreateWindowAsync(options);
            });
        
        //exit app on all windows closed
        Electron.App.WindowAllClosed += () => Electron.App.Quit();
        return appBuilder;
    }

    public static IWebHostBuilder UseElectronWrapper(this IWebHostBuilder webHostBuilder, string[] args) =>
        webHostBuilder.UseElectron(args);
}