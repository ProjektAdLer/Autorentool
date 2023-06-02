using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Topic;

public class TopicCommandFactory : ITopicCommandFactory
{
    public ICreateTopic GetCreateCommand(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction)
        => new CreateTopic(learningWorld, name, mappingAction);

    public IDeleteTopic GetDeleteCommand(LearningWorld learningWorld, ITopic topic, Action<LearningWorld> mappingAction)
        => new DeleteTopic(learningWorld, topic, mappingAction);

    public IEditTopic GetEditCommand(Entities.Topic topic, string name, Action<Entities.Topic> mappingAction)
        => new EditTopic(topic, name, mappingAction);
}