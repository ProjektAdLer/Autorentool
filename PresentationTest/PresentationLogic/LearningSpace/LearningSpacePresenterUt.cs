using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PresentationTest.PresentationLogic.LearningSpace;

[TestFixture]
public class LearningSpacePresenterUt
{
    #region LearningSpace

    #region EditLearningSpace

    [Test]
    public void EditLearningSpace_LearningSpaceVmIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditLearningSpace("a","b","c","d","e"));
        Assert.That(ex!.Message, Is.EqualTo("LearningSpaceVm is null"));
    }

    [Test]
    public void EditLearningSpace_CallsPresentationLogic()
    {
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.EditLearningSpace("space", "b","c","d","e");
        
        presentationLogic.Received().EditLearningSpace(space, "space", "b", "c","d","e");
    }

    #endregion

    #region OnEditSpaceDialogClose

    [Test]
    public void OnEditSpaceDialogClose_CallsPresentationLogic()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var presentationLogic = Substitute.For<IPresentationLogic>();
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        
        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);
        
        presentationLogic.Received().EditLearningSpace(space, "a","b","e","f","g");
    }
    
    [Test]
    public void OnEditSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditSpaceDialogClose_ThrowsWhenSpaceIsNull()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditSpaceDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("LearningSpaceVm is null"));
    }

    #endregion

    #endregion

    #region LearningElement

    [Test]
    public void AddLearningElement_SelectedLearningSpaceIsNull_ThrowsException()
    {
        var element = new LearningElementViewModel("foo", "bar", null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, null);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    #region CreateNewLearningElement

    [Test]
    public void AddNewLearningElement_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateLearningElementDialogOpen);
        
        systemUnderTest.AddNewLearningElement();
        
        Assert.That(systemUnderTest.CreateLearningElementDialogOpen);
    }
    
    [Test]
    public void SetSelectedLearningObject_SelectedLearningSpaceIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var element = new LearningElementViewModel("foo", "bar", null, "bar", "foo", "bar", LearningElementDifficultyEnum.Easy, null, 6);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningObject(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
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

    #region OnCreateElementDialogClose

    [Test]
    public void OnCreateElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsWhenSelectedSpaceNull()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_WithImage()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space,"a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Image, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_WithPdf()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Pdf.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space,"a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Pdf, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_DragAndDropContent()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});
        var element = new LearningElementViewModel("a", "b", null, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.Space.ToString();
        dictionary["Assignment"] = "foobar";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Pdf.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.CreateLearningElementWithPreloadedContent(content);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Pdf, content, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
    }

    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseElementType()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
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
        systemUnderTest.SetLearningSpace(space);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned element type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseContentType()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
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
        systemUnderTest.SetLearningSpace(space);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned content type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseDifficulty()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        
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
        systemUnderTest.SetLearningSpace(space);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned difficulty"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Test.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "seven";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Test, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,0);
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

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
        dictionary["Workload (min)"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Video, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,0);
    }

    #endregion

    #region OnEditElementDialogClose
    
    [Test]
    public void OnEditElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditElementDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null,"bar",
            "foo", "bar", LearningElementDifficultyEnum.Easy, null);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = "Learning Space";
        dictionary["Assignment"] = "foo";
        dictionary["Type"] = "c";
        dictionary["Content"] = "d";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "Medium";
        dictionary["Workload (min)"] = "5";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g", LearningElementDifficultyEnum.Medium, 5);
    }

    [Test]
    public void OnEditElementDialogClose_ThrowsCouldntParseDifficulty()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, null);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "mediums";
        dictionary["Workload (min)"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned difficulty"));
    }
    
    [Test]
    public void OnEditElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, null);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "five";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,0);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, null);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.Space.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-5";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,0);
    }

    [Test]
    public void OnEditElementDialogClose_ElementParentIsNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = ElementParentEnum.Space.ToString();
        dictionary["Assignment"] = "foo";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-5";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Parent element is null"));
    }

    #endregion

    #region DeleteSelectedLearningObject
    
    [Test]
    public void DeleteSelectedLearningObject_ThrowsWhenSelectedSpaceNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void DeleteSelectedLearningObject_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.SelectedLearningObject, Is.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void DeleteSelectedLearningObject_CallsPresentationLogic_WithElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("f", "f", content, "f", "f", "f", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic:mockPresentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.DeleteSelectedLearningObject();

        mockPresentationLogic.Received().DeleteLearningElement(space,element);
    }

    [Test]
    public void DeleteSelectedLearningObject_ThrowsNotImplemented_WithUnknownObject()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        space.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region OpenEditSelectedLearningObjectDialog

    [Test]
    public void OpenEditSelectedLearningObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void OpenEditSelectedLearningObjectDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.EditSelectedLearningObject();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.SelectedLearningObject, Is.EqualTo(null));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void OpenEditSelectedLearningObjectDialog_CallsMethod_WithElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("n", "sn", content,"a", "d", "g", LearningElementDifficultyEnum.Easy,space, 3);
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);
        
        systemUnderTest.EditSelectedLearningObject();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningElementDialogOpen, Is.True);
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Name"], Is.EqualTo(element.Name));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Shortname"], Is.EqualTo(element.Shortname));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Authors"], Is.EqualTo(element.Authors));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Description"], Is.EqualTo(element.Description));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Goals"], Is.EqualTo(element.Goals));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Difficulty"], Is.EqualTo(element.Difficulty.ToString()));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Workload (min)"], Is.EqualTo(element.Workload.ToString()));
        });
    }
    
    [Test]
    public void OpenEditSelectedLearningObjectDialog_ElementParentIsNull_ThrowsException()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("n", "sn", null, "a", "d", "g", LearningElementDifficultyEnum.Easy, null);
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.EditSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Element Parent is null"));
        
    }

    [Test]
    public void
        OpenEditSelectedLearningObjectDialog_ThrowsNotImplemented_WithUnknownObject()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        space.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.EditSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region SaveSelectedLearningObject

    [Test]
    public void SaveSelectedLearningObjectAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void SaveSelectedLearningObjectAsync_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningObject is null"));
    }

    [Test]
    public void SaveSelectedLearningObject_CallsPresentationLogic_WithElement()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("f", "f", content,"f",
            "f", "f", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.SaveSelectedLearningObjectAsync();

        presentationLogic.Received().SaveLearningElementAsync(element);
    }

    [Test]
    public void SaveSelectedLearningObject_ThrowsNotImplemented_WithUnknownObjectAsync()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        space.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion
    
    #region LoadLearningElement

    [Test]
    public void LoadLearningElement_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void LoadLearningElement_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.LoadLearningElementAsync();

        presentationLogic.Received().LoadLearningElementAsync();
    }

    [Test]
    public void LoadLearningElement_AddsLearningElementToLearningWorld()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.LoadLearningElementAsync()
            .Returns(new LearningElementViewModel("n", "sn", null, "a", "d", "g", LearningElementDifficultyEnum.Easy, null));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        Assert.That(systemUnderTest.LearningSpaceVm, Is.Not.Null);
        Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements, Is.Empty);

        systemUnderTest.LoadLearningElementAsync();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements.Count, Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements.First().Name, Is.EqualTo("n"));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements.First().Shortname, Is.EqualTo("sn"));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements.First().Parent, Is.EqualTo(space));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements.First().Authors, Is.EqualTo("a"));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements.First().Description, Is.EqualTo("d"));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements.First().Goals, Is.EqualTo("g"));
        });
    }

    #endregion
    
    #endregion


    private LearningSpacePresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        return new LearningSpacePresenter(presentationLogic, logger);
    }
}