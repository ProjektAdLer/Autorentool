using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Pathway;

public class PathwayCommandFactory : IPathwayCommandFactory
{
    public ICreateLearningPathWay GetCreateCommand(LearningWorld learningWorld, IObjectInPathWay sourceObject,
        IObjectInPathWay targetObject, Action<LearningWorld> mappingAction, ILogger<PathwayCommandFactory> logger) =>
        new CreateLearningPathWay(learningWorld, sourceObject, targetObject, mappingAction, logger);

    public IDeleteLearningPathWay GetDeleteCommand(LearningWorld learningWorld, LearningPathway learningPathway,
        Action<LearningWorld> mappingAction, ILogger<PathwayCommandFactory> logger) =>
        new DeleteLearningPathWay(learningWorld, learningPathway, mappingAction, logger);

    public IDragObjectInPathWay GetDragCommand(IObjectInPathWay learningObject, double oldPositionX,
        double oldPositionY, double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction,
        ILogger<PathwayCommandFactory> logger) =>
        new DragObjectInPathWay(learningObject, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, logger);
}