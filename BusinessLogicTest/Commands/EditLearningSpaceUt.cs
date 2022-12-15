using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class EditLearningSpaceUt
{
    [Test]
    public void Execute_EditsLearningSpace()
    {
        var space = new LearningSpace("a", "b", "c", "d", "e", 5);
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var topic = new Topic("abc");
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningSpace(space, name, shortname, authors, description, goals, requiredPoints, topic, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("a"));
            Assert.That(space.Shortname, Is.EqualTo("b"));
            Assert.That(space.Authors, Is.EqualTo("c"));
            Assert.That(space.Description, Is.EqualTo("d"));
            Assert.That(space.Goals, Is.EqualTo("e"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new LearningSpace("a", "b", "c", "d", "e", 5);
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        var topic = new Topic("abc");
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningSpace(space, name, shortname, authors, description, goals, requiredPoints, topic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningSpace()
    {
        var space = new LearningSpace("g", "h", "i", "j", "k", 5);
        var name = "space1";
        var shortname = "sp1";
        var authors = "marvin";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        var topic = new Topic("abc");
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        
        var command = new EditLearningSpace(space, name, shortname, authors, description, goals, requiredPoints, topic, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Shortname, Is.EqualTo("h"));
            Assert.That(space.Authors, Is.EqualTo("i"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Shortname, Is.EqualTo("h"));
            Assert.That(space.Authors, Is.EqualTo("i"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Shortname, Is.EqualTo("sp1"));
            Assert.That(space.Authors, Is.EqualTo("marvin"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
        });
    }
}