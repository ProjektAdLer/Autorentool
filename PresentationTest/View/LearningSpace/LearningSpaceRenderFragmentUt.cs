using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.View.LearningSpace;
using Presentation.View.Toolbox;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningSpace;

[TestFixture]
public class LearningSpaceRenderFragmentUt
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
        var viewModel = Substitute.For<ILearningSpaceViewModel>();

        var systemUnderTest = GetLearningSpaceRenderFragmentForTesting(viewModel);
        
        Assert.That(systemUnderTest.Instance.ViewModel, Is.EqualTo(viewModel));
    }

    [Test]
    public void Render_ViewModelProvided_RendersLearningObjectRenderFragmentBaseCorrectly()
    {
        var viewModel = Substitute.For<ILearningSpaceViewModel>();
        viewModel.Name.Returns("my crazy name");
        viewModel.Description.Returns("my extremely descriptive description");
        
        var systemUnderTest = GetLearningSpaceRenderFragmentForTesting(viewModel);

        var stub = systemUnderTest.FindComponentOrFail<Stub<LearningObjectRenderFragmentBase>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(LearningObjectRenderFragmentBase.Obj)], Is.EqualTo(viewModel));
            Assert.That(stub.Instance.Parameters[nameof(LearningObjectRenderFragmentBase.CssClassSelector)],
                Is.EqualTo("learning-space"));
        });

        var childContent =
            _ctx.Render(
                (RenderFragment)stub.Instance.Parameters[nameof(LearningObjectRenderFragmentBase.ChildContent)]);
        childContent.MarkupMatches($"<p>{viewModel.Description}</p>");
    }

    private IRenderedComponent<LearningSpaceRenderFragment> GetLearningSpaceRenderFragmentForTesting(
        ILearningSpaceViewModel viewModel)
    {
        return _ctx.RenderComponent<LearningSpaceRenderFragment>(parameters => parameters
            .Add(p => p.ViewModel, viewModel));
    }
}