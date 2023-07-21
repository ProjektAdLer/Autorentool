using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Topic;

public class TopicCommandFactory : ITopicCommandFactory
{
    public ICreateTopic GetCreateCommand(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction,
        ILogger<TopicCommandFactory> logger)
        => new CreateTopic(learningWorld, name, mappingAction, logger);

    public IDeleteTopic GetDeleteCommand(LearningWorld learningWorld, ITopic topic, Action<LearningWorld> mappingAction,
        ILogger<TopicCommandFactory> logger)
        => new DeleteTopic(learningWorld, topic, mappingAction, logger);

    public IEditTopic GetEditCommand(Entities.Topic topic, string name, Action<Entities.Topic> mappingAction,
        ILogger<TopicCommandFactory> logger)
        => new EditTopic(topic, name, mappingAction, logger);
}