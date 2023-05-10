using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Topic;

public interface ITopicCommandFactory
{
    ICreateTopic GetCreateCommand(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction);
    IDeleteTopic GetDeleteCommand(LearningWorld learningWorld, ITopic topic, Action<LearningWorld> mappingAction);
    IEditTopic GetEditCommand(Entities.Topic topic, string name, Action<Entities.Topic> mappingAction);
}