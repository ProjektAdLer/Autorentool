using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class EditLearningElementUt
{
    [Test]
    public void Execute_EditsLearningElement_WorldParent()
    {
        var parent = new LearningWorld("l", "k", "j", "j", "j", "l");
        var content = new LearningContent("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElement("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, parent, 8,17f, 29f);
        parent.LearningElements.Add(element);
        
        var name = "new element";
        var shortname = "ne";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var mappingAction = Substitute.For<Action<LearningElement>>();

        var command = new EditLearningElement(element, parent, name, shortname, authors, description, goals, difficulty,
            workload, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Shortname, Is.EqualTo(shortname));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo(authors));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
    }
    
    [Test]
    public void Execute_EditsLearningElement_SpaceParent()
    {
        var parent = new LearningSpace("l", "k", "j", "j", "j");
        var content = new LearningContent("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElement("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, parent, 8,17f, 29f);
        parent.LearningElements.Add(element);
        
        var name = "new element";
        var shortname = "ne";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var mappingAction = Substitute.For<Action<LearningElement>>();

        var command = new EditLearningElement(element, parent, name, shortname, authors, description, goals, difficulty,
            workload, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
        
        command.Execute();
        
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
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = new LearningSpace("l", "k", "j", "j", "j");
        var element = new LearningElement("a", "b", null!, "c", "d", "e", LearningElementDifficultyEnum.Easy);
        var name = "new element";
        var shortname = "ne";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var mappingAction = Substitute.For<Action<LearningElement>>();;

        var command = new EditLearningElement(element, parent, name, shortname, authors, description, goals, difficulty, workload, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningElement()
    {
        var parent = new LearningSpace("l", "k", "j", "j", "j");
        var content = new LearningContent("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElement("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Medium, parent, 8,17f, 29f);
        parent.LearningElements.Add(element);
        
        var name = "new element";
        var shortname = "ne";
        var authors = "marvin";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var mappingAction = Substitute.For<Action<LearningElement>>();

        var command = new EditLearningElement(element, parent, name, shortname, authors, description, goals, difficulty,
            workload, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
        
        command.Execute();
        
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
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Shortname, Is.EqualTo("b"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Authors, Is.EqualTo("e"));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
        
        command.Redo();
        
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
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(parent.LearningElements, Has.Count.EqualTo(1));
        });
    }

}