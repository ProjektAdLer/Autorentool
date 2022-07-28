using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.View.LearningSpace;
using AuthoringTool.View.LearningWorld;
using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.LearningWorld;

[TestFixture]
public class LearningWorldViewUt
{
#pragma warning disable CS8618 set in setup - n.stich
    private TestContext _ctx;
    private IMouseService _mouseService;
    private ILearningWorldPresenter _worldPresenter;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _worldPresenter = Substitute.For<ILearningWorldPresenter>();
        _ctx.ComponentFactories.AddStub<LearningSpaceView>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_worldPresenter);
    }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.MouseService, Is.EqualTo(_mouseService));
            Assert.That(systemUnderTest.Instance.LearningWorldP, Is.EqualTo(_worldPresenter));
        });
    }

    [Test]
    public void Render_ChildContentSet_RendersChildContent()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "barbaz");
            builder.AddContent(2, "foobar");
            builder.CloseElement();
        };
        
        var systemUnderTest = GetLearningWorldViewForTesting(childContent);

        Assert.That(systemUnderTest.FindOrFail("div.barbaz").TextContent, Is.EqualTo("foobar"));
    }

    [Test]
    public void Render_ShowingLearningSpaceFalse_DoesNotRenderLearningSpaceView()
    {
        _worldPresenter.ShowingLearningSpaceView.Returns(false);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        Assert.That(systemUnderTest.FindComponent<Stub<LearningSpaceView>>,
            Throws.TypeOf<ComponentNotFoundException>());
    }

    [Test]
    public void Render_ShowingLearningSpaceTrue_DoesRenderLearningSpaceViewWithButton()
    {
        _worldPresenter.ShowingLearningSpaceView.Returns(true);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var spaceView = systemUnderTest.FindComponentOrFail<Stub<LearningSpaceView>>();
        var childContent = (RenderFragment)spaceView.Instance.Parameters[nameof(LearningSpaceView.ChildContent)];
        Assert.That(childContent, Is.Not.Null);
        var childContentRendered = _ctx.Render(childContent);
        childContentRendered.MarkupMatches(
            @"<button class=""btn btn-primary"">Close Learning Space View</button>");
    }

    [Test]
    public void Render_LearningWorldSet_RendersNameAndWorkload()
    {
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        learningWorld.Name.Returns("my insanely sophisticated name");
        learningWorld.Workload.Returns(42);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var h2 = systemUnderTest.FindOrFail("h2");
        h2.MarkupMatches(@"<h2>World: my insanely sophisticated name</h2>");
        var h5 = systemUnderTest.FindOrFail("h5");
        h5.MarkupMatches(@"<h5>Workload: 42 minutes</h5>");
    }
    
    

    private IRenderedComponent<LearningWorldView> GetLearningWorldViewForTesting(RenderFragment? childContent = null)
    {
        return _ctx.RenderComponent<LearningWorldView>(parameters => parameters
            .Add(p => p.ChildContent, childContent));
    }
}