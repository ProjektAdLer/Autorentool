using AuthoringToolLib.PresentationLogic.LearningWorld;
using AuthoringToolLib.View.LearningWorld;
using AuthoringToolLib.View.Toolbox;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolLibTest.View.LearningWorld;

[TestFixture]
public class LearningWorldRenderFragmentUt
{
#pragma warning disable CS8618 - n.stich
    private TestContext _ctx;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<LearningObjectRenderFragmentBase>();
    }

    [Test]
    public void Render_SetsParametersCorrectly()
    {
        var viewModel = Substitute.For<ILearningWorldViewModel>();
        
        var systemUnderTest = GetLearningWorldRenderFragmentForTesting(viewModel);
        
        Assert.That(systemUnderTest.Instance.ViewModel, Is.EqualTo(viewModel));
    }

    [Test]
    public void Render_ViewModelProvided_RendersLearningObjectRenderFragmentBaseCorrectly()
    {
        var viewModel = Substitute.For<ILearningWorldViewModel>();
        
        var systemUnderTest = GetLearningWorldRenderFragmentForTesting(viewModel);

        var stub = systemUnderTest.FindComponentOrFail<Stub<LearningObjectRenderFragmentBase>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(LearningObjectRenderFragmentBase.Obj)], Is.EqualTo(viewModel));
            Assert.That(stub.Instance.Parameters[nameof(LearningObjectRenderFragmentBase.CssClassSelector)],
                Is.EqualTo("learning-world"));
        });
        var childContent =
            _ctx.Render(
                (RenderFragment)stub.Instance.Parameters[nameof(LearningObjectRenderFragmentBase.ChildContent)]);
        childContent.MarkupMatches(@"<i class=""bi bi-globe""></i>");
    }

    private IRenderedComponent<LearningWorldRenderFragment> GetLearningWorldRenderFragmentForTesting(ILearningWorldViewModel viewModel)
    {
        return _ctx.RenderComponent<LearningWorldRenderFragment>(parameters => parameters
            .Add(p => p.ViewModel, viewModel));
    }
}