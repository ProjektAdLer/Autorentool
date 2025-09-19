using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using H5pPlayer.Main;
using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TestContext = Bunit.TestContext;

namespace H5pPlayerTest.IntegrationTest;

[TestFixture]
public class DisplayH5pIt
{

    [Test]
    // ANF-ID: [HSE8]
    public void TerminateDisplayH5p()
    {
        _displayH5pController!.TerminateDisplayH5p();

        Assert.That(_wasOnH5pPlayerFinishedCalled, Is.True, "Expected OnH5pPlayerFinished to be called.");
        Assert.That(_receivedResult, Is.Not.Null);
        Assert.That(_receivedResult!.Value.ActiveH5pState, Is.EqualTo(H5pState.NotValidated.ToString()));
    }





    private string _basePath;
    private TestContext _testContext;
    private IDisplayH5pUc? _displayH5pUc;
    private IDisplayH5pController? _displayH5pController;
    private IDisplayH5pPresenter? _displayH5pPresenter;
    private ILoggerFactory? _loggerFactory;
    private IDisplayH5pFactory? _displayH5pFactory;
    private bool _wasOnH5pPlayerFinishedCalled;
    private H5pPlayerResultTO? _receivedResult;

    [SetUp]
    public void Setup()
    {
        _basePath = OperatingSystem.IsWindows() ? "C:" : "/";
        _testContext = new TestContext();
        var fakeJavaScriptAdapter = Substitute.For<ICallJavaScriptAdapter>();


        var fakeTerminateH5pPlayerUc = CreateFakeTerminateH5pPlayerUc(
            fakeJavaScriptAdapter,
            null,
            InitializeOnPlayerFinished()
        );
        _displayH5pFactory = new DisplayH5pFactory();
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _displayH5pFactory.CreateDisplayH5pStructure(fakeJavaScriptAdapter,fakeTerminateH5pPlayerUc, _loggerFactory);
        _displayH5pUc = _displayH5pFactory.DisplayH5pUc;
        var h5pEntity = CreateH5pEntity();
        _displayH5pUc!.H5pEntity = h5pEntity;
        _displayH5pController = _displayH5pFactory.DisplayH5pController;
        _displayH5pPresenter = _displayH5pFactory.DisplayH5pPresenter;
    }

    private Action<H5pPlayerResultTO> InitializeOnPlayerFinished()
    {
        Action<H5pPlayerResultTO> onH5pPlayerFinished = result =>
        {
            _wasOnH5pPlayerFinishedCalled = true;
            _receivedResult = result;
        };
        return onH5pPlayerFinished;
    }


    private static TerminateH5pPlayerUc CreateFakeTerminateH5pPlayerUc(
        ICallJavaScriptAdapter? callJavaScriptAdapter = null,
        IFileSystemDataAccess? fileSystemDataAccess = null,
        Action<H5pPlayerResultTO>? onH5pPlayerFinished = null)
    {
        callJavaScriptAdapter ??= Substitute.For<ICallJavaScriptAdapter>();
        fileSystemDataAccess ??= Substitute.For<IFileSystemDataAccess>();
        onH5pPlayerFinished ??= _ => { }; // Fake Lambda

        return new TerminateH5pPlayerUc(
            callJavaScriptAdapter,
            fileSystemDataAccess,
            onH5pPlayerFinished
        );
    }

    private H5pEntity CreateH5pEntity()
    {
        var h5pEntity = new H5pEntity();
        h5pEntity.UnzippedH5psPath = Path.Combine(_basePath, "ValidPath1.h5p");
        h5pEntity.H5pZipSourcePath = @Path.Combine(_basePath, "ValidPath2.h5p");
        return h5pEntity;
    }
    
    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
        _wasOnH5pPlayerFinishedCalled = false;
        _receivedResult = null;
        _loggerFactory?.Dispose();
    } 
}