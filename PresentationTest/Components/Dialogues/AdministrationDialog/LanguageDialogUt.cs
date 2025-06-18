using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Culture;
using Presentation.Components.Dialogues.AdministrationDialog;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Dialogues.AdministrationDialog;

[TestFixture]
public class LanguageDialogUt
{
    [SetUp]
    public void SetUp()
    {
        _ctx = new TestContext();
        var localizer = Substitute.For<IStringLocalizer<LanguageDialog>>();
        localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _ctx.Services.AddSingleton(localizer);
        _ctx.ComponentFactories.AddStub<CultureSelector>();
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx;
    
    [Test]
    public void Render_ParametersSet()
    {
        var systemUnderTest = _ctx.RenderComponent<LanguageDialog>();
        
        Assert.That(systemUnderTest.Instance, Is.Not.Null);
        
        var cultureSelector = systemUnderTest.FindComponent<Stub<CultureSelector>>();
        Assert.That(cultureSelector.Instance, Is.Not.Null);
        
    }
}