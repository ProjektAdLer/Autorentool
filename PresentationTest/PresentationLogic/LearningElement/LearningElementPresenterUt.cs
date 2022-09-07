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
    public void EditLearningElement_EditsViewModelCorrectly_WorldParent()
    {
        var systemUnderTest = new LearningElementPresenter();
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, null, 8,17f, 29f);
        
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
        var element = new LearningElementViewModel("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, null, 9,17f, 29f);
        
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
        var element = new LearningElementViewModel("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Easy, parent, 4,17f, 29f);
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
        var element = new LearningElementViewModel("a", "b", content ,
            "e", "f","g", LearningElementDifficultyEnum.Hard, parent, 9,17f, 29f);
        parent.LearningElements.Add(element);
        
        Assert.That(parent.LearningElements, Contains.Item(element));
        
        systemUnderTest.RemoveLearningElementFromParentAssignment(element);
        
        Assert.That(parent.LearningElements, Is.Empty);
    }
}