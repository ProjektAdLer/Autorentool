using AngleSharp.Dom;
using AuthoringTool.View.Shared;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.Shared;

[TestFixture]
public class MainLayoutUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private IPresentationLogic _presentationLogic;
    private IShutdownManager _shutdownManager;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _shutdownManager = Substitute.For<IShutdownManager>();
        _ctx.Services.AddSingleton(_presentationLogic);
        _ctx.Services.AddSingleton(_shutdownManager);
        _ctx.ComponentFactories.AddStub<MudThemeProvider>();
        _ctx.ComponentFactories.AddStub<MudDialogProvider>();
        _ctx.ComponentFactories.AddStub<MudSnackbarProvider>();
        _ctx.Services.AddLogging();
    }
    
    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetFragmentForTesting();
        
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
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

    [Test]
    public void Render_RunningElectron_DoesDisplayCloseAppButton()
    {
        _presentationLogic.RunningElectron.Returns(true);
        var systemUnderTest = GetFragmentForTesting();
        
        IElement? buttonDiv = null;
        Assert.That(() => buttonDiv = systemUnderTest.Find("div.absolute.top-0.right-0"), Throws.Nothing);
        if (buttonDiv is null)
            Assert.Fail("Could not find close app button div");

        buttonDiv!.MarkupMatches(
            @"<div class=""absolute top-0 right-0""><button class=""btn btn-danger"">Close application</button></div>");
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