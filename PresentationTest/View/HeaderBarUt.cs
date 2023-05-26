using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.Culture;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View;

[TestFixture]
public class HeaderBarUt
{
    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
    private IMediator _mediator;
    private IStringLocalizer<HeaderBar> _stringLocalizer;
    private ISnackbar _snackbar;
    private IDialogService _dialogService;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.ComponentFactories.AddStub<CloseAppButton>();
        _testContext.ComponentFactories.AddStub<CultureSelector>();
        _testContext.ComponentFactories.AddStub<LmsLoginButton>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _mediator = Substitute.For<IMediator>();
        _stringLocalizer = Substitute.For<IStringLocalizer<HeaderBar>>();
        _snackbar = Substitute.For<ISnackbar>();
        _dialogService = Substitute.For<IDialogService>();
        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_stringLocalizer);
        _testContext.Services.AddSingleton(_selectedViewModelsProvider);
        _testContext.Services.AddSingleton(_mediator);
        _testContext.Services.AddSingleton(_snackbar);
        _testContext.Services.AddSingleton(_dialogService);
    }

    [Test]
    public void Render_RunningElectronTrue_ContainsCloseAppButtonStub()
    {
        _presentationLogic.RunningElectron.Returns(true);

        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponentOrFail<Stub<CloseAppButton>>(), Throws.Nothing);
    }

    [Test]
    public void Render_RunningElectronFalse_ContainsNoCloseAppButtonStub()
    {
        _presentationLogic.RunningElectron.Returns(false);

        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponent<Stub<CloseAppButton>>(),
            Throws.TypeOf<ComponentNotFoundException>());
    }

    [Test]
    public void Render_ContainsCultureSelectorStub()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponent<Stub<CultureSelector>>(), Throws.Nothing);
    }

    [Test]
    public void Render_ContainsLmsLoginButtonStub()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponent<Stub<LmsLoginButton>>(), Throws.Nothing);
    }

    [Test]
    public void Render_ShowsLocalizedAuthoringToolName()
    {
        _stringLocalizer["AuthoringTool.Text"].Returns(new LocalizedString("AuthoringTool.Text", "TestName"));
        _stringLocalizer["AuthoringTool.Version"].Returns(new LocalizedString("AuthoringTool.Version", "v3"));
        
        var systemUnderTest = GetRenderedComponent();
        
        var element = systemUnderTest.Find("header div h1");
        element.MarkupMatches(@"<h1 class=""font-bold text-lg"">TestName v3</h1>");
    }

    private IRenderedComponent<HeaderBar> GetRenderedComponent()
    {
        return _testContext.RenderComponent<HeaderBar>();
    }
}