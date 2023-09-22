using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View.LearningWorld;

namespace IntegrationTest.View.LearningWorld;

[TestFixture]
public class LearningWorldTreeViewIt : MudBlazorTestFixture<LearningWorldTreeView>
{
#pragma warning disable CS0108, CS0114
    [SetUp]
    public void SetUp()
    {
        LearningWorldPresenter = Substitute.For<ILearningWorldPresenterOverviewInterface>();
        Context.Services.AddSingleton(LearningWorldPresenter);
    }
#pragma warning restore CS0108, CS0114
    private ILearningWorldPresenterOverviewInterface LearningWorldPresenter { get; set; } = null!;

    [Test]
    public void WorldInPresenterNull_RendersNothing()
    {
        LearningWorldPresenter.LearningWorldVm.Returns((LearningWorldViewModel?)null);

        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Markup, Is.Empty);
    }

    private IRenderedComponent<LearningWorldTreeView> GetRenderedComponent()
    {
        return Context.RenderComponent<LearningWorldTreeView>();
    }
}