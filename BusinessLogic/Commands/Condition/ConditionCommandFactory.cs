using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class ConditionCommandFactory : IConditionCommandFactory
{
    public ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition, double positionX,
        double positionY, Action<LearningWorld> mappingAction)
    {
        return new CreatePathWayCondition(learningWorld, condition, positionX, positionY, mappingAction);
    }

    public ICreatePathWayCondition GetCreateCommand(LearningWorld learningWorld, ConditionEnum condition,
        IObjectInPathWay sourceObject, ISelectableObjectInWorld targetObject, Action<LearningWorld> mappingAction)
    {
        return new CreatePathWayCondition(learningWorld, condition, sourceObject, targetObject, mappingAction);
    }

    public IDeletePathWayCondition GetDeleteCommand(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction)
    {
        return new DeletePathWayCondition(learningWorld, pathWayCondition, mappingAction);
    }

    public IEditPathWayCondition GetEditCommand(PathWayCondition pathWayCondition, ConditionEnum condition, Action<PathWayCondition> mappingAction)
    {
        return new EditPathWayCondition(pathWayCondition, condition, mappingAction);
    }
}