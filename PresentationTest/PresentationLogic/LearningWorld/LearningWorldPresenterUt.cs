using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace PresentationTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
    #region LearningSpace

    [Test]
    public void SetSelectedLearningSpace_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningSpace(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void SetSelectedLearningSpace_CallsSpacePresenter()
    { 
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("g", "g", "g", "g", "g");
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.SetSelectedLearningSpace(space);

        learningSpacePresenter.Received().SetLearningSpace(space);
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

        systemUnderTest.CreateNewLearningSpace("foo", "bar", "foo", "bar", "foo", 5);

        presentationLogic.Received().CreateLearningSpace(world, Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<double>(), Arg.Any<double>());
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
    public void OnCreateSpaceDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningSpace = space;

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
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningSpace(world, "n", "sn", "a", "d", "g", 10, Arg.Any<double>(), Arg.Any<double>());
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
        world.LearningSpaces.Add(space);

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

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: spacePresenter);
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningSpace = space;


        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);

        spacePresenter.Received().EditLearningSpace("n", "sn", "a", "d", "g", 10);
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

    #region DeleteSelectedLearningSpace

    [Test]
    public void DeleteSelectedLearningSpace_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningSpace());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteSelectedLearningSpace_DoesNotThrowWhenSelectedSpaceNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.LearningWorldVm!.SelectedLearningSpace, Is.Null);
            Assert.That(systemUnderTest.LearningWorldVm.LearningSpaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningSpace());
        });
    }

    [Test]
    public void DeleteSelectedLearningSpace_DeletesSpaceFromViewModel_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningSpace = space;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningSpace();

        presentationLogic.Received().DeleteLearningSpace(world, space);
    }

    #endregion

    #region OpenEditSelectedLearningSpaceDialog

    [Test]
    public void OpenEditSelectedLearningSpaceDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedLearningSpaceDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OpenEditSelectedLearningSpaceDialog_DoesNotThrowWhenSelectedSpaceNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OpenEditSelectedLearningSpaceDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.LearningWorldVm!.SelectedLearningSpace, Is.EqualTo(null));
            Assert.That(systemUnderTest.LearningWorldVm.LearningSpaces, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningSpace());
        });
    }

    [Test]
    public void OpenEditSelectedLearningSpaceDialog_CallsMethod()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        world.LearningSpaces.Add(space);
        world.SelectedLearningSpace = space;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OpenEditSelectedLearningSpaceDialog();
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
        world.SelectedLearningSpace = space;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningSpaceAsync();

        await presentationLogic.Received().SaveLearningSpaceAsync(space);
    }

    #endregion

    #region LoadLearningSpace/Element

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
        world.SelectedLearningSpace = learningSpace;

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
    
    #region SetOnHoveredLearningSpace
    
    [Test]
    public void SetOnHoveredLearningSpace_ObjectAtPositionIsNull_SetsHoveredLearningSpaceToNull()
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
        systemUnderTest.LearningWorldVm.OnHoveredLearningSpace = targetSpace;
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredLearningSpace, Is.EqualTo(targetSpace));
        
        systemUnderTest.SetOnHoveredLearningSpace(sourceSpace, 400,400);
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredLearningSpace, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredLearningSpace_ObjectAtPositionIsSourceSpace_SetsHoveredLearningSpaceToNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var sourceSpace = new LearningSpaceViewModel("g", "g", "g", "g", "g",
            positionX:25, positionY:25);
        world.LearningSpaces.Add(sourceSpace);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world; ;

        systemUnderTest.SetOnHoveredLearningSpace(sourceSpace, 30,30);
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredLearningSpace, Is.Null);
    }
    
    [Test]
    public void SetOnHoveredLearningSpace_SetsCorrectObjectAtPosition()
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
        systemUnderTest.LearningWorldVm.OnHoveredLearningSpace = targetSpace1;
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredLearningSpace, Is.EqualTo(targetSpace1));
        
        systemUnderTest.SetOnHoveredLearningSpace(sourceSpace, 510,510);
        
        Assert.That(systemUnderTest.LearningWorldVm.OnHoveredLearningSpace, Is.EqualTo(targetSpace2));
    }

    [Test]
    public void SetOnHoveredLearningSpace_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetOnHoveredLearningSpace(space, 3,3));
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
        var targetSpace = new LearningSpaceViewModel("u", "u", "u", "u", "u",
            positionX:250, positionY:250);
        world.LearningSpaces.Add(targetSpace);
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        
        systemUnderTest.DeleteLearningPathWay(targetSpace);
        
        presentationLogic.Received().DeleteLearningPathWay(world, targetSpace);
    }
    
    [Test]
    public void DeleteLearningPathWay_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "v", "d", "f", "f", 4);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteLearningPathWay(space));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
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