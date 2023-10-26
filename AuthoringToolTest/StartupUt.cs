using System.IO.Abstractions;
using System.Net.Http.Handlers;
using AuthoringTool;
using AutoMapper;
using BackendAccess.BackendServices;
using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Adaptivity.Action;
using BusinessLogic.Commands.Adaptivity.Rule;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Commands.World;
using BusinessLogic.Validation;
using BusinessLogic.Validation.Validators;
using DataAccess.Persistence;
using Generator.DSL;
using Generator.WorldExport;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.DropZone;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;
using Shared.Configuration;
using IHttpClientFactory = Shared.Networking.IHttpClientFactory;

namespace AuthoringToolTest;

[TestFixture]
public class StartupUt
{
    [Test]
    public void Startup_Constructor_AllPropertiesInitialized()
    {
        var configuration = new ConfigurationManager();
        configuration["foo"] = "bar";
        var environment = Substitute.For<IWebHostEnvironment>();

        var systemUnderTest = new Startup(configuration, environment);

        Assert.That(systemUnderTest.Configuration, Is.SameAs(configuration));
        Assert.That(systemUnderTest.Configuration["foo"], Is.EqualTo("bar"));
    }


    private static readonly Type[] ConfigureAuthoringToolRequiredTypes =
    {
        typeof(IApplicationConfiguration)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureAuthoringToolRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllAuthoringToolServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigureUtilitiesRequiredTypes =
    {
        typeof(IMemoryCache), typeof(IMouseService), typeof(IFileSystem)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureUtilitiesRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllUtilitiesServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }


    private static readonly Type[] ConfigureDataAccessRequiredTypes =
    {
        typeof(IXmlFileHandler<LearningWorldPe>), typeof(IXmlFileHandler<LearningElementPe>),
        typeof(IXmlFileHandler<LearningSpacePe>),
        typeof(IDataAccess), typeof(IContentFileHandler)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureDataAccessRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllDataAccessServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigureApiAccessRequiredTypes =
    {
        typeof(IBackendAccess), typeof(HttpClient), typeof(IUserWebApiServices)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureApiAccessRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllApiAccessServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigurePresentationLogicRequiredTypes =
    {
        typeof(IPresentationLogic), typeof(IAuthoringToolWorkspacePresenter), typeof(ILearningWorldPresenter),
        typeof(ILearningSpacePresenter), typeof(IAuthoringToolWorkspaceViewModel),
        typeof(ILearningElementDropZoneHelper), typeof(IElementModelHandler)
    };

    [Test]
    [TestCaseSource(nameof(ConfigurePresentationLogicRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllPresentationLogicServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }


    private static readonly Type[] ConfigureBusinessLogicRequiredTypes =
    {
        typeof(IBusinessLogic)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureBusinessLogicRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllBusinessLogicServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigureGeneratorRequiredTypes =
    {
        typeof(IWorldGenerator), typeof(IBackupFileGenerator), typeof(ICreateDsl), typeof(IReadDsl)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureGeneratorRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllGeneratorServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigureAutoMapperRequiredTypes =
    {
        typeof(IMapper), typeof(ICachingMapper)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureAutoMapperRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllAutoMapperServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }


    private static readonly Type[] ConfigureCommandRequiredTypes =
    {
        typeof(ICommandStateManager)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureCommandRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllCommandServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigureCommandFactoriesRequiredTypes =
    {
        typeof(IConditionCommandFactory), typeof(IElementCommandFactory), typeof(ILayoutCommandFactory),
        typeof(IPathwayCommandFactory), typeof(ISpaceCommandFactory), typeof(ITopicCommandFactory),
        typeof(IWorldCommandFactory), typeof(IBatchCommandFactory), typeof(IAdaptivityRuleCommandFactory),
        typeof(IAdaptivityActionCommandFactory)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureCommandFactoriesRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllCommandFactoriesServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigureValidationRequiredTypes =
    {
        typeof(LearningWorldValidator), typeof(LearningSpaceValidator), typeof(LearningElementValidator),
        typeof(LearningContentValidator), typeof(FileContentValidator), typeof(LinkContentValidator),
        typeof(MultipleChoiceQuestionValidator), typeof(MultipleChoiceMultipleResponseQuestionValidator),
        typeof(MultipleChoiceSingleResponseQuestionValidator), typeof(ChoiceValidator),
        typeof(AdaptivityContentValidator),
        typeof(ILearningSpaceNamesProvider), typeof(ILearningWorldNamesProvider), typeof(ILearningElementNamesProvider)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureValidationRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllValidationServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }

    private static readonly Type[] ConfigureNetworkingRequiredTypes =
    {
        typeof(IHttpClientFactory), typeof(ProgressMessageHandler)
    };

    [Test]
    [TestCaseSource(nameof(ConfigureNetworkingRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllNetworkingServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }


    private Startup GetStartupForTesting(IConfiguration? configuration = null, IWebHostEnvironment? environment = null)
    {
        configuration ??= new ConfigurationManager();
        environment ??= Substitute.For<IWebHostEnvironment>();
        return new Startup(configuration, environment);
    }

    private void ConfigureServicesCoreTest(Type requiredType)
    {
        var systemUnderTest = GetStartupForTesting();
        //prepare provider
        using var provider = PrepareProvider(systemUnderTest);
        Assert.That(provider.GetService(requiredType), Is.Not.Null);
    }

    private ServiceProvider PrepareProvider(Startup systemUnderTest)
    {
        var serviceCollection = new ServiceCollection();
        systemUnderTest.ConfigureServices(serviceCollection);
        serviceCollection.AddLogging();
        return serviceCollection.BuildServiceProvider();
    }

    //Internal methods, you should not need to touch these to add, modify or fix a test
}