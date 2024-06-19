using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningWorld;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningWorld;

[TestFixture]
public class LearningWorldTreeViewUt
{
    private TestContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _context.AddLocalizerForTest<LearningWorldTreeView>();
        LearningWorldPresenterOverview = Substitute.For<ILearningWorldPresenterOverviewInterface>();
        Mediator = Substitute.For<IMediator>();
        SelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _context.Services.AddSingleton(LearningWorldPresenterOverview);
        _context.Services.AddSingleton(Mediator);
        _context.Services.AddSingleton(SelectedViewModelsProvider);
        _context.ComponentFactories.AddStub<LearningWorldTreeViewItem>();
    }

    private ILearningWorldPresenterOverviewInterface LearningWorldPresenterOverview { get; set; }
    private IMediator Mediator { get; set; }
    private ISelectedViewModelsProvider SelectedViewModelsProvider { get; set; }
    private LearningWorldViewModel? World { get; set; }
    private LearningElementViewModel StoryEle1 { get; set; }
    private LearningElementViewModel StoryEle2 { get; set; }
    private LearningElementViewModel Ele1 { get; set; }

    [TearDown]
    public void Teardown()
    {
        _context.Dispose();
    }

    [Test]
    public void Render_WithSelectedSpace_ShowsThatSpaceOpened()
    {
        PreparePresenterAndSelectedVmProvider();

        var sut = GetRenderedComponent();

        var collapsables = sut.FindComponents<Collapsable>();
        Assert.That(collapsables, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(collapsables[0].Instance.InitiallyCollapsed, Is.False);
            Assert.That(collapsables[0].Find("span").ClassList.Contains("text-adlerblue-600"));
            Assert.That(collapsables[1].Instance.InitiallyCollapsed, Is.True);
            Assert.That(collapsables[1].Find("span").ClassList.Contains("text-adlergrey-600"));
        });
        var items = collapsables[0].FindComponents<Stub<LearningWorldTreeViewItem>>();
        Assert.That(items, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(items[0].Instance.Parameters["LearningElement"], Is.EqualTo(StoryEle1));
            Assert.That(items[0].Instance.Parameters["IsSelected"], Is.False);
            Assert.That(items[1].Instance.Parameters["LearningElement"], Is.EqualTo(Ele1));
            Assert.That(items[1].Instance.Parameters["IsSelected"], Is.True);
            Assert.That(items[2].Instance.Parameters["LearningElement"], Is.EqualTo(StoryEle2));
            Assert.That(items[2].Instance.Parameters["IsSelected"], Is.False);
        });
    }

    [Test]
    public void ClickSpace_SelectsSpaceAndOpensIt()
    {
        PreparePresenterAndSelectedVmProvider();

        var sut = GetRenderedComponent();

        var collapsables = sut.FindComponents<Collapsable>();

        Mediator.ClearReceivedCalls();
        SelectedViewModelsProvider.ClearReceivedCalls();

        collapsables[1].Find("span.text-adlergrey-600").Click();
        sut.Render();

        collapsables = sut.FindComponents<Collapsable>();

        Mediator.Received().RequestOpenSpaceDialog();
        SelectedViewModelsProvider.Received().SetLearningObjectInPathWay(World!.LearningSpaces.ElementAt(1), null);

        //simulate change event in selected view model provider
        SelectedViewModelsProvider.LearningObjectInPathWay.Returns(World.LearningSpaces.ElementAt(1));
        SelectedViewModelsProvider.PropertyChanged +=
            Raise.Event<PropertyChangedEventHandler>(this,
                new PropertyChangedEventArgs(nameof(ISelectedViewModelsProvider.LearningObjectInPathWay)));

        Assert.Multiple(() =>
        {
            Assert.That(collapsables[1].Find("span").ClassList.Contains("text-adlerblue-600"));
            Assert.That(collapsables[0].Find("span").ClassList.Contains("text-adlergrey-600"));
        });
    }


    [Test]
    public async Task ClickElement_SelectsElementAndChangesListItemParameter()
    {
        PreparePresenterAndSelectedVmProvider();

        var sut = GetRenderedComponent();

        var collapsables = sut.FindComponents<Collapsable>();

        Mediator.ClearReceivedCalls();
        SelectedViewModelsProvider.ClearReceivedCalls();

        var items = collapsables[0].FindComponents<Stub<LearningWorldTreeViewItem>>();
        await sut.InvokeAsync(async () =>
            await ((EventCallback<ILearningElementViewModel>)items[0].Instance.Parameters["OnSelect"]).InvokeAsync(Ele1));

        LearningWorldPresenterOverview.Received().SetSelectedLearningElement(Ele1);
        SelectedViewModelsProvider.LearningElement.Returns(Ele1);
        SelectedViewModelsProvider.PropertyChanged +=
            Raise.Event<PropertyChangedEventHandler>(this,
                new PropertyChangedEventArgs(nameof(ISelectedViewModelsProvider.LearningElement)));

        items = collapsables[0].FindComponents<Stub<LearningWorldTreeViewItem>>();

        Assert.Multiple(() =>
        {
            Assert.That(items[0].Instance.Parameters["IsSelected"], Is.False);
            Assert.That(items[1].Instance.Parameters["IsSelected"], Is.True);
            Assert.That(items[2].Instance.Parameters["IsSelected"], Is.False);
        });
    }

    private void PreparePresenterAndSelectedVmProvider()
    {
        World = ViewModelProvider.GetLearningWorld();
        World.LearningSpaces.Clear();
        var vm1 = ViewModelProvider.GetLearningSpace();
        vm1.LearningSpaceLayout.LearningElements.Clear();
        vm1.LearningSpaceLayout.StoryElements.Clear();
        StoryEle1 = ViewModelProvider.GetLearningElement(append: "entrance",
            content: ViewModelProvider.GetStoryContent());
        StoryEle2 = ViewModelProvider.GetLearningElement(append: "exit", content: ViewModelProvider.GetStoryContent());
        Ele1 = ViewModelProvider.GetAdaptivityElement(append: "ele1");
        vm1.LearningSpaceLayout.StoryElements[0] = StoryEle1;
        vm1.LearningSpaceLayout.StoryElements[1] = StoryEle2;
        vm1.LearningSpaceLayout.LearningElements[0] = Ele1;
        var vm2 = ViewModelProvider.GetLearningSpace();
        World.LearningSpaces.Add(vm1);
        World.LearningSpaces.Add(vm2);
        World.LearningPathWays.Add(new LearningPathwayViewModel(vm1, vm2));
        SelectedViewModelsProvider.LearningObjectInPathWay.Returns(vm1);
        SelectedViewModelsProvider.LearningElement.Returns(Ele1);
        LearningWorldPresenterOverview.LearningWorldVm.Returns(World);
    }


    private IRenderedComponent<LearningWorldTreeView> GetRenderedComponent()
    {
        return _context.RenderComponent<LearningWorldTreeView>();
    }
}