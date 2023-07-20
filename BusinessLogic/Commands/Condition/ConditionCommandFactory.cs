using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class ConditionCommandFactory : IConditionCommandFactory
{
    public ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        double positionX,
        double positionY, Action<LearningWorld> mappingAction, ILogger<ConditionCommandFactory> logger)
    {
        return new CreatePathWayCondition(learningWorld, condition, positionX, positionY, mappingAction, logger);
    }

    public ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        IObjectInPathWay sourceObject, ISelectableObjectInWorld targetObject, Action<LearningWorld> mappingAction,
        ILogger<ConditionCommandFactory> logger)
    {
        return new CreatePathWayCondition(learningWorld, condition, sourceObject, targetObject, mappingAction, logger);
    }

    public IDeletePathWayCondition GetDeleteCommand(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction, ILogger<ConditionCommandFactory> logger)
    {
        return new DeletePathWayCondition(learningWorld, pathWayCondition, mappingAction, logger);
    }

    public IEditPathWayCondition GetEditCommand(PathWayCondition pathWayCondition, ConditionEnum condition,
        Action<PathWayCondition> mappingAction, ILogger<ConditionCommandFactory> logger)
    {
        return new EditPathWayCondition(pathWayCondition, condition, mappingAction, logger);
    }
}