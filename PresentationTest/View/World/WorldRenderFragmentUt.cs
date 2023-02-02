using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.World;
using Presentation.View.World;
using Presentation.View.Toolbox;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.World;

[TestFixture]
public class WorldRenderFragmentUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<DisplayableObjectRenderFragmentBase>();
    }

    [Test]
    public void Render_SetsParametersCorrectly()
    {
        var viewModel = Substitute.For<IWorldViewModel>();
        
        var systemUnderTest = GetWorldRenderFragmentForTesting(viewModel);
        
        Assert.That(systemUnderTest.Instance.ViewModel, Is.EqualTo(viewModel));
    }

    [Test]
    public void Render_ViewModelProvided_RendersDisplayableObjectRenderFragmentBaseCorrectly()
    {
        var viewModel = Substitute.For<IWorldViewModel>();
        
        var systemUnderTest = GetWorldRenderFragmentForTesting(viewModel);

        var stub = systemUnderTest.FindComponentOrFail<Stub<DisplayableObjectRenderFragmentBase>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(DisplayableObjectRenderFragmentBase.Obj)], Is.EqualTo(viewModel));
            Assert.That(stub.Instance.Parameters[nameof(DisplayableObjectRenderFragmentBase.CssClassSelector)],
                Is.EqualTo("world"));
        });
        var childContent =
            _ctx.Render(
                (RenderFragment)stub.Instance.Parameters[nameof(DisplayableObjectRenderFragmentBase.ChildContent)]);
        childContent.MarkupMatches(@"<i class=""bi bi-globe""></i>");
    }

    private IRenderedComponent<WorldRenderFragment> GetWorldRenderFragmentForTesting(IWorldViewModel viewModel)
    {
        return _ctx.RenderComponent<WorldRenderFragment>(parameters => parameters
            .Add(p => p.ViewModel, viewModel));
    }
}