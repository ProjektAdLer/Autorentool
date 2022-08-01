using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningContent;
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
    public void CreateNewLearningWorld_CreatesCorrectViewModel()
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
    public void EditLearningWorld_EditsViewModelCorrectly()
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

    [Test]
    public void SetSelectedLearningObject_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var element = new LearningElementViewModel("foo", "bar",
            null, null, "bar", "foo", "bar", LearningElementDifficultyEnum.Easy, 6);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningObject(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }
        
        
    #region CreateNewLearningSpace/Element

    [Test]
    public void AddLearningSpace_CreateSecondSpaceWithSameName_ThrowsException()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space1 = new LearningSpaceViewModel("foo", "bar", "foo", "bar", "baz");
        var space2 = new LearningSpaceViewModel("foo", "bar", "foo", "bar", "baz");
        world.LearningSpaces.Add(space1);
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningSpace(space2));
        Assert.That(ex!.Message, Is.EqualTo("World already contains a space with same name"));
    }
    
    [Test]
    public void CreateNewLearningSpace_ThrowsWhenSelectedWorldNull()
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
    public void AddLearningElement_CreateSecondElementWithSameName_ThrowsException()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element1 = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        var element2 = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        world.LearningElements.Add(element1);
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningElement(element2));
        Assert.That(ex!.Message, Is.EqualTo("World already contains an element with same name"));
    }
    
    [Test]
    public void AddLearningElement_SelectedLearningWorldIsNull_ThrowsException()
    {
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void CreateNewLearningElement_ThrowsWhenSelectedWorldNull()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("foo", "bar",
            null, null, "bar", "foo", "bar", LearningElementDifficultyEnum.Easy, 6));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewLearningElement("foo",
            "bar", null, ElementTypeEnum.Transfer, ContentTypeEnum.Image, null, "bar", "foo", "bar",LearningElementDifficultyEnum.Easy,6));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void CreateNewLearningSpace_CallsLearningSpacePresenter()
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
    public void CreateNewLearningElement_CallsLearningElementPresenter_TransferElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("foo", "bar", null,
            null, "foo", "bar", "foo",LearningElementDifficultyEnum.Easy,2));
        var parent = new LearningWorldViewModel("foo", "boo", "bla", "blub", "bibi", "bubu");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.CreateNewLearningElement("name", "sn", parent, ElementTypeEnum.Transfer, ContentTypeEnum.Image, content,
            "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);

        learningElementPresenter.Received()
            .CreateNewTransferElement("name", "sn", parent, ContentTypeEnum.Image, content, "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void CreateNewLearningElement_CallsLearningElementPresenter_TestElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTestElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("foo", "bar", null,
            null, "foo", "bar", "foo",LearningElementDifficultyEnum.Easy,2));
        var parent = new LearningWorldViewModel("foo", "boo", "bla", "blub", "bibi", "bubu");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.CreateNewLearningElement("name", "sn", parent, ElementTypeEnum.Test, ContentTypeEnum.H5P, content,
            "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);

        learningElementPresenter.Received()
            .CreateNewTestElement("name", "sn", parent, ContentTypeEnum.H5P, content, "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void CreateNewLearningElement_CallsLearningElementPresenter_InteractionElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewInteractionElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("foo", "bar", null,
            null, "foo", "bar", "foo",LearningElementDifficultyEnum.Easy,2));
        var parent = new LearningWorldViewModel("foo", "boo", "bla", "blub", "bibi", "bubu");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.CreateNewLearningElement("name", "sn", parent, ElementTypeEnum.Interaction, ContentTypeEnum.H5P, content,
            "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);

        learningElementPresenter.Received()
            .CreateNewInteractionElement("name", "sn", parent, ContentTypeEnum.H5P, content, "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void CreateNewLearningElement_CallsLearningElementPresenter_ActivationElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewActivationElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("foo", "bar", null,
            null, "foo", "bar", "foo",LearningElementDifficultyEnum.Easy,2));
        var parent = new LearningWorldViewModel("foo", "boo", "bla", "blub", "bibi", "bubu");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.CreateNewLearningElement("name", "sn", parent, ElementTypeEnum.Activation, ContentTypeEnum.H5P, content,
            "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);

        learningElementPresenter.Received()
            .CreateNewActivationElement("name", "sn", parent, ContentTypeEnum.H5P, content, "cont", "aut", "desc",LearningElementDifficultyEnum.Easy,2);
    }

    [Test]
    public void CreateNewLearningSpace_AddsLearningSpaceToViewModel()
    {
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>(); //new LearningSpacePresenter();
        learningSpacePresenter
            .CreateNewLearningSpace(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(),
                Arg.Any<String>()).Returns(args =>
                new LearningSpaceViewModel((String) args[0], (String) args[1], (String) args[2], (String) args[3],
                    (String) args[4]));
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
    public void CreateNewLearningElement_AddsLearningElementToWorldViewModel()
    {
        var learningElementPresenter = new LearningElementPresenter();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        Assert.That(world.LearningElements, Is.Empty);

        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.CreateNewLearningElement("foo", "bar", world, ElementTypeEnum.Transfer, ContentTypeEnum.Image,
            content, "bar", "foo", "bar",LearningElementDifficultyEnum.Easy,2);

        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        var element = world.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("foo"));
            Assert.That(element.Shortname, Is.EqualTo("bar"));
            Assert.That(element.Parent, Is.EqualTo(world));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("bar"));
            Assert.That(element.Description, Is.EqualTo("foo"));
            Assert.That(element.Goals, Is.EqualTo("bar"));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Easy));
            Assert.That(element.Workload, Is.EqualTo(2));
        });
    }


    [Test]
    public void CreateNewLearningElement_AddsLearningElementToSpaceViewModel()
    {
        var learningElementPresenter = new LearningElementPresenter();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        Assert.That(space.LearningElements, Is.Empty);

        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.CreateNewLearningElement("foo", "bar", space, ElementTypeEnum.Transfer, ContentTypeEnum.Image,
            content, "bar", "foo", "bar",LearningElementDifficultyEnum.Medium,1);

        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        var element = space.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("foo"));
            Assert.That(element.Shortname, Is.EqualTo("bar"));
            Assert.That(element.Parent, Is.EqualTo(space));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("bar"));
            Assert.That(element.Description, Is.EqualTo("foo"));
            Assert.That(element.Goals, Is.EqualTo("bar"));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.Workload, Is.EqualTo(1));
        });
    }

    [Test]
    public void CreateLearningElementWithPreloadedContent_SetsFieldToTrue()
    {
        var learningContent = new LearningContentViewModel("n", "t", Array.Empty<byte>());
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(systemUnderTest.CreateLearningElementDialogOpen, Is.False);
        
        systemUnderTest.CreateLearningElementWithPreloadedContent(learningContent);
        
        Assert.That(systemUnderTest.CreateLearningElementDialogOpen, Is.True);
        
    }

    #endregion

    #region OnCreateSpace/ElementDialogClose

    [Test]
    public void OnCreateSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnCreateElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnCreateSpaceDialogClose_CallsLearningSpacePresenter()
    {
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.CreateNewLearningSpace(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LearningSpaceViewModel("ba", "ba", "ba", "ba", "ba"));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter);
        systemUnderTest.SetLearningWorld(null, world);


        systemUnderTest.OnCreateSpaceDialogClose(returnValueTuple);

        learningSpacePresenter.Received().CreateNewLearningSpace("n", "sn", "a", "d", "g");
    }

    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_WorldParent_TransferElement_Image()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", world, ContentTypeEnum.Image, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_WorldParent_TransferElement_Video()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Video.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", world, ContentTypeEnum.Video, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_WorldParent_TransferElement_Pdf()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Pdf.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", world, ContentTypeEnum.Pdf, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void OnCreateElementDialogClose_allsLearningElementPresenter_WorldParent_TestElement()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTestElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Test.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTestElement("a", "b", world, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_WorldParent_InteractionElement()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewInteractionElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Interaction.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewInteractionElement("a", "b", world, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_WorldParent_ActivationElement()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewActivationElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Activation.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewActivationElement("a", "b", world, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }

    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_SpaceParent()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", "ba", "ba",LearningElementDifficultyEnum.Easy,9));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("foobar", "fb", "foo", "bar", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.Space.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.LearningSpaces.Add(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", space, ContentTypeEnum.Image, null, "e", "f", "g",LearningElementDifficultyEnum.Medium,7);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_DragAndDropContent()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Pdf.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.CreateLearningElementWithPreloadedContent(content);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", world, ContentTypeEnum.Pdf, content, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }

    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseParentType()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = null;
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned parent type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseElementType()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();;
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = "Transfers";
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned element type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseContentType()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();;
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();;
        dictionary["Content"] = "Images";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned content type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseDifficulty()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();;
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();;
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "Easys";
        dictionary["Workload (min)"] = "7";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned difficulty"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_CouldntParseWorkloadToInt()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTestElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Test.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "seven";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTestElement("a", "b", world, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,0);
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativeWorkloadToZero()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTestElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Test.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTestElement("a", "b", world, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,0);
    }

    #endregion

    #region OnEditSpace/ElementDialogClose

    [Test]
    public void OnEditSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }

    [Test]
    public void OnEditElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, null);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditElementDialogClose_ThrowsCouldntParseParentType()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>() , Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", null, "ba", LearningElementDifficultyEnum.Medium,5));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = "worlds";
        dictionary["Assignment"] = "foo";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = element;

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned parent type"));
    }
    
    [Test]
    public void OnEditElementDialogClose_ThrowsCouldntParseDifficulty()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>() , Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", null, "ba", LearningElementDifficultyEnum.Medium,5));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "mediums";
        dictionary["Workload (min)"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = element;

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned difficulty"));
    }
    
    [Test]
    public void OnEditElementDialogClose_CouldntParseWorkloadToInt()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>() , Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", null, "ba", LearningElementDifficultyEnum.Medium,5));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "five";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element,"a", "b", world, "e", "f", "g",LearningElementDifficultyEnum.Easy,0);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativeWorkloadToZero()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>() , Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", null, "ba", LearningElementDifficultyEnum.Medium,5));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-5";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element,"a", "b", world, "e", "f", "g",LearningElementDifficultyEnum.Easy,0);
    }

    [Test]
    public void OnEditSpaceDialogClose_CallsLearningSpacePresenter()
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
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningSpacePresenter: learningSpacePresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = space;


        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);

        learningSpacePresenter.Received().EditLearningSpace(space, "n", "sn", "a", "d", "g");
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
    
    [Test]
    public void OnEditSpaceDialogClose_LearningObjectIsNotALEarningSpace_ThrowsException()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>() , Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", null, "ba", LearningElementDifficultyEnum.Medium,5));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        world.LearningElements.Add(element);
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = element;
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningObject is not a LearningSpace"));
    }

    [Test]
    public void OnEditElementDialogClose_CallsLearningElementPresenter_WorldParent()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>() , Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", null, "ba", LearningElementDifficultyEnum.Medium,5));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element, "a", "b", world, "e", "f", "g", LearningElementDifficultyEnum.Easy, 4);
    }

    [Test]
    public void OnEditElementDialogClose_MovesElementFromWorldToSpace()
    {
        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();
        mockElementPresenter.When(x => x.EditLearningElement(Arg.Any<LearningElementViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningSpaceViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>())).Do(x =>
        {
            var ele = x.Arg<LearningElementViewModel>();
            var space = x.Arg<LearningSpaceViewModel>();
            (((LearningWorldViewModel) ele.Parent!)).LearningElements.Remove(ele);
            space.LearningElements.Add(ele);
        });
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var element = new LearningElementViewModel("z", "y", world, content, "w", "nll", "v", LearningElementDifficultyEnum.Easy,4);
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "z";
        dictionary["Shortname"] = "y";
        dictionary["Parent"] = ElementParentEnum.Space.ToString();
        dictionary["Assignment"] = "a";
        dictionary["Authors"] = "v";
        dictionary["Description"] = "u";
        dictionary["Goals"] = "t";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "8";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: mockElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
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
    public void OnEditElementDialogClose_CallsLearningElementPresenter_SpaceParent()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", "ba", "ba",LearningElementDifficultyEnum.Easy, 3));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 5, 4});
        var element = new LearningElementViewModel("foo", "bar", null, content, "foo",
            "nll", "bar", LearningElementDifficultyEnum.Hard);
        var space = new LearningSpaceViewModel("foobar", "fb", "foo", "bar", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.Space.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);
        world.LearningElements.Add(element);
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element, "a", "b", space, "e", "f", "g", LearningElementDifficultyEnum.Easy,2);
    }
    
    [Test]
    public void OnEditElementDialogClose_ElementParentIsNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.Space.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Parent element is null"));
    }

    #endregion

    #region DeleteSelectedLearningObject

    [Test]
    public void DeleteSelectedLearningObject_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteSelectedLearningObject_DoesNotThrowWhenSelectedObjectNull()
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
    public void DeleteSelectedLearningObject_DeletesSpaceFromViewModel_WithSpace()
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
    public void DeleteSelectedLearningObject_CallsElementPresenter_WithElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("t", "s", new byte[] {4, 3, 2});
        var element = new LearningElementViewModel("f", "f", world, content, "f", "nll", "f", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: mockElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.DeleteSelectedLearningObject();

        mockElementPresenter.Received().RemoveLearningElementFromParentAssignment(element);
    }

    [Test]
    public void DeleteSelectedLearningObject_UnknownObjectThrowsNotImplemented()
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
    public void DeleteSelectedLearningObject_MutatesSelectionInViewModel_WithSpace()
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
    public void DeleteSelectedLearningObject_MutatesSelectionInViewModel_WithElement()
    {
        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();
        mockElementPresenter.When(x => x.RemoveLearningElementFromParentAssignment(Arg.Any<LearningElementViewModel>()))
            .Do(x =>
            {
                var ele = x.Arg<LearningElementViewModel>();
                (((LearningWorldViewModel) ele.Parent!)).LearningElements.Remove(ele);
            });
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content1 = new LearningContentViewModel("r", "s", new byte[] {0x02, 0x01});
        var content2 = new LearningContentViewModel("t", "q", new byte[] {0x03, 0x06});
        var element1 = new LearningElementViewModel("f", "f", world, content1, "f", "f", "f", LearningElementDifficultyEnum.Medium);
        var element2 = new LearningElementViewModel("e", "e", world, content2, "f", "f", "f", LearningElementDifficultyEnum.Medium);
        world.LearningElements.Add(element1);
        world.LearningElements.Add(element2);
        world.SelectedLearningObject = element1;

        Assert.That(world.SelectedLearningObject, Is.EqualTo(element1));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: mockElementPresenter);
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.SelectedLearningObject, Is.EqualTo(element2));

        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.SelectedLearningObject, Is.Null);
    }

    #endregion

    #region OpenEditSelectedLearningObjectDialog

    [Test]
    public void OpenEditSelectedLearningObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OpenEditSelectedLearningObjectDialog_DoesNotThrowWhenSelectedObjectNull()
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
    public void OpenEditSelectedLearningObjectDialog_CallsMethod_WithSpace()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("n", "sn", "a", "d", "g");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OpenEditSelectedLearningObjectDialog();
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
        });
    }

    [Test]
    public void OpenEditSelectedLearningObjectDialog_CallsMethod_WithElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("n", "sn", world, null, "a", "d", "g", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningElementDialogOpen, Is.True);
            Assert.That(systemUnderTest.EditElementDialogInitialValues, Is.Not.Null);
            //overriding nullability because we test for that above - n.stich
            Assert.That(systemUnderTest.EditElementDialogInitialValues!["Name"], Is.EqualTo(element.Name));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Shortname"], Is.EqualTo(element.Shortname));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Parent"], Is.EqualTo(ElementParentEnum.World.ToString()));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Assignment"], Is.EqualTo(element.Parent?.Name));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Authors"], Is.EqualTo(element.Authors));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Description"], Is.EqualTo(element.Description));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Goals"], Is.EqualTo(element.Goals));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Difficulty"], Is.EqualTo(element.Difficulty.ToString()));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Workload (min)"], Is.EqualTo(element.Workload.ToString()));
        });
    }
    
    [Test]
    public void OpenEditSelectedLearningObjectDialog_ElementParentIsNull_ThrowsException()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("n", "sn", null, null, "a", "d", "g", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("Element Parent is null"));
        
    }

    [Test]
    public void
        OpenEditSelectedLearningObjectDialog_ThrowsNotImplemented_WithUnknownObject()
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
    public void SaveSelectedLearningObjectAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void SaveSelectedLearningObjectAsync_DoesNotThrowWhenSelectedObjectNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningObject is null"));
    }

    [Test]
    public void SaveSelectedLearningObject_CallsPresentationLogic_WithSpace()
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
    public void SaveSelectedLearningObject_CallsPresentationLogic_WithElement()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("g", "h", new byte[] {3, 2, 1});
        var element = new LearningElementViewModel("f","f", world,content,"f","f","f", LearningElementDifficultyEnum.Medium);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.SaveSelectedLearningObjectAsync();

        presentationLogic.Received().SaveLearningElementAsync(element);
    }

    [Test]
    public void SaveSelectedLearningObjectAsync_UnknownObjectThrowsNotImplemented()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () =>
            await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region LoadLearningSpace/Element

    [Test]
    public void LoadLearningSpace_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningSpace());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LoadLearningElement_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningElement());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LoadLearningSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.LoadLearningSpace();

        presentationLogic.Received().LoadLearningSpaceAsync();
    }

    [Test]
    public void LoadLearningElement_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningWorld(null, world);
        systemUnderTest.LoadLearningElement();

        presentationLogic.Received().LoadLearningElementAsync();
    }

    [Test]
    public void LoadLearningSpace_AddsLearningSpaceToLearningWorld()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.LoadLearningSpaceAsync().Returns(new LearningSpaceViewModel("n", "sn", "a", "d", "g"));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningWorld(null, world);
        Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
        Assert.That(systemUnderTest.LearningWorldVm?.LearningSpaces, Is.Empty);

        systemUnderTest.LoadLearningSpace();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm?.LearningSpaces.Count, Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningSpaces.First().Name, Is.EqualTo("n"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningSpaces.First().Shortname, Is.EqualTo("sn"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningSpaces.First().Authors, Is.EqualTo("a"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningSpaces.First().Description, Is.EqualTo("d"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningSpaces.First().Goals, Is.EqualTo("g"));
        });
    }

    [Test]
    public void LoadLearningElement_AddsLearningElementToLearningWorld()
    {
        var content = new LearningContentViewModel("a", "b", new byte[] {0x02, 0x01});
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.LoadLearningElementAsync()
            .Returns(new LearningElementViewModel("n", "sn", null, content, "a", "d", "g", LearningElementDifficultyEnum.Easy));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningWorld(null, world);
        Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
        Assert.That(systemUnderTest.LearningWorldVm?.LearningElements, Is.Empty);

        systemUnderTest.LoadLearningElement();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.Count, Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.First().Name, Is.EqualTo("n"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.First().Shortname, Is.EqualTo("sn"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.First().Parent, Is.EqualTo(world));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.First().LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.First().Authors, Is.EqualTo("a"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.First().Description, Is.EqualTo("d"));
            Assert.That(systemUnderTest.LearningWorldVm?.LearningElements.First().Goals, Is.EqualTo("g"));
        });
    }

    #endregion
    
    #region Open/CloseLearningSpaceView

    [Test]
    public void ShowAndCloseLearningSpaceView_OpensAndClosesLearningSpaceView_SetsShowingLearningSpaceViewBool()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningSpace = new LearningSpaceViewModel("a", "b", "c", "d", "e", null);
        world.SelectedLearningObject = learningSpace;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningWorld(null, world);

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.False);
        
        systemUnderTest.ShowSelectedLearningSpaceView();

        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.True);
        
        systemUnderTest.CloseLearningSpaceView();
        
        Assert.That(systemUnderTest.ShowingLearningSpaceView, Is.False);
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