using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using TestHelpers;

namespace PresentationTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;

    [Test]
    public void EditLearningWorld_ThrowsExceptionIfLearningWorldIsNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningWorld("n", "s", "a", "l", "d", "g"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void EditLearningWorld_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.EditLearningWorld("n", "s", "a", "l", "d", "g");
        presentationLogic.Received().EditLearningWorld(world, "n", "s", "a", "l", "d", "g");
    }

    [Test]
    public void SetSelectedLearningObject_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var space = ViewModelProvider.GetLearningSpace();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningObject(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void SetSelectedLearningObject_CallsSpacePresenter()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        systemUnderTest.SetSelectedLearningObject(space);

        learningSpacePresenter.Received().SetLearningSpace(space);
    }

    [Test]
    public void DragObjectInPathWay_CallsPresentationLogic()
    {
        var space = ViewModelProvider.GetLearningSpace();
        double oldPositionX = 5;
        double oldPositionY = 7;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.DragObjectInPathWay(space,
            new DraggedEventArgs<IObjectInPathWayViewModel>(space, oldPositionX, oldPositionY));

        presentationLogic.Received().DragObjectInPathWay(space, oldPositionX, oldPositionY);
    }

    [Test]
    public void SetSelectedLearningSpace_SetsSelectedLearningSpaceAndSetsEditLearningSpaceDialogOpenToTrue()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningSpace(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(space, null);
        mediator.Received().RequestOpenSpaceDialog();
    }

    [Test]
    public void EditSelectedLearningSpace_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedLearningSpace());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void EditSelectedLearningSpace_SelectedObjectInPathWayIsNotLearningSpace_ThrowsException()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(condition);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedLearningSpace());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningObjectInPathWay is not LearningSpaceViewModel"));
    }

    [Test]
    public void EditSelectedLearningSpace_CallsMediator()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        systemUnderTest.EditSelectedLearningSpace();
        mediator.Received().RequestOpenSpaceDialog();
    }

    [Test]
    public void DeleteLearningSpace_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DeleteLearningSpace(space);

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void DeleteLearningSpace_SelectedLearningWorldIsNull_ThrowsException()
    {
        var space = ViewModelProvider.GetLearningSpace();

        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningSpace(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void RightClickedLearningSpace_SetsRightClickedLearningObjectToSpace()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(space);

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(space));
    }

    [Test]
    public void HideRightClickMenu_SetsRightClickedLearningObjectToNull()
    {
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting();

        systemUnderTest.RightClickOnObjectInPathWay(space);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(space));

        systemUnderTest.HideRightClickMenu();

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void ClickedLearningSpace_SetsSelectedLearningObjectToSpace()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningWorld.Returns(world);
        systemUnderTest.ClickOnObjectInWorld(space);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(space, null);
    }

    [Test]
    public void DoubleClickedLearningSpace_SetsSelectedLearningObjectAndShowsLearningSpaceView()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DoubleClickOnObjectInPathway(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        Assert.That(selectedViewModelsProvider.LearningObjectInPathWay, Is.EqualTo(space));
        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.True);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void AddNewLearningSpace_ThrowsWhenWorldIsNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddNewLearningSpace());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void AddNewLearningSpace_CallsSelectedViewModelsProviderAndMediator()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest =
            CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider, mediator: mediator);
        systemUnderTest.LearningWorldVm = world;


        systemUnderTest.AddNewLearningSpace();

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(null, null);
        mediator.Received().RequestOpenSpaceDialog();
    }

    [Test]
    public void CreateNewLearningSpace_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm?.LearningSpaces.Add(space);

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);

        presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<int>(), Arg.Any<Theme>(), Arg.Any<double>(),
            Arg.Any<double>(), Arg.Any<TopicViewModel>());
    }

    [Test]
    public void CreateNewLearningSpace_SelectedLearningWorldIsNull_CallsErrorService()
    {
        var errorService = Substitute.For<IErrorService>();
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(errorService: errorService,
            selectedViewModelsProvider: mockSelectedViewModelsProvider);

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", 5, Theme.Campus);

        errorService.Received().SetError("Error", "Error while creating learning space; No Learning World selected");
    }

    [Test]
    public void DeleteSelectedLearningObject_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteSelectedLearningObject_DoesNotThrowWhenSelectedSpaceNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningWorld.Returns(world);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns((LearningSpaceViewModel)null!);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(selectedViewModelsProvider.LearningObjectInPathWay, Is.Null);
            Assert.That(systemUnderTest.LearningWorldVm.LearningSpaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesSpaceFromViewModel_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.SetLearningObjectInPathWay(space, null);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesConditionFromViewModel_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.PathWayConditions.Add(condition);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(condition);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesPathWayFromViewModel_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space = ViewModelProvider.GetLearningSpace();
        var pathWay = ViewModelProvider.GetLearningPathway(source: space, target: condition);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningPathWays.Add(pathWay);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(pathWay);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeleteLearningPathWay(world, pathWay);
    }

    [Test]
    public void SaveSelectedLearningSpaceAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void SaveSelectedLearningSpaceAsync_DoesNotThrowWhenSelectedSpaceNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns((LearningSpaceViewModel)null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public async Task SaveSelectedLearningSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();

        await presentationLogic.Received().SaveLearningSpaceAsync(space);
    }

    [Test]
    public async Task SaveSelectedLearningSpace_SerializationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        presentationLogic.When(x => x.SaveLearningSpaceAsync(space))
            .Do(_ => throw new SerializationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, errorService: errorService,
                selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();
        errorService.Received().SetError("Error while saving learning space", Arg.Any<string>());
    }

    [Test]
    public async Task SaveSelectedLearningSpace_InvalidOperationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        world.LearningSpaces.Add(space);
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);
        presentationLogic.When(x => x.SaveLearningSpaceAsync(space))
            .Do(_ => throw new InvalidOperationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, errorService: errorService,
                selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();
        errorService.Received().SetError("Error while saving learning space", Arg.Any<string>());
    }

    [Test]
    public void LoadLearningSpace_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public async Task LoadLearningSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningSpaceAsync();

        await presentationLogic.Received().LoadLearningSpaceAsync(world);
    }

    [Test]
    public async Task LoadLearningSpace_SerializationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        presentationLogic.LoadLearningSpaceAsync(world).Throws(new SerializationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningSpaceAsync();
        errorService.Received().SetError("Error while loading learning space", Arg.Any<string>());
    }

    [Test]
    public async Task LoadLearningSpace_InvalidOperationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        presentationLogic.LoadLearningSpaceAsync(world).Throws(new InvalidOperationException());

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic, errorService: errorService);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningSpaceAsync();
        errorService.Received().SetError("Error while loading learning space", Arg.Any<string>());
    }

    [Test]
    public void ShowAndCloseLearningSpaceView_OpensAndClosesLearningSpaceView_SetsShowingLearningSpaceViewBool()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.False);

        systemUnderTest.ShowSelectedLearningSpaceView();

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.True);

        systemUnderTest.CloseLearningSpaceView();

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.False);
    }

    [Test]
    public void SetSelectedLearningObject_SetsPathWayToSelectedObject()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space = ViewModelProvider.GetLearningSpace();
        var pathWay = ViewModelProvider.GetLearningPathway(source: space, target: condition);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(pathWay);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(pathWay, null);
    }

    [Test]
    public void DeleteLearningObject_ThrowsWhenWorldIsNull()
    {
        var condition = ViewModelProvider.GetPathWayCondition();

        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningObject(condition));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteLearningObject_CallsPresentationLogic_WithPathWayCondition()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(condition);

        systemUnderTest.DeleteLearningObject(condition);

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeleteLearningObject_CallsPresentationLogic_WithLearningSpace()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(space);

        systemUnderTest.DeleteLearningObject(space);

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void SetSelectedLearningObject_SetsConditionToSelectedObject()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(condition);

        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(condition, null);
    }

    [Test]
    public void CreatePathWayCondition_ThrowsWhenWorldIsNull()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreatePathWayCondition());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void CreatePathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.CreatePathWayCondition();

        presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.Or, 0, 0);
    }

    [Test]
    public void CreatePathWayCondition_ApplicationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, errorService: errorService,
                selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.LearningWorldVm = world;
        presentationLogic.When(x => x.CreatePathWayCondition(world, ConditionEnum.Or, 0, 0))
            .Do(_ => throw new ApplicationException("Test"));

        systemUnderTest.CreatePathWayCondition();
        errorService.Received().SetError("Error while creating PathWayCondition", "Test");
    }

    [Test]
    public void DeletePathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DeletePathWayCondition(condition);

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeletePathWayCondition_ThrowsWhenWorldIsNull()
    {
        var condition = ViewModelProvider.GetPathWayCondition();

        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeletePathWayCondition(condition));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void RightClickedPathWayCondition_SetsRightClickedLearningObjectToSpace()
    {
        var condition = ViewModelProvider.GetPathWayCondition();
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(condition);

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(condition));
    }

    [Test]
    public void HideRightClickMenuFromCondition_SetsRightClickedLearningObjectToNull()
    {
        var conditionViewModel = ViewModelProvider.GetPathWayCondition();
        var systemUnderTest = CreatePresenterForTesting();

        systemUnderTest.RightClickOnObjectInPathWay(conditionViewModel);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(conditionViewModel));

        systemUnderTest.HideRightClickMenu();

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void ClickedPathWayCondition_SetsSelectedLearningObjectToCondition()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var conditionViewModel = ViewModelProvider.GetPathWayCondition();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.ClickOnObjectInWorld(conditionViewModel);
        selectedViewModelsProvider.Received().SetLearningObjectInPathWay(conditionViewModel, null);
    }

    [Test]
    public void DoubleClickedPathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DoubleClickOnObjectInPathway(condition);

        presentationLogic.Received().EditPathWayCondition(condition, ConditionEnum.Or);
    }

    [Test]
    public void SetOnHoveredPathWayObject_ObjectAtPositionIsNull_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        var targetSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay = targetSpace;

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace));

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 400, 400);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceSpace_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 30, 30);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceCondition_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceCondition = ViewModelProvider.GetPathWayCondition();
        world.PathWayConditions.Add(sourceCondition);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceCondition, 30, 30);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_SetsCorrectObjectAtPosition()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace(positionX: 25, positionY: 25);
        var targetSpace1 = ViewModelProvider.GetLearningSpace(positionX: 250, positionY: 250);
        var targetSpace2 = ViewModelProvider.GetLearningSpace(positionX: 500, positionY: 500);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace1);
        world.LearningSpaces.Add(targetSpace2);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay = targetSpace1;

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace1));

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 510, 510);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace2));
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var space = ViewModelProvider.GetLearningSpace();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetOnHoveredObjectInPathWay(space, 3, 3));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void CreateLearningPathWay_WithoutTargetSpace_DoesNotCallPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        var targetSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 25, 25);

        presentationLogic.DidNotReceive().CreateLearningPathWay(world, sourceSpace, targetSpace);
    }

    [Test]
    public void CreateLearningPathWay_TargetSpaceIsSourceSpace_DoesNotCallPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace();
        var targetSpace = ViewModelProvider.GetLearningSpace();
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 60, 60);

        presentationLogic.DidNotReceive().CreateLearningPathWay(world, sourceSpace, sourceSpace);
    }

    [Test]
    public void CreateLearningPathWay_WithTargetSpace_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var sourceSpace = ViewModelProvider.GetLearningSpace(positionX: 25, positionY: 25);
        var targetSpace = ViewModelProvider.GetLearningSpace(positionX: 250, positionY: 250);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 260, 260);

        presentationLogic.Received().CreateLearningPathWay(world, sourceSpace, targetSpace);
    }

    [Test]
    public void CreateLearningPathWay_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var space = ViewModelProvider.GetLearningSpace();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateLearningPathWay(space, 3, 3));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteLearningPathWay_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var condition = ViewModelProvider.GetPathWayCondition();
        var space = ViewModelProvider.GetLearningSpace();
        var learningPathWay = ViewModelProvider.GetLearningPathway(source: condition, target: space);
        world.LearningPathWays.Add(learningPathWay);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteLearningPathWay(space);

        presentationLogic.Received().DeleteLearningPathWay(world, learningPathWay);
    }

    [Test]
    public void DeleteLearningPathWay_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var space = ViewModelProvider.GetLearningSpace();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningPathWay(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteLearningPathWay_PathWayIsNull_ThrowsException()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningPathWay(null!));
        Assert.That(ex!.Message, Is.EqualTo("LearningPathWay is null"));
    }

    [Test]
    public void SetSelectedLearningElement_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void SetSelectedLearningElement_SetsSelectedLearningElement()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: selectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningElement(element);
        selectedViewModelsProvider.Received(1).SetLearningElement(element, null);
        selectedViewModelsProvider.LearningElement.Returns(element);

        Assert.That(selectedViewModelsProvider.LearningElement, Is.EqualTo(element));
    }

    [Test]
    public void EditLearningElement_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
                ElementModel.l_h5p_slotmachine_1, 0, 0, null!));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void EditLearningElement_UnplacedHasASpaceParent_ThrowsException()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        var systemUnderTest = CreatePresenterForTesting();
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content, parent: space);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
                ElementModel.l_h5p_slotmachine_1, 0, 0, null!));

        Assert.That(ex!.Message, Is.EqualTo("LearningElement is unplaced but has a space parent"));
    }

    [Test]
    public void EditLearningElement_UnplacedDoesNotContainElement_ThrowsException()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var systemUnderTest = CreatePresenterForTesting();
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
                ElementModel.l_h5p_slotmachine_1, 0, 0, null!));

        Assert.That(ex!.Message, Is.EqualTo("LearningElement is placed but has a different or null parent"));
    }

    [Test]
    public void EditLearningElement_ElementParentIsNotNull_CallsSpacePresenter()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);
        var space = ViewModelProvider.GetLearningSpace();
        element.Parent = space;
        selectedViewModelsProvider.LearningObjectInPathWay.Returns(space);

        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.EditLearningElement(space, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0, null!);

        learningSpacePresenter.Received(1).EditLearningElement(element, "a", "b", "c",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 0, 0, null!);
    }

    [Test]
    public void EditLearningElement_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        systemUnderTest.EditLearningElement(null, element, "a", "b", "c", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0, content);
        presentationLogic.Received().EditLearningElement(null, element, "a", "b", "c",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 0, 0, content);
    }

    [Test]
    public void ShowSelectedElementContentAsync_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.ShowSelectedElementContentAsync(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        await presentationLogic.Received().ShowLearningElementContentAsync(element);
        selectedViewModelsProvider.Received().SetLearningElement(element, null);
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_ArgumentOutOfRangeException_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        presentationLogic.When(x => x.ShowLearningElementContentAsync(element))
            .Do(_ => throw new ArgumentOutOfRangeException());

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_IOException_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        presentationLogic.When(x => x.ShowLearningElementContentAsync(element))
            .Do(_ => throw new IOException());

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public async Task ShowSelectedElementContentAsync_InvalidOperationException_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            errorService: errorService,
            selectedViewModelsProvider: selectedViewModelsProvider);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        presentationLogic.When(x => x.ShowLearningElementContentAsync(element))
            .Do(_ => throw new InvalidOperationException());

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        await systemUnderTest.ShowSelectedElementContentAsync(element);
        errorService.Received().SetError("Error while showing learning element content", Arg.Any<string>());
    }

    [Test]
    public void DeleteLearningElement_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteLearningElement_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        var content = ViewModelProvider.GetLinkContent();
        var element = ViewModelProvider.GetLearningElement(content: content);

        systemUnderTest.LearningWorldVm = world;
        world.UnplacedLearningElements.Add(element);
        systemUnderTest.DeleteLearningElement(element);
        presentationLogic.Received().DeleteLearningElementInWorld(world, element);
    }

    [Test]
    public void GetAllContent_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        systemUnderTest.GetAllContent();
        presentationLogic.Received().GetAllContent();
    }

    [Test]
    public void CreateUnplacedLearningElement_SelectedLearningWorldIsNull_ThrowsException()
    {
        var mockSelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = CreatePresenterForTesting(selectedViewModelsProvider: mockSelectedViewModelsProvider);
        var element = ViewModelProvider.GetLearningElement();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.CreateUnplacedLearningElement("a", null!, "c", "d", LearningElementDifficultyEnum.Easy,
                ElementModel.l_h5p_slotmachine_1, 0, 0));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void CreateUnplacedLearningElement_CallsPresentationLogic()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        var content = ViewModelProvider.GetLinkContent();

        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateUnplacedLearningElement("abc", content, "a", "b", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_slotmachine_1, 0, 0);
        presentationLogic.Received().CreateUnplacedLearningElement(world, "abc", content, "a", "b",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 0, 0);
    }

    private LearningWorldPresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null,
        ILogger<LearningWorldPresenter>? logger = null, IMediator? mediator = null,
        ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        IErrorService? errorService = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        mediator ??= Substitute.For<IMediator>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        errorService ??= Substitute.For<IErrorService>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, logger,
            mediator, selectedViewModelsProvider, errorService);
    }
}