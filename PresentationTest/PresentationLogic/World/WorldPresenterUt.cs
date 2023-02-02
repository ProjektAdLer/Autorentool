using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Shared;

namespace PresentationTest.PresentationLogic.World;

[TestFixture]
public class WorldPresenterUt
{
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;

    #region Space

    [Test]
    public void SetSelectedObject_SelectedWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new SpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedObject(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public void SetSelectedObject_CallsSpacePresenter()
    { 
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var spacePresenter = Substitute.For<ISpacePresenter>();

        var systemUnderTest = CreatePresenterForTesting(spacePresenter: spacePresenter);
        systemUnderTest.WorldVm = world;
        systemUnderTest.SetSelectedObject(space);

        spacePresenter.Received().SetSpace(space);
    }
    
    [Test]
    public void DragObjectInPathWay_CallsPresentationLogic()
    {
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        double oldPositionX = 5;
        double oldPositionY = 7;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.DragObjectInPathWay(space, new DraggedEventArgs<IObjectInPathWayViewModel> (space, oldPositionX, oldPositionY));

        presentationLogic.Received().DragObjectInPathWay(space, oldPositionX, oldPositionY);
    }

    [Test]
    public void EditSpace_SetsSelectedSpaceAndSetsEditSpaceDialogOpenToTrue()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.EditObjectInPathWay(space);
        
        Assert.That(world.SelectedObject, Is.EqualTo(space));
        Assert.That(systemUnderTest.EditSpaceDialogOpen, Is.True);
    }
    
    [Test]
    public void DeleteSpace_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        systemUnderTest.DeleteSpace(space);

        presentationLogic.Received().DeleteSpace(world, space);
    }
    
    [Test]
    public void DeleteSpace_SelectedWorldIsNull_ThrowsException()
    {
        var space = new SpaceViewModel("g", "g", "g", "g", "g");

        var systemUnderTest = CreatePresenterForTesting();
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSpace(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }
    
    [Test]
    public void RightClickedSpace_SetsRightClickedObjectToSpace()
    {
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(space);
        
        Assert.That(systemUnderTest.RightClickedObject, Is.EqualTo(space));
    }
    
    [Test]
    public void HideRightClickMenu_SetsRightClickedObjectToNull()
    {
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        
        systemUnderTest.RightClickOnObjectInPathWay(space);
        Assert.That(systemUnderTest.RightClickedObject, Is.EqualTo(space));
        
        systemUnderTest.HideRightClickMenu();
        
        Assert.That(systemUnderTest.RightClickedObject, Is.Null);
    }
    
    [Test]
    public void ClickedSpace_SetsSelectedObjectToSpace()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.ClickOnObjectInWorld(space);
        
        Assert.That(world.SelectedObject, Is.EqualTo(space));
    }

    [Test]
    public void DoubleClickedSpace_SetsSelectedObjectAndShowsSpaceView()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.DoubleClickOnSpaceInWorld(space);
        
        Assert.That(world.SelectedObject, Is.EqualTo(space));
        Assert.That(systemUnderTest.ShowingSpaceView, Is.True);
        Assert.That(systemUnderTest.RightClickedObject, Is.Null);
    }
        
        
    #region Create/AddSpace
    
    [Test]
    public void AddSpace_WorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new SpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddSpace(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public void AddSpace_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var presentationLogic = Substitute.For<IPresentationLogic>();
        
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        systemUnderTest.AddSpace(space);

        presentationLogic.Received().AddSpace(world,space);
    }

    [Test]
    public void AddNewSpace_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateSpaceDialogOpen);
        
        systemUnderTest.AddNewSpace();
        
        Assert.That(systemUnderTest.CreateSpaceDialogOpen);
    }

    [Test]
    public void CreateNewSpace_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("g", "g", "g", "g", "g");
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        systemUnderTest.WorldVm?.Spaces.Add(space);

        systemUnderTest.CreateNewSpace("foo", "bar", "foo", "bar", "foo", 5);

        presentationLogic.Received().CreateSpace(world, Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void CreateNewSpace_SelectedWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewSpace("foo", "bar", "foo", "bar", "foo", 5));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    #endregion

    #region OnCreateSpaceDialogClose

    [Test]
    public void OnCreateSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - n.stich
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnCreateSpaceDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("f", "f", "f", "f", "f");
        world.Spaces.Add(space);
        world.SelectedObject = space;

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        dictionary["Required Points"] = "10";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;

        systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple);

        presentationLogic.Received().CreateSpace(world, "n", "sn", "a", "d", "g", 10, Arg.Any<double>(), Arg.Any<double>());
    }

    #endregion

    #region OnEditSpaceDialogClose

    [Test]
    public void OnEditSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnEditSpaceDialogClose_CallsSpacePresenter()
    {
        var spacePresenter = Substitute.For<ISpacePresenter>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("foo", "bar", "foo", "bar", "foo");
        world.Spaces.Add(space);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        dictionary["Required Points"] = "10";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(spacePresenter: spacePresenter);
        systemUnderTest.WorldVm = world;
        world.SelectedObject = space;


        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);

        spacePresenter.Received().EditSpace("n", "sn", "a", "d", "g", 10);
    }
    
    [Test]
    public void OnEditSpaceDialogClose_WorldIsNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("World is null"));
    }

    #endregion

    #region DeleteSelectedObject

    [Test]
    public void DeleteSelectedObject_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public void DeleteSelectedObject_DoesNotThrowWhenSelectedSpaceNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.WorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.WorldVm!.SelectedObject, Is.Null);
            Assert.That(systemUnderTest.WorldVm.Spaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedObject());
        });
    }

    [Test]
    public void DeleteSelectedObject_DeletesSpaceFromViewModel_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("f", "f", "f", "f", "f");
        world.Spaces.Add(space);
        world.SelectedObject = space;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;

        systemUnderTest.DeleteSelectedObject();

        presentationLogic.Received().DeleteSpace(world, space);
    }
    
    [Test]
    public void DeleteSelectedObject_DeletesConditionFromViewModel_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        world.PathWayConditions.Add(condition);
        world.SelectedObject = condition;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;

        systemUnderTest.DeleteSelectedObject();

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }
    
    [Test]
    public void DeleteSelectedObject_DeletesPathWayFromViewModel_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var space = new SpaceViewModel("f","f","f,","f","f");
        var pathWay = new PathwayViewModel(space, condition);
        world.PathWays.Add(pathWay);
        world.SelectedObject = pathWay;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;

        systemUnderTest.DeleteSelectedObject();

        presentationLogic.Received().DeletePathWay(world, pathWay);
    }

    #endregion

    #region OpenEditSelectedObjectDialog

    [Test]
    public void OpenEditSelectedObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public void OpenEditSelectedObjectDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        systemUnderTest.OpenEditSelectedObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.WorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.WorldVm!.SelectedObject, Is.EqualTo(null));
            Assert.That(systemUnderTest.WorldVm.Spaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.OpenEditSelectedObjectDialog());
        });
    }
    
    [Test]
    public void OpenEditSelectedObjectDialog_DoesNotThrowWhenSelectedObjectIsPathWay()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var space = new SpaceViewModel("f","f","f","f","f");
        var pathWay = new PathwayViewModel(condition, space);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.WorldVm.PathWays.Add(pathWay);
        systemUnderTest.WorldVm.SelectedObject = pathWay;

        systemUnderTest.OpenEditSelectedObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.WorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.WorldVm!.SelectedObject, Is.EqualTo(pathWay));
            Assert.That(systemUnderTest.WorldVm.Spaces, Is.Empty);
            Assert.That(systemUnderTest.WorldVm.PathWays, Is.Not.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.OpenEditSelectedObjectDialog());
        });
    }

    [Test]
    public void OpenEditSelectedSpaceDialog_CallsMethod()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("n", "sn", "a", "d", "g");
        world.Spaces.Add(space);
        world.SelectedObject = space;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        systemUnderTest.OpenEditSelectedObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditSpaceDialogOpen, Is.True);
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues, Is.Not.Null);
            //overriding nullability because we test for that above - n.stich
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues!["Name"], Is.EqualTo(space.Name));
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues["Shortname"], Is.EqualTo(space.Shortname));
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues["Authors"], Is.EqualTo(space.Authors));
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues["Description"], Is.EqualTo(space.Description));
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues["Goals"], Is.EqualTo(space.Goals));
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues["Required Points"], Is.EqualTo(space.RequiredPoints.ToString()));
            
            Assert.That(systemUnderTest.EditSpaceDialogAnnotations, Is.Not.Null);
            Assert.That(systemUnderTest.EditSpaceDialogAnnotations!.Count, Is.EqualTo(1));
            Assert.That(systemUnderTest.EditSpaceDialogAnnotations["Required Points"], Is.EqualTo("/"+space.Points));
        });
    }
    
    [Test]
    public void OpenEditSelectedPathWayConditionDialog_CallsMethod()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.PathWayConditions.Add(condition);
        world.SelectedObject = condition;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        systemUnderTest.OpenEditSelectedObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditPathWayConditionDialogOpen, Is.True);
            Assert.That(systemUnderTest.EditConditionDialogInitialValues, Is.Not.Null);
            //overriding nullability because we test for that above - m.ho
            Assert.That(systemUnderTest.EditConditionDialogInitialValues!["Condition"], Is.EqualTo(condition.Condition.ToString()));
        });
    }

    #endregion

    #region SaveSelectedSpace

    [Test]
    public void SaveSelectedSpaceAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = null;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public void SaveSelectedSpaceAsync_DoesNotThrowWhenSelectedSpaceNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }

    [Test]
    public async Task SaveSelectedSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("f", "f", "f", "f", "f");
        world.Spaces.Add(space);
        world.SelectedObject = space;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.WorldVm = world;
        await systemUnderTest.SaveSelectedSpaceAsync();

        await presentationLogic.Received().SaveSpaceAsync(space);
    }

    #endregion

    #region LoadSpace

    [Test]
    public void LoadSpace_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = null;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }

    [Test]
    public async Task LoadSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("a", "b", "c", "d", "e");
        world.Spaces.Add(space);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.WorldVm = world;
        await systemUnderTest.LoadSpaceAsync();

        await presentationLogic.Received().LoadSpaceAsync(world);
    }

    #endregion
    
    #region Open/CloseSpaceView

    [Test]
    public void ShowAndCloseSpaceView_OpensAndClosesSpaceView_SetsShowingSpaceViewBool()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("a", "b", "c", "d", "e");
        world.SelectedObject = space;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        Assert.That(systemUnderTest.ShowingSpaceView, Is.False);
        
        systemUnderTest.ShowSelectedSpaceView();

        Assert.That(systemUnderTest.ShowingSpaceView, Is.True);
        
        systemUnderTest.CloseSpaceView();
        
        Assert.That(systemUnderTest.ShowingSpaceView, Is.False);
    }
    
    #endregion

    #endregion

    #region PathWays
    
    [Test]
    public void SetSelectedObject_SetsPathWayToSelectedObject()
    { 
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var space = new SpaceViewModel("f", "f", "f", "f", "f");
        var pathWay = new PathwayViewModel(condition, space);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.SetSelectedObject(pathWay);

        Assert.That(systemUnderTest.WorldVm.SelectedObject, Is.EqualTo(pathWay));
    }
    
    #region PathWayCondition
    
    [Test]
    public void AddNewPathWayCondition_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreatePathWayConditionDialogOpen);
        
        systemUnderTest.AddNewPathWayCondition();
        
        Assert.That(systemUnderTest.CreatePathWayConditionDialogOpen);
    }
    
    [Test]
    public void SetSelectedObject_SetsConditionToSelectedObject()
    { 
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.SetSelectedObject(condition);

        Assert.That(systemUnderTest.WorldVm.SelectedObject, Is.EqualTo(condition));
    }
    
    [Test]
    public void DeletePathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.SelectedObject = condition;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.WorldVm = world;
        systemUnderTest.DeletePathWayCondition(condition);

        presentationLogic.Received().DeletePathWayCondition(world, condition);
    }
    
    [Test]
    public void DeletePathWayCondition_ThrowsWhenWorldIsNull()
    {
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeletePathWayCondition(condition));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }
    
    [Test]
    public void RightClickedPathWayCondition_SetsRightClickedObjectToSpace()
    {
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(condition);
        
        Assert.That(systemUnderTest.RightClickedObject, Is.EqualTo(condition));
    }
    
    [Test]
    public void HideRightClickMenuFromCondition_SetsRightClickedObjectToNull()
    {
        var conditionViewModel = new PathWayConditionViewModel(ConditionEnum.Or,2,1);
        var systemUnderTest = CreatePresenterForTesting();
        
        systemUnderTest.RightClickOnObjectInPathWay(conditionViewModel);
        Assert.That(systemUnderTest.RightClickedObject, Is.EqualTo(conditionViewModel));
        
        systemUnderTest.HideRightClickMenu();
        
        Assert.That(systemUnderTest.RightClickedObject, Is.Null);
    }
    
    [Test]
    public void ClickedPathWayCondition_SetsSelectedObjectToCondition()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var conditionViewModel = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.ClickOnObjectInWorld(conditionViewModel);
        
        Assert.That(world.SelectedObject, Is.EqualTo(conditionViewModel));
    }
    
    [Test]
    public void DoubleClickedPathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.WorldVm = world;
        systemUnderTest.DoubleClickOnSpaceInWorld(condition);
        
        presentationLogic.Received().EditPathWayCondition(condition, ConditionEnum.Or);
    }

    #region OnCreatePathWayConditionDialogClose

    [Test]
    public void OnCreatePathWayConditionDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnCreatePathWayConditionDialogClose_ThrowsWhenWorldIsNull()
    {
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("World is null"));
    }
    
    [Test]
    public void OnCreatePathWayConditionDialogClose_ModalDialogCancel_Returns()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = ConditionEnum.And;
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Cancel;
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.WorldVm = world;
        systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple);

        presentationLogic.DidNotReceive().CreatePathWayCondition(world, condition, 20, 100);
    }
    
    [Test]
    public void OnCreatePathWayConditionDialogClose_ThrowsWhenConditionIsNotValid()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = "XOR";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Condition is not a valid enum value"));
    }

    [Test]
    public void OnCreatePathWayConditionDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;

        systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple);

        presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 20, 100);
    }

    #endregion

    #region OnEditPathWayConditionDialogClose

    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenConditionIsNotValid()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = "XOR";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Condition is not a valid enum value"));
    }
    
    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenSelectedObjectIsNotACondition()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new SpaceViewModel("f","f", "f", "f", "f", 3);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.WorldVm.SelectedObject = space;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("DraggableObject is not a pathWayCondition"));
    }

    [Test]
    public void OnEditPathWayConditionDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.PathWayConditions.Add(condition);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        world.SelectedObject = condition;


        systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple);

        presentationLogic.Received().EditPathWayCondition(condition, ConditionEnum.And);
    }
    
    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenWorldIsNull()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("World is null"));
    }

    #endregion
    
    #endregion
    
    #region SetOnHoveredObjectInPathWay
    
    [Test]
    public void SetOnHoveredPathWayObject_ObjectAtPositionIsNull_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new SpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace = new SpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        world.Spaces.Add(sourceSpace);
        world.Spaces.Add(targetSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.WorldVm.OnHoveredObjectInPathWay = targetSpace;
        
        Assert.That(systemUnderTest.WorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace));
        
        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 400,400);
        
        Assert.That(systemUnderTest.WorldVm.OnHoveredObjectInPathWay, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceSpace_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new SpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        world.Spaces.Add(sourceSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world; ;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 30,30);
        
        Assert.That(systemUnderTest.WorldVm.OnHoveredObjectInPathWay, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceCondition_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceCondition = new PathWayConditionViewModel( ConditionEnum.And,
            positionX:25, positionY:25);
        world.PathWayConditions.Add(sourceCondition);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world; ;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceCondition, 30,30);
        
        Assert.That(systemUnderTest.WorldVm.OnHoveredObjectInPathWay, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredObjectInPathWay_SetsCorrectObjectAtPosition()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new SpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace1 = new SpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        var targetSpace2 = new SpaceViewModel("u", "u", "u", "u", "u",
            positionX:500, positionY:500);
        world.Spaces.Add(sourceSpace);
        world.Spaces.Add(targetSpace1);
        world.Spaces.Add(targetSpace2);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.WorldVm = world;
        systemUnderTest.WorldVm.OnHoveredObjectInPathWay = targetSpace1;
        
        Assert.That(systemUnderTest.WorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace1));
        
        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 510,510);
        
        Assert.That(systemUnderTest.WorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace2));
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_SelectedWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new SpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetOnHoveredObjectInPathWay(space, 3,3));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }
    
    #endregion
    
    
     #region CreatePathWay
    
    [Test]
    public void CreatePathWay_WithoutTargetSpace_DoesNotCallPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new SpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace = new SpaceViewModel("u", "u", "u", "u", "u",
            positionX:50, positionY:50);
        world.Spaces.Add(sourceSpace);
        world.Spaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        
        systemUnderTest.CreatePathWay(sourceSpace, 25,25);
        
        presentationLogic.DidNotReceive().CreatePathWay(world, sourceSpace, targetSpace);
    }
    
    [Test]
    public void CreatePathWay_TargetSpaceIsSourceSpace_DoesNotCallPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new SpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace = new SpaceViewModel("u", "u", "u", "u", "u",
            positionX:50, positionY:50);
        world.Spaces.Add(sourceSpace);
        world.Spaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        
        systemUnderTest.CreatePathWay(sourceSpace, 60,60);
        
        presentationLogic.DidNotReceive().CreatePathWay(world, sourceSpace, sourceSpace);
    }
    
    [Test]
    public void CreatePathWay_WithTargetSpace_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new SpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace = new SpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        world.Spaces.Add(sourceSpace);
        world.Spaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;

        systemUnderTest.CreatePathWay(sourceSpace, 260,260);
        
        presentationLogic.Received().CreatePathWay(world, sourceSpace, targetSpace);
    }
    
    [Test]
    public void CreatePathWay_TargetSpaceHasInBoundObject_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new SpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var sourceCondition = new PathWayConditionViewModel(ConditionEnum.And,3,1);
        var targetSpace = new SpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        targetSpace.InBoundObjects.Add(sourceCondition);
        world.PathWayConditions.Add(sourceCondition);
        world.Spaces.Add(sourceSpace);
        world.Spaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;

        systemUnderTest.CreatePathWay(sourceSpace, 260,260);
        
        presentationLogic.DidNotReceive().CreatePathWay(world, sourceSpace, targetSpace);
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);
        
        systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple);
        
        presentationLogic.Received().CreatePathWayConditionBetweenObjects(world, ConditionEnum.And, sourceSpace, targetSpace);
    }

    [Test]
    public void CreatePathWay_SelectedWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new SpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreatePathWay(space, 3,3));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }
    
    #endregion

    #region DeletePathWay

    [Test]
    public void DeletePathWay_CallsPresentationLogic()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var space = new SpaceViewModel("a", "d", "f", "g", "f", 2);
        var pathWay = new PathwayViewModel(condition,space);
        world.PathWays.Add(pathWay);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        
        systemUnderTest.DeletePathWay(space);
        
        presentationLogic.Received().DeletePathWay(world, pathWay);
    }
    
    [Test]
    public void DeletePathWay_SelectedWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new SpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeletePathWay(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedWorld is null"));
    }
    
    [Test]
    public void DeletePathWay_PathWayIsNull_ThrowsException()
    {
        var world = new WorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.WorldVm = world;
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeletePathWay(null!));
        Assert.That(ex!.Message, Is.EqualTo("PathWay is null"));
    }
    
    #endregion

    #endregion

    private WorldPresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ISpacePresenter? spacePresenter = null,
        ILogger<WorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        spacePresenter ??= Substitute.For<ISpacePresenter>();
        logger ??= Substitute.For<ILogger<WorldPresenter>>();
        _authoringToolWorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        return new WorldPresenter(presentationLogic, spacePresenter, logger, _authoringToolWorkspaceViewModel);
    }
}