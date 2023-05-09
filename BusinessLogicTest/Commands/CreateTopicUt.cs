using BusinessLogic.Commands;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreateTopicUt
{
    [Test]
    public void Execute_CreatesTopic()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f")
        {
            UnsavedChanges = false
        };
        var name = "topic1";
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreateTopic(world, name, mappingAction);
        
        Assert.IsEmpty(world.Topics);
        Assert.IsFalse(actionWasInvoked);
        Assert.IsFalse(world.UnsavedChanges);

        command.Execute();
        
        Assert.That(world.Topics, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        Assert.IsTrue(world.UnsavedChanges);
        var topic = world.Topics.First();
        Assert.That(topic.Name, Is.EqualTo("topic1"));
    }
    
    [Test]
    public void Execute_NameOfTopicAlreadyExists_CreatesTopicWithEditedName()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "topic1";
        var topic = new Topic(name);
        world.Topics.Add(topic);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreateTopic(world, name, mappingAction);
        
        Assert.That(world.Topics, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(world.Topics, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        var topic1 = world.Topics.Last();
        Assert.That(topic1.Name, Is.EqualTo("topic1(1)"));
        
        command.Execute();
        
        Assert.That(world.Topics, Has.Count.EqualTo(3));
        Assert.IsTrue(actionWasInvoked); 
        var topic2 = world.Topics.Last();
        Assert.That(topic2.Name, Is.EqualTo("topic1(2)"));
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "topic1";
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateTopic(world, name, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesCreateTopic()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "topic1";
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreateTopic(world, name, mappingAction);
        
        Assert.That(world.Topics, Has.Count.EqualTo(0));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.Topics, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.Topics, Has.Count.EqualTo(0));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.Topics, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
    }
}