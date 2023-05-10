using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class PathwayCommandFactory : IPathwayCommandFactory
{
    public ICreateLearningPathWay GetCreateCommand(LearningWorld learningWorld, IObjectInPathWay sourceObject,
        IObjectInPathWay targetObject, Action<LearningWorld> mappingAction) =>
        new CreateLearningPathWay(learningWorld, sourceObject, targetObject, mappingAction);

    public IDeleteLearningPathWay GetDeleteCommand(LearningWorld learningWorld, LearningPathway learningPathway,
        Action<LearningWorld> mappingAction) =>
        new DeleteLearningPathWay(learningWorld, learningPathway, mappingAction);

    public IDragObjectInPathWay GetDragCommand(IObjectInPathWay learningObject, double oldPositionX,
        double oldPositionY, double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction) =>
        new DragObjectInPathWay(learningObject, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction);
}