using System.IO.Abstractions;
using System.Reflection;
using ApiAccess.API;
using ApiAccess.WebApi;
using AuthoringTool.Mapping;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Validation;
using DataAccess.Persistence;
using ElectronWrapper;
using FluentValidation;
using Generator.API;
using Generator.DSL;
using Generator.WorldExport;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor.Services;
using Presentation.Components.Forms;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.DropZone;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.MyLearningWorlds;
using Shared;
using Shared.Configuration;
using Tailwind;

namespace AuthoringTool;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        //Blazor and Electron (framework)
        services.AddRazorPages();
        services.AddServerSideBlazor();

        //MudBlazor
        services.AddMudServices();


        //AuthoringToolLib
        //PLEASE add any services you add dependencies to to the unit tests in StartupUt!!!
        ConfigureAuthoringTool(services);
        ConfigurePresentationLogic(services);
        ConfigureBusinessLogic(services);
        ConfigureGenerator(services);
        ConfigureDataAccess(services);
        ConfigureToolbox(services);
        ConfigureMyLearningWorlds(services);
        ConfigureUtilities(services);
        ConfigureAutoMapper(services);
        ConfigureCommands(services);
        ConfigureValidation(services);
        ConfigureApiAccess(services);


        //Electron Wrapper layer
        services.AddElectronInternals();
        services.AddElectronWrappers();
        //Wrapper around HybridSupport
        var hybridSupportWrapper = new HybridSupportWrapper();
        services.AddSingleton<IHybridSupportWrapper, HybridSupportWrapper>(_ => hybridSupportWrapper);
        //Wrapper around Shell
        var shellWrapper = new ShellWrapper();
        services.AddSingleton<IShellWrapper, ShellWrapper>(_ => shellWrapper);

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

    private void ConfigureValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.Load("BusinessLogic"));
        services.AddSingleton<ILearningWorldNamesProvider>(p =>
            p.GetService<IAuthoringToolWorkspaceViewModel>() ?? throw new InvalidOperationException());
        services.AddSingleton<ILearningSpaceNamesProvider>(p =>
            p.GetService<ILearningWorldPresenter>() ?? throw new InvalidOperationException());
        services.AddSingleton<ILearningElementNamesProvider, LearningElementNamesProvider>();
    }

    private void ConfigureAuthoringTool(IServiceCollection services)
    {
        services.AddSingleton<IAuthoringToolConfiguration, AuthoringToolConfiguration>();
    }

    private void ConfigurePresentationLogic(IServiceCollection services)
    {
        services.AddSingleton<IAuthoringToolWorkspacePresenter, AuthoringToolWorkspacePresenter>();
        services.AddSingleton<IPresentationLogic, PresentationLogic>();
        services.AddSingleton<ILearningWorldPresenter, LearningWorldPresenter>();
        services.AddSingleton(p =>
            (ILearningWorldPresenterOverviewInterface) p.GetService(typeof(ILearningWorldPresenter))!);
        services.AddSingleton<ILearningSpacePresenter, LearningSpacePresenter>();
        services.AddSingleton<IAuthoringToolWorkspaceViewModel, AuthoringToolWorkspaceViewModel>();
        services.AddSingleton<ILearningElementDropZoneHelper, LearningElementDropZoneHelper>();
        services.AddTransient(typeof(IFormDataContainer<,>), typeof(FormDataContainer<,>));
    }

    private void ConfigureBusinessLogic(IServiceCollection services)
    {
        services.AddSingleton<IBusinessLogic, BusinessLogic.API.BusinessLogic>();
    }

    private void ConfigureDataAccess(IServiceCollection services)
    {
        services.AddTransient(typeof(IXmlFileHandler<>), typeof(XmlFileHandler<>));
        services.AddSingleton<IDataAccess, DataAccess.API.DataAccess>();
        services.AddSingleton<IContentFileHandler, ContentFileHandler>();
    }

    private void ConfigureGenerator(IServiceCollection services)
    {
        services.AddSingleton<IWorldGenerator, WorldGenerator>();
        services.AddSingleton<IBackupFileGenerator, BackupFileGenerator>();
        services.AddSingleton<ICreateDsl, CreateDsl>();
        services.AddSingleton<IReadDsl, ReadDsl>();
    }

    private void ConfigureApiAccess(IServiceCollection services)
    {
        services.AddSingleton<IBackendAccess, BackendAccess>();
        services.AddSingleton<IUserWebApiServices, UserWebApiServices>();
        // Add Http Client
        services.AddHttpClient();
    }

    private static void ConfigureToolbox(IServiceCollection services)
    {
        services.AddSingleton(p =>
            (IAuthoringToolWorkspacePresenterToolboxInterface) p.GetService(typeof(IAuthoringToolWorkspacePresenter))!);
        services.AddSingleton(p =>
            (ILearningWorldPresenterToolboxInterface) p.GetService(typeof(ILearningWorldPresenter))!);
        services.AddSingleton(p =>
            (ILearningSpacePresenterToolboxInterface) p.GetService(typeof(ILearningSpacePresenter))!);
    }

    private static void ConfigureMyLearningWorlds(IServiceCollection services)
    {
        services.AddSingleton<IMyLearningWorldsProvider, MyLearningWorldsProvider>();
        services.AddSingleton<ILearningWorldSavePathsHandler, LearningWorldSavePathsHandler>();
    }

    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        var config = new MapperConfiguration(cfg =>
        {
            ViewModelEntityMappingProfile.Configure(cfg);
            EntityPersistEntityMappingProfile.Configure(cfg);
            FormModelEntityMappingProfile.Configure(cfg);
            ViewModelFormModelMappingProfile.Configure(cfg);
            ApiResponseEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappers();
        });

        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        services.AddSingleton<ICachingMapper, CachingMapper>();
    }

    private static void ConfigureUtilities(IServiceCollection services)
    {
        services.AddTransient<IMemoryCache>(_ => new MemoryCache(new MemoryCacheOptions()));
        services.AddSingleton<IMouseService, MouseService>();
        services.AddTransient<IFileSystem, FileSystem>();
    }

    private void ConfigureCommands(IServiceCollection services)
    {
        services.AddSingleton<ICommandStateManager, CommandStateManager>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.RunTailwind("tailwind");
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