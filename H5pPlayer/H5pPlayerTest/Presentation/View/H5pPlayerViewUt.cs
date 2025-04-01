using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Main;
using H5pPlayer.Presentation.PresentationLogic;
using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;
using H5pPlayer.Presentation.View;
using NSubstitute;

namespace H5pPlayerTest.Presentation.View;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TestContext = Bunit.TestContext;

[TestFixture]
public class H5pPlayerViewUt
{
        
    private TestContext _testContext;
    private H5pPlayerViewModel? _viewModel;
    private H5pPlayerController? _fakeController;


    [Test]
    public void StartToDisplayH5p()
    {
        var systemUnderTest = ArrangeAndFirstRender();
        systemUnderTest.Instance.H5pPlayerVm!.IsDisplayModeActive = true;
        
        Assert.That(systemUnderTest.Instance.ActiveView, Is.Null);
        
        systemUnderTest.Render();
        
        var displayH5pViewComponent = systemUnderTest.FindComponent<DisplayH5pView>();
        Assert.That(displayH5pViewComponent, Is.Not.Null);
    }
    
    
    [Test]
    public void StartToValidateH5p()
    {
        var systemUnderTest = ArrangeAndFirstRender();
        systemUnderTest.Instance.H5pPlayerVm!.IsValidationModeActive = true;
        
        Assert.That(systemUnderTest.Instance.ActiveView, Is.Null);
        
        systemUnderTest.Render();
        
        // var displayH5pViewComponent = systemUnderTest.FindComponent<ValidateH5p>();
        // Assert.That(displayH5pViewComponent, Is.Not.Null);
    }

    private IRenderedComponent<H5pPlayerView> ArrangeAndFirstRender()
    {
        return _testContext.RenderComponent<H5pPlayerView>();
    }

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

      

        var startH5pPlayerFactory = CreateStartH5pPlayerFactory();
        var displayH5pFactory = Substitute.For<IDisplayH5pFactory>();
        

        _testContext.Services.AddScoped(_ => startH5pPlayerFactory);
        _testContext.Services.AddScoped(_ => displayH5pFactory);
    }

    private IStartH5pPlayerFactory CreateStartH5pPlayerFactory()
    {
        var startH5pPlayerFactory = Substitute.For<IStartH5pPlayerFactory>();
        
        _viewModel = new H5pPlayerViewModel(new Action(() =>{}));
        startH5pPlayerFactory.H5pPlayerVm.Returns(_viewModel);
        
        var stubUseCase = Substitute.For<IStartH5pPlayerUCInputPort>();
        var presenter = new H5pPlayerPresenter(_viewModel);
        _fakeController = new H5pPlayerController(stubUseCase, presenter);
        startH5pPlayerFactory.H5pPlayerController.Returns(_fakeController);

        var displayUc = Substitute.For<IDisplayH5pUC>();
        startH5pPlayerFactory.DisplayH5pUc.Returns(displayUc);
        
        var validateUc = Substitute.For<IValidateH5pUc>();
        startH5pPlayerFactory.ValidateH5pUc.Returns(validateUc);
        return startH5pPlayerFactory;
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

}