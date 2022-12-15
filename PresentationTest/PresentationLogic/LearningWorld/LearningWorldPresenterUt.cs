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
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;

namespace PresentationTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
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
        systemUnderTest.DragObjectInPathWay(space, new DraggedEventArgs<IObjectInPathWayViewModel> (space, oldPositionX, oldPositionY));

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
        
        Assert.That(world.SelectedLearningObject, Is.EqualTo(space));
        Assert.That(systemUnderTest.EditLearningSpaceDialogOpen, Is.True);
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
        
        Assert.That(world.SelectedLearningObject, Is.EqualTo(space));
    }

    [Test]
    public void DoubleClickedLearningSpace_SetsSelectedLearningObjectAndShowsLearningSpaceView()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DoubleClickOnObjectInWorld(space);
        
        Assert.That(world.SelectedLearningObject, Is.EqualTo(space));
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

        presentationLogic.Received().AddLearningSpace(world,space);
    }

    [Test]
    public void AddNewLearningSpace_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateLearningSpaceDialogOpen);
        
        systemUnderTest.AddNewLearningSpace();
        
        Assert.That(systemUnderTest.CreateLearningSpaceDialogOpen);
    }

    #endregion

    #region OnCreateSpaceDialogClose

    [Test]
    public void OnCreateSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - n.stich
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnCreateSpaceDialogClose_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - n.mho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OnCreateSpaceDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        var topic = new TopicViewModel("abc");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;
        world.Topics.Add(topic);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        dictionary["Topic"] = "abc";
        dictionary["Required Points"] = "10";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningSpace(world, "n", "sn", "a", "d", "g", 10, Arg.Any<double>(), Arg.Any<double>(), topic);
    }

    #endregion

    #region OnEditSpaceDialogClose

    [Test]
    public void OnEditSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnEditSpaceDialogClose_CallsLearningSpacePresenter()
    {
        var spacePresenter = Substitute.For<ILearningSpacePresenter>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("foo", "bar", "foo", "bar", "foo");
        var topic = new TopicViewModel("abc");
        world.LearningSpaces.Add(space);
        world.Topics.Add(topic);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        dictionary["Topic"] = "abc";
        dictionary["Required Points"] = "10";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: spacePresenter);
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = space;


        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);

        spacePresenter.Received().EditLearningSpace("n", "sn", "a", "d", "g", 10, topic);
    }
    
    [Test]
    public void OnEditSpaceDialogClose_LearningWorldIsNull_ThrowsException()
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
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
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
            Assert.That(systemUnderTest.LearningWorldVm!.SelectedLearningObject, Is.Null);
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
        world.SelectedLearningObject = space;
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
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        world.PathWayConditions.Add(condition);
        world.SelectedLearningObject = condition;
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
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var space = new LearningSpaceViewModel("f","f","f,","f","f");
        var pathWay = new LearningPathwayViewModel(space, condition);
        world.LearningPathWays.Add(pathWay);
        world.SelectedLearningObject = pathWay;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        presentationLogic.Received().DeleteLearningPathWay(world, pathWay);
    }

    #endregion

    #region OpenEditSelectedObjectDialog

    [Test]
    public void OpenEditSelectedObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OpenEditSelectedObjectDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OpenEditSelectedObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.LearningWorldVm!.SelectedLearningObject, Is.EqualTo(null));
            Assert.That(systemUnderTest.LearningWorldVm.LearningSpaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.OpenEditSelectedObjectDialog());
        });
    }
    
    [Test]
    public void OpenEditSelectedObjectDialog_DoesNotThrowWhenSelectedObjectIsLearningPathWay()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var space = new LearningSpaceViewModel("f","f","f","f","f");
        var pathWay = new LearningPathwayViewModel(condition, space);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.LearningPathWays.Add(pathWay);
        systemUnderTest.LearningWorldVm.SelectedLearningObject = pathWay;

        systemUnderTest.OpenEditSelectedObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.LearningWorldVm!.SelectedLearningObject, Is.EqualTo(pathWay));
            Assert.That(systemUnderTest.LearningWorldVm.LearningSpaces, Is.Empty);
            Assert.That(systemUnderTest.LearningWorldVm.LearningPathWays, Is.Not.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.OpenEditSelectedObjectDialog());
        });
    }

    [Test]
    public void OpenEditSelectedLearningSpaceDialog_CallsMethod()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OpenEditSelectedObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningSpaceDialogOpen, Is.True);
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
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.PathWayConditions.Add(condition);
        world.SelectedLearningObject = condition;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

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
        world.SelectedLearningObject = space;

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
        world.SelectedLearningObject = learningSpace;

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

        Assert.That(systemUnderTest.LearningWorldVm.SelectedLearningObject, Is.EqualTo(pathWay));
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
    public void SetSelectedLearningObject_SetsConditionToSelectedObject()
    { 
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningObject(condition);

        Assert.That(systemUnderTest.LearningWorldVm.SelectedLearningObject, Is.EqualTo(condition));
    }
    
    [Test]
    public void DeletePathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.SelectedLearningObject = condition;

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
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.RightClickOnObjectInPathWay(condition);
        
        Assert.That(systemUnderTest.RightClickedLearningObject, Is.EqualTo(condition));
    }
    
    [Test]
    public void HideRightClickMenuFromCondition_SetsRightClickedLearningObjectToNull()
    {
        var conditionViewModel = new PathWayConditionViewModel(ConditionEnum.Or,2,1);
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
        var conditionViewModel = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.ClickOnObjectInWorld(conditionViewModel);
        
        Assert.That(world.SelectedLearningObject, Is.EqualTo(conditionViewModel));
    }
    
    [Test]
    public void DoubleClickedPathWayCondition_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.DoubleClickOnObjectInWorld(condition);
        
        presentationLogic.Received().EditPathWayCondition(condition, ConditionEnum.Or);
    }

    #region OnCreatePathWayConditionDialogClose

    [Test]
    public void OnCreatePathWayConditionDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnCreatePathWayConditionDialogClose_ThrowsWhenLearningWorldIsNull()
    {
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
    }
    
    [Test]
    public void OnCreatePathWayConditionDialogClose_ModalDialogCancel_Returns()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = ConditionEnum.And;
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Cancel;
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple);

        presentationLogic.DidNotReceive().CreatePathWayCondition(world, condition, 20, 100);
    }
    
    [Test]
    public void OnCreatePathWayConditionDialogClose_ThrowsWhenConditionIsNotValid()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = "XOR";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Condition is not a valid enum value"));
    }

    [Test]
    public void OnCreatePathWayConditionDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple);

        presentationLogic.Received().CreatePathWayCondition(world, ConditionEnum.And, 20, 100);
    }

    #endregion

    #region OnEditPathWayConditionDialogClose

    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenConditionIsNotValid()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = "XOR";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Condition is not a valid enum value"));
    }
    
    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenSelectedObjectIsNotACondition()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f","f", "f", "f", "f", 3);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.SelectedLearningObject = space;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningObject is not a pathWayCondition"));
    }

    [Test]
    public void OnEditPathWayConditionDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        world.PathWayConditions.Add(condition);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = condition;


        systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple);

        presentationLogic.Received().EditPathWayCondition(condition, ConditionEnum.And);
    }
    
    [Test]
    public void OnEditPathWayConditionDialogClose_ThrowsWhenLearningWorldIsNull()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditPathWayConditionDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
    }

    #endregion
    
    #endregion
    
    #region SetOnHoveredObjectInPathWay
    
    [Test]
    public void SetOnHoveredPathWayObject_ObjectAtPositionIsNull_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay = targetSpace;
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace));
        
        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 400,400);
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceSpace_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        world.LearningSpaces.Add(sourceSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world; ;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 30,30);
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredObjectInPathWay_ObjectAtPositionIsSourceCondition_SetsOnHoveredObjectInPathWayToNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceCondition = new PathWayConditionViewModel( ConditionEnum.And,
            positionX:25, positionY:25);
        world.PathWayConditions.Add(sourceCondition);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world; ;

        systemUnderTest.SetOnHoveredObjectInPathWay(sourceCondition, 30,30);
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredObjectInPathWay_SetsCorrectObjectAtPosition()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace1 = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        var targetSpace2 = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:500, positionY:500);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace1);
        world.LearningSpaces.Add(targetSpace2);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay = targetSpace1;
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace1));
        
        systemUnderTest.SetOnHoveredObjectInPathWay(sourceSpace, 510,510);
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredObjectInPathWay, Is.EqualTo(targetSpace2));
    }

    [Test]
    public void SetOnHoveredObjectInPathWay_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetOnHoveredObjectInPathWay(space, 3,3));
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
            positionX:25, positionY:25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:50, positionY:50);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        
        systemUnderTest.CreateLearningPathWay(sourceSpace, 25,25);
        
        presentationLogic.DidNotReceive().CreateLearningPathWay(world, sourceSpace, targetSpace);
    }
    
    [Test]
    public void CreateLearningPathWay_TargetSpaceIsSourceSpace_DoesNotCallPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:50, positionY:50);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        
        systemUnderTest.CreateLearningPathWay(sourceSpace, 60,60);
        
        presentationLogic.DidNotReceive().CreateLearningPathWay(world, sourceSpace, sourceSpace);
    }
    
    [Test]
    public void CreateLearningPathWay_WithTargetSpace_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 260,260);
        
        presentationLogic.Received().CreateLearningPathWay(world, sourceSpace, targetSpace);
    }
    
    [Test]
    public void CreateLearningPathWay_TargetSpaceHasInBoundObject_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        var sourceCondition = new PathWayConditionViewModel(ConditionEnum.And,3,1);
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        targetSpace.InBoundObjects.Add(sourceCondition);
        world.PathWayConditions.Add(sourceCondition);
        world.LearningSpaces.Add(sourceSpace);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.CreateLearningPathWay(sourceSpace, 260,260);
        
        presentationLogic.DidNotReceive().CreateLearningPathWay(world, sourceSpace, targetSpace);
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Condition"] = ConditionEnum.And.ToString();
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);
        
        systemUnderTest.OnCreatePathWayConditionDialogClose(returnValueTuple);
        
        presentationLogic.Received().CreatePathWayConditionBetweenObjects(world, ConditionEnum.And, sourceSpace, targetSpace);
    }

    [Test]
    public void CreateLearningPathWay_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateLearningPathWay(space, 3,3));
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
        var learningPathWay = new LearningPathwayViewModel(condition,space);
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
    
    #region Topics
    
    #region CreateTopic

    [Test]
    public void AddNewTopic_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateTopicDialogOpen);
        
        systemUnderTest.AddNewTopic();
        
        Assert.That(systemUnderTest.CreateTopicDialogOpen);
    }
    
    [Test]
    public void OnCreateTopicDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateTopicDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnCreateTopicDialogClose_ThrowsWhenLearningWorldIsNull()
    {
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateTopicDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
    }
    
    [Test]
    public void OnCreateTopicDialogClose_ModalDialogCancel_Returns()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Cancel;
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.OnCreateTopicDialogClose(returnValueTuple);

        presentationLogic.DidNotReceive().CreateTopic(world, "name");
    }

    [Test]
    public void OnCreateTopicDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "blabla";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateTopicDialogClose(returnValueTuple);

        presentationLogic.Received().CreateTopic(world, "blabla");
    }
    
    #endregion
    
    #region EditTopic

    [Test]
    public void OpenEditTopicDialog_ThrowsWhenLearningWorldIsNull()
    {
        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditTopicDialog());
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
    }

    [Test]
    public void OpenEditTopicDialog_NoTopicsInWorld_Returns()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        Assert.That(world.Topics.Count, Is.EqualTo(0));
        
        systemUnderTest.OpenEditTopicDialog();
        
        Assert.That(!systemUnderTest.EditTopicDialogOpen);
    }
    
    [Test]
    public void OpenEditTopicDialog_OpensDialog()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        
        world.Topics.Add(new TopicViewModel("a"));

        Assert.That(world.Topics.Count, Is.EqualTo(1));
        Assert.That(systemUnderTest.EditTopicDialogInitialValues, Is.Null);
        
        systemUnderTest.OpenEditTopicDialog();
        
        Assert.That(systemUnderTest.EditTopicDialogInitialValues, Contains.Item("a"));
        Assert.That(systemUnderTest.EditTopicDialogOpen);
    }
    
    [Test]
    public void OnEditTopicDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditTopicDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnEditTopicDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var topic = new TopicViewModel("a");
        world.Topics.Add(topic);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Topics"] = "a";
        dictionary["New Name"] = "b";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        
        systemUnderTest.OnEditTopicDialogClose(returnValueTuple);

        presentationLogic.Received().EditTopic(topic, "b");
    }
    
    [Test]
    public void OnEditTopicDialogClose_ThrowsWhenLearningWorldIsNull()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["New Name"] = "a";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditTopicDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
    }
    
    [Test]
    public void OnEditTopicDialogClose_ModalDialogCancel_Returns()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Cancel;
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.OnEditTopicDialogClose(returnValueTuple);

        presentationLogic.DidNotReceive().CreateTopic(world, "name");
    }
    
    #endregion
    
    #region DeleteTopic

    [Test]
    public void OpenDeleteTopicDialog_ThrowsWhenLearningWorldIsNull()
    {
        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenDeleteTopicDialog());
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
    }

    [Test]
    public void OpenDeleteTopicDialog_NoTopicsInWorld_Returns()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        Assert.That(world.Topics.Count, Is.EqualTo(0));
        
        systemUnderTest.OpenDeleteTopicDialog();
        
        Assert.That(!systemUnderTest.DeleteTopicDialogOpen);
    }
    
    [Test]
    public void OpenDeleteTopicDialog_OpensDialog()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        
        world.Topics.Add(new TopicViewModel("a"));

        Assert.That(world.Topics.Count, Is.EqualTo(1));
        Assert.That(systemUnderTest.DeleteTopicDialogInitialValues, Is.Null);
        
        systemUnderTest.OpenDeleteTopicDialog();
        
        Assert.That(systemUnderTest.DeleteTopicDialogInitialValues, Contains.Item("a"));
        Assert.That(systemUnderTest.DeleteTopicDialogOpen);
    }
    
    [Test]
    public void OnDeleteTopicDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnDeleteTopicDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnDeleteTopicDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var topic = new TopicViewModel("a");
        world.Topics.Add(topic);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Topics"] = "a";
        dictionary["New Name"] = "b";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        
        systemUnderTest.OnDeleteTopicDialogClose(returnValueTuple);

        presentationLogic.Received().DeleteTopic(world, topic);
    }
    
    [Test]
    public void OnDeleteTopicDialogClose_ThrowsWhenLearningWorldIsNull()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["New Name"] = "a";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnDeleteTopicDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningWorld is null"));
    }
    
    [Test]
    public void OnDeleteTopicDialogClose_ModalDialogCancel_Returns()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Cancel;
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var returnValueTuple =
            //nullability overridden because required for test - m.ho
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.OnDeleteTopicDialogClose(returnValueTuple);

        presentationLogic.DidNotReceive().CreateTopic(world, "name");
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
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, logger);
    }
}