using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Main;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;
using H5pPlayer.Presentation.View;
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


    private string _basePath;
    private TestContext _testContext;
    private IValidateH5pUc? _validateH5pUc;
    private IValidateH5pViewModel? _validateH5pVm;
    private IValidateH5pController? _validateH5pController;
    private IValidateH5pPresenter? _validateH5pPresenter;
    private IValidateH5pFactory? _validateH5pFactory;

    [SetUp]
    public void Setup()
    {
        _basePath = OperatingSystem.IsWindows() ? "C:" : "/";
        _testContext = new TestContext();
        var fakeJavaScriptAdapter = Substitute.For<ICallJavaScriptAdapter>();
        _validateH5pFactory = new ValidateH5pFactory();
        _validateH5pFactory.CreateValidateH5pStructure(fakeJavaScriptAdapter);
        _validateH5pUc = _validateH5pFactory.ValidateH5pUc;
        var unzippedH5psPath = Path.Combine(_basePath, "ValidPath1.h5p");
        var h5pZipSourcePath = @Path.Combine(_basePath, "ValidPath2.h5p");
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);
        _validateH5pUc!.H5pEntity = h5pEntity;
        _validateH5pVm = _validateH5pFactory.ValidateH5pVm;
        Action fakeAction = () => { };
        _validateH5pVm!.OnChange += fakeAction;
        _validateH5pController = _validateH5pFactory.ValidateH5pController;
        _validateH5pPresenter = _validateH5pFactory.ValidateH5pPresenter;
    }

    private static H5pEntity CreateH5pEntity(
        string unzippedH5psPath, string h5pZipSourcePath)
    {
        var h5pEntity = new H5pEntity();
        h5pEntity.UnzippedH5psPath = unzippedH5psPath;
        h5pEntity.H5pZipSourcePath = h5pZipSourcePath;
        return h5pEntity;
    }
    
    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    } 
}