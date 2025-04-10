using H5pPlayer.BusinessLogic.Api.JavaScript;
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


    
    private TestContext _testContext;
    private IValidateH5pUc? _validateH5pUc;
    private IValidateH5pViewModel? _validateH5pVm;
    private IValidateH5pController? _validateH5pController;
    private IValidateH5pPresenter? _validateH5pPresenter;
    private IValidateH5pFactory? _validateH5pFactory;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        var fakeJavaScriptAdapter = Substitute.For<ICallJavaScriptAdapter>();
        _validateH5pFactory = new ValidateH5pFactory();
        _validateH5pFactory.CreateValidateH5pStructure(fakeJavaScriptAdapter);
        _validateH5pUc = _validateH5pFactory.ValidateH5pUc;
        _validateH5pVm = _validateH5pFactory.ValidateH5pVm;
        Action fakeAction = () => { };
        _validateH5pVm!.OnChange += fakeAction;
        _validateH5pController = _validateH5pFactory.ValidateH5pController;
        _validateH5pPresenter = _validateH5pFactory.ValidateH5pPresenter;
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    } 
}