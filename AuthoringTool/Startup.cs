using System.IO.Abstractions;
using System.Net.Http.Handlers;
using System.Reflection;
using AuthoringTool.Mapping;
using AutoMapper;
using BackendAccess.BackendServices;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Commands.World;
using BusinessLogic.Validation;
using DataAccess.Persistence;
using ElectronWrapper;
using FluentValidation;
using Generator.API;
using Generator.DSL;
using Generator.WorldExport;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor.Services;
using Presentation.Components.Culture;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.DropZone;
using Presentation.PresentationLogic.ElectronNET;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.MyLearningWorlds;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
using Shared.Configuration;
using Tailwind;
using HttpClientFactory = Shared.Networking.HttpClientFactory;
using IHttpClientFactory = Shared.Networking.IHttpClientFactory;

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
        services
            .AddRazorPages()
            .AddViewLocalization();
        services.AddServerSideBlazor();

        //MudBlazor
        services.AddMudServices();

        //localization
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Trace);
        });


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
        ConfigureCommandFactories(services);
        ConfigureValidation(services);
        ConfigureApiAccess(services);
        ConfigureMediator(services);
        ConfigureSelectedViewModelsProvider(services);
        ConfigureNetworking(services);


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

    private static void ConfigureNetworking(IServiceCollection services)
    {
        services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
        services.AddTransient<ProgressMessageHandler>(_ => new ProgressMessageHandler(new HttpClientHandler()));
    }

    private void ConfigureValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.Load("BusinessLogic"));
        services.AddTransient(typeof(IValidationWrapper<>), typeof(ValidationWrapper<>));
        services.AddSingleton<ILearningWorldNamesProvider>(p =>
            p.GetService<IAuthoringToolWorkspaceViewModel>() ?? throw new InvalidOperationException());
        services.AddScoped<ILearningSpaceNamesProvider>(p =>
            p.GetService<ILearningWorldPresenter>() ?? throw new InvalidOperationException());
        services.AddScoped<ILearningElementNamesProvider, LearningElementNamesProvider>();
    }

    private void ConfigureAuthoringTool(IServiceCollection services)
    {
        services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
    }

    private void ConfigurePresentationLogic(IServiceCollection services)
    {
        services.AddScoped<IAuthoringToolWorkspacePresenter, AuthoringToolWorkspacePresenter>();
        services.AddSingleton<IPresentationLogic, PresentationLogic>();
        services.AddScoped<ILearningWorldPresenter, LearningWorldPresenter>();
        services.AddScoped(p =>
            (ILearningWorldPresenterOverviewInterface) p.GetService(typeof(ILearningWorldPresenter))!);
        services.AddSingleton<ILearningSpacePresenter, LearningSpacePresenter>();
        services.AddSingleton<IAuthoringToolWorkspaceViewModel, AuthoringToolWorkspaceViewModel>();
        services.AddScoped<IErrorService, ErrorService>();
        services.AddScoped<ILearningElementDropZoneHelper, LearningElementDropZoneHelper>();
        services.AddTransient(typeof(IFormDataContainer<,>), typeof(FormDataContainer<,>));
        services.AddSingleton<IElementModelHandler, ElementModelHandler>();
        services.AddScoped<INavigationManagerWrapper, NavigationManagerWrapper>();
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
        services.AddSingleton<IBackendAccess, BackendAccess.API.BackendAccess>();
        services.AddSingleton<IUserWebApiServices, UserWebApiServices>();
        // Add Http Client
        services.AddHttpClient();
    }

    private void ConfigureMediator(IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
    }

    private void ConfigureSelectedViewModelsProvider(IServiceCollection services)
    {
        services.AddSingleton<ISelectedViewModelsProvider, SelectedViewModelsProvider>();
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
        services.AddScoped<IMyLearningWorldsProvider, MyLearningWorldsProvider>();
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
            cfg.AddCollectionMappersOnce();
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
        services.AddSingleton<IOnUndoRedo>(p => (CommandStateManager) p.GetService<ICommandStateManager>()!);
    }

    private void ConfigureCommandFactories(IServiceCollection services)
    {
        services.AddSingleton<IConditionCommandFactory, ConditionCommandFactory>();
        services.AddSingleton<IElementCommandFactory, ElementCommandFactory>();
        services.AddSingleton<ILayoutCommandFactory, LayoutCommandFactory>();
        services.AddSingleton<IPathwayCommandFactory, PathwayCommandFactory>();
        services.AddSingleton<ISpaceCommandFactory, SpaceCommandFactory>();
        services.AddSingleton<ITopicCommandFactory, TopicCommandFactory>();
        services.AddSingleton<IWorldCommandFactory, WorldCommandFactory>();
        services.AddSingleton<IBatchCommandFactory, BatchCommandFactory>();
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

        // Add localization cultures
        var supportedCultures = new[] {"de-DE", "en-DE"};
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        // Clear all other providers and register the cookie provider as the only one
        localizationOptions.RequestCultureProviders.Clear();
        localizationOptions.AddInitialRequestCultureProvider(new CookieRequestCultureProvider());
        // Require request localization (this applies the requested culture to the actual application)
        app.UseRequestLocalization(localizationOptions);

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
        app.ConfigureElectronWindow(out var window);
        ElectronDialogManager.BackupBrowserWindow = window;
    }
}