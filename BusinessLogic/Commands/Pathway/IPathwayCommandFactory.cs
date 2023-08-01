using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

/// <summary>
/// Factory for creating commands relating to learning pathways.
/// </summary>
public interface IPathwayCommandFactory
{
    /// <summary>
    /// Creates a command to create a learning pathway.
    /// </summary>
    ICreateLearningPathWay GetCreateCommand(LearningWorld learningWorld, IObjectInPathWay sourceObject,
        IObjectInPathWay targetObject, Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to delete a learning pathway.
    /// </summary>
    IDeleteLearningPathWay GetDeleteCommand
        (LearningWorld learningWorld, LearningPathway learningPathway, Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to drag a learning pathway.
    /// </summary>
    IDragObjectInPathWay GetDragCommand(IObjectInPathWay learningObject, double oldPositionX, double oldPositionY,
        double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction);
}