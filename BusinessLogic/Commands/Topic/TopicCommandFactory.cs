using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Topic;

public class TopicCommandFactory : ITopicCommandFactory
{
    public TopicCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateTopic GetCreateCommand(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction)
        => new CreateTopic(learningWorld, name, mappingAction, LoggerFactory.CreateLogger<CreateTopic>());

    public IDeleteTopic GetDeleteCommand(LearningWorld learningWorld, ITopic topic, Action<LearningWorld> mappingAction)
        => new DeleteTopic(learningWorld, topic, mappingAction, LoggerFactory.CreateLogger<DeleteTopic>());

    public IEditTopic GetEditCommand(Entities.Topic topic, string name, Action<Entities.Topic> mappingAction)
        => new EditTopic(topic, name, mappingAction, LoggerFactory.CreateLogger<EditTopic>());
}