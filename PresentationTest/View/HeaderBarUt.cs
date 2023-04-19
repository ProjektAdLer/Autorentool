using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.Culture;
using Presentation.PresentationLogic.API;
using Presentation.View;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View;

[TestFixture]
public class HeaderBarUt
{
    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private IStringLocalizer<HeaderBar> _stringLocalizer;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.ComponentFactories.AddStub<CloseAppButton>();
        _testContext.ComponentFactories.AddStub<CultureSelector>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _stringLocalizer = Substitute.For<IStringLocalizer<HeaderBar>>();
        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_stringLocalizer);
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

    private IRenderedComponent<HeaderBar> GetRenderedComponent()
    {
        return _testContext.RenderComponent<HeaderBar>();

    }
}