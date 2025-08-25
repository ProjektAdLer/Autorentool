using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Main;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TestContext = Bunit.TestContext;

namespace H5pPlayerTest.IntegrationTest;

[TestFixture]
public class ValidateH5pIt
{
    // [Test]
    // public void StartToValidateH5p()
    // {
    //     
    //     var systemUnderTest = _testContext.RenderComponent<ValidateH5pView>(parameters => parameters
    //         .Add(p => p.ValidateH5pVm, _validateH5pVm)
    //         .Add(p => p.ValidateH5pController, _validateH5pController)
    //     );
    //     
    //     // to do test with refactoring
    //     //simulate call from view: _validateH5pFactory.ValidateH5pUc.StartToValidateH5p()
    // }

    
    [Test]
    public void ValidateH5p()
    {
        var validateH5pTO = new ValidateH5pTO(true);

        _validateH5pUc!.ValidateH5p(validateH5pTO);
        
        Assert.That(_validateH5pVm!.IsCompletable, Is.True);
    }

    [Test]
    // ANF-ID: [HSE8]
    public void TerminateValidateH5p()
    {
        _validateH5pController!.TerminateValidateH5p();

        Assert.That(_wasOnH5pPlayerFinishedCalled, Is.True, "Expected OnH5pPlayerFinished to be called.");
        Assert.That(_receivedResult, Is.Not.Null);
        Assert.That(_receivedResult!.Value.ActiveH5pState, Is.EqualTo(H5pState.NotValidated.ToString()));
    }



    [Test]
    public void SetActiveH5pStateToNotUsable()
    {
        _validateH5pController!.SetActiveH5pStateToNotUsable();

        Assert.That(_validateH5pVm!.ActiveH5PState, Is.EqualTo(H5pState.NotUsable));
    }

    [Test]
    public void SetActiveH5pStateToPrimitive()
    {
        _validateH5pController!.SetActiveH5pStateToPrimitive();

        Assert.That(_validateH5pVm!.ActiveH5PState, Is.EqualTo(H5pState.Primitive));
    }
    
    [Test]
    public void SetActiveH5pStateToCompletable()
    {
        _validateH5pController!.SetActiveH5pStateToCompletable();

        Assert.That(_validateH5pVm!.ActiveH5PState, Is.EqualTo(H5pState.Completable));
    }


    private string _basePath;
    private TestContext _testContext;
    private IValidateH5pUc? _validateH5pUc;
    private IValidateH5pViewModel? _validateH5pVm;
    private IValidateH5pController? _validateH5pController;
    private IValidateH5pPresenter? _validateH5pPresenter;
    private ILoggerFactory? _loggerFactory;
    private IValidateH5pFactory? _validateH5pFactory;
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
        _validateH5pFactory = new ValidateH5pFactory();
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _validateH5pFactory.CreateValidateH5pStructure(fakeJavaScriptAdapter,fakeTerminateH5pPlayerUc,_loggerFactory);
        _validateH5pUc = _validateH5pFactory.ValidateH5pUc;
        var h5pEntity = CreateH5pEntity();
        _validateH5pUc!.H5pEntity = h5pEntity;
        _validateH5pVm = _validateH5pFactory.ValidateH5pVm;
        Action fakeAction = () => { };
        _validateH5pVm!.OnChange += fakeAction;
        _validateH5pController = _validateH5pFactory.ValidateH5pController;
        _validateH5pPresenter = _validateH5pFactory.ValidateH5pPresenter;
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

    private  H5pEntity CreateH5pEntity()
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