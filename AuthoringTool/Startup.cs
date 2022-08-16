using System.IO.Abstractions;
using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.ElectronNET;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.ModalDialog;
using AuthoringTool.PresentationLogic.Toolbox;
using AuthoringTool.View.Toolbox;
using ElectronWrapper;
using Microsoft.Extensions.Caching.Memory;

namespace AuthoringTool;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //Blazor and Electron (framework)
        services.AddRazorPages();
        services.AddServerSideBlazor();
        
        
        //AuthoringTool
        //PLEASE add any services you add dependencies to to the unit tests in StartupUt!!!
        ConfigureAuthoringTool(services);
        ConfigurePresentationLogic(services);
        ConfigureBusinessLogic(services);
        ConfigureDataAccess(services);
        ConfigureToolbox(services);
        ConfigureMappers(services);
        ConfigureUtilities(services);

        
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

    private void ConfigureAuthoringTool(IServiceCollection services)
    {
        services.AddSingleton<IAuthoringToolConfiguration, AuthoringToolConfiguration>();
    }

    private void ConfigurePresentationLogic(IServiceCollection services)
    {
        services.AddSingleton<IAuthoringToolWorkspacePresenter, AuthoringToolWorkspacePresenter>();
        services.AddSingleton<IPresentationLogic, PresentationLogic.API.PresentationLogic>();
        services.AddSingleton<ILearningWorldPresenter, LearningWorldPresenter>();
        services.AddSingleton<ILearningSpacePresenter, LearningSpacePresenter>();
        services.AddSingleton<ILearningElementPresenter, LearningElementPresenter>();
        services.AddSingleton<IAuthoringToolWorkspaceViewModel, AuthoringToolWorkspaceViewModel>();
        services.AddSingleton<ILearningSpaceViewModalDialogFactory, ModalDialogFactory>();
        services.AddSingleton<ILearningSpaceViewModalDialogInputFieldsFactory, ModalDialogInputFieldsFactory>();
        services.AddSingleton<ILearningWorldViewModalDialogFactory, ModalDialogFactory>();
        services.AddSingleton<ILearningWorldViewModalDialogInputFieldsFactory, ModalDialogInputFieldsFactory>();
        services.AddSingleton<IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory, ModalDialogInputFieldsFactory>();
        services.AddSingleton<IAuthoringToolWorkspaceViewModalDialogFactory, ModalDialogFactory>();
    }

    private void ConfigureBusinessLogic(IServiceCollection services)
    {
        services.AddSingleton<IBusinessLogic, BusinessLogic.API.BusinessLogic>();
    }

    private void ConfigureDataAccess(IServiceCollection services)
    {
        services.AddTransient(typeof(IXmlFileHandler<>), typeof(XmlFileHandler<>));
        services.AddSingleton<IDataAccess, DataAccess.API.DataAccess>();
        services.AddSingleton<ICreateDsl, CreateDsl>();
        services.AddSingleton<IReadDsl, ReadDsl>();
        services.AddSingleton<IContentFileHandler, ContentFileHandler>();
        services.AddSingleton<IBackupFileGenerator, BackupFileGenerator>();
    }

    private static void ConfigureToolbox(IServiceCollection services)
    {
        services.AddSingleton<IAbstractToolboxRenderFragmentFactory, ToolboxRenderFragmentFactory>();
        services.AddSingleton<IToolboxEntriesProviderModifiable, ToolboxEntriesProvider>();
        services.AddSingleton(p => (IToolboxEntriesProvider)p.GetService(typeof(IToolboxEntriesProviderModifiable))!);
        services.AddSingleton<IToolboxController, ToolboxController>();
        services.AddSingleton<IToolboxResultFilter, ToolboxResultFilter>();
        services.AddSingleton(p =>
            (IAuthoringToolWorkspacePresenterToolboxInterface)p.GetService(typeof(IAuthoringToolWorkspacePresenter))!);
        services.AddSingleton(p =>
            (ILearningWorldPresenterToolboxInterface)p.GetService(typeof(ILearningWorldPresenter))!);
        services.AddSingleton(p =>
            (ILearningSpacePresenterToolboxInterface)p.GetService(typeof(ILearningSpacePresenter))!);
    }

    private static void ConfigureMappers(IServiceCollection services)
    {
        services.AddSingleton<ILearningElementMapper, LearningElementMapper>();
        services.AddSingleton<IImageTransferElementMapper, ImageTransferElementMapper>();
        services.AddSingleton<IVideoTransferElementMapper, VideoTransferElementMapper>();
        services.AddSingleton<IPdfTransferElementMapper, PdfTransferElementMapper>();
        services.AddSingleton<IVideoActivationElementMapper, VideoActivationElementMapper>();
        services.AddSingleton<IH5PActivationElementMapper, H5PActivationElementMapper>();
        services.AddSingleton<IH5PInteractionElementMapper, H5PInteractionElementMapper>();
        services.AddSingleton<IH5PTestElementMapper, H5PTestElementMapper>();
        services.AddSingleton<ILearningSpaceMapper, LearningSpaceMapper>();
        services.AddSingleton<ILearningWorldMapper, LearningWorldMapper>();
        services.AddSingleton<ILearningContentMapper, LearningContentMapper>();
        services.AddSingleton<IEntityMapping, EntityMapping>();
    }

    private static void ConfigureUtilities(IServiceCollection services)
    {
        services.AddTransient<IMemoryCache>(_ => new MemoryCache(new MemoryCacheOptions()));
        services.AddSingleton<IMouseService, MouseService>();
        services.AddTransient<IFileSystem, FileSystem>();
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