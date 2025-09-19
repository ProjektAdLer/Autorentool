using H5pPlayer.Presentation.View;
using NSubstitute;
using Bunit;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.Main;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TestContext = Bunit.TestContext;

namespace H5pPlayerTest.IntegrationTest;

[TestFixture]
public class StartH5pPlayerIt
{
    [Test]
    public void CreateStartH5pPlayerPresentationStructure()
    {
        var _systemUnderTest = CreateAndFirstRenderH5pPlayerView();
        var viewModel = _systemUnderTest.Instance.H5pPlayerVm;
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(_systemUnderTest.Instance.H5pPlayerController, Is.Not.Null);
        Assert.That(_systemUnderTest.Instance.H5pPlayerController.StartH5pPlayerUc, Is.Not.Null);
        Assert.That(_systemUnderTest.Instance.H5pPlayerController.H5pPlayerPresenter, Is.Not.Null);
        Assert.That(_systemUnderTest.Instance.H5pPlayerController.H5pPlayerPresenter.H5pPlayerVm, Is.EqualTo(viewModel));
    }

    [Test]
    public void StartH5pPlayerToDisplay()
    {
        CreateAndFirstRenderH5pPlayerView(H5pDisplayMode.Display);
        
        Assert.That(_startH5pPlayerFactory!.H5pPlayerVm!.IsValidationModeActive, Is.False);
        Assert.That(_startH5pPlayerFactory.H5pPlayerVm.IsDisplayModeActive, Is.True);
    }
    
    [Test]
    public void StartH5pPlayerToValidate()
    {
        CreateAndFirstRenderH5pPlayerView(H5pDisplayMode.Validate);
        
        Assert.That(_startH5pPlayerFactory!.H5pPlayerVm!.IsValidationModeActive, Is.True);
        Assert.That(_startH5pPlayerFactory.H5pPlayerVm.IsDisplayModeActive, Is.False);
    }

    private IRenderedComponent<H5pPlayerView> CreateAndFirstRenderH5pPlayerView(
        H5pDisplayMode displayMode = H5pDisplayMode.Display,
        string? h5pZipSourcePath = null,
        string? unzippedH5psPath = null)
    {
        h5pZipSourcePath ??= Path.Combine(Path.GetTempPath(), "h5p-zip-test");
        unzippedH5psPath ??= Path.Combine(Path.GetTempPath(), "h5p-unzipped-test");

        // Create a test wrapper that includes the MudPopoverProvider
        var testWrapper = _testContext.RenderComponent<TestWrapper>(parameters => parameters
            .Add(p => p.DisplayMode, displayMode)
            .Add(p => p.H5pZipSourcePath, h5pZipSourcePath)
            .Add(p => p.UnzippedH5psPath, unzippedH5psPath)
        );

        return testWrapper.FindComponent<H5pPlayerView>();
    }



    private TestContext _testContext;
    private IStartH5pPlayerFactory? _startH5pPlayerFactory;
    private ILoggerFactory? _loggerFactory;
    private ICallJavaScriptAdapter? _fakeJavaScriptAdapter;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        
        // Set JSInterop to loose mode to handle MudBlazor JS calls
        _testContext.JSInterop.Mode = JSRuntimeMode.Loose;
        
        // Add localization services for internationalization
        _testContext.Services.AddLocalization();
        
        // If AddLocalization() doesn't work, add fake localizer services
        var fakeStringLocalizer = Substitute.For<IStringLocalizer>();
        var fakeStringLocalizerFactory = Substitute.For<IStringLocalizerFactory>();
        fakeStringLocalizerFactory.Create(Arg.Any<Type>()).Returns(fakeStringLocalizer);
        fakeStringLocalizerFactory.Create(Arg.Any<string>(), Arg.Any<string>()).Returns(fakeStringLocalizer);
        _testContext.Services.AddSingleton(fakeStringLocalizerFactory);
        _testContext.Services.AddSingleton(fakeStringLocalizer);
        
        // Create fake JavaScript adapter for your H5P calls
        _fakeJavaScriptAdapter = Substitute.For<ICallJavaScriptAdapter>();
        
        // Add MudBlazor services
        _testContext.Services.AddMudServices();
        
        // Setup your factories and dependencies
        var displayH5pFactory = new DisplayH5pFactory();
        var validateH5pFactory = new ValidateH5pFactory();
        var fakeFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        _loggerFactory = Substitute.For<ILoggerFactory>();
        
        // Inject the fake JavaScript adapter into the factory
        _startH5pPlayerFactory = new StartH5pPlayerFactory(
            displayH5pFactory, 
            validateH5pFactory, 
            fakeFileSystemDataAccess, 
            _loggerFactory,
            _fakeJavaScriptAdapter);  // Pass the fake adapter
 
        _testContext.Services.AddTransient(_ => _startH5pPlayerFactory);
        _testContext.Services.AddSingleton(Substitute.For<IDialogService>());
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
        _loggerFactory?.Dispose();
    }
    
    // Helper component for testing
    public class TestWrapper : ComponentBase
    {
        [Parameter] public H5pDisplayMode DisplayMode { get; set; }
        [Parameter] public string? H5pZipSourcePath { get; set; }
        [Parameter] public string? UnzippedH5psPath { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            
            builder.OpenComponent<H5pPlayerView>(1);
            builder.AddAttribute(2, nameof(H5pPlayerView.DisplayMode), DisplayMode);
            builder.AddAttribute(3, nameof(H5pPlayerView.H5pZipSourcePath), H5pZipSourcePath);
            builder.AddAttribute(4, nameof(H5pPlayerView.UnzippedH5psPath), UnzippedH5psPath);
            builder.CloseComponent();
        }
    }
}