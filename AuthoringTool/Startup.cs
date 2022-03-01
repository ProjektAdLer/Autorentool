﻿using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.PresentationLogic.API;
using ElectronNET.API;
using ElectronNET.API.Entities;


public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddElectron();
        services.AddServerSideBlazor();
        services.AddSingleton<IDataAccess, DataAccess>();
        services.AddSingleton<IAuthoringToolConfiguration, AuthoringToolConfiguration>();
        services.AddSingleton<IBusinessLogic,BusinessLogic>();
        services.AddSingleton<IPresentationLogic,PresentationLogic>();
        if (HybridSupport.IsElectronActive)
        {
        }
        else
        {
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
        if (HybridSupport.IsElectronActive)
            Task.Run(async () =>
            {
                var options = new BrowserWindowOptions
                {
                    Fullscreenable = true,
                };
                return await Electron.WindowManager.CreateWindowAsync(options);
            });
        //exit app on all windows closed
        Electron.App.WindowAllClosed += () => Electron.App.Exit();

        loggerFactory.AddLog4Net();
    }
    
}