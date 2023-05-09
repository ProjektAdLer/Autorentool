using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Condition;

public interface IConditionCommandFactory
{
    ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition, double positionX,
        double positionY, Action<LearningWorld> mappingAction);

    ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        IObjectInPathWay sourceObject, ISelectableObjectInWorld targetObject, Action<LearningWorld> mappingAction);

    IDeletePathWayCondition GetDeleteCommand(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction);

    IEditPathWayCondition GetEditCommand(PathWayCondition pathWayCondition, ConditionEnum condition,
        Action<PathWayCondition> mappingAction);
}