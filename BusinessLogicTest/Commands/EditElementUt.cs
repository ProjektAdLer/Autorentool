using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class EditElementUt
{
    [Test]
    public void Execute_EditsElement()
    {
        var parent = new Space("l", "k", "j", "j", "j", 5);
        var content = new Content("bar", "foo", "");
        var element = new Element("a", "b", content, "url",
            "e", "f", "g", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.SpaceLayout.Elements = new IElement[]{element};

    var name = "new element";
        var shortname = "ne";
        var url = "google.com";
        var authors = "marvin";
        var description = "video of learn stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = ElementDifficultyEnum.Easy;
        bool actionWasInvoked = false;
        Action<Element> mappingAction = _ => actionWasInvoked = true;

        var command = new EditElement(element, parent, name, shortname, url, authors, description, goals, difficulty,
            workload, points, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(ElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = new Space("l", "k", "j", "j", "j", 5);
        var element = new Element("a", "b", null!, "url","c", "d", "e", ElementDifficultyEnum.Easy);
        var name = "new element";
        var shortname = "ne";
        var url = "google.com";
        var authors = "marvin";
        var description = "video of learn stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = ElementDifficultyEnum.Easy;
        bool actionWasInvoked = false;
        Action<Element> mappingAction = _ => actionWasInvoked = true;

        var command = new EditElement(element, parent, name, shortname, url ,authors, description, goals, difficulty, workload, points, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditElement()
    {
        var parent = new Space("l", "k", "j", "j", "j", 5);
        var content = new Content("bar", "foo", "");
        var element = new Element("a", "b", content, "url",
            "e", "f","g", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.SpaceLayout.Elements = new IElement[] {element};
        
        var name = "new element";
        var shortname = "ne";
        var url = "google.com";
        var authors = "marvin";
        var description = "video of learn stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = ElementDifficultyEnum.Easy;
        bool actionWasInvoked = false;
        Action<Element> mappingAction = _ => actionWasInvoked = true;

        var command = new EditElement(element, parent, name, shortname, url, authors, description, goals, difficulty,
            workload, points, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(ElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(ElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.Content, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
        });
    }

}