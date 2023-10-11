using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bunit;
using Bunit.TestDoubles;
using BusinessLogic.Commands;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.DropZone;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View;
using Presentation.View.Layout;
using Presentation.View.LearningSpace;
using PresentationTest;

namespace IntegrationTest.View;

[TestFixture]
public class MasterLayoutIt : MudBlazorTestFixture<MasterLayout>
{
    [SetUp]
    public void Setup()
    {
        Mediator = Substitute.For<IMediator>();
        DropZoneHelper = Substitute.For<ILearningElementDropZoneHelper>();
        UndoRedoEventSource = Substitute.For<IOnUndoRedo>();
        SelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        WorkspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        Context.Services.AddSingleton(Mediator);
        Context.Services.AddSingleton(DropZoneHelper);
        Context.Services.AddSingleton(UndoRedoEventSource);
        Context.Services.AddSingleton(SelectedViewModelsProvider);
        Context.Services.AddSingleton(WorkspacePresenter);

        Context.ComponentFactories.AddStub<HeaderBar>();
    }

    public IAuthoringToolWorkspacePresenter WorkspacePresenter { get; set; } = null!;

    public ISelectedViewModelsProvider SelectedViewModelsProvider { get; set; } = null!;

    public IOnUndoRedo UndoRedoEventSource { get; set; } = null!;

    public ILearningElementDropZoneHelper DropZoneHelper { get; set; } = null!;

    public IMediator Mediator { get; set; } = null!;

    [Test]
    public void Initialized_DependenciesInjectedCorrectly()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Mediator, Is.EqualTo(Mediator));
            Assert.That(systemUnderTest.Instance.ElementDropZoneHelper, Is.EqualTo(DropZoneHelper));
            Assert.That(systemUnderTest.Instance.OnUndoRedoEventSource, Is.EqualTo(UndoRedoEventSource));
            Assert.That(systemUnderTest.Instance.SelectedViewModelsProvider, Is.EqualTo(SelectedViewModelsProvider));
            Assert.That(systemUnderTest.Instance.AuthoringToolWorkspacePresenter, Is.EqualTo(WorkspacePresenter));
            Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(Localizer));
        });
    }

    [Test]
    public void OnInitialized_RegistersCorrectly()
    {
        var systemUnderTest = GetRenderedComponent();

        Mediator.Received().PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
        UndoRedoEventSource.OnRedo += Arg.Any<Action<ICommand>>();
        UndoRedoEventSource.OnUndo += Arg.Any<Action<ICommand>>();
        SelectedViewModelsProvider.PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
    }

    [Test]
    public void Dispose_UnregistersCorrectly()
    {
        var systemUnderTest = GetRenderedComponent();

        Mediator.Received().PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
        UndoRedoEventSource.OnRedo += Arg.Any<Action<ICommand>>();
        UndoRedoEventSource.OnUndo += Arg.Any<Action<ICommand>>();
        SelectedViewModelsProvider.PropertyChanged += Arg.Any<PropertyChangedEventHandler>();

        systemUnderTest.Instance.Dispose();

        Mediator.Received().PropertyChanged -= Arg.Any<PropertyChangedEventHandler>();
        UndoRedoEventSource.OnRedo -= Arg.Any<Action<ICommand>>();
        UndoRedoEventSource.OnUndo -= Arg.Any<Action<ICommand>>();
        SelectedViewModelsProvider.PropertyChanged -= Arg.Any<PropertyChangedEventHandler>();
    }

    [Test]
    public void Render_ContainsHeaderbar_AndSidebars_AndCorrectChildInCenterContainer()
    {
        Context.ComponentFactories.AddStub<SidebarItem>();
        Context.ComponentFactories.AddStub<LearningSpaceView>();
        SelectedViewModelsProvider.LearningObjectInPathWay.Returns(Substitute.For<ILearningSpaceViewModel>());

        var systemUnderTest = GetRenderedComponent();

        Assert.Multiple(() =>
        {
            IReadOnlyList<IRenderedComponent<Stub<SidebarItem>>> readOnlyList = null!;
            Assert.That(() => systemUnderTest.FindComponent<Stub<HeaderBar>>(), Throws.Nothing);
            Assert.That(() =>
            {
                readOnlyList = systemUnderTest.FindComponents<Stub<SidebarItem>>();
                return readOnlyList;
            }, Throws.Nothing);
            Assert.That(readOnlyList, Has.Count.EqualTo(6));
            Assert.That(() => systemUnderTest.FindComponent<Stub<LearningSpaceView>>(), Throws.Nothing);
        });

        SelectedViewModelsProvider.LearningObjectInPathWay.Returns((ISelectableObjectInWorldViewModel)null!);
        systemUnderTest.Render();
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.FindComponent<Stub<LearningSpaceView>>(), Throws.Exception);
            Assert.That(() => systemUnderTest.FindComponent<CenterContainer>().FindOrFail("img"), Throws.Nothing);
        });
    }

    [Test]
    public void OnAfterRenderInternal_CallsMediatorOnFirstRender_DoesNotCallIfNotFirstRender()
    {
        var systemUnderTest = GetRenderedComponent();
        //clear calls because lifetime method will have called OnAfterRenderInternal at least twice already
        Mediator.ClearReceivedCalls();

        systemUnderTest.Instance.OnAfterRenderInternal(true);

        Mediator.Received(1).RequestOpenWorldDialog();
        Mediator.Received(1).RequestOpenPathwayView();

        systemUnderTest.Instance.OnAfterRenderInternal(false);

        Mediator.Received(1).RequestOpenWorldDialog();
        Mediator.Received(1).RequestOpenPathwayView();
    }

    [Test]
    public void OnAfterRenderInternal_SelectedLearningWorldNull_NavigatesToLearningWorldsOverview()
    {
        SelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel)null!);
        var navManager = (TestExtensions.MockNavigationManager)Context.Services.GetService<NavigationManager>()!;
        var systemUnderTest = GetRenderedComponent();
        navManager.Reset();

        Assert.That(navManager.InvokedUris, Is.Empty);
        systemUnderTest.Instance.OnAfterRenderInternal(true);
        Assert.That(navManager.InvokedUris, Contains.Item("/MyLearningWorldsOverview"));
    }

    [Test]
    public void Render_PassesMediatorValuesToSidebarItemsCorrectly()
    {
        Context.ComponentFactories.AddStub<SidebarItem>();
        Context.ComponentFactories.AddStub<LearningSpaceView>();
        var systemUnderTest = GetRenderedComponent();

        var sidebarItems = systemUnderTest.FindComponents<Stub<SidebarItem>>();
        CheckAllSidebarsInactive();

        Mediator.WorldDialogOpen.Returns(true);
        CheckSidebarOpen(0);
        Mediator.WorldDialogOpen.Returns(false);
        CheckAllSidebarsInactive();

        Mediator.SpaceDialogOpen.Returns(true);
        CheckSidebarOpen(1);
        Mediator.SpaceDialogOpen.Returns(false);
        CheckAllSidebarsInactive();

        Mediator.ElementDialogOpen.Returns(true);
        CheckSidebarOpen(2);
        Mediator.ElementDialogOpen.Returns(false);
        CheckAllSidebarsInactive();

        Mediator.ContentDialogOpen.Returns(true);
        CheckSidebarOpen(3);
        Mediator.ContentDialogOpen.Returns(false);
        CheckAllSidebarsInactive();

        Mediator.WorldPathwayViewOpen.Returns(true);
        CheckSidebarOpen(4);
        Mediator.WorldPathwayViewOpen.Returns(false);
        CheckAllSidebarsInactive();

        Mediator.WorldTreeViewOpen.Returns(true);
        CheckSidebarOpen(5);
        Mediator.WorldTreeViewOpen.Returns(false);
        CheckAllSidebarsInactive();
        return;

        void CheckAllSidebarsInactive()
        {
            systemUnderTest.Render();
            Assert.That(sidebarItems.All(sidebar => !(bool)sidebar.Instance.Parameters["IsActive"]));
        }

        void CheckSidebarOpen(int sidebarIndex)
        {
            systemUnderTest.Render();
            Assert.That(sidebarItems[sidebarIndex].Instance.Parameters["IsActive"], Is.True);
        }
    }

    [Test]
    public void Render_PassesCorrectMediatorMethodsToSidebarItems()
    {
        Context.ComponentFactories.AddStub<SidebarItem>();
        Context.ComponentFactories.AddStub<LearningSpaceView>();

        var systemUnderTest = GetRenderedComponent();
        var sidebarItems = systemUnderTest.FindComponents<Stub<SidebarItem>>();

        Assert.That(sidebarItems[0].Instance.Parameters["RequestIsActiveToggle"],
            Is.EqualTo(EventCallback.Factory.Create<bool>(systemUnderTest.Instance,
                Mediator.RequestToggleWorldDialog)));
        Assert.That(sidebarItems[1].Instance.Parameters["RequestIsActiveToggle"],
            Is.EqualTo(EventCallback.Factory.Create<bool>(systemUnderTest.Instance,
                Mediator.RequestToggleSpaceDialog)));
        Assert.That(sidebarItems[2].Instance.Parameters["RequestIsActiveToggle"],
            Is.EqualTo(EventCallback.Factory.Create<bool>(systemUnderTest.Instance,
                Mediator.RequestToggleElementDialog)));
        Assert.That(sidebarItems[3].Instance.Parameters["RequestIsActiveToggle"],
            Is.EqualTo(EventCallback.Factory.Create<bool>(systemUnderTest.Instance,
                Mediator.RequestToggleContentDialog)));
        Assert.That(sidebarItems[4].Instance.Parameters["RequestIsActiveToggle"],
            Is.EqualTo(EventCallback.Factory.Create<bool>(systemUnderTest.Instance,
                Mediator.RequestToggleWorldPathwayView)));
        Assert.That(sidebarItems[5].Instance.Parameters["RequestIsActiveToggle"],
            Is.EqualTo(EventCallback.Factory.Create<bool>(systemUnderTest.Instance,
                Mediator.RequestToggleWorldTreeView)));
    }

    private IRenderedComponent<MasterLayout> GetRenderedComponent()
    {
        return Context.RenderComponent<MasterLayout>();
    }
}