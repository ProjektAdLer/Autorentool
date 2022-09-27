using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.LearningWorld;

[TestFixture]
public class LearningWorldPresenterUt
{
    #region LearningObject

    [Test]
    public void SetSelectedLearningObject_SelectedLearningWorldIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var element = new LearningElementViewModel("foo", "bar", null!, "bar", "foo", "bar", LearningElementDifficultyEnum.Easy, null, 6);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningObject(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }
        
        
    #region CreateNewLearningSpace/Element

    [Test]
    public void AddNewLearningSpace_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateLearningSpaceDialogOpen);
        
        systemUnderTest.AddNewLearningSpace();
        
        Assert.That(systemUnderTest.CreateLearningSpaceDialogOpen);
    }

    [Test]
    public void AddNewLearningElement_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateLearningElementDialogOpen);
        
        systemUnderTest.AddNewLearningElement();
        
        Assert.That(systemUnderTest.CreateLearningElementDialogOpen);
    }
    
    [Test]
    public void AddLearningElement_SelectedLearningWorldIsNull_ThrowsException()
    {
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
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
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>());
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
    public void OnCreateElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            //nullability overridden because required for test - n.stich
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsWhenSelectedWorldNull()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);
        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OnCreateSpaceDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

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

        presentationLogic.Received().CreateLearningSpace(world, "n", "sn", "a", "d", "g", 10);
    }

    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_WithImage()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(world,"a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Image, null!, "d", "e", "f",LearningElementDifficultyEnum.Easy,2, 4);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_WithPdf()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);

        const ModalDialogReturnValue modalDialogReturnValue = ModalDialogReturnValue.Ok;
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(world,"a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Pdf, null!, "d", "e", "f",LearningElementDifficultyEnum.Easy,2, 4);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_DragAndDropContent()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});
        var element = new LearningElementViewModel("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);

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
        dictionary["Points"] = "4";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        systemUnderTest.CreateLearningElementWithPreloadedContent(content);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(world, "a", "b",  ElementTypeEnum.Transfer, ContentTypeEnum.Pdf, content, "d", "e", "f",LearningElementDifficultyEnum.Easy,2, 4);
    }

    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseParentType()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = null!;
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

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
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = "Transfers";
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

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
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = "Images";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

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
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "Easys";
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned difficulty"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.World.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Text.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "seven";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(world, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Text, null!, "d", "e", "f",LearningElementDifficultyEnum.Easy,0, 4);
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);

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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(world,"a", "b", ElementTypeEnum.Test,  ContentTypeEnum.H5P, null!, "d", "e", "f",LearningElementDifficultyEnum.Easy,0, 4);
    }
    
       [Test]
    public void OnCreateElementDialogClose_CouldntParsePointsToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);

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
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "four";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(world, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Video, null!, "d", "e", "f",LearningElementDifficultyEnum.Easy,7, 0);
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativePointsToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);

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
        dictionary["Workload (min)"] = "4";
        dictionary["Points"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(world,"a", "b", ElementTypeEnum.Test,  ContentTypeEnum.H5P, null!, "d", "e", "f",LearningElementDifficultyEnum.Easy,4, 0);
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
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

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
            new ModalDialogOnCloseResult(modalDialogReturnValue, null!);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditElementDialogClose_ThrowsCouldntParseParentType()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = element;

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned parent type"));
    }
    
    [Test]
    public void OnEditElementDialogClose_ThrowsCouldntParseDifficulty()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = element;

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned difficulty"));
    }
    
    [Test]
    public void OnEditElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(world, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,0,4);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(world, element,"a", "b", "e", "f", "g",LearningElementDifficultyEnum.Easy,0,4);
    }
    
        [Test]
    public void OnEditElementDialogClose_CouldntParsePointsToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, world);
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
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "four";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(world, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,5,0);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativePointsToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, world);
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
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(world, element,"a", "b", "e", "f", "g",LearningElementDifficultyEnum.Easy,5,0);
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
        world.SelectedLearningObject = space;


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
    
    [Test]
    public void OnEditSpaceDialogClose_LearningObjectIsNotALEarningSpace_ThrowsException()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "foo",
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

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        world.SelectedLearningObject = element;
        
        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningObject is not a LearningSpace"));
    }

    [Test]
    public void OnEditElementDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 5, 4});
        var element = new LearningElementViewModel("foo", "bar", content, "foo",
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
        dictionary["Points"] = "3";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        world.LearningElements.Add(element);
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g", LearningElementDifficultyEnum.Easy,2,3);
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
        dictionary["Points"] = "3";
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
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void DeleteSelectedLearningObject_DoesNotThrowWhenSelectedObjectNull()
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
            Assert.That(systemUnderTest.LearningWorldVm.LearningObjects, Is.Empty);
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
    public void DeleteSelectedLearningObject_CallsPresentationLogic_WithElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("t", "s", new byte[] {4, 3, 2});
        var element = new LearningElementViewModel("f", "f", content, "f", "nll", "f", LearningElementDifficultyEnum.Easy, world);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: mockPresentationLogic);
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.DeleteSelectedLearningObject();

        mockPresentationLogic.Received().DeleteLearningElement(world,element);
    }

    [Test]
    public void DeleteSelectedLearningObject_UnknownObjectThrowsNotImplemented()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region OpenEditSelectedLearningObjectDialog

    [Test]
    public void OpenEditSelectedLearningObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void OpenEditSelectedLearningObjectDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldVm, Is.Not.Null);
            //nullability overridden because of assert - n.stich
            Assert.That(systemUnderTest.LearningWorldVm!.SelectedLearningObject, Is.EqualTo(null));
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
        systemUnderTest.LearningWorldVm = world;

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
            Assert.That(systemUnderTest.EditSpaceDialogInitialValues["Required Points"], Is.EqualTo(space.RequiredPoints.ToString()));
            
            Assert.That(systemUnderTest.EditSpaceDialogAnnotations, Is.Not.Null);
            Assert.That(systemUnderTest.EditSpaceDialogAnnotations!.Count, Is.EqualTo(1));
            Assert.That(systemUnderTest.EditSpaceDialogAnnotations["Required Points"], Is.EqualTo("/"+space.Points));
        });
    }

    [Test]
    public void OpenEditSelectedLearningObjectDialog_CallsMethod_WithElement()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("n", "sn", null!, "a", "d", "g", LearningElementDifficultyEnum.Easy, world);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

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
        var element = new LearningElementViewModel("n", "sn", null!, "a", "d", "g", LearningElementDifficultyEnum.Easy);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;
        
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
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region SaveSelectedLearningObject

    [Test]
    public void SaveSelectedLearningObjectAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

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
        systemUnderTest.LearningWorldVm = world;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningObject is null"));
    }

    [Test]
    public async Task SaveSelectedLearningObject_CallsPresentationLogic_WithSpace()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningObjectAsync();

        await presentationLogic.Received().SaveLearningSpaceAsync(space);
    }

    [Test]
    public async Task SaveSelectedLearningObject_CallsPresentationLogic_WithElement()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var content = new LearningContentViewModel("g", "h", new byte[] {3, 2, 1});
        var element = new LearningElementViewModel("f","f",content,"f","f","f", LearningElementDifficultyEnum.Medium, world);
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.SaveSelectedLearningObjectAsync();

        await presentationLogic.Received().SaveLearningElementAsync(element);
    }

    [Test]
    public void SaveSelectedLearningObjectAsync_UnknownObjectThrowsNotImplemented()
    {
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = world;

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
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningSpaceAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LoadLearningElement_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.LearningWorldVm = null;

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningElementAsync());
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

    [Test]
    public async Task LoadLearningElement_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        var element = new LearningElementViewModel("a", "b", null!, "d", "e", "f", LearningElementDifficultyEnum.Medium, world);
        world.LearningElements.Add(element);
        
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.LearningWorldVm = world;
        await systemUnderTest.LoadLearningElementAsync();

        await presentationLogic.Received().LoadLearningElementAsync(world);
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