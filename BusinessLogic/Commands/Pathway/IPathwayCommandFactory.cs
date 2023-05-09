using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public interface IPathwayCommandFactory
{
    ICreateLearningPathWay GetCreateCommand(LearningWorld learningWorld, IObjectInPathWay sourceObject,
        IObjectInPathWay targetObject, Action<LearningWorld> mappingAction);

    IDeleteLearningPathWay GetDeleteCommand
        (LearningWorld learningWorld, LearningPathway learningPathway, Action<LearningWorld> mappingAction);

    IDragObjectInPathWay GetDragCommand(IObjectInPathWay learningObject, double oldPositionX, double oldPositionY,
        double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction);
}