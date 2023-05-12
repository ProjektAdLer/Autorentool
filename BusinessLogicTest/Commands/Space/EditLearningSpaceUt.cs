using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Space;

[TestFixture]

public class EditLearningSpaceUt
{
    [Test]
    public void Execute_EditsLearningSpace()
    {
        var space = new LearningSpace("a", "d", "e", 5)
        {
            UnsavedChanges = false
        };
        var name = "space1";
        var description = "space for learning";
        var goals = "learning";
        var topic = new BusinessLogic.Entities.Topic("abc");
        var requiredPoints = 10;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningSpace(space, name, description, goals, requiredPoints, topic, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.Name, Is.EqualTo("a"));
            Assert.That(space.Description, Is.EqualTo("d"));
            Assert.That(space.Goals, Is.EqualTo("e"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new LearningSpace("a", "d", "e", 5);
        var name = "space1";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        var topic = new BusinessLogic.Entities.Topic("abc");
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningSpace(space, name, description, goals, requiredPoints, topic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningSpace()
    {
        var space = new LearningSpace("g", "j", "k", 5)
        {
            UnsavedChanges = false
        };
        var name = "space1";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        var topic = new BusinessLogic.Entities.Topic("abc");
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        
        var command = new EditLearningSpace(space, name, description, goals, requiredPoints, topic, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
            Assert.That(space.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.Goals, Is.EqualTo("k"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
            Assert.That(space.UnsavedChanges, Is.False);

        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}