using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.Space;
using Presentation.View.Space;
using Presentation.View.Toolbox;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.Space;

[TestFixture]
public class SpaceRenderFragmentUt
{
#pragma warning disable CS8618 //set in setup - n.stich
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
        var viewModel = Substitute.For<ISpaceViewModel>();

        var systemUnderTest = GetSpaceRenderFragmentForTesting(viewModel);
        
        Assert.That(systemUnderTest.Instance.ViewModel, Is.EqualTo(viewModel));
    }

    [Test]
    public void Render_ViewModelProvided_RendersObjectRenderFragmentBaseCorrectly()
    {
        var viewModel = Substitute.For<ISpaceViewModel>();
        viewModel.Name.Returns("my crazy name");
        viewModel.Description.Returns("my extremely descriptive description");
        
        var systemUnderTest = GetSpaceRenderFragmentForTesting(viewModel);

        var stub = systemUnderTest.FindComponentOrFail<Stub<DisplayableObjectRenderFragmentBase>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(DisplayableObjectRenderFragmentBase.Obj)], Is.EqualTo(viewModel));
            Assert.That(stub.Instance.Parameters[nameof(DisplayableObjectRenderFragmentBase.CssClassSelector)],
                Is.EqualTo("space"));
        });

        var childContent =
            _ctx.Render(
                (RenderFragment)stub.Instance.Parameters[nameof(DisplayableObjectRenderFragmentBase.ChildContent)]);
        childContent.MarkupMatches($"<p>{viewModel.Description}</p>");
    }

    private IRenderedComponent<SpaceRenderFragment> GetSpaceRenderFragmentForTesting(
        ISpaceViewModel viewModel)
    {
        return _ctx.RenderComponent<SpaceRenderFragment>(parameters => parameters
            .Add(p => p.ViewModel, viewModel));
    }
}