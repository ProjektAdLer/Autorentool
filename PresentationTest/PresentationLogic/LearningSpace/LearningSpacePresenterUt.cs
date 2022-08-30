using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace PresentationTest.PresentationLogic.LearningSpace;

[TestFixture]
public class LearningSpacePresenterUt
{
    #region LearningSpace
    
    [Test]
    public void CreateNewLearningSpace_CreatesCorrectViewModel()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var name = "a";
        var shortname = "b";
        var authors = "d";
        var description = "e";
        var goals = "f";

        var space = systemUnderTest.CreateNewLearningSpace(name, shortname, authors, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo(name));
            Assert.That(space.Shortname, Is.EqualTo(shortname));
            Assert.That(space.Authors, Is.EqualTo(authors));
            Assert.That(space.Description, Is.EqualTo(description));
            Assert.That(space.Goals, Is.EqualTo(goals));
        });
    }

    [Test]
    public void EditLearningSpace_EditsViewModelCorrectly()
    {
        var systemUnderTest = CreatePresenterForTesting();
        ILearningSpaceViewModel space = new LearningSpaceViewModel("a", "b", "c", "d", "e");

        var name = "space1";
        var shortname = "sp";
        var authors = "marvin";
        var description = "room full of elements";
        var goals = "learning";

        space = systemUnderTest.EditLearningSpace(space, name, shortname, authors, description, goals);
        
        Assert.Multiple(() =>
        {
         Assert.That(space.Name, Is.EqualTo(name));   
         Assert.That(space.Shortname, Is.EqualTo(shortname));   
         Assert.That(space.Authors, Is.EqualTo(authors));   
         Assert.That(space.Description, Is.EqualTo(description));   
         Assert.That(space.Goals, Is.EqualTo(goals));   
        });

    }
    
    #region OnEditSpaceDialogClose
    
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
    
    [Test]
    public void OnEditSpaceDialogClose_EditsLearningSpace()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

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
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("a"));
            Assert.That(space.Shortname, Is.EqualTo("b"));
            Assert.That(space.Authors, Is.EqualTo("e"));
            Assert.That(space.Description, Is.EqualTo("f"));
            Assert.That(space.Goals, Is.EqualTo("g"));
        });
    }

    #endregion

    #endregion

    #region LearningElement

    [Test]
    public void AddLearningElement_SelectedLearningSpaceIsNull_ThrowsException()
    {
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);

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
        var element = new LearningElementViewModel("foo", "bar",
            null, null, "bar", "foo", "bar", LearningElementDifficultyEnum.Easy, 6);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SetSelectedLearningObject(element));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void CreateNewLearningElement_ThrowsWhenSelectedSpaceNull()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>() ,Arg.Any<int>()
            ).Returns(new LearningElementViewModel("foo", "bar",
            null, null, "foo", "foo", "bar", LearningElementDifficultyEnum.Easy, 6));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewLearningElement("foo",
            "bar", null, ElementTypeEnum.Transfer, ContentTypeEnum.Image, null, "bar", "foo", "bar", LearningElementDifficultyEnum.Easy, 6));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void CreateNewLearningElement_CallsLearningElementPresenter_TransferElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
            ).Returns(new LearningElementViewModel("foo", "bar", null,
            null,"foo", "bar", "foo",  LearningElementDifficultyEnum.Easy,8));
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.CreateNewLearningElement("name", "sn", space, ElementTypeEnum.Transfer, ContentTypeEnum.Image, content, 
            "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);

        learningElementPresenter.Received()
            .CreateNewTransferElement("name", "sn", space, ContentTypeEnum.Image, content, "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);
    }
    
    [Test]
    public void CreateNewLearningElement_CallsLearningElementPresenter_TestElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTestElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("foo", "bar", null,
            null,"foo", "bar", "foo",  LearningElementDifficultyEnum.Easy,8));
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.CreateNewLearningElement("name", "sn", space, ElementTypeEnum.Test, ContentTypeEnum.Image, content, 
            "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);

        learningElementPresenter.Received()
            .CreateNewTestElement("name", "sn", space, ContentTypeEnum.Image, content, "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);
    }
    
    [Test]
    public void CreateNewLearningElement_CallsLearningElementPresenter_InteractionElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewInteractionElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("foo", "bar", null,
            null,"foo", "bar", "foo",  LearningElementDifficultyEnum.Easy,8));
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.CreateNewLearningElement("name", "sn", space, ElementTypeEnum.Interaction, ContentTypeEnum.Image, content, 
            "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);

        learningElementPresenter.Received()
            .CreateNewInteractionElement("name", "sn", space, ContentTypeEnum.Image, content, "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);
    }
    
    [Test]
    public void CreateNewLearningElement_CallsLearningElementPresenter_ActivationElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewActivationElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("foo", "bar", null,
            null,"foo", "bar", "foo",  LearningElementDifficultyEnum.Easy,8));
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.CreateNewLearningElement("name", "sn", space, ElementTypeEnum.Activation, ContentTypeEnum.Image, content, 
            "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);

        learningElementPresenter.Received()
            .CreateNewActivationElement("name", "sn", space, ContentTypeEnum.Image, content, "cont", "aut", "desc",  LearningElementDifficultyEnum.Easy,8);
    }

    [Test]
    public void CreateNewLearningElement_AddsLearningElementToSpaceViewModel()
    {
        var learningElementPresenter = new LearningElementPresenter();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});


        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        Assert.That(space.LearningElements, Is.Empty);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.CreateNewLearningElement("foo", "bar", space,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, content, "bar", "foo", "bar",  LearningElementDifficultyEnum.Easy,3);

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
            Assert.That(element.Workload, Is.EqualTo(3));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Easy));
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
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_TransferElement_Image()
    {
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, content, "ba", "ba", "ba",  LearningElementDifficultyEnum.Easy, 6));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

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

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", space, ContentTypeEnum.Image, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_TransferElement_Video()
    {
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, content, "ba", "ba", "ba",  LearningElementDifficultyEnum.Easy, 6));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Transfer.ToString();
        dictionary["Content"] = ContentTypeEnum.Video.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", space, ContentTypeEnum.Video, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_TransferElement_Pdf()
    {
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTransferElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, content, "ba", "ba", "ba",  LearningElementDifficultyEnum.Easy, 6));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

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
        var returnValueTuple = new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", space, ContentTypeEnum.Pdf, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_TestElement()
    {
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTestElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, content, "ba", "ba", "ba",  LearningElementDifficultyEnum.Easy, 6));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Test.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTestElement("a", "b", space, ContentTypeEnum.H5P, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_InteractionElement()
    {
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewInteractionElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, content, "ba", "ba", "ba",  LearningElementDifficultyEnum.Easy, 6));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Interaction.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewInteractionElement("a", "b", space, ContentTypeEnum.H5P, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
    }
    
    [Test]
    public void OnCreateElementDialogClose_CallsLearningElementPresenter_ActivationElement()
    {
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewActivationElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),Arg.Any<int>()
            ).Returns(new LearningElementViewModel("ba", "ba",
            null, content, "ba", "ba", "ba",  LearningElementDifficultyEnum.Easy, 6));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Type"] = ElementTypeEnum.Activation.ToString();
        dictionary["Content"] = ContentTypeEnum.H5P.ToString();
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        dictionary["Difficulty"] = LearningElementDifficultyEnum.Easy.ToString();
        dictionary["Workload (min)"] = "3";
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewActivationElement("a", "b", space, ContentTypeEnum.H5P, null, "e", "f", "g", LearningElementDifficultyEnum.Easy,3);
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
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("a", "b", new byte[] {0, 1, 2});

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

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.CreateLearningElementWithPreloadedContent(content);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTransferElement("a", "b", space, ContentTypeEnum.Pdf, content, "d", "e", "f",LearningElementDifficultyEnum.Easy,2);
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
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewTestElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<ContentTypeEnum>(), Arg.Any<LearningContentViewModel>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<int>()
        ).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba",  "ba", "ba",LearningElementDifficultyEnum.Easy,3));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

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

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTestElement("a", "b", space, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,0);
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
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

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
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewTestElement("a", "b", space, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,0);
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
    public void OnEditElementDialogClose_CallsLearningElementPresenter()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<LearningElementDifficultyEnum>(),
            Arg.Any<int>()).Returns(new LearningElementViewModel("ba", "ba",
            null, null, "ba", "ba", "ba",  LearningElementDifficultyEnum.Easy,8));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null,"bar",
            "foo", "bar", LearningElementDifficultyEnum.Easy);
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

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element, "a", "b", space, "e", "f", "g", LearningElementDifficultyEnum.Medium, 5);
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
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
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
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

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
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
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
        var returnValueTuple =
            new ModalDialogOnCloseResult(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element,"a", "b", space, "e", "f", "g",LearningElementDifficultyEnum.Easy,0);
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
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, null, "foo",
            "wa", "bar", LearningElementDifficultyEnum.Hard);
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

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element,"a", "b", space, "e", "f", "g",LearningElementDifficultyEnum.Easy,0);
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
    public void DeleteSelectedLearningObject_CallsElementPresenter_WithElement()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("f", "f", space, content, "f", "f", "f", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;

        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter:mockElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.DeleteSelectedLearningObject();

        mockElementPresenter.Received().RemoveLearningElementFromParentAssignment(element);
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

    [Test]
    public void DeleteSelectedLearningObject_MutatesSelectionInViewModel_WithElement()
    {
        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();
        mockElementPresenter.When(x => x.RemoveLearningElementFromParentAssignment(Arg.Any<LearningElementViewModel>())).Do(x =>
        {
            var ele = x.Arg<LearningElementViewModel>();
            (((LearningSpaceViewModel) ele.Parent!)).LearningElements.Remove(ele);
        });
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var content1 = new LearningContentViewModel("bare", "foof", new byte[] {0x01, 0x02});
        var content2 = new LearningContentViewModel("bars", "foos", new byte[] {0x04, 0x03});
        var element1 = new LearningElementViewModel("f", "f", space, content1, "f", "f", "f", LearningElementDifficultyEnum.Easy);
        var element2 = new LearningElementViewModel("e", "e", space, content2, "f", "f", "f", LearningElementDifficultyEnum.Easy);
        space.LearningElements.Add(element1);
        space.LearningElements.Add(element2);
        space.SelectedLearningObject = element1;

        Assert.That(space.SelectedLearningObject, Is.EqualTo(element1));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter:mockElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(space.SelectedLearningObject, Is.EqualTo(element2));
        
        systemUnderTest.DeleteSelectedLearningObject();
        
        Assert.That(space.SelectedLearningObject, Is.Null);
        
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
        var element = new LearningElementViewModel("n", "sn", space, content,"a", "d", "g", LearningElementDifficultyEnum.Easy,3);
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
        var element = new LearningElementViewModel("n", "sn", null, null, "a", "d", "g", LearningElementDifficultyEnum.Easy);
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
        var element = new LearningElementViewModel("f", "f", space, content,"f",
            "f", "f", LearningElementDifficultyEnum.Easy);
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
            .Returns(new LearningElementViewModel("n", "sn", null, null, "a", "d", "g", LearningElementDifficultyEnum.Easy));
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
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        return new LearningSpacePresenter(presentationLogic, learningElementPresenter, logger);
    }
}