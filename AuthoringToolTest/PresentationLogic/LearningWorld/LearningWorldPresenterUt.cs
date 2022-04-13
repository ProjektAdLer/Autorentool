using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
    [Test]
    public void LearningWorldPresenter_CreateNewLearningWorld_CreatesCorrectViewModel()
    {
        var name = "cool world";
        var shortname = "cw";
        var authors = "niklas";
        var language = "german";
        var description = "A very cool world";
        var goals = "lots of learning stuff";

        var systemUnderTest = CreatePresenterForTesting();

        var world = systemUnderTest.CreateNewLearningWorld(name, shortname, authors, language, description, goals);

        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo(name));
            Assert.That(world.Shortname, Is.EqualTo(shortname));
            Assert.That(world.Authors, Is.EqualTo(authors));
            Assert.That(world.Language, Is.EqualTo(language));
            Assert.That(world.Description, Is.EqualTo(description));
            Assert.That(world.Goals, Is.EqualTo(goals));
        });
    }

    [Test]
    public void LearningWorldPresenter_EditLearningWorld_EditsViewModelCorrectly()
    {
        var world = new LearningWorldViewModel("f", "fa", "a", "u", "e", "o");
        var name = "cool world";
        var shortname = "cw";
        var authors = "niklas";
        var language = "german";
        var description = "A very cool world";
        var goals = "lots of learning stuff";

        var systemUnderTest = CreatePresenterForTesting();

        world = systemUnderTest.EditLearningWorld(world, name, shortname, authors, language, description, goals);

        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo(name));
            Assert.That(world.Shortname, Is.EqualTo(shortname));
            Assert.That(world.Authors, Is.EqualTo(authors));
            Assert.That(world.Language, Is.EqualTo(language));
            Assert.That(world.Description, Is.EqualTo(description));
            Assert.That(world.Goals, Is.EqualTo(goals));
        });
    }

    #region LearningObject

    #region CreateNewLearningSpace/Element

    [Test]
    public void LearningWorldPresenter_CreateNewLearningSpace_ThrowsWhenSelectedWorldNull()
    {
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.CreateNewLearningSpace(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(
            new LearningSpaceViewModel("foo", "bar", "foo", "bar", "baz"));

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewLearningSpace("foo",
            "this", "does", "not", "matter"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LearningWorldPresenter_CreateNewLearningElement_ThrowsWhenSelectedWorldNull()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("foo", "bar",
            null, "Video", "bar", "foo", "foo", "bar"));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewLearningElement("foo",
            "bar", null, "foo", "bar", "foo", "bar",
            "foo"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LearningWorldPresenter_CreateNewLearningSpace_CallsLearningSpacePresenter()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.CreateNewLearningSpace(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(
            new LearningSpaceViewModel("foo", "bar", "foo", "bar", "baz"));

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.CreateNewLearningSpace("foo", "bar", "foo", "bar", "foo");

        learningSpacePresenter.Received().CreateNewLearningSpace(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void LearningWorldPresenter_CreateNewLearningElement_CallsLearningElementPresenter()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("foo", "bar", null,
            "foo", "bar", "foo", "bar", "foo"));
        var parent = new LearningWorldViewModel("foo", "boo", "bla", "blub", "bibi", "bubu");

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.CreateNewLearningElement("name", "sn", parent, "type",
            "cont", "aut", "desc", "goal");

        learningElementPresenter.Received()
            .CreateNewLearningElement("name", "sn", parent, "type", "cont", "aut", "desc", "goal");
    }

    [Test]
    public void LearningWorldPresenter_CreateNewLearningSpace_AddsLearningSpaceToViewModel()
    {
        var learningSpacePresenter = new LearningSpacePresenter();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting( //workspaceVm,
            learningSpacePresenter: learningSpacePresenter);

        Assert.IsEmpty(world.LearningSpaces);

        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.CreateNewLearningSpace("foo", "bar", "foo", "bar", "foo");

        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        var space = world.LearningSpaces.First();
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("foo"));
            Assert.That(space.Shortname, Is.EqualTo("bar"));
            Assert.That(space.Authors, Is.EqualTo("foo"));
            Assert.That(space.Description, Is.EqualTo("bar"));
            Assert.That(space.Goals, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void LearningWorldPresenter_CreateNewLearningElement_AddsLearningElementToWorldViewModel()
    {
        var learningElementPresenter = new LearningElementPresenter();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        Assert.That(world.LearningElements, Is.Empty);

        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.CreateNewLearningElement("foo", "bar", world,
            "foo", "bar", "foo", "bar", "foo");

        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        var element = world.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("foo"));
            Assert.That(element.Shortname, Is.EqualTo("bar"));
            Assert.That(element.Parent, Is.EqualTo(world));
            Assert.That(element.Type, Is.EqualTo("foo"));
            Assert.That(element.Content, Is.EqualTo("bar"));
            Assert.That(element.Authors, Is.EqualTo("foo"));
            Assert.That(element.Description, Is.EqualTo("bar"));
            Assert.That(element.Goals, Is.EqualTo("foo"));
        });
    }
    
        
    [Test]
    public void LearningWorldPresenter_CreateNewLearningElement_AddsLearningElementToSpaceViewModel()
    {
        var learningElementPresenter = new LearningElementPresenter();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        Assert.That(space.LearningElements, Is.Empty);

        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.CreateNewLearningElement("foo", "bar", space,
            "foo", "bar", "foo", "bar", "foo");

        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        var element = space.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("foo"));
            Assert.That(element.Shortname, Is.EqualTo("bar"));
            Assert.That(element.Parent, Is.EqualTo(space));
            Assert.That(element.Type, Is.EqualTo("foo"));
            Assert.That(element.Content, Is.EqualTo("bar"));
            Assert.That(element.Authors, Is.EqualTo("foo"));
            Assert.That(element.Description, Is.EqualTo("bar"));
            Assert.That(element.Goals, Is.EqualTo("foo"));
        });
    }

    #endregion

    #region OnEditSpace/ElementDalogClose

    [Test]
    public void LearningWorldPresenter_OnEditSpaceDialogClose_CallsLearningSpacePresenter()
    {
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.EditLearningSpace(Arg.Any<LearningSpaceViewModel>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LearningSpaceViewModel("ba", "ba", "ba", "ba", "ba"));
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
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = space;


        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);

        learningSpacePresenter.Received().EditLearningSpace(space, "n", "sn", "a", "d", "g");
    }

    [Test]
    public void LearningWorldPresenter_OnEditElementDialogClose_WithLearningWorld_CallsLearningElementPresenter()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("ba", "ba",
            null, "ba", "ba", "ba", "ba", "ba"));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, "bar", "foo",
            "bar", "foo", "bar");
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = "Learning world";
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = "c";
        dictionary["Content"] = "d";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element, "a", "b", world, "c", "d", "e", "f", "g");
    }

    [Test]
    public void LearningWorldPresenter_OnEditElementDialogClose_MovesElementFromWorldToSpace()
    {
        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();
        mockElementPresenter.When(x => x.EditLearningElement(Arg.Any<LearningElementViewModel>(),
            Arg.Any<string>(),Arg.Any<string>(),Arg.Any<LearningSpaceViewModel>(),Arg.Any<string>(),
            Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>())).
            Do(x =>
        {
            var ele = x.Arg<LearningElementViewModel>();
            var space = x.Arg<LearningSpaceViewModel>();
            (((LearningWorldViewModel) ele.Parent!)).LearningElements.Remove(ele);
            space.LearningElements.Add(ele);
        });
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var element = new LearningElementViewModel("z", "y", world, "x", "w", "v", "u", "t");
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "z";
        dictionary["Shortname"] = "y";
        dictionary["Parent"] = "Learning space";
        dictionary["Assignment"] = "a";
        dictionary["Type"] = "x";
        dictionary["Content"] = "w";
        dictionary["Authors"] = "v";
        dictionary["Description"] = "u";
        dictionary["Goals"] = "t";
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: mockElementPresenter);
        systemUnderTest.SetLearningWorld(null,world);
        world.LearningSpaces.Add(space);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;
        
        Assert.That(world.LearningElements, Contains.Item(element));
        Assert.That(space.LearningElements, Is.Empty);

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.That(space.LearningElements, Contains.Item(element));
    }
    

    [Test]
    public void LearningWorldPresenter_OnEditElementDialogClose_WithLearningSpace_CallsLearningElementPresenter()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("ba", "ba",
            null, "ba", "ba", "ba", "ba", "ba"));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, "bar", "foo",
            "bar", "foo", "bar");
        var space = new LearningSpaceViewModel("foobar", "fb", "foo", "bar", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = "Learning space";
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = "c";
        dictionary["Content"] = "d";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.LearningElements.Add(element);
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element, "a", "b", space, "c", "d", "e", "f", "g");
    }
    
    #endregion

    #region DeleteSelectedLearningObject

    [Test]
    public void LearningWorldPresenter_DeleteSelectedLearningObject_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LearningWorldPresenter_DeleteSelectedLearningObject_DoesNotThrowWhenSelectedObjectNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm.SelectedLearningObject, Is.Null);
            Assert.That(systemUnderTest.LearningWorldVm.LearningObjects, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void LearningWorldPresenter_DeleteSelectedLearningObject_WithSpace_DeletesSpaceFromViewModel()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        Assert.That(world.LearningSpaces, Contains.Item(space));
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.LearningSpaces, Is.Empty);
    }

    [Test]
    public void LearningWorldPresenter_DeleteSelectedLearningObject_WithElement_CallsElementPresenter()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("f", "f", world, "f", "f", "f", "f", "f");
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter:mockElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.DeleteSelectedLearningObject();

        mockElementPresenter.Received().RemoveLearningElementFromParentAssignment(element);
    }

    [Test]
    public void LearningWorldPresenter_DeleteSelectedLearningObject_WithUnknownObject_ThrowsNotImplemented()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    [Test]
    public void LearningWorldPresenter_DeleteSelectedLearningObject_WithSpace_MutatesSelectionInViewModel()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        Assert.That(world.SelectedLearningObject, Is.EqualTo(space));

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.SelectedLearningObject, Is.Null);
    }

    [Test]
    public void LearningWorldPresenter_DeleteSelectedLearningObject_WithElement_MutatesSelectionInViewModel()
    {
        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();
        mockElementPresenter.When(x => x.RemoveLearningElementFromParentAssignment(Arg.Any<LearningElementViewModel>())).Do(x =>
        {
            var ele = x.Arg<LearningElementViewModel>();
            (((LearningWorldViewModel) ele.Parent!)).LearningElements.Remove(ele);
        });
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element1 = new LearningElementViewModel("f", "f", world, "f", "f", "f", "f", "f");
        var element2 = new LearningElementViewModel("e", "e", world, "f", "f", "f", "f", "f");
        world.LearningElements.Add(element1);
        world.LearningElements.Add(element2);
        world.SelectedLearningObject = element1;

        Assert.That(world.SelectedLearningObject, Is.EqualTo(element1));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter:mockElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.SelectedLearningObject, Is.EqualTo(element2));
        
        systemUnderTest.DeleteSelectedLearningObject();
        
        Assert.That(world.SelectedLearningObject, Is.Null);
        
    }

    #endregion

    #region OpenEditSelectedLearningObjectDialog

    [Test]
    public void LearningWorldPresenter_OpenEditSelectedLearningObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LearningWorldPresenter_OpenEditSelectedLearningObjectDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm.SelectedLearningObject, Is.EqualTo(null));
            Assert.That(systemUnderTest.LearningWorldVm.LearningObjects, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void LearningWorldPresenter_OpenEditSelectedLearningObjectDialog_WithSpace_CallsMethod()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;
        world.EditDialogInitialValues = new Dictionary<string, string>();
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);
        
        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningSpaceDialogOpen, Is.True);
            Assert.That(world.EditDialogInitialValues["Name"], Is.EqualTo(space.Name));
            Assert.That(world.EditDialogInitialValues["Shortname"], Is.EqualTo(space.Shortname));
            Assert.That(world.EditDialogInitialValues["Authors"], Is.EqualTo(space.Authors));
            Assert.That(world.EditDialogInitialValues["Description"], Is.EqualTo(space.Description));
            Assert.That(world.EditDialogInitialValues["Goals"], Is.EqualTo(space.Goals));
        });
    }

    [Test]
    public void LearningWorldPresenter_OpenEditSelectedLearningObjectDialog_WithElement_CallsMethod()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("n", "sn", world, "t","c","a", "d", "g");
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;
        world.EditDialogInitialValues = new Dictionary<string, string>();
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);
        
        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningElementDialogOpen, Is.True);
            Assert.That(world.EditDialogInitialValues["Name"], Is.EqualTo(element.Name));
            Assert.That(world.EditDialogInitialValues["Shortname"], Is.EqualTo(element.Shortname));
            Assert.That(world.EditDialogInitialValues["Parent"], Is.EqualTo("Learning world"));
            Assert.That(world.EditDialogInitialValues["Assignment"], Is.EqualTo(element.Parent?.Name));
            Assert.That(world.EditDialogInitialValues["Type"], Is.EqualTo(element.Type));
            Assert.That(world.EditDialogInitialValues["Content"], Is.EqualTo(element.Content));
            Assert.That(world.EditDialogInitialValues["Authors"], Is.EqualTo(element.Authors));
            Assert.That(world.EditDialogInitialValues["Description"], Is.EqualTo(element.Description));
            Assert.That(world.EditDialogInitialValues["Goals"], Is.EqualTo(element.Goals));
        });
    }

    [Test]
    public void
        LearningWorldPresenter_OpenEditSelectedLearningObjectDialog_WithUnknownObject_ThrowsNotImplemented()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region SaveSelectedLearningObject

    [Test]
    public void LearningWorldPresenter_SaveSelectedLearningObjectAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LearningWorldPresenter_SaveSelectedLearningObjectAsync_DoesNotThrowWhenSelectedObjectNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningObject is null"));
    }

    [Test]
    public void LearningWorldPresenter_SaveSelectedLearningObject_WithSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.SaveSelectedLearningObjectAsync();

        presentationLogic.Received().SaveLearningSpaceAsync(space);
    }

    [Test]
    public void LearningWorldPresenter_SaveSelectedLearningObject_WithElement_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("f", "f", world, "f", "f", "f",
            "f", "f");
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.SaveSelectedLearningObjectAsync();

        presentationLogic.Received().SaveLearningElementAsync(element);
    }

    [Test]
    public void LearningWorldPresenter_SaveSelectedLearningObject_WithUnknownObjectAsync_ThrowsNotImplemented()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #endregion

    private LearningWorldPresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null,
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, learningElementPresenter, logger);
    }
}