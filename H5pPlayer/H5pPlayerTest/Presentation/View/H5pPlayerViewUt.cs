using Bunit;
using H5pPlayer.Main;
using H5pPlayer.Presentation.View;
using H5pPlayer.Presentation.PresentationLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace H5pPlayerTest.Presentation.View
{
    [TestFixture]
    public class H5pPlayerViewUt
    {
        private TestContext _testContext;
        private IStartH5pPlayerFactory _fakeFactory;
        private H5pPlayerViewModel _fakeViewModel;

        [SetUp]
        public void Setup()
        {
            _testContext = new TestContext();

            
            CreateFakeViewModel();


            _testContext.Services.AddSingleton(_fakeFactory);
            _testContext.Services.AddSingleton(Substitute.For<IJSRuntime>());
        }

        private void CreateFakeViewModel()
        {
            Action fakeAction = () => { };
            _fakeViewModel = new H5pPlayerViewModel(fakeAction);
        }

    

        private IRenderedComponent<H5pPlayerView> ArrangeAndFirstRender()
        {
            return _testContext.RenderComponent<H5pPlayerView>();
        }

        [TearDown]
        public void TearDown()
        {
            _testContext.Dispose();
        }
    }
}
