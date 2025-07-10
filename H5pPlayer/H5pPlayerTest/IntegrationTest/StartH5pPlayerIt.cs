using H5pPlayer.Presentation.View;
using NSubstitute;
using Bunit;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.Main;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
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

        var systemUnderTest = _testContext.RenderComponent<H5pPlayerView>(parameters => parameters
            .Add(p => p.DisplayMode, displayMode)
            .Add(p => p.H5pZipSourcePath, h5pZipSourcePath)
            .Add(p => p.UnzippedH5psPath, unzippedH5psPath)
        );

        return systemUnderTest;
    }


    private TestContext _testContext;
    private IStartH5pPlayerFactory? _startH5pPlayerFactory;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        var displayH5pFactory = new DisplayH5pFactory();
        var validateH5pFactory = new ValidateH5pFactory();
        var fakeFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        _startH5pPlayerFactory = new StartH5pPlayerFactory(displayH5pFactory, validateH5pFactory, fakeFileSystemDataAccess);
 
        _testContext.Services.AddTransient(_ => _startH5pPlayerFactory);
        _testContext.Services.AddSingleton<IDialogService>(Substitute.For<IDialogService>());
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

}