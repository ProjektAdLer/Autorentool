using System;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningElement.ActivationElement;
using Presentation.PresentationLogic.LearningElement.InteractionElement;
using Presentation.PresentationLogic.LearningElement.TestElement;
using Presentation.PresentationLogic.LearningElement.TransferElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.LearningElement;

[TestFixture]

public class LearningElementPresenterUt
{
    [Test]
    public void CreateNewTransferElement_CreatesCorrectViewModel_WorldParent_Image()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var contentType = ContentTypeEnum.Image;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 6;

        var element = systemUnderTest.CreateNewTransferElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload,0f, 0f);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(ImageTransferElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewTransferElement_CreatesCorrectViewModel_WorldParent_Video()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var contentType = ContentTypeEnum.Video;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 6;

        var element = systemUnderTest.CreateNewTransferElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload,0f, 0f);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoTransferElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewTransferElement_CreatesCorrectViewModel_WorldParent_Pdf()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var contentType = ContentTypeEnum.Pdf;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 6;

        var element = systemUnderTest.CreateNewTransferElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload,0f, 0f);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(PdfTransferElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewTransferElement_CreatesCorrectViewModel_SpaceParent_Image()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var contentType = ContentTypeEnum.Image;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 6;

        var element = systemUnderTest.CreateNewTransferElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload,0f, 0f);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(ImageTransferElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewTransferElement_CreatesCorrectViewModel_SpaceParent_Video()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var contentType = ContentTypeEnum.Video;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 6;

        var element = systemUnderTest.CreateNewTransferElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload,0f, 0f);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoTransferElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewTransferElement_CreatesCorrectViewModel_SpaceParent_Pdf()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var contentType = ContentTypeEnum.Pdf;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 6;

        var element = systemUnderTest.CreateNewTransferElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload,0f, 0f);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(PdfTransferElementViewModel)));
        });
    }

    [Test]
    public void CreateNewTransferElement_ThrowsApplicationException()
    {
        var systemUnderTest = new LearningElementPresenter();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.CreateNewTransferElement("a", "b", null, ContentTypeEnum.H5P, null, "d", "e", "f",LearningElementDifficultyEnum.Easy,6));
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
    }
    
    [Test]
    public void CreateNewActivationElement_CreatesCorrectViewModel_WorldParent_Video()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var contentType = ContentTypeEnum.Video;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 7;

        var element = systemUnderTest.CreateNewActivationElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoActivationElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewActivationElement_CreatesCorrectViewModel_SpaceParent_H5P()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var contentType = ContentTypeEnum.H5P;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 7;

        var element = systemUnderTest.CreateNewActivationElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PActivationElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewActivationElement_CreatesCorrectViewModel_SpaceParent_Video()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var contentType = ContentTypeEnum.Video;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 7;

        var element = systemUnderTest.CreateNewActivationElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoActivationElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewActivationElement_CreatesCorrectViewModel_WorldParent_H5P()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var contentType = ContentTypeEnum.H5P;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 7;

        var element = systemUnderTest.CreateNewActivationElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PActivationElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewActivationElement_ThrowsApplicationException()
    {
        var systemUnderTest = new LearningElementPresenter();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.CreateNewActivationElement("a", "b", null, ContentTypeEnum.Image, null, "d", "e", "f", LearningElementDifficultyEnum.Easy,8));
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
    }
    
    [Test]
    public void CreateNewInteractionElement_CreatesCorrectViewModel_WorldParent()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var contentType = ContentTypeEnum.H5P;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 9;

        var element = systemUnderTest.CreateNewInteractionElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PInteractionElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewInteractionElement_CreatesCorrectViewModel_SpaceParent()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var contentType = ContentTypeEnum.H5P;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 9;

        var element = systemUnderTest.CreateNewInteractionElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PInteractionElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewInteractionElement_ThrowsApplicationException()
    {
        var systemUnderTest = new LearningElementPresenter();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.CreateNewInteractionElement("a", "b", null, ContentTypeEnum.Image, null, "d", "e", "f", LearningElementDifficultyEnum.Easy, 2));
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
    }
    
    [Test]
    public void CreateNewTestElement_CreatesCorrectViewModel_WorldParent()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var contentType = ContentTypeEnum.H5P;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 2;

        var element = systemUnderTest.CreateNewTestElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PTestElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewTestElement_CreatesCorrectViewModel_SpaceParent()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var contentType = ContentTypeEnum.H5P;
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var authors = "d";
        var description = "e";
        var goals = "f";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var workload = 2;

        var element = systemUnderTest.CreateNewTestElement(name, shortname, parent, contentType, content,
            authors, description, goals, difficulty, workload);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PTestElementViewModel)));
        });
    }
    
    [Test]
    public void CreateNewTestElement_ThrowsApplicationException()
    {
        var systemUnderTest = new LearningElementPresenter();

        var ex = Assert.Throws<ApplicationException>(() =>
            systemUnderTest.CreateNewTestElement("a", "b", null, ContentTypeEnum.Image, null, "d", "e", "f", LearningElementDifficultyEnum.Medium, 4));
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
    }

    [Test]
    public void AddLearningElementParentAssignment_ThrowsNotImplemented()
    {
        var systemUnderTest = new LearningElementPresenter();

        var ex = Assert.Throws<NotImplementedException>(() =>
            systemUnderTest.CreateNewTransferElement("a", "b", null, ContentTypeEnum.Image, null, "d", "e", "f", LearningElementDifficultyEnum.Hard, 3));
        Assert.That(ex!.Message, Is.EqualTo("Type of Assignment is not implemented"));
    }
    
    [Test]
    public void EditLearningElement_EditsViewModelCorrectly_WorldParent()
    {
        var systemUnderTest = new LearningElementPresenter();
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("a", "b", null, content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, 8, 17f,29f);
        
        var name = "new element";
        var shortname = "ne";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var posx = 22f;

        element = systemUnderTest.EditLearningElement(element, name, shortname, parent, authors, description,
            goals, difficulty, workload, posx);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.PositionX, Is.EqualTo(posx));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionY, Is.EqualTo(29f));
        });
    }
    
    [Test]
    public void EditLearningElement_EditsViewModelCorrectly_SpaceParent()
    {
        var systemUnderTest = new LearningElementPresenter();
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("a", "b", null, content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, 9, 17f,29f);
        
        var name = "new element";
        var shortname = "ne";
        var parent = new LearningSpaceViewModel("","boo", "bla", "", "");
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var posx = 22f;

        element = systemUnderTest.EditLearningElement(element, name, shortname, parent, authors, description,
            goals, difficulty, workload, posx);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.PositionX, Is.EqualTo(posx));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionY, Is.EqualTo(29f));
        });
    }

    [Test]
    public void RemoveLearningElementFromParentAssignment_RemovesElementFromWorld()
    {
        var systemUnderTest = new LearningElementPresenter();
        var parent = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("a", "b", parent, content,
            "e", "f","g", LearningElementDifficultyEnum.Easy, 4, 17f,29f);
        parent.LearningElements.Add(element);
        
        Assert.That(parent.LearningElements, Contains.Item(element));
        
        systemUnderTest.RemoveLearningElementFromParentAssignment(element);
        
        Assert.That(parent.LearningElements, Is.Empty);
    }
    
    [Test]
    public void RemoveLearningElementFromParentAssignment_RemovesElementFromSpace()
    {
        var systemUnderTest = new LearningElementPresenter();
        var parent = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("a", "b", parent, content ,
            "e", "f","g", LearningElementDifficultyEnum.Hard, 9, 17f,29f);
        parent.LearningElements.Add(element);
        
        Assert.That(parent.LearningElements, Contains.Item(element));
        
        systemUnderTest.RemoveLearningElementFromParentAssignment(element);
        
        Assert.That(parent.LearningElements, Is.Empty);
    }
}