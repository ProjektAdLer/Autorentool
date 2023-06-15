using AuthoringTool.View.Shared;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Culture;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.Mediator;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.Shared;

[TestFixture]
public class MainLayoutUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private IStringLocalizer<MainLayout> _stringLocalizer;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _stringLocalizer = Substitute.For<IStringLocalizer<MainLayout>>();
        _stringLocalizer[Arg.Any<string>()]
            .Returns(cinfo => new LocalizedString(cinfo.Arg<string>(), cinfo.Arg<string>()));
        
        _ctx.Services.AddSingleton(_stringLocalizer);
        _ctx.Services.AddLogging();

        _ctx.ComponentFactories.AddStub<MudThemeProvider>();
        _ctx.ComponentFactories.AddStub<MudDialogProvider>();
        _ctx.ComponentFactories.AddStub<MudSnackbarProvider>();
        _ctx.ComponentFactories.AddStub<CultureSelector>();
    }
    
    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetFragmentForTesting();
     
        Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(_stringLocalizer));
    }

    [Test]
    public void Render_ContainsMudBlazorStubs()
    {
        var systemUnderTest = GetFragmentForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.FindComponent<Stub<MudThemeProvider>>(), Throws.Nothing);
            Assert.That(() => systemUnderTest.FindComponent<Stub<MudDialogProvider>>(), Throws.Nothing);
            Assert.That(() => systemUnderTest.FindComponent<Stub<MudSnackbarProvider>>(), Throws.Nothing);
        });
    }

    private IRenderedComponent<MainLayout> GetFragmentForTesting(RenderFragment? body = null)
    {
        var options = Options.Create(new LocalizationOptions {ResourcesPath = "View/Shared"});
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        var localizer = new StringLocalizer<MainLayout>(factory);
        body ??= delegate {  };
        return _ctx.RenderComponent<MainLayout>(
            parameters => parameters
                .Add(p => p.Body, body)
            );
    }
}