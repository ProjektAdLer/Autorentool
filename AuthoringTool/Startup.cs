using System.IO.Abstractions;
using System.Net.Http.Handlers;
using System.Reflection;
using AuthoringTool.Mapping;
using AutoMapper;
using BackendAccess.BackendServices;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Adaptivity.Action;
using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Commands.Adaptivity.Rule;
using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.LearningOutcomes;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.ErrorManagement;
using BusinessLogic.Validation;
using BusinessLogic.Validation.Validators;
using DataAccess.Persistence;
using ElectronWrapper;
using FluentValidation;
using Generator.API;
using Generator.ATF;
using Generator.WorldExport;
using H5pPlayer.BusinessLogic.Api.CleanupH5pPlayer;
using H5pPlayer.Main;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using MudBlazor.Services;
using Presentation.Components.ContentFiles;
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
using Serilog;
using Serilog.Settings.Configuration;
using Serilog.Sinks.SystemConsole.Themes;
using Shared;
using Shared.Configuration;
using Shared.Networking;
using Shared.Theme;
using Tailwind;
using HttpClientFactory = Shared.Networking.HttpClientFactory;
using IHttpClientFactory = Shared.Networking.IHttpClientFactory;

namespace AuthoringTool;
// ReSharper disable InconsistentNaming
public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

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

        var logFileName = Environment.IsDevelopment() ? "log-dev.txt" : "log.txt";
        var logFilePath = Path.Combine(ApplicationPaths.LogsFolder, logFileName);
        try
        {
            var options = new ConfigurationReaderOptions(typeof(ConsoleLoggerExtensions).Assembly);
            var loggerConfig = new LoggerConfiguration();
            loggerConfig.ReadFrom.Configuration(Configuration, options)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.File(path: logFilePath, buffered: false, rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 100000000,
                    retainedFileCountLimit: 5);
            Log.Logger = loggerConfig.CreateLogger();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
            builder.SetMinimumLevel(LogLevel.Trace);
        });


        //AuthoringToolLib
        //PLEASE add any services you add dependencies to to the unit tests in StartupUt!!!
        ConfigureAuthoringTool(services);
        ConfigurePresentationLogic(services);
        ConfigureBusinessLogic(services);
        ConfigureGenerator(services);
        ConfigureDataAccess(services);
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
        H5pPlayerStartup.ConfigureH5pPlayer(services);


        //Electron Wrapper layer
        services.AddElectronInternals();
        services.AddElectronWrappers();
        //Wrapper around HybridSupport
        var hybridSupportWrapper = new HybridSupportWrapper();
        services.AddSingleton<IHybridSupportWrapper, HybridSupportWrapper>(_ => hybridSupportWrapper);
        //Wrapper around Shell
        var shellWrapper = new ShellWrapper();
        services.AddSingleton<IShellWrapper, ShellWrapper>(_ => shellWrapper);

        var readAuthService = new ReadAuthService();
        services.AddSingleton<IReadAuthService, ReadAuthService>(_ => readAuthService);

        //Insert electron dependant services as required
        if (hybridSupportWrapper.IsElectronActive)
        {
            readAuthService.ReadAuth();
            services.AddSingleton<IShutdownManager, ElectronShutdownManager>();
            services.AddSingleton<IElectronDialogManager, ElectronDialogManager>();
        }
        else
        {
            services.AddSingleton<IShutdownManager, BrowserShutdownManager>();
        }
    }

    internal static void ConfigureNetworking(IServiceCollection services)
    {
        services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
        services.AddTransient<ProgressMessageHandler>(_ => new ProgressMessageHandler(new HttpClientHandler()));
        services.AddSingleton<IPreflightHttpClient, PreflightHttpClient>(_ => new PreflightHttpClient());
    }

    internal static void ConfigureValidation(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.Load("BusinessLogic"));
        services.AddTransient(typeof(IValidationWrapper<>), typeof(ValidationWrapper<>));
        services.AddTransient<IValidator<MultipleChoiceSingleResponseQuestion>,
            MultipleChoiceSingleResponseQuestionValidator>();
        services.AddTransient<IValidator<MultipleChoiceMultipleResponseQuestion>,
            MultipleChoiceMultipleResponseQuestionValidator>();
        services.AddSingleton<ILearningWorldNamesProvider>(p =>
            p.GetService<IAuthoringToolWorkspaceViewModel>() ?? throw new InvalidOperationException());
        services.AddScoped<ILearningSpaceNamesProvider>(p =>
            p.GetService<ILearningWorldPresenter>() ?? throw new InvalidOperationException());
        services.AddScoped<ILearningElementNamesProvider, LearningElementNamesProvider>();
        services.AddSingleton<ILearningWorldStructureValidator, LearningWorldStructureValidator>();
    }

    internal static void ConfigureAuthoringTool(IServiceCollection services)
    {
        services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
    }

    internal static void ConfigurePresentationLogic(IServiceCollection services)
    {
        services.AddScoped<IAuthoringToolWorkspacePresenter, AuthoringToolWorkspacePresenter>();
        services.AddScoped<IPresentationLogic, PresentationLogic>();
        services.AddScoped<ILearningWorldPresenter, LearningWorldPresenter>();
        services.AddScoped(p =>
            (ILearningWorldPresenterOverviewInterface)p.GetService(typeof(ILearningWorldPresenter))!);
        services.AddScoped<ILearningSpacePresenter, LearningSpacePresenter>();
        services.AddSingleton<IAuthoringToolWorkspaceViewModel, AuthoringToolWorkspaceViewModel>();
        services.AddScoped<IErrorService, ErrorService>();
        services.AddScoped<ILearningElementDropZoneHelper, LearningElementDropZoneHelper>();
        services.AddTransient(typeof(IFormDataContainer<,>), typeof(FormDataContainer<,>));
        services.AddSingleton<IElementModelHandler, ElementModelHandler>();
        services.AddScoped<INavigationManagerWrapper, NavigationManagerWrapper>();
        services.AddScoped<IH5PPlayerPluginManager, H5PPlayerPluginManager>();
    }

    internal static void ConfigureBusinessLogic(IServiceCollection services)
    {
        services.AddSingleton<IBusinessLogic, BusinessLogic.API.BusinessLogic>();
        services.AddSingleton<IErrorManager, ErrorManager>();
    }

    internal static void ConfigureDataAccess(IServiceCollection services)
    {
        services.AddTransient(typeof(IXmlFileHandler<>), typeof(XmlFileHandler<>));
        services.AddSingleton<IDataAccess, DataAccess.API.DataAccess>();
        services.AddSingleton<IContentFileHandler, ContentFileHandler>();
    }

    internal static void ConfigureGenerator(IServiceCollection services)
    {
        services.AddSingleton<IWorldGenerator, WorldGenerator>();
        services.AddSingleton<IBackupFileGenerator, BackupFileGenerator>();
        services.AddSingleton<ICreateAtf, CreateAtf>();
        services.AddSingleton<IReadAtf, ReadAtf>();
    }

    internal static void ConfigureApiAccess(IServiceCollection services)
    {
        services.AddSingleton<IBackendAccess, BackendAccess.API.BackendAccess>();
        services.AddSingleton<IUserWebApiServices, UserWebApiServices>();
        // Add Http Client
        services.AddHttpClient();
    }

    internal static void ConfigureMediator(IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
    }

    internal static void ConfigureSelectedViewModelsProvider(IServiceCollection services)
    {
        services.AddSingleton<ISelectedViewModelsProvider, SelectedViewModelsProvider>();
    }

    internal static void ConfigureMyLearningWorlds(IServiceCollection services)
    {
        services.AddScoped<IMyLearningWorldsProvider, MyLearningWorldsProvider>();
        services.AddSingleton<ILearningWorldSavePathsHandler, LearningWorldSavePathsHandler>();
    }

    internal static void ConfigureAutoMapper(IServiceCollection services)
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

    internal static void ConfigureUtilities(IServiceCollection services)
    {
        services.AddTransient<IMemoryCache>(_ => new MemoryCache(new MemoryCacheOptions()));
        services.AddSingleton<IMouseService, MouseService>();
        services.AddTransient<IFileSystem, FileSystem>();
    }

    internal static void ConfigureCommands(IServiceCollection services)
    {
        services.AddSingleton<ICommandStateManager, CommandStateManager>();
        services.AddSingleton<IOnUndoRedo>(p => (CommandStateManager)p.GetService<ICommandStateManager>()!);
    }

    internal static void ConfigureCommandFactories(IServiceCollection services)
    {
        services.AddSingleton<IQuestionCommandFactory, QuestionCommandFactory>();
        services.AddSingleton<ITaskCommandFactory, TaskCommandFactory>();
        services.AddSingleton<IConditionCommandFactory, ConditionCommandFactory>();
        services.AddSingleton<IElementCommandFactory, ElementCommandFactory>();
        services.AddSingleton<ILayoutCommandFactory, LayoutCommandFactory>();
        services.AddSingleton<IPathwayCommandFactory, PathwayCommandFactory>();
        services.AddSingleton<ISpaceCommandFactory, SpaceCommandFactory>();
        services.AddSingleton<ITopicCommandFactory, TopicCommandFactory>();
        services.AddSingleton<IWorldCommandFactory, WorldCommandFactory>();
        services.AddSingleton<IUnsavedChangesResetHelper, UnsavedChangesResetHelper>();
        services.AddSingleton<IBatchCommandFactory, BatchCommandFactory>();
        services.AddSingleton<IAdaptivityRuleCommandFactory, AdaptivityRuleCommandFactory>();
        services.AddSingleton<IAdaptivityActionCommandFactory, AdaptivityActionCommandFactory>();
        services.AddSingleton<ILearningOutcomeCommandFactory, LearningOutcomeCommandFactory>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        CleanupH5pPlayer();

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
        var supportedCultures = new[] { "de-DE", "en-DE" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        // Clear all other providers and register the cookie provider as the only one
        localizationOptions.RequestCultureProviders.Clear();
        localizationOptions.AddInitialRequestCultureProvider(new CookieRequestCultureProvider());
        // Require request localization (this applies the requested culture to the actual application)
        app.UseRequestLocalization(localizationOptions);
        ThemeHelper<SpaceTheme>.Initialize(app.ApplicationServices.GetRequiredService<IStringLocalizer<SpaceTheme>>());
        ThemeHelper<WorldTheme>.Initialize(app.ApplicationServices.GetRequiredService<IStringLocalizer<WorldTheme>>());
        LearningElementDifficultyHelper.Initialize(app.ApplicationServices
            .GetRequiredService<IStringLocalizer<LearningElementDifficultyEnum>>());

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
        app.ElectronWindow(out var window);
        ElectronDialogManager.BackupBrowserWindow = window;
    }

    /// <summary>
    /// During the start process of the authoring tool we trigger the cleanup of the h5p-player
    /// For example, we delete the temporary files that are only needed to play the H5Ps and remain in the event of an uncontrolled crash. 
    /// </summary>
    private static void CleanupH5pPlayer()
    {
        var cleanupH5pPlayerPortFactory = new CleanupH5pPlayerPortFactory();
        var cleanupH5pPlayerPort = cleanupH5pPlayerPortFactory.CreateCleanupH5pPlayerPort();
        cleanupH5pPlayerPort.CleanDirectoryForTemporaryH5psInWwwroot();
    }
    // ReSharper restore InconsistentNaming
}