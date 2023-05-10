using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Topic;

/// <summary>
/// Factory for creating commands relating to topics.
/// </summary>
public interface ITopicCommandFactory
{
    /// <summary>
    /// Creates a command to create a topic.
    /// </summary>
    ICreateTopic GetCreateCommand(LearningWorld learningWorld, string name, Action<LearningWorld> mappingAction);
    
    /// <summary>
    /// Creates a command to delete a topic.
    /// </summary>
    IDeleteTopic GetDeleteCommand(LearningWorld learningWorld, ITopic topic, Action<LearningWorld> mappingAction);
    
    /// <summary>
    /// Creates a command to edit a topic.
    /// </summary>
    IEditTopic GetEditCommand(Entities.Topic topic, string name, Action<Entities.Topic> mappingAction);
}