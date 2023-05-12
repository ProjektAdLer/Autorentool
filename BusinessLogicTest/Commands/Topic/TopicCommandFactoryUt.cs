using BusinessLogic.Commands.Topic;
using BusinessLogic.Entities;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Topic;

[TestFixture]
public class TopicCommandFactoryUt
{
    private TopicCommandFactory _factory = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new TopicCommandFactory();
    }

    [Test]
    public void GetCreateCommand_WithLearningWorldAndName_ReturnsCreateTopicCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var name = "Topic 1";
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetCreateCommand(learningWorld, name, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateTopic>());
        var resultCasted = result as CreateTopic;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.Topic.Name, Is.EqualTo(name));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetDeleteCommand_WithLearningWorldAndTopic_ReturnsDeleteTopicCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var topic = EntityProvider.GetTopic();
        learningWorld.Topics.Add(topic);
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetDeleteCommand(learningWorld, topic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteTopic>());
        var resultCasted = result as DeleteTopic;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.Topic, Is.EqualTo(topic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetEditCommand_WithTopicAndName_ReturnsEditTopicCommand()
    {
        // Arrange
        var topic = EntityProvider.GetTopic();
        var name = "Topic 2";
        Action<BusinessLogic.Entities.Topic> mappingAction = t => { };

        // Act
        var result = _factory.GetEditCommand(topic, name, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditTopic>());
        var resultCasted = result as EditTopic;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Topic, Is.EqualTo(topic));
            Assert.That(resultCasted.TopicName, Is.EqualTo(name));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }
}
