using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;
using H5pPlayer.Presentation.View;
using NSubstitute;
using Bunit;
using H5pPlayer.Main;
using Microsoft.Extensions.DependencyInjection;
using TestContext = Bunit.TestContext;


namespace H5pPlayerTest.IntegrationTest;

[TestFixture]
public class StartH5pPlayerIt
{
    private TestContext _testContext;
    private IStartH5pPlayerFactory _startH5pPlayerFactory;
    
    [Test]
    public void CreateViewControllerPresenterViewModelStructure()
    {
        var systemUnderTest = _testContext.RenderComponent<H5pPlayerView>();
        
        systemUnderTest.Instance.InitializeStartH5pPlayer();

        var viewModel = systemUnderTest.Instance.H5pPlayerVm;
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.H5pPlayerController, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.H5pPlayerController.StartH5PPlayerUc, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.H5pPlayerController.H5PPlayerPresenter, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.H5pPlayerController.H5PPlayerPresenter.H5pPlayerVm, Is.EqualTo(viewModel));
        
    }


    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _startH5pPlayerFactory = new StartH5pPlayerFactory();

        _testContext.Services.AddTransient(_ => _startH5pPlayerFactory);
        
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

}