using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteTopicUt
{
    [Test]
    public void Execute_Undo_Redo_DeletesTopic()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var topic = new Topic("Topic1");
        world.Topics.Add(topic);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeleteTopic(world, topic, mappingAction);
        
        Assert.That(world.Topics, Does.Contain(topic));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.Topics, Is.Empty);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.Topics, Does.Contain(topic));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        Assert.That(world.Topics, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var topic = new Topic("Topic1");
        world.Topics.Add(topic);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteTopic(world, topic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
}