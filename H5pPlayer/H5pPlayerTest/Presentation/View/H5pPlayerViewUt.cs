using Bunit.Rendering;
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

        RenderViewToStartDisplayH5p(systemUnderTest);

        var displayH5pViewComponent = systemUnderTest.FindComponent<DisplayH5pView>();
        Assert.That(displayH5pViewComponent, Is.Not.Null); 
        Assert.Throws<ComponentNotFoundException>(() => systemUnderTest.FindComponent<ValidateH5pView>()); 
    }

    private static void RenderViewToStartDisplayH5p(IRenderedComponent<H5pPlayerView> systemUnderTest)
    {
        systemUnderTest.Render(); // Triggers OnAfterRender -> sets ActiveView for the *next* render
        systemUnderTest.Render(); // *This* render uses the previously set ActiveView
    }


    [Test]
    public void StartToValidateH5p()
    {
        var systemUnderTest = ArrangeAndFirstRender();
        systemUnderTest.Instance.H5pPlayerVm!.IsValidationModeActive = true;
        
        RenderViewToStartValidateH5p(systemUnderTest);

        var validateH5pViewComponent = systemUnderTest.FindComponent<ValidateH5pView>();
         Assert.That(validateH5pViewComponent, Is.Not.Null);
         Assert.Throws<ComponentNotFoundException>(() => systemUnderTest.FindComponent<DisplayH5pView>()); 
    }

    private static void RenderViewToStartValidateH5p(IRenderedComponent<H5pPlayerView> systemUnderTest)
    {
        systemUnderTest.Render(); // Triggers OnAfterRender -> sets ActiveView for the *next* render
        systemUnderTest.Render(); // *This* render uses the previously set ActiveView
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
        var validateH5pFactory = Substitute.For<IValidateH5pFactory>();
        

        _testContext.Services.AddScoped(_ => startH5pPlayerFactory);
        _testContext.Services.AddScoped(_ => displayH5pFactory);
        _testContext.Services.AddScoped(_ => validateH5pFactory);
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