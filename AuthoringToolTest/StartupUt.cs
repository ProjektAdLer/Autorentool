using System;
using System.IO.Abstractions;
using AuthoringTool;
using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.ModalDialog;
using AuthoringTool.PresentationLogic.Toolbox;
using AuthoringTool.View.Toolbox;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AuthoringToolTest;

[TestFixture]
public class StartupUt
{
    [Test]
    public void Startup_Constructor_AllPropertiesInitialized()
    {
        var configuration = new ConfigurationManager();
        configuration["foo"] = "bar";

        var systemUnderTest = new Startup(configuration);
        
        Assert.That(systemUnderTest.Configuration, Is.SameAs(configuration));
        Assert.That(systemUnderTest.Configuration["foo"], Is.EqualTo("bar"));
    }
    
    
    
    private static readonly Type[] ConfigureAuthoringToolRequiredTypes =
    {
        typeof(IAuthoringToolConfiguration)
    };
    [Test]
    [TestCaseSource(nameof(ConfigureAuthoringToolRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllAuthoringToolServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }
    
    
    private static readonly Type[] ConfigureToolboxRequiredTypes =
    {
        typeof(IAbstractToolboxRenderFragmentFactory), typeof(IToolboxEntriesProviderModifiable),
        typeof(IToolboxEntriesProvider), typeof(IToolboxController), typeof(IToolboxResultFilter),
        typeof(IAuthoringToolWorkspacePresenterToolboxInterface), typeof(ILearningWorldPresenterToolboxInterface),
        typeof(ILearningSpacePresenterToolboxInterface)
    };
    [Test]
    [TestCaseSource(nameof(ConfigureToolboxRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllToolboxServices(Type requiredType)
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
        typeof(IXmlFileHandler<LearningWorldPe>), typeof(IXmlFileHandler<LearningElementPe>), typeof(IXmlFileHandler<LearningSpacePe>),
        typeof(IDataAccess), typeof(ICreateDsl), typeof(IReadDsl), typeof(IContentFileHandler), typeof(IBackupFileGenerator),
        
    };
    [Test]
    [TestCaseSource(nameof(ConfigureDataAccessRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllDataAccessServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }


    private static readonly Type[] ConfigurePresentationLogicRequiredTypes =
    {
        typeof(IPresentationLogic), typeof(IAuthoringToolWorkspacePresenter), typeof(ILearningWorldPresenter),
        typeof(ILearningSpacePresenter), typeof(ILearningElementPresenter), typeof(IAuthoringToolWorkspaceViewModel),
        typeof(ILearningSpaceViewModalDialogFactory), typeof(ILearningSpaceViewModalDialogInputFieldsFactory),
        typeof(ILearningWorldViewModalDialogFactory), typeof(ILearningWorldViewModalDialogInputFieldsFactory),
        typeof(IAuthoringToolWorkspaceViewModalDialogFactory), typeof(IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory),
        typeof(ILearningWorldViewModalDialogInputFieldsFactory)
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
    
    private static readonly Type[] ConfigureAutoMapperRequiredTypes =
    {
        typeof(IMapper)
    };
    [Test]
    [TestCaseSource(nameof(ConfigureAutoMapperRequiredTypes))]
    public void Startup_ConfigureServices_CanResolveAllAutoMapperServices(Type requiredType)
    {
        ConfigureServicesCoreTest(requiredType);
    }
    
    
    private Startup GetStartupForTesting(IConfiguration? configuration = null)
    {
        configuration ??= new ConfigurationManager();
        return new Startup(configuration);
    }

    //Internal methods, you should not need to touch these to add, modify or fix a test
    #region internal
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

    #endregion
}