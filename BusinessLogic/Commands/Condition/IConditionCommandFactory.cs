using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Condition;

/// <summary>
/// Factory for creating commands relating to pathway conditions.
/// </summary>
public interface IConditionCommandFactory
{
    /// <summary>
    /// Creates a command to create a condition.
    /// </summary>
    ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition, double positionX,
        double positionY, Action<LearningWorld> mappingAction, ILogger<ConditionCommandFactory> logger);

    /// <summary>
    /// Creates a command to create a condition.
    /// </summary>
    ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        IObjectInPathWay sourceObject, ISelectableObjectInWorld targetObject, Action<LearningWorld> mappingAction,
        ILogger<ConditionCommandFactory> logger);

    /// <summary>
    /// Creates a command to delete a condition.
    /// </summary>
    IDeletePathWayCondition GetDeleteCommand(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction, ILogger<ConditionCommandFactory> logger);

    /// <summary>
    /// Creates a command to edit a condition.
    /// </summary>
    IEditPathWayCondition GetEditCommand(PathWayCondition pathWayCondition, ConditionEnum condition,
        Action<PathWayCondition> mappingAction, ILogger<ConditionCommandFactory> logger);
}