using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;

    #region LearningWorld

    [Test]
    public void EditLearningWorld_ThrowsExceptionIfLearningWorldIsNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.EditLearningWorld("n", "s", "a", "l", "d", "g"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void EditLearningWorld_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.EditLearningWorld("n", "s", "a", "l", "d", "g");
        presentationLogic.Received().EditLearningWorld(world, "n", "s", "a", "l", "d", "g");
    }

    #endregion

    #region LearningSpace

    [Test]
    public void SetSelectedLearningObject_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningObject(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void SetSelectedLearningObject_CallsSpacePresenter()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(space);

        learningSpacePresenter.Received().SetLearningSpace(space);
    }

    [Test]
    public void DragObjectInPathWay_CallsPresentationLogic()
    {
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        double oldPositionX = 5;
        double oldPositionY = 7;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.DragObjectInPathWay(space,
            new DraggedEventArgs<IObjectInPathWayViewModel>(space, oldPositionX, oldPositionY));

        presentationLogic.Received().DragObjectInPathWay(space, oldPositionX, oldPositionY);
    }

    [Test]
    public void EditLearningSpace_SetsSelectedLearningSpaceAndSetsEditLearningSpaceDialogOpenToTrue()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.EditObjectInPathWay(space);

        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
    }

    [Test]
    public void DeleteLearningSpace_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DeleteLearningSpace(space);

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void DeleteLearningSpace_SelectedLearningWorldIsNull_ThrowsException()
    {
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");

        var systemUnderTest = CreatePresenterForTesting();
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningSpace(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void RightClickedLearningSpace_SetsRightClickedLearningObjectToSpace()
    {
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(space);

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(space));
    }

    [Test]
    public void HideRightClickMenu_SetsRightClickedLearningObjectToNull()
    {
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();

        systemUnderTest.RightClickOnObjectInPathWay(space);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(space));

        systemUnderTest.HideRightClickMenu();

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void ClickedLearningSpace_SetsSelectedLearningObjectToSpace()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.ClickOnObjectInWorld(space);

        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
    }

    [Test]
    public void DoubleClickedLearningSpace_SetsSelectedLearningObjectAndShowsLearningSpaceView()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DoubleClickOnObjectInPathway(space);

        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.True);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }


    #region Create/AddLearningSpace

    [Test]
    public void AddLearningSpace_LearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningSpace(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void AddLearningSpace_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.AddLearningSpace(space);

        presentationLogic.Received().AddLearningSpace(world, space);
    }

    [Test]
    public void CreateNewLearningSpace_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm?.LearningSpaces.Add(space);

        systemUnderTest.CreateLearningSpace("foo", "bar", "foo", "bar", "foo", 5);

        presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<double>(),
            Arg.Any<double>());
    }

    [Test]
    public void CreateNewLearningSpace_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.CreateLearningSpace("foo", "bar", "foo", "bar", "foo", 5));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    #endregion

    #region DeleteSelectedLearningObject

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
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.LearningWorldVm!.SelectedLearningObjectInPathWay, Is.Null);
            Assert.That(systemUnderTest.LearningWorldVm.LearningSpaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesSpaceFromViewModel_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObjectInPathWay = space;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesConditionFromViewModel_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.PathWayConditions.Add(condition);
        world.SelectedLearningObjectInPathWay = condition;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeleteSelectedLearningObject_DeletesPathWayFromViewModel_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var space = new LearningSpaceViewModel("f", "f", "f,", "f", "f");
        var pathWay = new LearningPathwayViewModel(space, condition);
        world.LearningPathWays.Add(pathWay);
        world.SelectedLearningObjectInPathWay = pathWay;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeleteLearningPathWay(world, pathWay);
    }

    #endregion

    #region SaveSelectedLearningSpace

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
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public async Task SaveSelectedLearningSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObjectInPathWay = space;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();

        await presentationLogic.Received().SaveLearningSpaceAsync(space);
    }

    #endregion

    #region LoadLearningSpace

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
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        world.LearningSpaces.Add(space);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningSpaceAsync();

        await presentationLogic.Received().LoadLearningSpaceAsync(world);
    }

    #endregion

    #region Open/CloseLearningSpaceView

    [Test]
    public void ShowAndCloseLearningSpaceView_OpensAndClosesLearningSpaceView_SetsShowingLearningSpaceViewBool()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningSpace = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        world.SelectedLearningObjectInPathWay = learningSpace;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.False);

        systemUnderTest.ShowSelectedLearningSpaceView();

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.True);

        systemUnderTest.CloseLearningSpaceView();

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.False);
    }

    #endregion

    #endregion

    #region LearningPathWays

    [Test]
    public void SetSelectedLearningObject_SetsPathWayToSelectedObject()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        var pathWay = new LearningPathwayViewModel(condition, space);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(pathWay);

        Assert.That(systemUnderTest.LearningWorldVm.SelectedLearningObjectInPathWay, Is.EqualTo(pathWay));
    }

    #region PathWayCondition

    [Test]
    public void SetSelectedLearningObject_SetsConditionToSelectedObject()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(condition);

        Assert.That(systemUnderTest.LearningWorldVm.SelectedLearningObjectInPathWay, Is.EqualTo(condition));
    }

    [Test]
    public void DeletePathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.SelectedLearningObjectInPathWay = condition;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DeletePathWayCondition(condition);

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }

    [Test]
    public void DeletePathWayCondition_ThrowsWhenWorldIsNull()
    {
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeletePathWayCondition(condition));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void RightClickedPathWayCondition_SetsRightClickedLearningObjectToSpace()
    {
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(condition);

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(condition));
    }

    [Test]
    public void HideRightClickMenuFromCondition_SetsRightClickedLearningObjectToNull()
    {
        var conditionViewModel = new PathWayConditionViewModel(ConditionEnum.Or, 2, 1);
        var systemUnderTest = CreatePresenterForTesting();

        systemUnderTest.RightClickOnObjectInPathWay(conditionViewModel);
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(conditionViewModel));

        systemUnderTest.HideRightClickMenu();

        Assert.That(systemUnderTest.RightClickedLearningObject, Is.Null);
    }

    [Test]
    public void ClickedPathWayCondition_SetsSelectedLearningObjectToCondition()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var conditionViewModel = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.ClickOnObjectInWorld(conditionViewModel);

        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(conditionViewModel));
    }

    [Test]
    public void DoubleClickedPathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DoubleClickOnObjectInPathway(condition);

        presentationLogic.Received().EditPathWayCondition(condition, ConditionEnum.Or);
    }

    #endregion

    #region SetOnHoveredObjectInPathWay

    [Test]
    public void SetOnHoveredPathWayObject_ObjectAtPositionIsNull_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX: 25, positionY: 25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX: 250, positionY: 250);
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
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX: 25, positionY: 25);
        world.LearningSpaces.Add(sourceSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 30, 30);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceCondition_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceCondition = new PathWayConditionViewModel(ConditionEnum.And,
            positionX: 25, positionY: 25);
        world.PathWayConditions.Add(sourceCondition);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceCondition, 30, 30);

        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_SetsCorrectObjectAtPosition()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX: 25, positionY: 25);
        var targetSpace1 = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX: 250, positionY: 250);
        var targetSpace2 = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX: 500, positionY: 500);
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
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetOnHoveredObjectInPathWay(space, 3, 3));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    #endregion

    #region CreateLearningPathWay

    [Test]
    public void CreateLearningPathWay_WithoutTargetSpace_DoesNotCallPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX: 25, positionY: 25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX: 50, positionY: 50);
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
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX: 25, positionY: 25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX: 50, positionY: 50);
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
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX: 25, positionY: 25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX: 250, positionY: 250);
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
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateLearningPathWay(space, 3, 3));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    #endregion

    #region DeleteLearningPathWay

    [Test]
    public void DeleteLearningPathWay_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var space = new LearningSpaceViewModel("a", "d", "f", "g", "f", 2);
        var learningPathWay = new LearningPathwayViewModel(condition, space);
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
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningPathWay(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteLearningPathWay_PathWayIsNull_ThrowsException()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningPathWay(null!));
        Assert.That(ex!.Message, Is.EqualTo("LearningPathWay is null"));
    }

    #endregion

    #endregion

    private LearningWorldPresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null,
        ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        _authoringToolWorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, logger,
            _authoringToolWorkspaceViewModel);
    }
}