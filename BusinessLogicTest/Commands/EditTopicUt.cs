using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class EditTopicUt
{
    [Test]
    public void Execute_Undo_Redo_EditsTopic()
    {
        var topic = new Topic("Topic 1");
        var name = "NewTopic1";
        bool actionWasInvoked = false;
        Action<Topic> mappingAction = _ => actionWasInvoked = true;
        
        var command = new EditTopic(topic, name, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(topic.Name, Is.EqualTo("Topic 1"));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(topic.Name, Is.EqualTo(name));
        });
        
        actionWasInvoked = false;
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(topic.Name, Is.EqualTo("Topic 1"));
        });

        actionWasInvoked = false;
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(topic.Name, Is.EqualTo(name));
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var topic = new Topic("Topic 1");
        var name = "NewTopic1";
        bool actionWasInvoked = false;
        Action<Topic> mappingAction = _ => actionWasInvoked = true;

        var command = new EditTopic(topic, name, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
}