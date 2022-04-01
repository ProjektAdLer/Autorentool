using AuthoringTool.PresentationLogic.LearningElement;
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
        var type = "c";
        var content = "d";
        var authors = "d";
        var description = "e";
        var goals = "f";

        var element = systemUnderTest.CreateNewLearningElement(name, shortname, type, content,
            authors, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
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
        var element = new LearningElementViewModel("a", "b", "c", "d", "e",
            "f","g");
        
        var name = "new element";
        var shortname = "ne";
        var type = "transfer";
        var content = "video";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";

        element = systemUnderTest.EditLearningElement(element, name, shortname, type, content, authors, description,
            goals);
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Type, Is.EqualTo(type));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
        });
    }
}