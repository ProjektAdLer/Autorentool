using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.Space.SpaceLayout;
using Shared;

namespace PresentationTest.PresentationLogic.Space;

[TestFixture]
public class SpacePresenterUt
{
    #region Space

    #region EditSpace

    [Test]
    public void EditSpace_SpaceVmIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSpace("a","b","c","d","e", 5));
        Assert.That(ex!.Message, Is.EqualTo("SpaceVm is null"));
    }

    [Test]
    public void EditSpace_CallsPresentationLogic()
    {
        var space = new SpaceViewModel("a", "b", "c", "d", "e");
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        
        systemUnderTest.SetSpace(space);
        systemUnderTest.EditSpace("space", "b","c","d","e", 5);
        
        presentationLogic.Received().EditSpace(space, "space", "b", "c","d","e", 5);
    }

    #endregion

    #region OnEditSpaceDialogClose

    [Test]
    public void OnEditSpaceDialogClose_CallsPresentationLogic()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");
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
        systemUnderTest.SetSpace(space);
        
        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);
        
        presentationLogic.Received().EditSpace(space, "a","b","e","f","g", 5);
    }
    
    [Test]
    public void OnEditSpaceDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);

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
        Assert.That(ex!.Message, Is.EqualTo("SpaceVm is null"));
    }

    #endregion

    #endregion

    #region Element

    [Test]
    public void AddElement_SelectedSpaceIsNull_ThrowsException()
    {
        var element = new ElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", ElementDifficultyEnum.Hard);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.AddElement(element, 0));
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }
    
    [Test]
    public void AddElement_CallsPresentationLogic()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var content = new ContentViewModel("bar", "foo", "");
        var element = new ElementViewModel("f", "f", content, "url","f", "f", "f", ElementDifficultyEnum.Easy, space);
        space.SpaceLayout.PutElement(0, element);
        space.SelectedElement = element;

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic:mockPresentationLogic);
        systemUnderTest.SetSpace(space);

        systemUnderTest.AddElement(element, 1);

        mockPresentationLogic.Received().AddElement(space, 1, element);
    }
    
    [Test]
    public void DragElement_CallsPresentationLogic()
    {
        var element = new ElementViewModel("g", "g", null!, "g", "g", "g", "g", ElementDifficultyEnum.Easy);
        double oldPositionX = 5;
        double oldPositionY = 7;
        var presentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.DragElement(element, new DraggedEventArgs<IElementViewModel>(element, oldPositionX, oldPositionY));

        presentationLogic.Received().DragElement(element, oldPositionX, oldPositionY);
    }

    #region CreateNewElement

    [Test]
    public void AddNewElement_SetsFieldToTrue()
    {
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(!systemUnderTest.CreateElementDialogOpen);
        
        systemUnderTest.AddNewElement();
        
        Assert.That(systemUnderTest.CreateElementDialogOpen);
    }
    
    [Test]
    public void SetSelectedElement_SelectedSpaceIsNull_ThrowsException()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var element = new ElementViewModel("foo", "bar", null!, "url","bar", "foo", "bar", ElementDifficultyEnum.Easy, null, 6);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedElement(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }

    [Test]
    public void CreateElementWithPreloadedContent_SetsFieldToTrue()
    {
        var content = new ContentViewModel("n", "t", "");
        var systemUnderTest = CreatePresenterForTesting();
        
        Assert.That(systemUnderTest.CreateElementDialogOpen, Is.False);
        
        systemUnderTest.CreateElementWithPreloadedContent(content);
        
        Assert.That(systemUnderTest.CreateElementDialogOpen, Is.True);
        
    }

    #endregion

    #region OnCreateElementDialogClose

    [Test]
    public void OnCreateElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_WithImage()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        systemUnderTest.AddNewElement(1);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateElement(space,1, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Image, null!, "url", "e", "f", "g", ElementDifficultyEnum.Easy,3,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_WithPdf()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        systemUnderTest.AddNewElement(1);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateElement(space,1, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.PDF, null!, "url", "e", "f", "g", ElementDifficultyEnum.Easy,3,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsPresentationLogic_DragAndDropContent()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var content = new ContentViewModel("a", "b", "");
        var element = new ElementViewModel("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "2";
        dictionary["Points"] = "4";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        systemUnderTest.CreateElementWithPreloadedContent(content);
        systemUnderTest.AddNewElement(1);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateElement(space, 1, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.PDF, content, "url","d", "e", "f",ElementDifficultyEnum.Easy,2,4, Arg.Any<double>(), Arg.Any<double>());
    }

    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseElementType()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = "Transfers";
        dictionary["Content"] = ContentTypeEnum.Image.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = ElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned element type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_ThrowsCouldntParseContentType()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");
        
        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = "Images";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = ElementDifficultyEnum.Medium.ToString();
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);
        
        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Couldn't parse returned content type"));
    }
    
    [Test]
    public void OnCreateElementDialogClose_NoDifficultySpecified_SetsDifficultyToNone()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");
        
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
        systemUnderTest.SetSpace(space);
        systemUnderTest.AddNewElement(1);
        
        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);
        presentationLogic.Received().CreateElement(space, 1, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Image, null!, "url","e", "f", "g",ElementDifficultyEnum.None,7,4);

    }
    
    [Test]
    public void OnCreateElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "seven";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        systemUnderTest.AddNewElement(1);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateElement(space, 1, "a", "b", ElementTypeEnum.Test, ContentTypeEnum.H5P, null!, "url","d", "e", "f",ElementDifficultyEnum.Easy,0,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-4";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        systemUnderTest.AddNewElement(1);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateElement(space, 1, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.Text, null!, "url","d", "e", "f",ElementDifficultyEnum.Easy,0,4, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_CouldntParsePointsToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "7";
        dictionary["Points"] = "four";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        systemUnderTest.AddNewElement(1);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateElement(space, 1, "a", "b", ElementTypeEnum.Test, ContentTypeEnum.H5P, null!, "url","d", "e", "f",ElementDifficultyEnum.Easy,7,0, Arg.Any<double>(), Arg.Any<double>());
    }
    
    [Test]
    public void OnCreateElementDialogClose_NegativePointsToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "4";
        dictionary["Points"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        systemUnderTest.AddNewElement(1);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        presentationLogic.Received().CreateElement(space, 1, "a", "b", ElementTypeEnum.Transfer, ContentTypeEnum.H5P, null!, "url","d", "e", "f",ElementDifficultyEnum.Easy,4,0, Arg.Any<double>(), Arg.Any<double>());
    }

    #endregion

    #region OnEditElementDialogClose
    
    [Test]
    public void OnEditElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", ElementDifficultyEnum.Hard);
        space.SpaceLayout.PutElement(0, element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);
        systemUnderTest.SetSelectedElement(element);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void OnEditElementDialogClose_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("foo", "bar", null!,"url","bar",
            "foo", "bar", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = "c";
        dictionary["Content"] = "d";
        dictionary["Url"] = "Google.com";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "Medium";
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "4";
        
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        space.SelectedElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditElement(space, element, "a", "b",  "Google.com", "e", 
            "f", "g", ElementDifficultyEnum.Medium, 5,4);
    }

    [Test]
    public void OnEditElementDialogClose_TNoDifficultySpecified_SetsDifficultyToNone()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", ElementDifficultyEnum.Hard);
        space.SpaceLayout.PutElement(0, element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Url"] = "Google.com";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = "mediums";
        dictionary["Workload (min)"] = "4";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        space.SelectedElement = element;
        
        systemUnderTest.OnEditElementDialogClose(returnValueTuple);
        presentationLogic.Received().EditElement(space, element, "a", "b", "Google.com", "e", "f", "g", ElementDifficultyEnum.None, 4,4);
    }
    
    [Test]
    public void OnEditElementDialogClose_CouldntParseWorkloadToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", ElementDifficultyEnum.Hard);
        space.SpaceLayout.PutElement(0, element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Url"] = "Google.com";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "five";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        space.SelectedElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditElement(space, element, "a", "b",  "Google.com", 
            "e", "f", "g",ElementDifficultyEnum.Easy,0,4);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativeWorkloadToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", ElementDifficultyEnum.Hard);
        space.SpaceLayout.PutElement(0, element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Url"] = "Google.com";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-5";
        dictionary["Points"] = "4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        space.SelectedElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditElement(space, element, "a", "b",  "Google.com", "e", 
            "f", "g",ElementDifficultyEnum.Easy,0,4);
    }
    
    [Test]
    public void OnEditElementDialogClose_CouldntParsePointsToInt()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", ElementDifficultyEnum.Hard, space);
        space.SpaceLayout.PutElement(0, element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Url"] = "Google.com";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "four";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        space.SelectedElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditElement(space, element, "a", "b",  "Google.com", "e", 
            "f", "g",ElementDifficultyEnum.Easy,5,0);
    }
    
    [Test]
    public void OnEditElementDialogClose_NegativePointsToZero()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("foo", "bar", null!, "url","foo",
            "wa", "bar", ElementDifficultyEnum.Hard, space);
        space.SpaceLayout.PutElement(0, element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Url"] = "Google.com";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "5";
        dictionary["Points"] = "-4";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);
        systemUnderTest.SetSpace(space);
        space.SelectedElement = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        presentationLogic.Received().EditElement(space, element, "a", "b",  "Google.com", "e", 
            "f", "g",ElementDifficultyEnum.Easy,5,0);
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
        dictionary["Difficulty"] = ElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "-5";
        dictionary["Points"] = "-4";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting();
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.OnEditElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("Parent element is null"));
    }

    #endregion

    #region DeleteSelectedElement
    
    [Test]
    public void DeleteSelectedElement_ThrowsWhenSelectedSpaceNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(null!);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedElement());
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }

    [Test]
    public void DeleteSelectedElement_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.SpaceVm?.SelectedElement, Is.Null);
            Assert.That(systemUnderTest.SpaceVm?.ContainedElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedElement());
        });
    }

    [Test]
    public void DeleteSelectedElement_CallsPresentationLogic_WithElement()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var content = new ContentViewModel("bar", "foo", "");
        var element = new ElementViewModel("f", "f", content, "url","f", "f", "f", ElementDifficultyEnum.Easy, space);
        space.SpaceLayout.PutElement(0, element);
        space.SelectedElement = element;

        var mockPresentationLogic = Substitute.For<IPresentationLogic>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic:mockPresentationLogic);
        systemUnderTest.SetSpace(space);

        systemUnderTest.DeleteSelectedElement();

        mockPresentationLogic.Received().DeleteElement(space,element);
    }

    #endregion

    #region OpenEditSelectedElementDialog

    [Test]
    public void OpenEditSelectedElementDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(null!);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedElement());
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }

    [Test]
    public void OpenEditSelectedElementDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);

        systemUnderTest.EditSelectedElement();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.SpaceVm?.SelectedElement, Is.EqualTo(null));
            Assert.That(systemUnderTest.SpaceVm?.ContainedElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedElement());
        });
    }

    [Test]
    public void OpenEditSelectedElementDialog_CallsMethod_WithElement()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var content = new ContentViewModel("bar", "foo", "");
        var element = new ElementViewModel("n", "sn", content,"url","a", "d", "g", ElementDifficultyEnum.Easy,space, 3);
        space.SpaceLayout.PutElement(0, element);
        space.SelectedElement = element;
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);
        
        systemUnderTest.EditSelectedElement();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditElementDialogOpen, Is.True);
            Assert.That(systemUnderTest.EditElementDialogInitialValues, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditElementDialogInitialValues!["Name"], Is.EqualTo(element.Name));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Shortname"], Is.EqualTo(element.Shortname));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Authors"], Is.EqualTo(element.Authors));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Description"], Is.EqualTo(element.Description));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Goals"], Is.EqualTo(element.Goals));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Difficulty"], Is.EqualTo(element.Difficulty.ToString()));
            Assert.That(systemUnderTest.EditElementDialogInitialValues["Workload (min)"], Is.EqualTo(element.Workload.ToString()));
        });
    }
    
    [Test]
    public void OpenEditSelectedElementDialog_ElementParentIsNull_ThrowsException()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("n", "sn", null!, "url","a", "d", "g", ElementDifficultyEnum.Easy);
        space.SpaceLayout.PutElement(0, element);
        space.SelectedElement = element;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);
        
        var ex = Assert.Throws<Exception>(() => systemUnderTest.EditSelectedElement());
        Assert.That(ex!.Message, Is.EqualTo("Element Parent is null"));
        
    }

    #endregion

    #region SaveSelectedElement

    [Test]
    public void SaveSelectedElementAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }

    [Test]
    public void SaveSelectedElementAsync_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedElementAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedElement is null"));
    }

    [Test]
    public async Task SaveSelectedElement_CallsPresentationLogic_WithElement()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var content = new ContentViewModel("bar", "foo", "");
        var element = new ElementViewModel("f", "f", content,"url","f",
            "f", "f", ElementDifficultyEnum.Easy, space);
        space.SpaceLayout.PutElement(0, element);
        space.SelectedElement = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetSpace(space);
        await systemUnderTest.SaveSelectedElementAsync();

        await presentationLogic.Received().SaveElementAsync(element);
    }

    #endregion
    
    #region ShowSelectedElementContent

    [Test]
    public void ShowSelectedElementContent_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.ShowSelectedElementContentAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }

    [Test]
    public void ShowSelectedElementContent_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(space);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.ShowSelectedElementContentAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedElement is null"));
    }

    [Test]
    public async Task ShowSelectedElementContent_CallsPresentationLogic_WithElement()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var content = new ContentViewModel("bar", "foo", "");
        var element = new ElementViewModel("f", "f", content,"url","f",
            "f", "f", ElementDifficultyEnum.Easy, space);
        space.SpaceLayout.PutElement(0, element);
        space.SelectedElement = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetSpace(space);
        await systemUnderTest.ShowSelectedElementContentAsync();

        await presentationLogic.Received().ShowElementContentAsync(element);
    }
    
    #endregion
    
    #region LoadElement

    [Test]
    public void LoadElementAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetSpace(null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await systemUnderTest.LoadElementAsync(1));
        Assert.That(ex!.Message, Is.EqualTo("SelectedSpace is null"));
    }

    [Test]
    public async Task LoadElementAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new SpaceViewModel("foo", "foo", "foo", "foo", "foo", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var element = new ElementViewModel("a", "b", null!, "url","d", "e", "f", ElementDifficultyEnum.Medium, space);
        space.SpaceLayout.PutElement(0, element);
        
        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetSpace(space);
        await systemUnderTest.LoadElementAsync(1);

        await presentationLogic.Received().LoadElementAsync(space, 1);
    }

    #endregion
    
    #endregion


    private SpacePresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILogger<SpacePresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= Substitute.For<ILogger<SpacePresenter>>();
        return new SpacePresenter(presentationLogic, logger);
    }
}