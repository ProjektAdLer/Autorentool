using BusinessLogic.Commands.Topic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Topic;

[TestFixture]

public class EditTopicUt
{
    [Test]
    public void Execute_Undo_Redo_EditsTopic()
    {
        var topic = new BusinessLogic.Entities.Topic("Topic 1")
        {
            UnsavedChanges = false
        };
        var name = "NewTopic1";
        bool actionWasInvoked = false;
        Action<BusinessLogic.Entities.Topic> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<TopicCommandFactory>>();
        
        var command = new EditTopic(topic, name, mappingAction, logger);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(topic.Name, Is.EqualTo("Topic 1"));
            Assert.That(topic.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(topic.Name, Is.EqualTo(name));
            Assert.That(topic.UnsavedChanges, Is.True);
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
        var topic = new BusinessLogic.Entities.Topic("Topic 1");
        var name = "NewTopic1";
        bool actionWasInvoked = false;
        Action<BusinessLogic.Entities.Topic> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<TopicCommandFactory>>();

        var command = new EditTopic(topic, name, mappingAction, logger);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
}