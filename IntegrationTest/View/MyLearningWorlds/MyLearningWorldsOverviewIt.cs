using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.MyLearningWorlds;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View;
using Presentation.View.MyLearningWorlds;
using TestHelpers;

namespace IntegrationTest.View.MyLearningWorlds;

[TestFixture]
public class MyLearningWorldsOverviewIt : MudBlazorTestFixture<MyLearningWorldsOverview>
{
    [SetUp]
    public void SetUp()
    {
        MyLearningWorldsProvider = Substitute.For<IMyLearningWorldsProvider>();
        PresentationLogic = Substitute.For<IPresentationLogic>();
        WorkspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        SelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        Context.Services.AddSingleton(MyLearningWorldsProvider);
        Context.Services.AddSingleton(PresentationLogic);
        Context.Services.AddSingleton(WorkspaceVm);
        Context.Services.AddSingleton(SelectedViewModelsProvider);
        Context.ComponentFactories.AddStub<HeaderBar>();
        Context.ComponentFactories.AddStub<CreateWorldForm>();
        Context.ComponentFactories.AddStub<LearningWorldCard>();
    }

    private IMyLearningWorldsProvider MyLearningWorldsProvider { get; set; }
    private IPresentationLogic PresentationLogic { get; set; }
    private IAuthoringToolWorkspaceViewModel WorkspaceVm { get; set; }
    private ISelectedViewModelsProvider SelectedViewModelsProvider { get; set; }

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
    // ANF-ID: [ASE5]
    public void Render_DisplaysLoadedAndSavedWorlds()
    {
        IList<ILearningWorldViewModel> worlds = new List<ILearningWorldViewModel>
        {
            ViewModelProvider.GetLearningWorld(),
            ViewModelProvider.GetLearningWorld(),
        };
        WorkspaceVm.LearningWorlds.Returns(worlds);
        var systemUnderTest = GetRenderedComponent();
        MyLearningWorldsProvider.Received(systemUnderTest.RenderCount).ReloadLearningWorldsInWorkspace();

        var learningWorldCards = systemUnderTest.FindComponents<Stub<LearningWorldCard>>();
        Assert.That(learningWorldCards, Has.Count.EqualTo(worlds.Count));
    }


    [Test]
    // ANF-ID: [ASE2]
    public async Task OpenWorldButtonOnCard_Clicked_CallsSelectedViewModelsProvider()
    {
        IList<ILearningWorldViewModel> worlds = new List<ILearningWorldViewModel>
        {
            ViewModelProvider.GetLearningWorld(),
            ViewModelProvider.GetLearningWorld(),
        };
        WorkspaceVm.LearningWorlds.Returns(worlds);
        var systemUnderTest = GetRenderedComponent();

        var learningWorldCard = systemUnderTest.FindComponent<Stub<LearningWorldCard>>().Instance;
        var callback = (EventCallback<ILearningWorldViewModel>)learningWorldCard.Parameters["OnOpenLearningWorld"];
        await systemUnderTest.InvokeAsync(() => callback.InvokeAsync(worlds[0]));

        SelectedViewModelsProvider.Received().SetLearningWorld(worlds[0], null);
    }

    [Test]
    // ANF-ID: [ASE1]
    public void CreateWorldButton_Clicked_OpensCreateWorldFormDrawer()
    {
        var systemUnderTest = GetRenderedComponent();

        var drawer = systemUnderTest.FindComponent<MudDrawer>();
        Assert.That(drawer.Instance.Open, Is.False);

        systemUnderTest.Find("div.create-world-button").Click();

        Assert.That(drawer.Instance.Open, Is.True);
    }

    [Test]
    // ANF-ID: [ASN0002]
    public void ImportButton_Clicked_CallsWorldsProvider()
    {
        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.Find("div.import-world-button").Click();

        PresentationLogic.Received().ImportLearningWorldFromArchiveAsync();
    }

    private IRenderedComponent<MyLearningWorldsOverview> GetRenderedComponent()
    {
        return Context.RenderComponent<MyLearningWorldsOverview>();
    }
}