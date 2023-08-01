using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Pathway;

public class PathwayCommandFactory : IPathwayCommandFactory
{
    public PathwayCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateLearningPathWay GetCreateCommand(LearningWorld learningWorld, IObjectInPathWay sourceObject,
        IObjectInPathWay targetObject, Action<LearningWorld> mappingAction) =>
        new CreateLearningPathWay(learningWorld, sourceObject, targetObject, mappingAction,
            LoggerFactory.CreateLogger<CreateLearningPathWay>());

    public IDeleteLearningPathWay GetDeleteCommand(LearningWorld learningWorld, LearningPathway learningPathway,
        Action<LearningWorld> mappingAction) =>
        new DeleteLearningPathWay(learningWorld, learningPathway, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningPathWay>());

    public IDragObjectInPathWay GetDragCommand(IObjectInPathWay learningObject, double oldPositionX,
        double oldPositionY, double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction) =>
        new DragObjectInPathWay(learningObject, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, LoggerFactory.CreateLogger<DragObjectInPathWay>());
}