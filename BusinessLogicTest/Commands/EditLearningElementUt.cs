using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class EditLearningElementUt
{
    [Test]
    public void Execute_EditsLearningElement()
    {
        var parent = new LearningSpace("l", "k", "j", "j", "j", 5);
        var content = new FileContent("bar", "foo", "");
        var element = new LearningElement("a", "b", content,
            "e", "f", "g", LearningElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.LearningSpaceLayout.LearningElements = new ILearningElement[]{element};

    var name = "new element";
        var shortname = "ne";
        var url = "google.com";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        bool actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, shortname, authors, description, goals, difficulty,
            workload, points, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = new LearningSpace("l", "k", "j", "j", "j", 5);
        var element = new LearningElement("a", "b", null!,"c", "d", "e", LearningElementDifficultyEnum.Easy);
        var name = "new element";
        var shortname = "ne";
        var url = "google.com";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        bool actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, shortname,authors, description, goals, difficulty, workload, points, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningElement()
    {
        var parent = new LearningSpace("l", "k", "j", "j", "j", 5);
        var content = new FileContent("bar", "foo", "");
        var element = new LearningElement("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.LearningSpaceLayout.LearningElements = new ILearningElement[] {element};
        
        var name = "new element";
        var shortname = "ne";
        var url = "google.com";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        bool actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, shortname, authors, description, goals, difficulty,
            workload, points, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
    }

}