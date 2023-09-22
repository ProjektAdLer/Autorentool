using System;
using System.ComponentModel;
using System.Linq;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View.LearningWorld;
using Shared.Command;
using TestHelpers;

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
        SetWorldForTest();
    }
#pragma warning restore CS0108, CS0114
    private ILearningWorldPresenterOverviewInterface LearningWorldPresenter { get; set; } = null!;
    private LearningWorldViewModel LearningWorldVm { get; set; } = null!;

    [Test]
    public void WorldInPresenterNull_Render_RendersNothing()
    {
        LearningWorldPresenter.LearningWorldVm.Returns((LearningWorldViewModel?)null);

        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Markup, Is.Empty);
    }

    [Test]
    public void WorldInPresenterSet_Render_RendersTreeViewAndItemForEachSpace()
    {
        var systemUnderTest = GetRenderedComponent();
        
        var treeViews = systemUnderTest.FindComponents<MudTreeView<string>>();
        var rootTreeView = treeViews[0];
        var treeViewItems = rootTreeView.FindComponents<MudTreeViewItem<string>>();
        Assert.Multiple(() =>
        {
            //root view + one view per space
            Assert.That(treeViews, Has.Count.EqualTo(3));
            //2 spaces + 2 elements in spaces
            Assert.That(treeViewItems, Has.Count.EqualTo(4));
        });
    }

    [Test]
    public void Render_RegistersToWorldPresenter_OnPropertyChanged_OnPropertyChanging_OnCommandUndRedoOrExecute()
    {
        var systemUnderTest = GetRenderedComponent();
        
        LearningWorldPresenter.Received(1).PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
        LearningWorldPresenter.Received(1).PropertyChanging += Arg.Any<PropertyChangingEventHandler>();
        LearningWorldPresenter.Received(1).OnCommandUndoRedoOrExecute += Arg.Any<EventHandler<CommandUndoRedoOrExecuteArgs>>();
    }

    [Test]
    public void SpacesInPathways_Render_SpacesInCorrectTotalOrder()
    {
        //have space 1 follow space 2
        LearningWorldVm.LearningSpaces.ElementAt(1).OutBoundObjects.Add(LearningWorldVm.LearningSpaces.ElementAt(0));
        LearningWorldVm.LearningSpaces.ElementAt(0).InBoundObjects.Add(LearningWorldVm.LearningSpaces.ElementAt(1));
        //add a third space out of this hierarchy
        LearningWorldVm.LearningSpaces.Add(ViewModelProvider.GetLearningSpace());
        LearningWorldVm.LearningSpaces.ElementAt(0).Name = "Space 1";
        LearningWorldVm.LearningSpaces.ElementAt(1).Name = "Space 2";
        LearningWorldVm.LearningSpaces.ElementAt(2).Name = "Space 3";
        
        var systemUnderTest = GetRenderedComponent();
        
        var treeViewItems = systemUnderTest.FindComponents<MudTreeViewItem<string>>();
        Assert.Multiple(() =>
        {
            Assert.That(treeViewItems.ElementAt(0).Instance.Value, Is.EqualTo("Space 2"));
            Assert.That(treeViewItems.ElementAt(2).Instance.Value, Is.EqualTo("Space 1"));
            Assert.That(treeViewItems.ElementAt(4).Instance.Value, Is.EqualTo("Space 3"));
        });
    }

    [Test]
    public void ClickSpace_CallsWorldPresenter_SetSpace()
    {
        var systemUnderTest = GetRenderedComponent();
        
        var treeViewItems = systemUnderTest.FindComponents<MudTreeViewItem<string>>();
        
        var clickableDiv = treeViewItems[0].Find("div.mud-treeview-item-content");
        clickableDiv.Click();
        
        LearningWorldPresenter.Received(1).SetSelectedLearningSpace(LearningWorldVm.LearningSpaces.First());
    }

    [Test]
    public void ClickElement_CallsWorldPresenter_SetElement()
    {
        var systemUnderTest = GetRenderedComponent();
        
        var treeViewItems = systemUnderTest.FindComponents<MudTreeViewItem<string>>();
        
        var clickableDiv = treeViewItems[1].Find("div.mud-treeview-item-content");
        clickableDiv.Click();
        
        LearningWorldPresenter.Received(1).SetSelectedLearningElement(LearningWorldVm.LearningSpaces.First().LearningSpaceLayout.LearningElements.First().Value);
    }

    private void SetWorldForTest()
    {
        LearningWorldVm = ViewModelProvider.GetLearningWorld();
        var spaceVm1 = ViewModelProvider.GetLearningSpace();
        var spaceVm2 = ViewModelProvider.GetLearningSpace();
        var elementVm1 = ViewModelProvider.GetLearningElement();
        var elementVm2 = ViewModelProvider.GetLearningElement();
        elementVm1.LearningContent = ViewModelProvider.GetLinkContent();
        elementVm2.LearningContent = new FileContentViewModel("bar", "txt", "/bar.txt");
        spaceVm1.LearningSpaceLayout.LearningElements.Add(0, elementVm1);
        spaceVm2.LearningSpaceLayout.LearningElements.Add(0, elementVm2);
        LearningWorldVm.LearningSpaces.Add(spaceVm1);
        LearningWorldVm.LearningSpaces.Add(spaceVm2);
        LearningWorldPresenter.LearningWorldVm.Returns(LearningWorldVm);
    }

    private IRenderedComponent<LearningWorldTreeView> GetRenderedComponent()
    {
        return Context.RenderComponent<LearningWorldTreeView>();
    }
}