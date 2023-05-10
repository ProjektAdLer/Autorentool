using BusinessLogic.Entities;
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
        double positionY, Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to create a condition.
    /// </summary>
    ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        IObjectInPathWay sourceObject, ISelectableObjectInWorld targetObject, Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to delete a condition.
    /// </summary>
    IDeletePathWayCondition GetDeleteCommand(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to edit a condition.
    /// </summary>
    IEditPathWayCondition GetEditCommand(PathWayCondition pathWayCondition, ConditionEnum condition,
        Action<PathWayCondition> mappingAction);
}