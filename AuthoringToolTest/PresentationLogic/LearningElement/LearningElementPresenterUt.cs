using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningElement;

[TestFixture]

public class LearningElementPresenterUt
{
    [Test]
    public void LearningElementPresenter_CreateNewLearningElement_CreatesCorrectViewModel()
    {
        var systemUnderTest = new LearningElementPresenter();
        var name = "a";
        var shortname = "b";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var type = "c";
        var content = "d";
        var authors = "d";
        var description = "e";
        var goals = "f";

        var element = systemUnderTest.CreateNewLearningElement(name, shortname, parent, type, content,
            authors, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Type, Is.EqualTo(type));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
        });
    }
    
    [Test]
    public void LearningElementPresenter_EditLearningElement_EditsViewModelCorrectly()
    {
        var systemUnderTest = new LearningElementPresenter();
        var element = new LearningElementViewModel("a", "b", null, "c", "d", null,
            "e", "f","g", 17f,29f);
        
        var name = "new element";
        var shortname = "ne";
        var parent = new LearningWorldViewModel("","boo", "bla", "", "", "");
        var type = "transfer";
        var content = "video";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var posx = 22f;

        element = systemUnderTest.EditLearningElement(element, name, shortname, parent,  type, content, authors, description,
            goals, posx);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Type, Is.EqualTo(type));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.PositionX, Is.EqualTo(posx));
            Assert.That(element.PositionY, Is.EqualTo(29f));
        });
    }

    [Test]
    public void LearningElementPresenter_RemoveLearningElementFromParentAssignment_RemovesElementFromWorld()
    {
        var systemUnderTest = new LearningElementPresenter();
        var parent = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var element = new LearningElementViewModel("a", "b", parent, "c", "d",null,
            "e", "f","g", 17f,29f);
        parent.LearningElements.Add(element);
        
        Assert.That(parent.LearningElements, Contains.Item(element));
        
        systemUnderTest.RemoveLearningElementFromParentAssignment(element);
        
        Assert.That(parent.LearningElements, Is.Empty);
    }
    
    [Test]
    public void LearningElementPresenter_RemoveLearningElementFromParentAssignment_RemovesElementFromSpace()
    {
        var systemUnderTest = new LearningElementPresenter();
        var parent = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var element = new LearningElementViewModel("a", "b", parent, "c", "d",null,
            "e", "f","g", 17f,29f);
        parent.LearningElements.Add(element);
        
        Assert.That(parent.LearningElements, Contains.Item(element));
        
        systemUnderTest.RemoveLearningElementFromParentAssignment(element);
        
        Assert.That(parent.LearningElements, Is.Empty);
    }
}