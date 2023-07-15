using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.MyLearningWorlds;
using Presentation.View;
using Presentation.View.MyLearningWorlds;
using Shared;

namespace IntegrationTest.View.MyLearningWorlds;

[TestFixture]
public class MyLearningWorldsOverviewIt : MudBlazorTestFixture<MyLearningWorldsOverview>
{
    [SetUp]
    public void SetUp()
    {
        MyLearningWorldsProvider = Substitute.For<IMyLearningWorldsProvider>();
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(MyLearningWorldsProvider);
        Context.Services.AddSingleton(PresentationLogic);
        Context.ComponentFactories.AddStub<HeaderBar>();
        Context.ComponentFactories.AddStub<CreateWorldForm>();
        Context.ComponentFactories.AddStub<LearningWorldCard>();
    }

    private IMyLearningWorldsProvider MyLearningWorldsProvider { get; set; }
    private IPresentationLogic PresentationLogic { get; set; }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.MyLearningWorldsProvider, Is.EqualTo(MyLearningWorldsProvider));
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
        Assert.That(systemUnderTest.Instance.NavManager, Is.Not.Null);
    }

    [Test]
    public void Render_DisplaysLoadedAndSavedWorlds()
    {
        var loadedLearningWorldPaths = new[] { new SavedLearningWorldPath() };
        var savedLearningWorldPaths = new[] { new SavedLearningWorldPath() };
        MyLearningWorldsProvider.GetLoadedLearningWorlds().Returns(loadedLearningWorldPaths);
        MyLearningWorldsProvider.GetSavedLearningWorlds().Returns(savedLearningWorldPaths);

        var systemUnderTest = GetRenderedComponent();

        var learningWorldCards = systemUnderTest.FindComponents<Stub<LearningWorldCard>>();
        Assert.That(learningWorldCards.Count,
            Is.EqualTo(loadedLearningWorldPaths.Length + savedLearningWorldPaths.Length));
        Assert.That(learningWorldCards[0].Instance.Parameters["LearningWorldPath"],
            Is.EqualTo(loadedLearningWorldPaths[0]));
        Assert.That(learningWorldCards[1].Instance.Parameters["LearningWorldPath"],
            Is.EqualTo(savedLearningWorldPaths[0]));
    }

    [Test]
    public void CreateWorldButton_Clicked_OpensCreateWorldFormDrawer()
    {
        var systemUnderTest = GetRenderedComponent();

        var drawer = systemUnderTest.FindComponent<MudDrawer>();
        Assert.That(drawer.Instance.Open, Is.False);

        systemUnderTest.Find("div.create-world-button").Click();

        Assert.That(drawer.Instance.Open, Is.True);
    }

    [Test]
    public void ImportButton_Clicked_CallsWorldsProvider()
    {
        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.Find("div.import-world-button").Click();

        MyLearningWorldsProvider.Received().LoadSavedLearningWorld();
    }

    private IRenderedComponent<MyLearningWorldsOverview> GetRenderedComponent()
    {
        return Context.RenderComponent<MyLearningWorldsOverview>();
    }
}