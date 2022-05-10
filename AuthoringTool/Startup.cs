using AuthoringTool.API;
using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.View.Toolbox;
using ElectronWrapper;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //NLog for logging
        //TODO: find out why nlog won't log our runtime errors to console, so its disabled for now :/
        /*
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddNLog();
        });
        */

        //AuthoringTool
        services.AddSingleton<IAuthoringToolConfiguration, AuthoringToolConfiguration>();
        services.AddTransient(typeof(IXmlFileHandler<>), typeof(XmlFileHandler<>));
        services.AddSingleton<IContentFileHandler, ContentFileHandler>();
        services.AddSingleton<IBackupFileGenerator, BackupFileGenerator>();
        services.AddSingleton<IDataAccess, DataAccess>();
        services.AddSingleton<IBusinessLogic, BusinessLogic>();
        services.AddSingleton<IPresentationLogic, PresentationLogic>();
        services.AddSingleton<IAuthoringTool, AuthoringTool.API.AuthoringTool>();
        services.AddSingleton<ILearningWorldPresenter, LearningWorldPresenter>();
        services.AddSingleton<ILearningSpacePresenter, LearningSpacePresenter>();
        services.AddSingleton<ILearningElementPresenter, LearningElementPresenter>();
        services.AddSingleton<IAuthoringToolWorkspaceViewModel, AuthoringToolWorkspaceViewModel>();
        services.AddSingleton<AuthoringToolWorkspacePresenter>();
        services.AddSingleton<IAbstractToolboxRenderFragmentFactory, ToolboxRenderFragmentFactory>();
        //ViewModel <-> Entity Mappers
        services.AddSingleton<ILearningElementMapper, LearningElementMapper>();
        services.AddSingleton<ILearningSpaceMapper, LearningSpaceMapper>();
        services.AddSingleton<ILearningWorldMapper, LearningWorldMapper>();
        services.AddSingleton<ILearningContentMapper, LearningContentMapper>();

        //Blazor and Electron
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton<IMouseService, MouseService>();
        //Electron Wrapper layer
        services.AddElectronInternals();
        services.AddElectronWrappers();
        //Wrapper around HybridSupport
        var hybridSupportWrapper = new HybridSupportWrapper();
        services.AddSingleton<IHybridSupportWrapper, HybridSupportWrapper>(_ => hybridSupportWrapper);
        
        //Insert electron dependant services as required
        if (hybridSupportWrapper.IsElectronActive)
        {
            services.AddSingleton<IShutdownManager, ElectronShutdownManager>();
            services.AddSingleton<IElectronDialogManager, ElectronDialogManager>();
        }
        else
        {
            services.AddSingleton<IShutdownManager, BrowserShutdownManager>();
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
        app.ConfigureElectronWindow();
    }
}