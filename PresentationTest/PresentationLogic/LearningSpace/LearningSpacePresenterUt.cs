using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
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

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditLearningSpace("a","b","c","d","e", 5));
        Assert.That(ex!.Message, Is.EqualTo("LearningSpaceVm is null"));
    }

    [Test]
    public void EditLearningSpace_CallsPresentationLogic()
    {
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.EditLearningSpace("space", "b","c","d","e", 5);
        
        presentationLogic.Received().EditLearningSpace(space, "space", "b", "c","d","e", 5);
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
        dictionary["Required Points"] = "5";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        
        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);
        
        presentationLogic.Received().EditLearningSpace(space, "a","b","e","f","g", 5);
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
        var element = new LearningElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }
    
    [Test]
    public void AddLearningElement_CallsPresentationLogic()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", "");
        var element = new LearningElementViewModel("f", "f", content, "url","f", "f", "f", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.SelectedLearningElement = element;

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic:mockPresentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.AddLearningElement(element);

        mockPresentationLogic.Received().AddLearningElement(space,element);
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
    public void SetSelectedLearningElement_SelectedLearningSpaceIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var element = new LearningElementViewModel("foo", "bar", null!, "url","bar", "foo", "bar", LearningElementDifficultyEnum.Easy, null, 6);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void CreateLearningElementWithPreloadedContent_SetsFieldToTrue()
    {
        var learningContent = new LearningContentViewModel("n", "t", "");
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
        dictionary["Points"] = "4";
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
        var element = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space,"a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Image, null!, "url", "e", "f", "g", LearningElementDifficultyEnum.Easy,3,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_WithPdf()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.PDF.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space,"a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.PDF, null!, "url", "e", "f", "g", LearningElementDifficultyEnum.Easy,3,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_DragAndDropContent()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("a", "b", "");
        var element = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.PDF.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        dictionary["Points"] = "4";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.CreateLearningElementWithPreloadedContent(content);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.PDF, content, "url","d", "e", "f",LearningElementDifficultyEnum.Easy,2,4, Arg.Any<double>(), Arg.Any<double>());
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
        dictionary["Points"] = "4";
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
        systemUnderTest.SetLearningSpace(space);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned content type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_NoDifficultySpecified_SetsDifficultyToNone()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "";
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        
        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);
        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Image, null!, "url","e", "f", "g",LearningElementDifficultyEnum.None,7,4);

    }
    
    [Test]
    public void OnCreateElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Test.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "seven";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Test, ContentTypeEnum.H5P, null!, "url","d", "e", "f",LearningElementDifficultyEnum.Easy,0,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Text.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-4";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Text, null!, "url","d", "e", "f",LearningElementDifficultyEnum.Easy,0,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_CouldntParsePointsToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Test.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "four";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Test, ContentTypeEnum.H5P, null!, "url","d", "e", "f",LearningElementDifficultyEnum.Easy,7,0, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativePointsToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null!, "url","c", "d", "e", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Url"] = "url";
        dictionary["Authors"] = "d";
        dictionary["Description"] = "e";
        dictionary["Goals"] = "f";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "4";
        dictionary["Points"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateLearningElement(space, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.H5P, null!, "url","d", "e", "f",LearningElementDifficultyEnum.Easy,4,0, Arg.Any<double>(), Arg.Any<double>());
    }

    #endregion

    #region OnEditElementDialogClose
    
    [Test]
    public void OnEditElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.SetSelectedLearningElement(element);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditElementDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null!,"url","bar",
            "foo", "bar", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = "c";
        dictionary["Content"] = "d";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "Medium";
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "4";
        
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g", LearningElementDifficultyEnum.Medium, 5,4);
    }

    [Test]
    public void OnEditElementDialogClose_TNoDifficultySpecified_SetsDifficultyToNone()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningElement = element;
        
        systemUnderTest.OnEditElementDialogClose(returnValueTuple);
        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g", LearningElementDifficultyEnum.None, 4,4);
    }
    
    [Test]
    public void OnEditElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
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
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,0,4);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-5";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,0,4);
    }
    
    [Test]
    public void OnEditElementDialogClose_CouldntParsePointsToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, space);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "four";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,5,0);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativePointsToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard, space);
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditLearningElement(space, element, "a", "b",  "e", "f", "g",LearningElementDifficultyEnum.Easy,5,0);
    }

    [Test]
    public void OnEditElementDialogClose_ElementParentIsNull_ThrowsException()
    {
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-5";
        dictionary["Points"] = "-4";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Parent element is null"));
    }

    #endregion

    #region DeleteSelectedLearningElement
    
    [Test]
    public void DeleteSelectedLearningElement_ThrowsWhenSelectedSpaceNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningElement());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void DeleteSelectedLearningElement_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.SelectedLearningElement, Is.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningElement());
        });
    }

    [Test]
    public void DeleteSelectedLearningElement_CallsPresentationLogic_WithElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", "");
        var element = new LearningElementViewModel("f", "f", content, "url","f", "f", "f", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.SelectedLearningElement = element;

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic:mockPresentationLogic);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.DeleteSelectedLearningElement();

        mockPresentationLogic.Received().DeleteLearningElement(space,element);
    }

    #endregion

    #region OpenEditSelectedLearningElementDialog

    [Test]
    public void OpenEditSelectedLearningElementDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedLearningElement());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void OpenEditSelectedLearningElementDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.EditSelectedLearningElement();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.SelectedLearningElement, Is.EqualTo(null));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningElement());
        });
    }

    [Test]
    public void OpenEditSelectedLearningElementDialog_CallsMethod_WithElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", "");
        var element = new LearningElementViewModel("n", "sn", content,"url","a", "d", "g", LearningElementDifficultyEnum.Easy,space, 3);
        space.LearningElements.Add(element);
        space.SelectedLearningElement = element;
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);
        
        systemUnderTest.EditSelectedLearningElement();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningElementDialogOpen, Is.True);
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues!["Name"], Is.EqualTo(element.Name));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Shortname"], Is.EqualTo(element.Shortname));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Authors"], Is.EqualTo(element.Authors));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Description"], Is.EqualTo(element.Description));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Goals"], Is.EqualTo(element.Goals));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Difficulty"], Is.EqualTo(element.Difficulty.ToString()));
            Assert.That(systemUnderTest.EditLearningElementDialogInitialValues["Workload (min)"], Is.EqualTo(element.Workload.ToString()));
        });
    }
    
    [Test]
    public void OpenEditSelectedLearningElementDialog_ElementParentIsNull_ThrowsException()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("n", "sn", null!, "url","a", "d", "g", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);
        space.SelectedLearningElement = element;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.EditSelectedLearningElement());
        Assert.That(ex!.Message, Is.EqualTo("Element Parent is null"));
        
    }

    #endregion

    #region SaveSelectedLearningElement

    [Test]
    public void SaveSelectedLearningElementAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void SaveSelectedLearningElementAsync_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningElement is null"));
    }

    [Test]
    public async Task SaveSelectedLearningElement_CallsPresentationLogic_WithElement()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", "");
        var element = new LearningElementViewModel("f", "f", content,"url","f",
            "f", "f", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.SelectedLearningElement = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        await systemUnderTest.SaveSelectedLearningElementAsync();

        await presentationLogic.Received().SaveLearningElementAsync(element);
    }

    #endregion
    
    #region LoadLearningElement

    [Test]
    public void LoadLearningElementAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadLearningElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public async Task LoadLearningElementAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("a", "b", null!, "url","d", "e", "f", LearningElementDifficultyEnum.Medium, space);
        space.LearningElements.Add(element);
        
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        await systemUnderTest.LoadLearningElementAsync();

        await presentationLogic.Received().LoadLearningElementAsync(space);
    }

    #endregion
    
    #endregion


    private LearningSpacePresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILogger<LearningSpacePresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= Substitute.For<ILogger<LearningSpacePresenter>>();
        return new LearningSpacePresenter(presentationLogic, logger);
    }
}