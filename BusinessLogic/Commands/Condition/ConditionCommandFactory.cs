using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class ConditionCommandFactory : IConditionCommandFactory
{
    public ConditionCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        double positionX,
        double positionY, Action<LearningWorld> mappingAction) =>
        new CreatePathWayCondition(learningWorld, condition, positionX, positionY, mappingAction,
            LoggerFactory.CreateLogger<CreatePathWayCondition>());

    public ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        IObjectInPathWay sourceObject, ISelectableObjectInWorld targetObject, Action<LearningWorld> mappingAction) =>
        new CreatePathWayCondition(learningWorld, condition, sourceObject, targetObject, mappingAction,
            LoggerFactory.CreateLogger<CreatePathWayCondition>());

    public IDeletePathWayCondition GetDeleteCommand(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction) =>
        new DeletePathWayCondition(learningWorld, pathWayCondition, mappingAction,
            LoggerFactory.CreateLogger<DeletePathWayCondition>());

    public IEditPathWayCondition GetEditCommand(PathWayCondition pathWayCondition, ConditionEnum condition,
        Action<PathWayCondition> mappingAction) =>
        new EditPathWayCondition(pathWayCondition, condition, mappingAction,
            LoggerFactory.CreateLogger<EditPathWayCondition>());
}