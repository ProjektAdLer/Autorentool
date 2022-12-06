using AngleSharp.Dom;
using AuthoringTool.View.Shared;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.Toolbox;
using Presentation.View.Toolbox;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.Shared;

[TestFixture]
public class MainLayoutUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private IPresentationLogic _presentationLogic;
    private IAbstractToolboxRenderFragmentFactory _abstractToolboxRenderFragmentFactory;
    private IToolboxEntriesProvider _toolboxEntriesProvider;
    private IToolboxResultFilter _toolboxResultFilter;
    private IShutdownManager _shutdownManager;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _abstractToolboxRenderFragmentFactory = Substitute.For<IAbstractToolboxRenderFragmentFactory>();
        _toolboxEntriesProvider = Substitute.For<IToolboxEntriesProvider>();
        _toolboxResultFilter = Substitute.For<IToolboxResultFilter>();
        _shutdownManager = Substitute.For<IShutdownManager>();
        _ctx.Services.AddSingleton(_presentationLogic);
        _ctx.Services.AddSingleton(_abstractToolboxRenderFragmentFactory);
        _ctx.Services.AddSingleton(_toolboxEntriesProvider);
        _ctx.Services.AddSingleton(_toolboxResultFilter);
        _ctx.Services.AddSingleton(_shutdownManager);
        _ctx.Services.AddLogging();
    }
    
    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetFragmentForTesting();
        
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
    }

    [Test]
    public void Render_DisplaysBodyInArticle()
    {
        RenderFragment body = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "barbaz");
            builder.AddContent(2, "foobar");
            builder.CloseElement();
        };
        
        var systemUnderTest = GetFragmentForTesting(body);

        IElement? article = null;
        Assert.That(() => article = systemUnderTest.Find("div.page main article.content.px-4"), Throws.Nothing);
        if (article is null)
            Assert.Fail("Could not find article");
        
        article!.MarkupMatches(@"<article class=""content px-4""><div class=""barbaz"">foobar</div></article>");
    }

    [Test]
    public void Render_DisplaysNavMenu()
    {
        var systemUnderTest = GetFragmentForTesting();
        
        IElement? sidebarMain = null;
        Assert.That(() => sidebarMain = systemUnderTest.Find("div.page div.sidebar main"), Throws.Nothing);
        if (sidebarMain is null)
            Assert.Fail("Could not find sidebar main");
        
        Assert.That(sidebarMain!.Children, Has.Length.EqualTo(3));
    }
    
    [Test]
    public void Render_DisplaysToolbox()
    {
        var systemUnderTest = GetFragmentForTesting();
        
        IElement? toolbox = null;
        Assert.That(() => toolbox = systemUnderTest.Find("div.page div.sidebar footer div.grid-layout-display-root"), Throws.Nothing);
        if (toolbox is null)
            Assert.Fail("Could not find toolbox");
    }

    [Test]
    public void Render_NotRunningElectron_DoesNotDisplayCloseAppButton()
    {
        _presentationLogic.RunningElectron.Returns(false);
        var systemUnderTest = GetFragmentForTesting();
        
        IElement? buttonDiv = null;
        Assert.That(() => buttonDiv = systemUnderTest.Find("div.page main div.top-row.px-4"), Throws.Nothing);
        if (buttonDiv is null)
            Assert.Fail("Could not find close app button div");

        buttonDiv!.MarkupMatches(
            @"<div class=""top-row px-4""></div>");
    }

    [Test]
    public void Render_RunningElectron_DoesDisplayCloseAppButton()
    {
        _presentationLogic.RunningElectron.Returns(true);
        var systemUnderTest = GetFragmentForTesting();
        
        IElement? buttonDiv = null;
        Assert.That(() => buttonDiv = systemUnderTest.Find("div.page main div.top-row.px-4"), Throws.Nothing);
        if (buttonDiv is null)
            Assert.Fail("Could not find close app button div");

        buttonDiv!.MarkupMatches(
            @"<div class=""top-row px-4""><button class=""btn btn-danger"">Close application</button></div>");
    }
    
    

    private IRenderedComponent<MainLayout> GetFragmentForTesting(RenderFragment? body = null)
    {
        body ??= delegate {  };
        return _ctx.RenderComponent<MainLayout>(
            parameters => parameters
                .Add(p => p.Body, body)
            );
    }
}